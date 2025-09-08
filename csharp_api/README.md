# C# API - Sistema de Gerenciamento de Usuários

API REST desenvolvida em C# .NET 9.0 para estudo, implementando arquitetura limpa (Clean Architecture) com padrões modernos de desenvolvimento.

## 🚀 Tecnologias Utilizadas

- **.NET 9.0** - Framework principal
- **Entity Framework Core 9.0** - ORM para acesso a dados
- **PostgreSQL** - Banco de dados
- **AutoMapper 14.0** - Mapeamento de objetos
- **FluentValidation 12.0** - Validação de dados
- **Swagger/OpenAPI** - Documentação da API

## 📁 Arquitetura do Projeto

```
csharp_api/
├── Api/                    # Camada de apresentação (Controllers)
├── Application/            # Camada de aplicação (Services, DTOs, Validation)
├── Domain/                 # Camada de domínio (Entities, Interfaces, AutoMapper)
├── Infrastructure/         # Camada de infraestrutura (Database, Repositories)
└── Migrations/            # Migrações do banco de dados
```

## 🛠️ Funcionalidades

- ✅ CRUD completo de usuários
- ✅ Validação de dados com FluentValidation
- ✅ Mapeamento automático com AutoMapper
- ✅ Documentação automática com Swagger
- ✅ Arquitetura em camadas (Clean Architecture)
- ✅ Injeção de dependência
- ✅ Primary Constructors (C# 12)

## 🚀 Como Executar

### Pré-requisitos
- .NET 9.0 SDK
- PostgreSQL

### Configuração
1. Clone o repositório
2. Configure a string de conexão no `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=csharp_api;Username=seu_usuario;Password=sua_senha"
  }
}
```

3. Execute as migrações:
```bash
dotnet ef database update
```

4. Execute o projeto:
```bash
dotnet run
```

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/user` | Lista todos os usuários |
| GET | `/api/user/{id}` | Busca usuário por ID |
| POST | `/api/user` | Cria novo usuário |
| PUT | `/api/user/{id}` | Atualiza usuário |
| DELETE | `/api/user/{id}` | Remove usuário |

## 📝 Modelo de Dados

### User
- `Id` (int) - Chave primária
- `Nome` (string) - Nome do usuário (máx. 30 caracteres)
- `Telefone` (string) - Telefone do usuário (máx. 15 caracteres)

## 🔧 Padrões e Princípios Implementados

### Padrões de Design
- **Repository Pattern** - Abstração do acesso a dados
- **Dependency Injection** - Inversão de controle
- **DTO Pattern** - Transferência de dados entre camadas
- **Clean Architecture** - Separação clara de responsabilidades
- **Primary Constructors** - Sintaxe moderna do C# 12

### Princípios SOLID
- **S - Single Responsibility Principle** - Cada classe tem uma única responsabilidade
  - `UserController` - Apenas controla requisições HTTP
  - `UserService` - Apenas lógica de negócio
  - `UserRepository` - Apenas acesso a dados
  - `UserDtoValidator` - Apenas validação de dados

- **O - Open/Closed Principle** - Aberto para extensão, fechado para modificação
  - Interfaces permitem extensão sem modificar código existente
  - `IUserService` e `IUserRepository` podem ser implementados de formas diferentes

- **L - Liskov Substitution Principle** - Objetos derivados devem ser substituíveis por objetos base
  - `UserRepository` pode ser substituído por qualquer implementação de `IUserRepository`
  - `UserService` pode ser substituído por qualquer implementação de `IUserService`

- **I - Interface Segregation Principle** - Interfaces específicas são melhores que interfaces genéricas
  - `IUserService` e `IUserRepository` são interfaces específicas e focadas
  - Cada interface contém apenas os métodos necessários para sua responsabilidade

- **D - Dependency Inversion Principle** - Dependa de abstrações, não de implementações concretas
  - `UserController` depende de `IUserService` (abstração)
  - `UserService` depende de `IUserRepository` (abstração)
  - Injeção de dependência no `Program.cs` configura as abstrações

---

**Desenvolvido por [Isaac Mello](https://www.linkedin.com/in/isaac-mello-168404281/)**
