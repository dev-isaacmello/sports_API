using AutoMapper;
using csharp_api.Application.Dtos.Reservation;
using csharp_api.Application.Interfaces;
using csharp_api.Domain.Entities;
using csharp_api.Domain.Interfaces;

namespace csharp_api.Application.Services;

public class ReservationService(
    IReservationRepository reservationRepository,
    ICourtRepository courtRepository,
    IUserRepository userRepository,
    IMapper mapper) : IReservationService
{
    public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
    {
        var reservations = await reservationRepository.GetAllReservationsAsync();
        return mapper.Map<IEnumerable<ReservationDto>>(reservations);
    }

    public async Task<ReservationDto> GetReservationByIdAsync(int id)
    {
        var reservation = await reservationRepository.GetReservationByIdAsync(id)
            ?? throw new KeyNotFoundException($"Reserva com ID {id} não encontrada");
        
        return mapper.Map<ReservationDto>(reservation);
    }

    public async Task<IEnumerable<ReservationDto>> GetReservationsByUserIdAsync(int userId)
    {
        var reservations = await reservationRepository.GetReservationsByUserIdAsync(userId);
        return mapper.Map<IEnumerable<ReservationDto>>(reservations);
    }

    public async Task<IEnumerable<ReservationDto>> GetReservationsByCourtIdAsync(int courtId)
    {
        var reservations = await reservationRepository.GetReservationsByCourtIdAsync(courtId);
        return mapper.Map<IEnumerable<ReservationDto>>(reservations);
    }

    public async Task<IEnumerable<ReservationDto>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
        {
            throw new InvalidOperationException("Data de início deve ser anterior à data de término");
        }

        var reservations = await reservationRepository.GetReservationsByDateRangeAsync(startDate, endDate);
        return mapper.Map<IEnumerable<ReservationDto>>(reservations);
    }

    public async Task<ReservationDto> CreateReservationAsync(int userId, CreateReservationDto createReservationDto)
    {
        // 1. Validar se o usuário existe
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"Usuário com ID {userId} não encontrado");
        }

        // 2. Validar se a quadra existe e está ativa
        var court = await courtRepository.GetCourtByIdAsync(createReservationDto.CourtId);
        if (court == null)
        {
            throw new KeyNotFoundException($"Quadra com ID {createReservationDto.CourtId} não encontrada");
        }

        if (!court.IsActive)
        {
            throw new InvalidOperationException("Quadra não está ativa para reservas");
        }

        // 3. Validar horários
        ValidateReservationTimes(createReservationDto.StartTime, createReservationDto.EndTime);

        // 4. Verificar se não há conflitos de horário
        var conflicts = await reservationRepository.GetConflictingReservationsAsync(
            createReservationDto.CourtId,
            createReservationDto.StartTime,
            createReservationDto.EndTime);

        if (conflicts.Any())
        {
            throw new InvalidOperationException(
                $"Já existe uma reserva para esta quadra no horário solicitado. " +
                $"Conflito com reserva ID: {conflicts.First().Id}");
        }

        // 5. Calcular o preço total
        var duration = (createReservationDto.EndTime - createReservationDto.StartTime).TotalHours;
        var totalPrice = (decimal)duration * court.PricePerHour;

        // 6. Criar a reserva
        var reservation = new Reservation
        {
            UserId = userId,
            CourtId = createReservationDto.CourtId,
            StartTime = createReservationDto.StartTime,
            EndTime = createReservationDto.EndTime,
            TotalPrice = totalPrice,
            Status = "Confirmed",
            Notes = createReservationDto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        var createdReservation = await reservationRepository.CreateReservationAsync(reservation);
        return mapper.Map<ReservationDto>(createdReservation);
    }

    public async Task<ReservationDto> UpdateReservationAsync(int id, int userId, UpdateReservationDto updateReservationDto)
    {
        // 1. Buscar reserva existente
        var reservation = await reservationRepository.GetReservationByIdAsync(id)
            ?? throw new KeyNotFoundException($"Reserva com ID {id} não encontrada");

        // 2. Validar permissão (só o próprio usuário pode atualizar sua reserva)
        if (reservation.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para alterar esta reserva");
        }

        // 3. Validar status (não pode alterar reserva cancelada ou concluída)
        if (reservation.Status == "Cancelled")
        {
            throw new InvalidOperationException("Não é possível alterar uma reserva cancelada");
        }

        if (reservation.Status == "Completed")
        {
            throw new InvalidOperationException("Não é possível alterar uma reserva já concluída");
        }

        // 4. Validar se a reserva ainda não começou (não pode alterar reserva em andamento)
        if (reservation.StartTime <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Não é possível alterar uma reserva que já começou");
        }

        // 5. Se alterar horários, validar e verificar conflitos
        if (updateReservationDto.StartTime.HasValue || updateReservationDto.EndTime.HasValue)
        {
            var newStartTime = updateReservationDto.StartTime ?? reservation.StartTime;
            var newEndTime = updateReservationDto.EndTime ?? reservation.EndTime;

            ValidateReservationTimes(newStartTime, newEndTime);

            var conflicts = await reservationRepository.GetConflictingReservationsAsync(
                reservation.CourtId,
                newStartTime,
                newEndTime,
                id);

            if (conflicts.Any())
            {
                throw new InvalidOperationException(
                    $"Já existe uma reserva para esta quadra no novo horário solicitado. " +
                    $"Conflito com reserva ID: {conflicts.First().Id}");
            }

            reservation.StartTime = newStartTime;
            reservation.EndTime = newEndTime;

            // Recalcular preço
            var court = await courtRepository.GetCourtByIdAsync(reservation.CourtId);
            var duration = (newEndTime - newStartTime).TotalHours;
            reservation.TotalPrice = (decimal)duration * court!.PricePerHour;
        }

        // 6. Atualizar notas se fornecidas
        if (updateReservationDto.Notes != null)
        {
            reservation.Notes = updateReservationDto.Notes;
        }

        var updatedReservation = await reservationRepository.UpdateReservationAsync(id, reservation);
        return mapper.Map<ReservationDto>(updatedReservation);
    }

    public async Task<ReservationDto> CancelReservationAsync(int id, int userId, CancelReservationDto cancelReservationDto)
    {
        // 1. Buscar reserva
        var reservation = await reservationRepository.GetReservationByIdAsync(id)
            ?? throw new KeyNotFoundException($"Reserva com ID {id} não encontrada");

        // 2. Validar permissão
        if (reservation.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para cancelar esta reserva");
        }

        // 3. Validar status
        if (reservation.Status == "Cancelled")
        {
            throw new InvalidOperationException("Reserva já está cancelada");
        }

        if (reservation.Status == "Completed")
        {
            throw new InvalidOperationException("Não é possível cancelar uma reserva já concluída");
        }

        // 4. Validar tempo de antecedência (mínimo 2 horas antes)
        var hoursUntilReservation = (reservation.StartTime - DateTime.UtcNow).TotalHours;
        if (hoursUntilReservation < 2)
        {
            throw new InvalidOperationException(
                "Cancelamento deve ser feito com pelo menos 2 horas de antecedência");
        }

        // 5. Cancelar reserva
        var cancelledReservation = await reservationRepository.CancelReservationAsync(
            id,
            cancelReservationDto.CancellationReason);

        return mapper.Map<ReservationDto>(cancelledReservation);
    }

    public async Task DeleteReservationAsync(int id, int userId, bool isAdmin)
    {
        var reservation = await reservationRepository.GetReservationByIdAsync(id)
            ?? throw new KeyNotFoundException($"Reserva com ID {id} não encontrada");

        // Apenas admin ou o próprio usuário pode deletar
        if (!isAdmin && reservation.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para deletar esta reserva");
        }

        await reservationRepository.DeleteReservationAsync(id);
    }

    // Método privado para validar horários
    private void ValidateReservationTimes(DateTime startTime, DateTime endTime)
    {
        // 1. Horário de início deve ser no futuro
        if (startTime <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Horário de início deve ser no futuro");
        }

        // 2. Horário de término deve ser após o início
        if (endTime <= startTime)
        {
            throw new InvalidOperationException("Horário de término deve ser após o horário de início");
        }

        // 3. Duração mínima de 1 hora
        var duration = (endTime - startTime).TotalHours;
        if (duration < 1)
        {
            throw new InvalidOperationException("Duração mínima da reserva é de 1 hora");
        }

        // 4. Duração máxima de 8 horas
        if (duration > 8)
        {
            throw new InvalidOperationException("Duração máxima da reserva é de 8 horas");
        }

        // 5. Horário de funcionamento (6h às 23h)
        if (startTime.Hour < 6 || endTime.Hour > 23 || (endTime.Hour == 23 && endTime.Minute > 0))
        {
            throw new InvalidOperationException("Horário de funcionamento: 06:00 às 23:00");
        }

        // 6. Reservas devem ser em intervalos de 1 hora
        if (startTime.Minute != 0 || startTime.Second != 0 || 
            endTime.Minute != 0 || endTime.Second != 0)
        {
            throw new InvalidOperationException("Reservas devem ser feitas em horários inteiros (ex: 14:00, 15:00)");
        }

        // 7. Limite de antecedência máxima de 30 dias
        var daysInAdvance = (startTime - DateTime.UtcNow).TotalDays;
        if (daysInAdvance > 30)
        {
            throw new InvalidOperationException("Reservas podem ser feitas com até 30 dias de antecedência");
        }
    }
}

