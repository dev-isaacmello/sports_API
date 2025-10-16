# 🏀 Sports Reservation API - Sistema de Reserva de Quadras Esportivas

API REST completa desenvolvida em C# .NET 9.0 para gerenciamento de reservas de quadras esportivas, implementando Clean Architecture, autenticação JWT e lógicas de negócio robustas.

## 🚀 Tecnologias Utilizadas

- **.NET 9.0** - Framework principal
- **Entity Framework Core 9.0** - ORM para acesso a dados
- **PostgreSQL** - Banco de dados relacional
- **AutoMapper 12.0** - Mapeamento automático de objetos
- **FluentValidation 12.0** - Validação de dados
- **JWT (JSON Web Tokens)** - Autenticação e autorização
- **BCrypt.Net** - Hash seguro de senhas
- **Swagger/OpenAPI** - Documentação interativa da API

## 📁 Arquitetura do Projeto

```
csharp_api/
├── Api/                           # Camada de apresentação
│   └── Controllers/
│       ├── AuthController.cs      # Autenticação (Register/Login)
│       ├── UserController.cs      # Gerenciamento de usuários (Admin)
│       ├── CourtController.cs     # Gerenciamento de quadras
│       └── ReservationController.cs # Gerenciamento de reservas
│
├── Application/                   # Camada de aplicação
│   ├── Dtos/
│   │   ├── Auth/                  # DTOs de autenticação
│   │   ├── Court/                 # DTOs de quadras
│   │   ├── Reservation/           # DTOs de reservas
│   │   └── UserDto.cs
│   ├── Interfaces/                # Contratos de serviços
│   ├── Services/                  # Lógica de negócio
│   │   ├── AuthService.cs
│   │   ├── TokenService.cs
│   │   ├── UserService.cs
│   │   ├── CourtService.cs
│   │   └── ReservationService.cs
│   └── Validation/                # Validadores FluentValidation
│
├── Domain/                        # Camada de domínio
│   ├── Entities/
│   │   ├── Users.cs               # Entidade de usuário
│   │   ├── Court.cs               # Entidade de quadra
│   │   └── Reservation.cs         # Entidade de reserva
│   ├── Enums/                     # Enumerações
│   ├── Interfaces/                # Contratos de repositórios
│   └── AutoMapper/                # Perfis de mapeamento
│
├── Infrastructure/                # Camada de infraestrutura
│   ├── Database/
│   │   └── AppDbContext.cs        # Contexto do EF Core
│   └── Repositories/              # Implementações de repositórios
│
└── Migrations/                    # Migrações do banco de dados
```

## 🛠️ Funcionalidades

### 🔐 Autenticação e Autorização
- ✅ Registro de usuários com validação robusta
- ✅ Login com JWT (Token válido por 8 horas)
- ✅ Autenticação baseada em roles (User/Admin)
- ✅ Hash seguro de senhas com BCrypt
- ✅ Validação de email único

### 👥 Gerenciamento de Usuários
- ✅ Listagem de usuários (Admin apenas)
- ✅ Busca por ID (Admin apenas)
- ✅ Remoção de usuários (Admin apenas)
- ✅ Controle de usuários ativos/inativos

### 🏟️ Gerenciamento de Quadras
- ✅ CRUD completo de quadras
- ✅ Filtros por tipo de esporte (Futebol, Vôlei, Basquete, etc.)
- ✅ Listagem de quadras ativas
- ✅ Informações de capacidade e cobertura
- ✅ Gerenciamento de preços por hora
- ✅ Ativação/desativação de quadras (Admin apenas)

### 📅 Sistema de Reservas com Lógicas Robustas
- ✅ Criação de reservas com validações automáticas
- ✅ Prevenção de conflitos de horário
- ✅ Cálculo automático de preços
- ✅ Atualização de reservas (com restrições)
- ✅ Cancelamento com regras de antecedência
- ✅ Histórico completo de reservas
- ✅ Filtros por usuário, quadra e período

### 🔒 Regras de Negócio Implementadas

#### Reservas
- **Horário de funcionamento**: 06:00 às 23:00
- **Duração**: Mínimo 1 hora, máximo 8 horas
- **Intervalos**: Apenas horários inteiros (ex: 14:00, 15:00)
- **Antecedência**: Até 30 dias para reservar
- **Cancelamento**: Mínimo 2 horas de antecedência
- **Conflitos**: Validação automática de sobreposição de horários
- **Status**: Pending, Confirmed, Cancelled, Completed
- **Restrições**: Não é possível alterar reserva que já começou
- **Permissões**: Usuários só podem gerenciar suas próprias reservas

#### Quadras
- **Preço**: Deve ser maior que zero e no máximo R$ 1.000/hora
- **Capacidade**: Deve ser maior que zero e no máximo 100 pessoas
- **Tipos**: Futebol, Futsal, Vôlei, Basquete, Tênis, Beach Tennis, Padel, HandBall, Squash
- **Status**: Apenas quadras ativas aceitam reservas

#### Usuários
- **Senha**: Mínimo 6 caracteres com letra maiúscula, minúscula e número
- **Email**: Validação de formato e unicidade
- **Telefone**: 10 ou 11 dígitos

## 🚀 Como Executar

### Pré-requisitos
- .NET 9.0 SDK
- PostgreSQL 12+
- Git

### Configuração

1. **Clone o repositório**
```bash
git clone <url-do-repositorio>
cd apiRest_csharp
```

2. **Configure a conexão com o banco de dados**

Edite o arquivo `csharp_api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=sports_reservation;Username=seu_usuario;Password=sua_senha"
  },
  "Jwt": {
    "Secret": "sua-chave-secreta-super-segura-com-pelo-menos-32-caracteres-aqui",
    "Issuer": "SportsReservationAPI",
    "Audience": "SportsReservationClient",
    "ExpirationHours": 8
  }
}
```

3. **Execute as migrações**
```bash
cd csharp_api
dotnet ef database update
```

4. **Execute o projeto**
```bash
dotnet run
```

5. **Acesse o Swagger**
```
https://localhost:5001/swagger
```

## 📡 Endpoints da API

### 🔐 Autenticação (`/api/Auth`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| POST | `/api/auth/register` | Registra novo usuário | Não |
| POST | `/api/auth/login` | Realiza login e retorna JWT | Não |

### 👥 Usuários (`/api/User`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/user` | Lista todos os usuários | Admin |
| GET | `/api/user/{id}` | Busca usuário por ID | Admin |
| DELETE | `/api/user/{id}` | Remove usuário | Admin |

### 🏟️ Quadras (`/api/Court`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/court` | Lista todas as quadras | Não |
| GET | `/api/court/active` | Lista quadras ativas | Não |
| GET | `/api/court/{id}` | Busca quadra por ID | Não |
| GET | `/api/court/type/{type}` | Busca quadras por tipo | Não |
| POST | `/api/court` | Cria nova quadra | Admin |
| PUT | `/api/court/{id}` | Atualiza quadra | Admin |
| DELETE | `/api/court/{id}` | Remove quadra | Admin |

### 📅 Reservas (`/api/Reservation`)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/reservation` | Lista todas as reservas | Admin |
| GET | `/api/reservation/{id}` | Busca reserva por ID | User/Admin |
| GET | `/api/reservation/my-reservations` | Lista reservas do usuário logado | User |
| GET | `/api/reservation/court/{courtId}` | Lista reservas por quadra | User |
| GET | `/api/reservation/date-range?startDate=&endDate=` | Lista reservas por período | User |
| POST | `/api/reservation` | Cria nova reserva | User |
| PUT | `/api/reservation/{id}` | Atualiza reserva | User (própria) |
| POST | `/api/reservation/{id}/cancel` | Cancela reserva | User (própria) |
| DELETE | `/api/reservation/{id}` | Remove reserva | User/Admin |

## 📝 Exemplos de Uso

### 1. Registrar Usuário
```json
POST /api/auth/register
{
  "nome": "João Silva",
  "email": "joao@example.com",
  "password": "Senha123",
  "telefone": "11987654321"
}
```

### 2. Login
```json
POST /api/auth/login
{
  "email": "joao@example.com",
  "password": "Senha123"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "email": "joao@example.com",
  "nome": "João Silva",
  "role": "User",
  "expiresAt": "2024-10-17T06:00:00Z"
}
```

### 3. Criar Quadra (Admin)
```json
POST /api/court
Authorization: Bearer {token}
{
  "name": "Quadra de Futebol 1",
  "description": "Quadra de grama sintética",
  "type": "Futebol",
  "pricePerHour": 150.00,
  "isCovered": true,
  "capacity": 22
}
```

### 4. Criar Reserva
```json
POST /api/reservation
Authorization: Bearer {token}
{
  "courtId": 1,
  "startTime": "2024-10-20T14:00:00Z",
  "endTime": "2024-10-20T16:00:00Z",
  "notes": "Reserva para time de futebol"
}
```

### 5. Cancelar Reserva
```json
POST /api/reservation/1/cancel
Authorization: Bearer {token}
{
  "cancellationReason": "Mudança de planos"
}
```

## 🔧 Padrões e Princípios Implementados

### Padrões de Design
- **Repository Pattern** - Abstração completa do acesso a dados
- **Dependency Injection** - Inversão de controle total
- **DTO Pattern** - Transferência segura de dados entre camadas
- **Clean Architecture** - Separação clara de responsabilidades
- **Primary Constructors** - Sintaxe moderna do C# 12
- **CQRS Light** - Separação de comandos e consultas nos serviços

### Princípios SOLID

#### S - Single Responsibility Principle
Cada classe tem uma única responsabilidade bem definida:
- `Controllers` - Apenas controlam requisições HTTP
- `Services` - Apenas lógica de negócio
- `Repositories` - Apenas acesso a dados
- `Validators` - Apenas validação de entrada

#### O - Open/Closed Principle
Sistema aberto para extensão, fechado para modificação:
- Interfaces permitem novas implementações sem modificar código existente
- Novos tipos de quadras podem ser adicionados sem alterar a estrutura

#### L - Liskov Substitution Principle
Objetos derivados são substituíveis:
- Qualquer implementação de `IReservationRepository` pode substituir a atual
- Polimorfismo aplicado em todas as camadas

#### I - Interface Segregation Principle
Interfaces específicas e focadas:
- `IAuthService`, `ICourtService`, `IReservationService` - interfaces dedicadas
- Nenhuma interface força implementação de métodos desnecessários

#### D - Dependency Inversion Principle
Dependência de abstrações, não implementações:
- Controllers dependem de `IService` (abstração)
- Services dependem de `IRepository` (abstração)
- Configuração via DI no `Program.cs`

## 🔐 Segurança

- ✅ Senhas hasheadas com BCrypt (salt automático)
- ✅ Tokens JWT com assinatura HMAC SHA256
- ✅ Validação de claims nos endpoints protegidos
- ✅ Autorização baseada em roles
- ✅ Validação de entrada em todas as requisições
- ✅ Proteção contra SQL Injection (EF Core)
- ✅ HTTPS configurável

## 📊 Modelo de Dados

### Users
- `Id` (int) - Chave primária
- `Nome` (string) - Nome completo
- `Email` (string) - Email único
- `Telefone` (string) - Telefone de contato
- `PasswordHash` (string) - Senha hasheada
- `Role` (string) - Papel (User/Admin)
- `CreatedAt` (DateTime) - Data de criação
- `IsActive` (bool) - Status do usuário

### Court
- `Id` (int) - Chave primária
- `Name` (string) - Nome da quadra
- `Description` (string) - Descrição
- `Type` (string) - Tipo de esporte
- `PricePerHour` (decimal) - Preço por hora
- `IsActive` (bool) - Status ativo
- `IsCovered` (bool) - Coberta ou descoberta
- `Capacity` (int) - Capacidade máxima
- `CreatedAt` (DateTime) - Data de criação

### Reservation
- `Id` (int) - Chave primária
- `UserId` (int) - FK para Users
- `CourtId` (int) - FK para Court
- `StartTime` (DateTime) - Início da reserva
- `EndTime` (DateTime) - Fim da reserva
- `TotalPrice` (decimal) - Preço total calculado
- `Status` (string) - Status da reserva
- `Notes` (string) - Observações
- `CreatedAt` (DateTime) - Data de criação
- `CancelledAt` (DateTime?) - Data de cancelamento
- `CancellationReason` (string?) - Motivo do cancelamento

## 🧪 Testando a API

### Com Swagger
1. Acesse `https://localhost:5001/swagger`
2. Execute `/api/auth/register` para criar um usuário
3. Execute `/api/auth/login` para obter o token
4. Clique em "Authorize" e insira: `Bearer {seu-token}`
5. Teste os endpoints protegidos

### Com Postman/Insomnia
1. Importe a collection (disponível em breve)
2. Configure a variável de ambiente `{{token}}`
3. Execute os endpoints conforme documentação

## 📈 Próximas Melhorias

- [ ] Implementar Refresh Tokens
- [ ] Adicionar paginação nas listagens
- [ ] Implementar sistema de notificações
- [ ] Adicionar upload de imagens para quadras
- [ ] Implementar dashboard de estatísticas
- [ ] Adicionar sistema de avaliações
- [ ] Implementar pagamentos online
- [ ] Adicionar testes unitários e de integração
- [ ] Implementar cache com Redis
- [ ] Adicionar logging estruturado

## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor:
1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

---

**Desenvolvido por [Isaac Mello](https://www.linkedin.com/in/isaac-mello-168404281/)**

**Versão:** 2.0.0 - Sports Reservation System  
**Data de Atualização:** Outubro 2024
