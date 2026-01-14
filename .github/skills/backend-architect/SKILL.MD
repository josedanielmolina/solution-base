# Skill: Backend Architecture (.NET 8)

## Descripción
Este skill define las reglas de arquitectura, patrones y convenciones para el desarrollo backend con .NET 8 que TODO el código nuevo debe seguir.

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

## 2. PATRONES OBLIGATORIOS

### 2.1 Result Pattern
**SIEMPRE usar Result<T> para operaciones que pueden fallar.**

```csharp
// ✅ CORRECTO
public async Task<Result<UserDto>> GetUserByIdAsync(int id)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null)
        return UserErrors.NotFound(id);
    
    return user.ToDto();
}

// ❌ INCORRECTO - No usar excepciones para flujo de control
public async Task<UserDto> GetUserByIdAsync(int id)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null)
        throw new NotFoundException($"User {id} not found");
    
    return user.ToDto();
}
```

### 2.2 Repository Pattern + Unit of Work
**Usar repositorio genérico para operaciones simples, especializado para complejas.**

```csharp
// ✅ CORRECTO - Operación simple con genérico
var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);

// ✅ CORRECTO - Consulta simple con FindAsync
var users = await _unitOfWork.Repository<User>()
    .FindAsync(u => u.Email == email);

// ✅ CORRECTO - Operación compleja con repositorio especializado
var orders = await _unitOfWork.Orders.GetOrdersWithDetailsAsync(userId, page, 10);
```

### 2.3 Facade Pattern
**Las operaciones se agrupan en Facades que orquestan múltiples operaciones.**

```csharp
// Estructura de Facade
public interface IUserFacade
{
    Task<Result<UserDto>> GetUserByIdAsync(int id);
    Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync();
    Task<Result<UserDto>> CreateUserAsync(CreateUserDto dto);
    Task<Result<UserDto>> UpdateUserAsync(int id, UpdateUserDto dto);
    Task<Result> DeleteUserAsync(int id);
}
```

### 2.4 Features Pattern
**Cada caso de uso es una clase independiente nombrada simplemente por lo que hace.**

```csharp
// Estructura de Feature (caso de uso)
public class CreateUser
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUser(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<UserDto>> ExecuteAsync(CreateUserDto dto)
    {
        // Validaciones
        // Lógica de negocio
        // Retorno con Result
    }
}
```

### 2.5 Mapeo Manual (SIN AutoMapper)
**Usar extension methods para mapeo entre entidades y DTOs.**

```csharp
// ✅ CORRECTO - Extension methods
public static class UserMappingExtensions
{
    // Entity → DTO
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }

    // DTO → Entity (creación)
    public static User ToEntity(this CreateUserDto dto, string passwordHash)
    {
        return new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = passwordHash,
            IsActive = true
        };
    }

    // DTO → Entity (actualización in-place)
    public static void MapTo(this UpdateUserDto dto, User entity)
    {
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.Email = dto.Email;
    }

    // Collections
    public static IEnumerable<UserDto> ToDtoList(this IEnumerable<User> users)
    {
        return users.Select(u => u.ToDto());
    }
}
```

### 2.6 FluentValidation
**Validar DTOs con FluentValidation.**

```csharp
public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches("[A-Z]").WithMessage("Password must contain uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain a number");
    }
}
```

### 2.7 Errores Tipados
**Definir errores específicos por dominio.**

```csharp
public static class UserErrors
{
    public static Error NotFound(int userId) => Error.NotFound(
        "User.NotFound",
        $"User with ID {userId} was not found");

    public static Error EmailAlreadyExists(string email) => Error.Conflict(
        "User.EmailAlreadyExists",
        $"Email {email} is already registered");

    public static Error InvalidCredentials => Error.Unauthorized(
        "User.InvalidCredentials",
        "Invalid email or password");
}
```

### 2.8 DTOs como Records
**Usar records para DTOs para garantizar inmutabilidad y sintaxis concisa.**

```csharp
// ✅ CORRECTO - Records para DTOs
public record UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    DateTime CreatedAt
);

public record CreateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string? PhoneNumber
);

public record UpdateUserDto(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber
);

// ❌ INCORRECTO - No usar clases para DTOs
public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
```

### 2.9 Encapsulación de Librerías de Terceros
**Toda librería de terceros debe encapsularse en un servicio.**

```csharp
// ✅ CORRECTO - Encapsular BCrypt en un servicio
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        // Encapsula BCrypt.Net
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}

// ✅ CORRECTO - Uso en Features a través de la interfaz
public class CreateUser
{
    private readonly IPasswordHasher _passwordHasher;
    
    public async Task<Result<UserDto>> ExecuteAsync(CreateUserDto dto)
    {
        var passwordHash = _passwordHasher.HashPassword(dto.Password);
        // ...
    }
}

// ❌ INCORRECTO - Usar librería directamente en la lógica de negocio
public class CreateUser
{
    public async Task<Result<UserDto>> ExecuteAsync(CreateUserDto dto)
    {
        // NO usar BCrypt directamente
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
    }
}
```

**Beneficios de la encapsulación:**
- Facilita cambiar de librería sin afectar la lógica de negocio
- Permite testing fácil con mocks
- Mantiene el código desacoplado
- Centraliza el uso de dependencias externas

---

## 3. PATRONES DE DESARROLLO API

### 3.1 Conversión Result a ActionResult
**Usar extension methods para convertir Result a ActionResult HTTP.**

```csharp
// ✅ CORRECTO - Extension method para Result<T>
public static IActionResult ToActionResult<T>(this Result<T> result)
{
    if (result.IsSuccess)
    {
        return new OkObjectResult(result.Value);
    }

    return result.Error!.Type switch
    {
        ErrorType.NotFound => new NotFoundObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
        ErrorType.Validation => new BadRequestObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
        ErrorType.Conflict => new ConflictObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
        ErrorType.Unauthorized => new UnauthorizedObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
        ErrorType.Forbidden => new ObjectResult(new { error = result.Error.Code, message = result.Error.Message }) { StatusCode = 403 },
        _ => new ObjectResult(new { error = result.Error.Code, message = result.Error.Message }) { StatusCode = 500 }
    };
}

// ✅ USO EN CONTROLLER
public class UsersController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userFacade.GetUserByIdAsync(id);
        return result.ToActionResult(); // Convierte Result a HTTP response
    }
}
```

### 3.2 Custom Middleware para Excepciones
**Usar middleware personalizado para capturar excepciones no controladas.**

```csharp
// ✅ CORRECTO - Exception Middleware
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            error = "Server.Error",
            message = _env.IsDevelopment() ? exception.Message : "An internal server error occurred",
            details = _env.IsDevelopment() ? exception.StackTrace : null
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}

// ✅ REGISTRAR EN Program.cs
app.UseMiddleware<ExceptionMiddleware>();
```

### 3.3 Logging Middleware
**Usar middleware para loguear todas las peticiones HTTP.**

```csharp
// ✅ CORRECTO - Logging Middleware
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        
        _logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);
        
        await _next(context);
        
        var duration = DateTime.UtcNow - startTime;
        _logger.LogInformation(
            "Response: {Method} {Path} - {StatusCode} ({DurationMs}ms)", 
            context.Request.Method, 
            context.Request.Path,
            context.Response.StatusCode,
            duration.TotalMilliseconds);
    }
}

// ✅ REGISTRAR EN Program.cs - ANTES de otros middleware
app.UseMiddleware<LoggingMiddleware>();
```

### 3.4 Controllers Sin Lógica de Negocio
**Los Controllers solo coordinan: validación, llamada a Facade, conversión de Response.**

```csharp
// ✅ CORRECTO - Controller ligero
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserFacade _userFacade;
    private readonly IValidator<CreateUserDto> _createValidator;

    public UsersController(IUserFacade userFacade, IValidator<CreateUserDto> createValidator)
    {
        _userFacade = userFacade;
        _createValidator = createValidator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        // 1. Validación
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        // 2. Llamar Facade
        var result = await _userFacade.CreateUserAsync(dto);
        
        // 3. Retornar respuesta
        return result.ToActionResult();
    }
}

// ❌ INCORRECTO - Lógica de negocio en Controller
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
{
    var user = new User { Email = dto.Email };
    var passwordHash = BCrypt.HashPassword(dto.Password); // NO AQUÍ
    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();
    return Ok(user);
}
```

### 3.5 Documentación XML en Controllers
**Documentar endpoints con comentarios XML para Swagger.**

```csharp
/// <summary>
/// Get all users
/// </summary>
/// <returns>List of users</returns>
[HttpGet]
public async Task<IActionResult> GetAll()
{
    var result = await _userFacade.GetAllUsersAsync();
    return result.ToActionResult();
}

/// <summary>
/// Get user by id
/// </summary>
/// <param name="id">User ID</param>
/// <returns>User details</returns>
[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    var result = await _userFacade.GetUserByIdAsync(id);
    return result.ToActionResult();
}

/// <summary>
/// Create a new user
/// </summary>
/// <param name="dto">User creation data</param>
/// <returns>Created user</returns>
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
{
    // ...
}
```

---

## 3.6 DOCUMENTACIÓN DE CÓDIGO OBLIGATORIA

**⚠️ REGLA FUNDAMENTAL**: Todo método público, clase, propiedad e interfaz DEBE estar documentado con XML comments siguiendo las [Guías Oficiales de .NET](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/).

### Elementos XML Obligatorios

| Elemento | Cuándo Usar | Obligatorio |
|----------|-------------|-------------|
| `<summary>` | Todas las clases, métodos, propiedades | ✅ SÍ |
| `<param>` | Cada parámetro de método | ✅ SÍ |
| `<returns>` | Métodos que retornan valor | ✅ SÍ |
| `<exception>` | Cuando se lanzan excepciones | ✅ SÍ |
| `<remarks>` | Explicación adicional compleja | 📝 Opcional |
| `<example>` | Ejemplos de uso | 📝 Opcional |
| `<seealso>` | Referencias cruzadas | 📝 Opcional |

### Clases y Interfaces

```csharp
/// <summary>
/// Repositorio para operaciones de datos de usuarios en la base de datos.
/// Proporciona acceso especializado para consultas complejas relacionadas con usuarios.
/// </summary>
/// <remarks>
/// Este repositorio hereda de <see cref="GenericRepository{User}"/> y extiende
/// funcionalidades específicas para el dominio de usuarios.
/// </remarks>
public interface IUserRepository : IGenericRepository<User>
{
    /// <summary>
    /// Obtiene un usuario por su dirección de correo electrónico.
    /// </summary>
    /// <param name="email">Correo electrónico del usuario a buscar.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// El resultado contiene el usuario encontrado o <c>null</c> si no existe.
    /// </returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Obtiene una lista paginada de usuarios activos.
    /// </summary>
    /// <param name="pageNumber">Número de página (base 1).</param>
    /// <param name="pageSize">Cantidad de registros por página.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// El resultado contiene la lista paginada de usuarios activos.
    /// </returns>
    Task<PaginatedList<User>> GetActiveUsersAsync(int pageNumber, int pageSize);
}
```

### Facades

```csharp
/// <summary>
/// Fachada para operaciones de gestión de usuarios.
/// Orquesta casos de uso y coordina múltiples servicios relacionados con usuarios.
/// </summary>
public interface IUserFacade
{
    /// <summary>
    /// Obtiene un usuario por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único del usuario.</param>
    /// <returns>
    /// Un <see cref="Result{UserDto}"/> que contiene el DTO del usuario si existe,
    /// o un error <see cref="UserErrors.NotFound"/> si no se encuentra.
    /// </returns>
    Task<Result<UserDto>> GetUserByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="dto">Datos del usuario a crear.</param>
    /// <returns>
    /// Un <see cref="Result{UserDto}"/> que contiene el usuario creado si la operación es exitosa,
    /// o un error de validación/duplicidad si falla.
    /// </returns>
    /// <remarks>
    /// La contraseña será hasheada automáticamente antes de ser almacenada.
    /// Se validará que el email no exista previamente en el sistema.
    /// </remarks>
    Task<Result<UserDto>> CreateUserAsync(CreateUserDto dto);

    /// <summary>
    /// Actualiza los datos de un usuario existente.
    /// </summary>
    /// <param name="id">Identificador del usuario a actualizar.</param>
    /// <param name="dto">Nuevos datos del usuario.</param>
    /// <returns>
    /// Un <see cref="Result{UserDto}"/> que contiene el usuario actualizado si la operación es exitosa,
    /// o un error si el usuario no existe o falla la validación.
    /// </returns>
    Task<Result<UserDto>> UpdateUserAsync(int id, UpdateUserDto dto);

    /// <summary>
    /// Elimina un usuario del sistema (soft delete).
    /// </summary>
    /// <param name="id">Identificador del usuario a eliminar.</param>
    /// <returns>
    /// Un <see cref="Result"/> que indica si la operación fue exitosa o contiene un error.
    /// </returns>
    /// <remarks>
    /// Realiza un borrado lógico, marcando el usuario como inactivo sin eliminarlo físicamente.
    /// </remarks>
    Task<Result> DeleteUserAsync(int id);
}
```

### Features (Casos de Uso)

```csharp
/// <summary>
/// Caso de uso para crear un nuevo usuario en el sistema.
/// </summary>
/// <remarks>
/// Este caso de uso valida los datos, verifica que el email no exista,
/// hashea la contraseña y persiste el usuario en la base de datos.
/// </remarks>
public class CreateUser
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="CreateUser"/>.
    /// </summary>
    /// <param name="unitOfWork">Unidad de trabajo para operaciones de base de datos.</param>
    /// <param name="passwordHasher">Servicio para hashear contraseñas.</param>
    public CreateUser(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Ejecuta el caso de uso de creación de usuario.
    /// </summary>
    /// <param name="dto">Datos del usuario a crear.</param>
    /// <returns>
    /// Un <see cref="Result{UserDto}"/> con el usuario creado si la operación es exitosa,
    /// o un error si falla la validación o el email ya existe.
    /// </returns>
    public async Task<Result<UserDto>> ExecuteAsync(CreateUserDto dto)
    {
        // Validar email único
        var existingUser = await _unitOfWork.Repository<User>()
            .FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        if (existingUser != null)
            return UserErrors.EmailAlreadyExists(dto.Email);

        // Hashear contraseña
        var passwordHash = _passwordHasher.Hash(dto.Password);

        // Crear entidad
        var user = dto.ToEntity(passwordHash);

        // Persistir
        await _unitOfWork.Repository<User>().AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return user.ToDto();
    }
}
```

### Servicios de Infraestructura

```csharp
/// <summary>
/// Servicio para hashear y verificar contraseñas usando BCrypt.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Genera un hash seguro a partir de una contraseña en texto plano.
    /// </summary>
    /// <param name="password">Contraseña en texto plano.</param>
    /// <returns>Hash de la contraseña usando BCrypt.</returns>
    string Hash(string password);

    /// <summary>
    /// Verifica si una contraseña en texto plano coincide con un hash.
    /// </summary>
    /// <param name="password">Contraseña en texto plano a verificar.</param>
    /// <param name="hash">Hash de contraseña almacenado.</param>
    /// <returns><c>true</c> si la contraseña coincide; de lo contrario, <c>false</c>.</returns>
    bool Verify(string password, string hash);
}

/// <summary>
/// Implementación de <see cref="IPasswordHasher"/> usando BCrypt.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    /// <inheritdoc/>
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <inheritdoc/>
    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
```

### DTOs y Modelos

```csharp
/// <summary>
/// DTO para la creación de un nuevo usuario.
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Dirección de correo electrónico única del usuario.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Contraseña del usuario en texto plano.
    /// Será hasheada antes de ser almacenada.
    /// </summary>
    public required string Password { get; set; }
}

/// <summary>
/// DTO de usuario para respuestas de API.
/// No incluye información sensible como contraseñas.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Identificador único del usuario.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre completo del usuario.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del usuario.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el usuario está activo en el sistema.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Fecha de creación del usuario.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
```

### Entidades de Dominio

```csharp
/// <summary>
/// Entidad de usuario del sistema.
/// Representa un usuario registrado con sus credenciales y datos básicos.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Dirección de correo electrónico única del usuario.
    /// Utilizada para autenticación.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hash de la contraseña del usuario.
    /// Nunca almacenar contraseñas en texto plano.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el usuario está activo en el sistema.
    /// Los usuarios inactivos no pueden autenticarse.
    /// </summary>
    public bool IsActive { get; set; }
}
```

### Controllers

```csharp
/// <summary>
/// Controlador para la gestión de usuarios.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserFacade _userFacade;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="UsersController"/>.
    /// </summary>
    /// <param name="userFacade">Fachada de usuarios.</param>
    public UsersController(IUserFacade userFacade)
    {
        _userFacade = userFacade;
    }

    /// <summary>
    /// Obtiene todos los usuarios del sistema.
    /// </summary>
    /// <returns>Lista de usuarios.</returns>
    /// <response code="200">Retorna la lista de usuarios.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userFacade.GetAllUsersAsync();
        return result.ToActionResult();
    }

    /// <summary>
    /// Obtiene un usuario por su ID.
    /// </summary>
    /// <param name="id">Identificador único del usuario.</param>
    /// <returns>Detalles del usuario.</returns>
    /// <response code="200">Usuario encontrado.</response>
    /// <response code="404">Usuario no encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userFacade.GetUserByIdAsync(id);
        return result.ToActionResult();
    }

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    /// <param name="dto">Datos del usuario a crear.</param>
    /// <returns>Usuario creado.</returns>
    /// <response code="201">Usuario creado exitosamente.</response>
    /// <response code="400">Datos de entrada inválidos.</response>
    /// <response code="409">El email ya está registrado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var result = await _userFacade.CreateUserAsync(dto);
        return result.ToActionResult();
    }
}
```

### Reglas de Documentación

✅ **OBLIGATORIO:**
- Documentar TODAS las clases públicas
- Documentar TODOS los métodos públicos
- Documentar TODAS las propiedades públicas
- Documentar TODOS los parámetros con `<param>`
- Documentar TODOS los valores de retorno con `<returns>`
- Usar `<remarks>` para lógica compleja o consideraciones especiales
- Incluir `<exception>` para todas las excepciones que se lanzan

📝 **RECOMENDADO:**
- Incluir `<example>` para APIs complejas
- Usar `<seealso>` para referencias cruzadas
- Documentar el propósito, no el "cómo" (el código debe ser auto-documentado)
- Mantener la documentación actualizada con cambios de código

❌ **EVITAR:**
- Documentación genérica sin valor ("Gets or sets the value")
- Documentación que repite el nombre del método
- Comentarios obvios que no agregan información
- Documentación desactualizada

### Habilitando Generación de XML Documentation

En cada proyecto `.csproj`:

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn> <!-- Suprimir warnings de documentación faltante en desarrollo -->
</PropertyGroup>
```

---

## 4. CONVENCIONES DE NOMBRADO

| Tipo | Convención | Ejemplo |
|------|------------|---------|
| Entidad | PascalCase, singular | `User`, `Order`, `Product` |
| DTO | PascalCase + Dto/Request/Response | `UserDto`, `CreateUserDto`, `UpdateUserDto` |
| Interface | I + PascalCase | `IUserRepository`, `IUserFacade`, `IJwtTokenGenerator` |
| Repository | PascalCase + Repository | `UserRepository`, `OrderRepository` |
| Operation | Verbo + Entity + Operation | `CreateUserOperation`, `GetUserByIdOperation` |
| Facade | Entity + Facade | `UserFacade`, `AuthFacade` |
| Errors | Entity + Errors | `UserErrors`, `AuthErrors` |
| Mapping | Entity + MappingExtensions | `UserMappingExtensions` |
| Validator | Dto + Validator | `CreateUserValidator`, `LoginValidator` |
| Service | PascalCase | `PasswordHasher`, `JwtTokenGenerator` |
| Middleware | PascalCase + Middleware | `ExceptionMiddleware`, `LoggingMiddleware` |
| Filter | PascalCase + Attribute | `ValidateModelAttribute` |
| Extension | PascalCase + Extensions | `ResultToActionResultExtensions` |

---

## 5. ESTRUCTURA DE ARCHIVOS POR FEATURE

```
Core.Domain/
├── Entities/
│   └── [Entity].cs
└── Interfaces/
    └── Repositories/
        └── I[Entity]Repository.cs

Core.Application/
├── DTOs/
│   └── [Entity]s/
│       ├── [Entity]Dto.cs
│       ├── Create[Entity]Dto.cs
│       └── Update[Entity]Dto.cs
├── Features/
│   └── [Entity]s/
│       ├── Create[Entity].cs
│       ├── Get[Entity].cs
│       ├── Update[Entity].cs
│       └── Delete[Entity].cs
├── Facades/
│   ├── I[Entity]Facade.cs
│   └── [Entity]Facade.cs
├── Mappings/
│   └── [Entity]MappingExtensions.cs
├── Validators/
│   ├── Create[Entity]Validator.cs
│   └── Update[Entity]Validator.cs
└── Common/
    └── Errors/
        └── [Entity]Errors.cs

Infrastructure.Persistence/
├── Configurations/
│   └── [Entity]Configuration.cs
└── Repositories/
    └── [Entity]Repository.cs

```

---

## 6. REGLAS DE CÓDIGO

1. **NO usar excepciones para flujo de control** - Usar Result Pattern
2. **NO instalar librerías de terceros sin autorización**
3. **NO usar AutoMapper** - Usar extension methods manuales
4. **SIEMPRE usar async/await** para operaciones I/O
5. **SIEMPRE inyectar dependencias por constructor**
6. **SIEMPRE validar con FluentValidation**
7. **SIEMPRE usar IUnitOfWork para transacciones**
8. **SIEMPRE usar JWT Bearer para autenticación**
9. **SIEMPRE configurar CORS para Angular**
10. **SIEMPRE encapsular librerías de terceros en servicios** - Facilita cambios futuros
11. **SIEMPRE usar records para DTOs** - Inmutabilidad y sintaxis concisa
12. **TODO el código en inglés**

---

## 7. CONFIGURACIONES REQUERIDAS

### JWT Authentication
```csharp
// appsettings.json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "SolutionBaseAPI",
    "Audience": "SolutionBaseClient",
    "ExpirationMinutes": 60
  }
}

// Program.cs
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is not configured");

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});
```

### CORS Configuration
```csharp
// Program.cs
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngular", policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Orden del middleware (IMPORTANTE)
app.UseCors("AllowAngular");      // PRIMERO
app.UseAuthentication();           // SEGUNDO
app.UseAuthorization();            // TERCERO
```

---

## 8. GESTIÓN DE BASE DE DATOS Y MIGRACIONES

### 8.1 Entity Framework Core Migrations

**TODAS las modificaciones de base de datos deben hacerse mediante migraciones de EF Core.**

#### ✅ Proceso Obligatorio

```
1. Modificar/Crear entidad → 2. Agregar/Actualizar Configuration → 3. Crear Migración → 4. Aplicar Migración
```

#### Ubicación de Migraciones

```
Infrastructure.Persistence/
├── Migrations/
│   ├── 20260110140914_InitialCreate.cs
│   ├── 20260110140914_InitialCreate.Designer.cs
│   ├── 20260111153000_AddPhoneNumberToUser.cs
│   └── ApplicationDbContextModelSnapshot.cs
```

---

### 8.2 Crear Migración

#### Cuando Crear una Migración

✅ **SIEMPRE crear migración cuando:**
- Agregas una nueva entidad
- Modificas propiedades de una entidad existente
- Agregas/eliminas relaciones entre entidades
- Cambias configuraciones de EF Core (índices, constraints, etc.)
- Modificas nombres de tablas o columnas

#### Comando para Crear Migración

```bash
# Desde la raíz del proyecto
dotnet ef migrations add [NombreMigration] --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API

# Ejemplo: Agregar campo PhoneNumber a User
dotnet ef migrations add AddPhoneNumberToUser --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API
```

#### Convención de Nombres para Migraciones

| Tipo de Cambio | Convención | Ejemplo |
|----------------|------------|---------|
| Nueva tabla | Add[Entity]Table | `AddProductTable` |
| Nuevo campo | Add[Field]To[Entity] | `AddPhoneNumberToUser` |
| Eliminar campo | Remove[Field]From[Entity] | `RemoveMiddleNameFromUser` |
| Modificar campo | Update[Field]In[Entity] | `UpdateEmailMaxLengthInUser` |
| Nuevo índice | AddIndexOn[Entity][Field] | `AddIndexOnUserEmail` |
| Nueva relación | Add[Entity1]To[Entity2]Relationship | `AddUserToOrderRelationship` |
| Migración inicial | InitialCreate | `InitialCreate` |

---

### 8.3 Aplicar Migración

#### En Desarrollo

```bash
# Aplicar todas las migraciones pendientes
dotnet ef database update --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API

# Aplicar hasta una migración específica
dotnet ef database update [NombreMigration] --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API

# Ver migraciones pendientes
dotnet ef migrations list --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API
```

#### En Producción

**⚠️ IMPORTANTE**: Las migraciones se aplican automáticamente al iniciar la aplicación mediante:

```csharp
// Program.cs (ya configurado)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync(); // Aplica migraciones pendientes
}
```

---

### 8.4 Revertir Migración

```bash
# Revertir a la migración anterior
dotnet ef database update [NombreMigracionAnterior] --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API

# Revertir completamente (borrar todas las tablas)
dotnet ef database update 0 --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API

# Eliminar la última migración (si no se ha aplicado)
dotnet ef migrations remove --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API
```

---

### 8.5 Entity Configuration (IEntityTypeConfiguration)

**TODA entidad DEBE tener su configuración de EF Core.**

```csharp
// ✅ CORRECTO - Configuración separada
// Infrastructure.Persistence/Configurations/UserConfiguration.cs
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Nombre de tabla
        builder.ToTable("Users");

        // Clave primaria
        builder.HasKey(u => u.Id);

        // Propiedades
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        // Índices
        builder.HasIndex(u => u.Email)
            .IsUnique();

        // Valores por defecto
        builder.Property(u => u.IsActive)
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}

// ✅ Registrar en DbContext
public class ApplicationDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplicar todas las configuraciones del assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

// ❌ INCORRECTO - Configuración en DbContext con Fluent API
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>(entity =>
    {
        entity.ToTable("Users");
        entity.HasKey(u => u.Id);
        // NO HACER ESTO - Usar IEntityTypeConfiguration
    });
}
```

---

### 8.6 Workflow Completo - Agregar Nueva Entidad

#### Paso 1: Crear Entidad
```csharp
// Core.Domain/Entities/Product.cs
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
```

#### Paso 2: Crear Configuration
```csharp
// Infrastructure.Persistence/Configurations/ProductConfiguration.cs
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Property(p => p.Stock)
            .IsRequired();
    }
}
```

#### Paso 3: Agregar DbSet en Context
```csharp
// Infrastructure.Persistence/Context/ApplicationDbContext.cs
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!; // NUEVO
}
```

#### Paso 4: Crear Migración
```bash
dotnet ef migrations add AddProductTable --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API
```

#### Paso 5: Revisar Migración Generada
```csharp
// Migrations/[timestamp]_AddProductTable.cs
public partial class AddProductTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Products",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(maxLength: 200, nullable: false),
                Description = table.Column<string>(maxLength: 1000, nullable: true),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Stock = table.Column<int>(nullable: false),
                CreatedAt = table.Column<DateTime>(nullable: false),
                UpdatedAt = table.Column<DateTime>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Products");
    }
}
```

#### Paso 6: Aplicar Migración
```bash
dotnet ef database update --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API
```

---

### 8.7 Modificar Entidad Existente

#### Ejemplo: Agregar campo PhoneNumber a User

**Paso 1**: Modificar entidad
```csharp
// Core.Domain/Entities/User.cs
public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } // NUEVO CAMPO
    public bool IsActive { get; set; } = true;
}
```

**Paso 2**: Actualizar Configuration
```csharp
// Infrastructure.Persistence/Configurations/UserConfiguration.cs
public void Configure(EntityTypeBuilder<User> builder)
{
    // ... configuración existente ...

    builder.Property(u => u.PhoneNumber) // NUEVO
        .HasMaxLength(20);
}
```

**Paso 3**: Crear migración
```bash
dotnet ef migrations add AddPhoneNumberToUser --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API
```

**Paso 4**: Aplicar migración
```bash
dotnet ef database update --project src/Infrastructure/Infrastructure.Persistence --startup-project src/Presentation/API
```

---

### 8.8 Mejores Prácticas de Migraciones

#### ✅ DO (Hacer)

- ✅ Crear migraciones pequeñas y atómicas
- ✅ Usar nombres descriptivos para migraciones
- ✅ Revisar el código generado de la migración antes de aplicar
- ✅ Probar la migración en desarrollo antes de producción
- ✅ Incluir migraciones en control de versiones (Git)
- ✅ Usar `IEntityTypeConfiguration` para toda configuración
- ✅ Aplicar migraciones automáticamente en startup (producción)
- ✅ Mantener migraciones lineales (sin branches divergentes)

#### ❌ DON'T (No Hacer)

- ❌ Modificar migraciones ya aplicadas en producción
- ❌ Hacer cambios directos en la BD sin migración
- ❌ Mezclar múltiples cambios no relacionados en una migración
- ❌ Eliminar migraciones del historial
- ❌ Usar SQL raw sin necesidad extrema
- ❌ Configurar entidades con Data Annotations en el dominio
- ❌ Olvidar hacer backup antes de migrar en producción

---

### 8.9 Scripts SQL Personalizados (Casos Especiales)

Si necesitas SQL personalizado en una migración:

```csharp
// Migrations/[timestamp]_AddCustomIndex.cs
protected override void Up(MigrationBuilder migrationBuilder)
{
    // Operación normal
    migrationBuilder.AddColumn<string>(
        name: "FullName",
        table: "Users",
        nullable: true);

    // SQL personalizado
    migrationBuilder.Sql(@"
        UPDATE Users 
        SET FullName = CONCAT(FirstName, ' ', LastName)
        WHERE FullName IS NULL
    ");

    // Agregar índice compuesto
    migrationBuilder.CreateIndex(
        name: "IX_Users_FirstName_LastName",
        table: "Users",
        columns: new[] { "FirstName", "LastName" });
}
```

---

### 8.10 Seeding de Datos Iniciales

Para datos maestros o iniciales:

```csharp
// Infrastructure.Persistence/Configurations/UserConfiguration.cs
public void Configure(EntityTypeBuilder<User> builder)
{
    // ... configuración normal ...

    // Seed data (solo para datos maestros críticos)
    builder.HasData(
        new User
        {
            Id = 1,
            FirstName = "Admin",
            LastName = "System",
            Email = "admin@system.com",
            PasswordHash = "$2a$11$...", // Hash pre-generado
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        }
    );
}
```

⚠️ **IMPORTANTE**: Usar seeding solo para datos maestros estables. Para datos variables, usar scripts de inicialización separados.

---

## 9. TECNOLOGÍAS Y VERSIONES

- **.NET 8**
- **Entity Framework Core** 8.0.11
- **MySQL** con Pomelo.EntityFrameworkCore.MySql 8.0.2
- **FluentValidation** 12.1.1
- **BCrypt.Net** 4.0.3
- **Microsoft.AspNetCore.Authentication.JwtBearer** 8.0.11
- **System.IdentityModel.Tokens.Jwt** 8.0.2
- **Microsoft.Extensions.Configuration.Abstractions** 8.0.0
- **Swashbuckle.AspNetCore** 6.6.2

### Testing
- **xUnit** 2.6.2
- **xUnit.runner.visualstudio** 2.5.4
- **FluentAssertions** 6.12.0
- **NSubstitute** 5.1.0
- **Microsoft.NET.Test.Sdk** 17.8.0
- **Testcontainers.MySql** 3.6.0
- **Microsoft.AspNetCore.Mvc.Testing** 8.0.11
- **Bogus** 35.4.0

---

## 10. TESTING

### 10.1 Estructura de Proyectos de Tests

```
tests/
├── Core.Application.Tests/      # Tests unitarios de Features, Facades, Validators
├── Infrastructure.Persistence.Tests/  # Tests de integración con BD
└── API.Tests/                   # Tests de integración de endpoints
```

### 10.2 Stack de Testing

| Paquete | Versión | Propósito |
|---------|---------|-----------|
| xUnit | 2.6.2 | Framework de testing |
| xUnit.runner.visualstudio | 2.5.4 | Runner para Visual Studio |
| FluentAssertions | 6.12.0 | Aserciones expresivas |
| NSubstitute | 5.1.0 | Mocking de dependencias |
| Microsoft.NET.Test.Sdk | 17.8.0 | SDK de testing |
| Testcontainers.MySql | 3.6.0 | Contenedores para tests de integración |
| Microsoft.AspNetCore.Mvc.Testing | 8.0.11 | Tests de integración de API |
| Bogus | 35.4.0 | Generación de datos de prueba |

### 10.3 Convenciones de Nombrado

| Tipo | Convención | Ejemplo |
|------|------------|---------|
| Clase de Test | [ClaseBajoTest]Tests | `CreateUserTests`, `UserFacadeTests` |
| Método de Test | [Método]_[Escenario]_[ResultadoEsperado] | `ExecuteAsync_WithValidData_ReturnsSuccess` |
| Archivo | [ClaseBajoTest]Tests.cs | `CreateUserTests.cs` |

### 10.4 Tests Unitarios (Core.Application.Tests)

**Tests unitarios para Features, Facades y Validators.**

```csharp
// ✅ CORRECTO - Test unitario de Feature
public class CreateUserTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly CreateUser _sut; // System Under Test

    public CreateUserTests()
    {
        // Arrange - Mocks
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _sut = new CreateUser(_unitOfWork, _passwordHasher);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidData_ReturnsSuccessWithUserDto()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "SecurePassword123!"
        };

        var repository = Substitute.For<IGenericRepository<User>>();
        repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>>())
            .Returns((User?)null); // Email no existe

        _unitOfWork.Repository<User>().Returns(repository);
        _passwordHasher.Hash(dto.Password).Returns("hashed_password");

        // Act
        var result = await _sut.ExecuteAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Email.Should().Be(dto.Email);
        
        await repository.Received(1).AddAsync(Arg.Any<User>());
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingEmail_ReturnsEmailAlreadyExistsError()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "existing@example.com",
            Password = "SecurePassword123!"
        };

        var existingUser = new User { Id = 1, Email = dto.Email };
        var repository = Substitute.For<IGenericRepository<User>>();
        repository.FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>>())
            .Returns(existingUser);

        _unitOfWork.Repository<User>().Returns(repository);

        // Act
        var result = await _sut.ExecuteAsync(dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("User.EmailAlreadyExists");
        
        await repository.DidNotReceive().AddAsync(Arg.Any<User>());
    }
}
```

### 10.5 Tests de Validators

```csharp
public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator = new();

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "SecurePass123!"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_WithEmptyEmail_ShouldHaveError(string? email)
    {
        // Arrange
        var dto = new CreateUserDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = email!,
            Password = "SecurePass123!"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Theory]
    [InlineData("short")]           // Muy corta
    [InlineData("nouppercase1")]    // Sin mayúscula
    [InlineData("NOLOWERCASE1")]    // Sin minúscula
    [InlineData("NoNumbers!")]      // Sin número
    public void Validate_WithInvalidPassword_ShouldHaveError(string password)
    {
        // Arrange
        var dto = new CreateUserDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = password
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }
}
```

### 10.6 Tests de Integración con BD (Testcontainers)

**Para tests que requieren base de datos real.**

```csharp
// Fixture compartido para contenedor MySQL
public class DatabaseFixture : IAsyncLifetime
{
    private readonly MySqlContainer _container = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithDatabase("testdb")
        .WithUsername("test")
        .WithPassword("test")
        .Build();

    public string ConnectionString => _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}

[Collection("Database")]
public class UserRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly ApplicationDbContext _context;

    public UserRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseMySql(_fixture.ConnectionString, ServerVersion.AutoDetect(_fixture.ConnectionString))
            .Options;
        
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetByEmailAsync_WithExistingEmail_ReturnsUser()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            PasswordHash = "hash",
            IsActive = true
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var repository = new UserRepository(_context);

        // Act
        var result = await repository.GetByEmailAsync("test@example.com");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("test@example.com");
    }
}
```

### 10.7 Tests de Integración de API

```csharp
public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public UsersControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Reemplazar servicios para testing
                var descriptor = services.SingleOrDefault(d => 
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        });
        
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithUsers()
    {
        // Act
        var response = await _client.GetAsync("/api/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedUser()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "SecurePass123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        user.Should().NotBeNull();
        user!.Email.Should().Be(dto.Email);
    }

    [Fact]
    public async Task GetById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/users/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
```

### 10.8 Generación de Datos con Bogus

```csharp
// Fakers para generación de datos de prueba
public static class UserFakers
{
    public static Faker<CreateUserDto> CreateUserDtoFaker => new Faker<CreateUserDto>()
        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
        .RuleFor(u => u.LastName, f => f.Name.LastName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Password, f => $"{f.Internet.Password(8)}A1!");

    public static Faker<User> UserFaker => new Faker<User>()
        .RuleFor(u => u.Id, f => f.IndexFaker + 1)
        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
        .RuleFor(u => u.LastName, f => f.Name.LastName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.PasswordHash, f => "hashed_password")
        .RuleFor(u => u.IsActive, true)
        .RuleFor(u => u.CreatedAt, f => f.Date.Past());
}

// Uso en tests
[Fact]
public async Task GetAllUsers_WithMultipleUsers_ReturnsAllUsers()
{
    // Arrange
    var users = UserFakers.UserFaker.Generate(10);
    foreach (var user in users)
    {
        await _context.Users.AddAsync(user);
    }
    await _context.SaveChangesAsync();

    // Act
    var result = await _facade.GetAllUsersAsync();

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().HaveCount(10);
}
```

### 10.9 Estructura de Carpetas de Tests

```
tests/
├── Core.Application.Tests/
│   ├── Core.Application.Tests.csproj
│   ├── Features/
│   │   └── Users/
│   │       ├── CreateUserTests.cs
│   │       ├── GetUserTests.cs
│   │       ├── UpdateUserTests.cs
│   │       └── DeleteUserTests.cs
│   ├── Facades/
│   │   └── UserFacadeTests.cs
│   ├── Validators/
│   │   ├── CreateUserValidatorTests.cs
│   │   └── UpdateUserValidatorTests.cs
│   └── Fakers/
│       └── UserFakers.cs
│
├── Infrastructure.Persistence.Tests/
│   ├── Infrastructure.Persistence.Tests.csproj
│   ├── Fixtures/
│   │   └── DatabaseFixture.cs
│   └── Repositories/
│       └── UserRepositoryTests.cs
│
└── API.Tests/
    ├── API.Tests.csproj
    ├── Controllers/
    │   └── UsersControllerTests.cs
    └── Integration/
        └── AuthenticationTests.cs
```

### 10.10 Comandos para Ejecutar Tests

```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar tests de un proyecto específico
dotnet test tests/Core.Application.Tests

# Ejecutar con cobertura
dotnet test --collect:"XPlat Code Coverage"

# Ejecutar tests con filtro por nombre
dotnet test --filter "FullyQualifiedName~CreateUser"

# Ejecutar tests con output detallado
dotnet test --logger "console;verbosity=detailed"
```

### 10.11 Reglas de Testing

#### ✅ DO (Hacer)

- ✅ Un test por escenario/comportamiento
- ✅ Nombres descriptivos que expliquen el caso
- ✅ Usar Arrange-Act-Assert (AAA)
- ✅ Mockear dependencias externas en tests unitarios
- ✅ Usar Testcontainers para tests de BD
- ✅ Generar datos con Bogus
- ✅ Tests independientes entre sí
- ✅ Limpiar estado entre tests

#### ❌ DON'T (No Hacer)

- ❌ Tests que dependen del orden de ejecución
- ❌ Tests que dependen de BD externa/real
- ❌ Múltiples aserciones no relacionadas
- ❌ Lógica condicional en tests
- ❌ Hardcodear datos que pueden cambiar
- ❌ Tests lentos sin razón (usar mocks)

---

## 11. CHECKLIST PARA NUEVO CÓDIGO

- [ ] Entidad hereda de `BaseEntity`
- [ ] Entity Configuration para EF Core (`IEntityTypeConfiguration`)
- [ ] **Migración creada y aplicada** (si modifica BD)
- [ ] DbSet agregado en `ApplicationDbContext` (si es nueva entidad)
- [ ] DTOs creados (Dto, CreateDto, UpdateDto)
- [ ] Mapping Extensions implementados
- [ ] Errors tipados definidos
- [ ] Validators con FluentValidation
- [ ] Features creados (casos de uso)
- [ ] Facade implementado
- [ ] Repository configurado (si necesario)
- [ ] Controller con Result Pattern
- [ ] Registrado en DependencyInjection
- [ ] JWT configurado si requiere auth
- [ ] CORS configurado correctamente
- [ ] Código en inglés
- [ ] **Tests unitarios creados** (Features, Validators)
- [ ] **Documentación actualizada** (ver [Feature Documentation Skill](../feature-documentation/SKILL.md))

---

## 11. DOCUMENTACIÓN DE FEATURES

**⚠️ OBLIGATORIO**: Al completar el desarrollo de cualquier feature o modificación, debes actualizar la documentación siguiendo el **[Feature Documentation Skill](../feature-documentation/SKILL.md)**.

### Cuándo Documentar

✅ **SIEMPRE documentar cuando:**
- Creas un nuevo endpoint
- Modificas un endpoint existente
- Cambias un DTO o modelo de datos
- Agregas/modificas validaciones
- Cambias reglas de negocio
- Corriges un bug que cambia comportamiento

### Dónde Documentar

```
docs/features/[FEATURE-NAME].md
```

### Qué Documentar (Mínimo)

1. **Endpoints**: Todos los endpoints con ejemplos de request/response
2. **DTOs**: Todos los modelos con descripción de propiedades
3. **Validaciones**: Reglas de FluentValidation
4. **Casos de Uso**: Flujo principal de cada operación
5. **Ejemplos**: Al menos cURL y C# HttpClient

### Proceso

```
Código Completo → Actualizar Docs → Commit (código + docs) → PR
```

**Referencia completa**: Ver [Feature Documentation Skill](../feature-documentation/SKILL.md)

---

**IMPORTANTE**: Cualquier desviación de estas reglas debe ser consultada y aprobada antes de implementarse.
