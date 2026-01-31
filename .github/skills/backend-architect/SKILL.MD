# Skill: Backend Architecture (.NET 8)

Este skill define las reglas de arquitectura, patrones y convenciones para el desarrollo backend con .NET 8.

---

## 1. ESTRUCTURA DEL PROYECTO

```
src/
├── Core/
│   ├── Core.Domain/           # Entidades, ValueObjects, Interfaces de repositorios
│   └── Core.Application/      # Features, Facades, DTOs, Validators, Result Pattern
├── Infrastructure/
│   ├── Infrastructure.Persistence/   # DbContext, Repositories, UnitOfWork, Migrations
│   └── Infrastructure.Services/      # Servicios externos (Email, Storage, Cache, JWT)
└── Presentation/
    └── API/                   # Controllers, Middleware, Filters, Extensions

tests/
├── Core.Application.Tests/           # Tests unitarios de Features, Facades, Validators
├── Infrastructure.Persistence.Tests/ # Tests de integración con BD
└── API.Tests/                        # Tests de integración de endpoints
```

---

## 2. PATRONES CLAVE

**Result Pattern:** `public async Task<Result<UserDto>> GetUserByIdAsync(int id) { var user = await _repo.GetByIdAsync(id); return user == null ? UserErrors.NotFound(id) : user.ToDto(); }`

**Repository + UoW:** `var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);` | Complejo: `await _unitOfWork.Orders.GetOrdersWithDetailsAsync(userId);`

**Facade:** Orquesta Features. `IUserFacade { Task<Result<UserDto>> GetUserByIdAsync(int id); Task<Result<UserDto>> CreateUserAsync(CreateUserDto dto); }`

**Features:** 1 caso de uso. `public class CreateUser { public async Task<Result<UserDto>> ExecuteAsync(CreateUserDto dto) { ... } }`

**CQRS (opcional):** `public record GetUserByIdQuery(int Id) : IRequest<Result<UserDto>>;`

**Validators:** `RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100); RuleFor(x => x.Email).EmailAddress();`

---

## 3. CONTROLLERS

Delegación a Facades. Mínima lógica.
```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    var result = await _userFacade.GetUserByIdAsync(id);
    return result.Match<IActionResult>(Ok, Problem);
}
```

---

## 4. DOMAIN

**Entidades:** Constructores privados + métodos factory.
```csharp
public class User : BaseEntity
{
    public string FirstName { get; private set; }
    private User() { }
    public static Result<User> Create(string firstName) { ... }
}
```

**Value Objects:** `public record Email { public static Result<Email> Create(string value) { ... } }`

**Entity Configuration:** `builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);`

---

## 5. INYECCIÓN DE DEPENDENCIAS

```csharp
// Application
services.AddScoped<IUserFacade, UserFacade>();
services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Persistence
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
services.AddScoped<IPasswordHasher, PasswordHasher>();
```

---

## 6. REGLAS OBLIGATORIAS

1. Result Pattern SIEMPRE (NO excepciones para flujo) | Repository + UoW
2. Facades orquestan Features (1 Feature = 1 caso de uso)
3. FluentValidation en DTOs | Controllers solo delegan
4. Entidades: constructores privados + factory methods
5. Entity Configurations separadas | CQRS opcional (MediatR)
6. async/await | Logging (Serilog) | Código en inglés

---

## 7. CHECKLIST

- [ ] Result Pattern | Repository + UoW | Facades | Features
- [ ] FluentValidation | Controllers delegan | Entity Configurations
- [ ] Tests unitarios + integración | Logging | Documentación

---

## 8. TECNOLOGÍAS

.NET 8 | EF Core 8 | FluentValidation | MediatR | xUnit | Serilog | SQL Server/PostgreSQL

