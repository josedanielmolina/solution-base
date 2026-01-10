necesito que implementes la siguiente solucion de .net que sera la base para proyectos:

Solution/
├── src/
│   ├── Core/
│   │   ├── Core.Domain/
│   │   │   ├── Entities/
│   │   │   │   └── User.cs
│   │   │   ├── ValueObjects/
│   │   │   ├── Enums/
│   │   │   ├── Exceptions/
│   │   │   │   └── DomainException.cs
│   │   │   └── Interfaces/
│   │   │       ├── Repositories/
│   │   │       │   ├── IGenericRepository.cs
│   │   │       │   └── IUserRepository.cs
│   │   │       └── IUnitOfWork.cs
│   │   │
│   │   └── Core.Application/
│   │       ├── Operations/
│   │       │   └── Users/
│   │       │       ├── CreateUserOperation.cs
│   │       │       ├── GetUserOperation.cs
│   │       │       ├── UpdateUserOperation.cs
│   │       │       └── DeleteUserOperation.cs
│   │       │
│   │       ├── Facades/
│   │       │   ├── IUserFacade.cs
│   │       │   └── UserFacade.cs
│   │       │
│   │       ├── DTOs/
│   │       │   └── Users/
│   │       │       ├── UserDto.cs
│   │       │       ├── CreateUserDto.cs
│   │       │       └── UpdateUserDto.cs
│   │       │
│   │       ├── Mappings/
│   │       │   └── UserMappingExtensions.cs
│   │       │
│   │       ├── Validators/
│   │       │   ├── CreateUserValidator.cs
│   │       │   └── UpdateUserValidator.cs
│   │       │
│   │       ├── Interfaces/
│   │       │   ├── IEmailService.cs
│   │       │   └── IStorageService.cs
│   │       │
│   │       ├── Common/
│   │       │   ├── Result/
│   │       │   │   ├── Result.cs
│   │       │   │   ├── Result{T}.cs
│   │       │   │   ├── Error.cs
│   │       │   │   └── ErrorType.cs
│   │       │   ├── Errors/
│   │       │   │   └── UserErrors.cs
│   │       │   ├── PaginatedList.cs
│   │       │   └── Extensions/
│   │       │       └── ResultExtensions.cs
│   │       │
│   │       └── DependencyInjection.cs
│   │
│   ├── Infrastructure/
│   │   ├── Infrastructure.Services/
│   │   │   ├── Services/
│   │   │   │   ├── EmailService.cs
│   │   │   │   ├── StorageService.cs
│   │   │   │   └── CacheService.cs
│   │   │   ├── ExternalApis/
│   │   │   │   └── PaymentGatewayClient.cs
│   │   │   └── DependencyInjection.cs
│   │   │
│   │   └── Infrastructure.Persistence/
│   │       ├── Context/
│   │       │   └── ApplicationDbContext.cs
│   │       │
│   │       ├── Configurations/
│   │       │   └── UserConfiguration.cs
│   │       │
│   │       ├── Repositories/
│   │       │   ├── GenericRepository.cs
│   │       │   └── UserRepository.cs
│   │       │
│   │       ├── UnitOfWork/
│   │       │   └── UnitOfWork.cs
│   │       │
│   │       ├── Migrations/
│   │       │
│   │       └── DependencyInjection.cs
│   │
│   ├── Presentation/
│   │   └── API/
│   │       ├── Controllers/
│   │       │   └── UsersController.cs
│   │       ├── Filters/
│   │       │   ├── ValidateModelAttribute.cs
│   │       │   └── ResultFilter.cs
│   │       ├── Middleware/
│   │       │   ├── ExceptionMiddleware.cs
│   │       │   └── LoggingMiddleware.cs
│   │       ├── Extensions/
│   │       │   └── ResultToActionResultExtensions.cs
│   │       ├── Program.cs
│   │       ├── appsettings.json
│   │       └── DependencyInjection.cs
│   │
│   └── UI/
│       └── WebApp/
│           ├── src/
│           │   ├── app/
│           │   │   ├── core/
│           │   │   │   ├── services/
│           │   │   │   ├── guards/
│           │   │   │   ├── interceptors/
│           │   │   │   └── models/
│           │   │   │       └── api-response.model.ts
│           │   │   ├── shared/
│           │   │   │   ├── components/
│           │   │   │   ├── pipes/
│           │   │   │   └── directives/
│           │   │   ├── features/
│           │   │   │   └── users/
│           │   │   │       ├── components/
│           │   │   │       ├── services/
│           │   │   │       └── models/
│           │   │   └── layouts/
│           │   ├── assets/
│           │   └── environments/
│           ├── angular.json
│           └── package.json
│
├── tests/
│   ├── Core.Domain.Tests/
│   ├── Core.Application.Tests/
│   │   ├── Operations/
│   │   └── Facades/
│   ├── Infrastructure.Tests/
│   └── API.Tests/
│
└── Solution.sln


el base para repository generic  y unit of work 
// ============================================
// 1. ENTIDAD BASE
// ============================================
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// ============================================
// 2. REPOSITORIO GENÉRICO (Interfaz)
// ============================================
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

// ============================================
// 3. REPOSITORIO GENÉRICO (Implementación)
// ============================================
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id);
    }
}

// ============================================
// 4. UNIT OF WORK (Interfaz)
// ============================================
public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : BaseEntity;
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

// ============================================
// 5. UNIT OF WORK (Implementación)
// ============================================
public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly Dictionary<Type, object> _repositories;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(DbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);
        
        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(Repository<>).MakeGenericType(type);
            var repositoryInstance = Activator.CreateInstance(repositoryType, _context);
            _repositories.Add(type, repositoryInstance!);
        }

        return (IRepository<T>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

// ============================================
// 6. ENTIDADES DE EJEMPLO
// ============================================
public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

// ============================================
// 7. CUÁNDO CREAR REPOSITORIO ESPECIALIZADO
// ============================================

// ❌ NO NECESARIO - Consulta simple que puede hacerse con el genérico
// Este era el problema del ejemplo anterior
public interface IUserRepositoryBad : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);  // Muy simple
    Task<IEnumerable<User>> GetActiveUsersAsync(); // Muy simple
}

// ✅ SÍ NECESARIO - Consultas complejas con joins, lógica de negocio
public interface IOrderRepository : IRepository<Order>
{
    // Consulta compleja con múltiples joins y agregaciones
    Task<IEnumerable<Order>> GetOrdersWithDetailsAsync(int userId, int page, int pageSize);
    
    // Lógica de negocio compleja
    Task<decimal> GetTotalRevenueByUserAsync(int userId, DateTime from, DateTime to);
    
    // Consultas con múltiples relaciones y filtros
    Task<IEnumerable<Order>> GetPendingOrdersWithLowStockProductsAsync();
    
    // Operación que requiere lógica específica del dominio
    Task<bool> CanCancelOrderAsync(int orderId);
    
    // Proyecciones específicas
    Task<OrderSummaryDto> GetOrderSummaryAsync(int orderId);
}

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(DbContext context) : base(context)
    {
    }

    // Consulta compleja con includes, filtros, paginación
    public async Task<IEnumerable<Order>> GetOrdersWithDetailsAsync(int userId, int page, int pageSize)
    {
        return await _dbSet
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.User)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    // Lógica de negocio con agregaciones
    public async Task<decimal> GetTotalRevenueByUserAsync(int userId, DateTime from, DateTime to)
    {
        return await _dbSet
            .Where(o => o.UserId == userId 
                && o.CreatedAt >= from 
                && o.CreatedAt <= to
                && o.Status == OrderStatus.Delivered)
            .SumAsync(o => o.Total);
    }

    // Consulta compleja con múltiples joins y condiciones
    public async Task<IEnumerable<Order>> GetPendingOrdersWithLowStockProductsAsync()
    {
        return await _dbSet
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Where(o => o.Status == OrderStatus.Pending 
                && o.Items.Any(i => i.Product.Stock < i.Quantity))
            .ToListAsync();
    }

    // Lógica de negocio específica del dominio
    public async Task<bool> CanCancelOrderAsync(int orderId)
    {
        var order = await _dbSet
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null) return false;

        // Reglas de negocio complejas
        var hoursSinceCreated = (DateTime.UtcNow - order.CreatedAt).TotalHours;
        return order.Status == OrderStatus.Pending 
            && hoursSinceCreated < 24 
            && order.Items.All(i => i.Product.Stock >= i.Quantity);
    }

    // Proyección a DTO específico
    public async Task<OrderSummaryDto> GetOrderSummaryAsync(int orderId)
    {
        return await _dbSet
            .Where(o => o.Id == orderId)
            .Select(o => new OrderSummaryDto
            {
                OrderId = o.Id,
                CustomerName = o.User.Name,
                TotalItems = o.Items.Count,
                TotalAmount = o.Total,
                Status = o.Status.ToString(),
                CanBeCancelled = o.Status == OrderStatus.Pending 
                    && EF.Functions.DateDiffHour(o.CreatedAt, DateTime.UtcNow) < 24
            })
            .FirstOrDefaultAsync() ?? new OrderSummaryDto();
    }
}

public class OrderSummaryDto
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int TotalItems { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool CanBeCancelled { get; set; }
}

// ============================================
// 8. UNIT OF WORK EXTENDIDO
// ============================================
public interface IAppUnitOfWork : IUnitOfWork
{
    // Solo repositorios especializados que realmente lo necesitan
    IOrderRepository Orders { get; }
}

public class AppUnitOfWork : UnitOfWork, IAppUnitOfWork
{
    private IOrderRepository? _orderRepository;

    public AppUnitOfWork(DbContext context) : base(context)
    {
    }

    public IOrderRepository Orders => 
        _orderRepository ??= new OrderRepository(_context);
}

// ============================================
// 9. REGISTRO DE SERVICIOS
// ============================================
public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddDbContext<DbContext>(options =>
            options.UseSqlServer("connection-string"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();

        // Solo repositorios especializados necesarios
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}

// ============================================
// 10. EJEMPLOS COMPARATIVOS DE USO
// ============================================
public class OrderService
{
    private readonly IAppUnitOfWork _unitOfWork;

    public OrderService(IAppUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // ✅ CORRECTO - Usar repositorio genérico para operaciones simples
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        // No necesitas repositorio especializado para esto
        return await _unitOfWork.Repository<User>().GetByIdAsync(userId);
    }

    // ✅ CORRECTO - Consultas simples con el genérico
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        // Usa el FindAsync del repositorio genérico
        var users = await _unitOfWork.Repository<User>()
            .FindAsync(u => u.Email == email);
        return users.FirstOrDefault();
    }

    // ✅ CORRECTO - Usar repositorio especializado para lógica compleja
    public async Task<IEnumerable<Order>> GetUserOrdersWithDetailsAsync(int userId, int page)
    {
        // Esto SÍ necesita el repositorio especializado
        return await _unitOfWork.Orders.GetOrdersWithDetailsAsync(userId, page, 10);
    }

    // ✅ CORRECTO - Lógica de negocio compleja
    public async Task<bool> CancelOrderAsync(int orderId)
    {
        // Validación compleja que está en el repositorio especializado
        if (!await _unitOfWork.Orders.CanCancelOrderAsync(orderId))
            return false;

        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null) return false;

        order.Status = OrderStatus.Cancelled;
        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    // ✅ CORRECTO - Transacción con múltiples repositorios
    public async Task CreateOrderAsync(int userId, List<OrderItemDto> items)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            // Usar genérico para User (operación simple)
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
            if (user == null) throw new Exception("Usuario no encontrado");

            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                Total = 0
            };

            // Usar genérico para Product (operaciones simples)
            var productRepo = _unitOfWork.Repository<Product>();
            
            foreach (var item in items)
            {
                var product = await productRepo.GetByIdAsync(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                    throw new Exception($"Stock insuficiente para {product?.Name}");

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = product.Price
                });

                product.Stock -= item.Quantity;
                await productRepo.UpdateAsync(product);
                order.Total += product.Price * item.Quantity;
            }

            // Usar genérico para Order (en este caso, si solo es inserción)
            await _unitOfWork.Repository<Order>().AddAsync(order);
            
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    // ✅ CORRECTO - Reporte que necesita el repositorio especializado
    public async Task<OrderReportDto> GetMonthlyReportAsync(int userId)
    {
        var from = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var to = from.AddMonths(1);

        return new OrderReportDto
        {
            TotalRevenue = await _unitOfWork.Orders.GetTotalRevenueByUserAsync(userId, from, to),
            PendingOrders = (await _unitOfWork.Orders
                .FindAsync(o => o.UserId == userId && o.Status == OrderStatus.Pending))
                .Count()
        };
    }
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderReportDto
{
    public decimal TotalRevenue { get; set; }
    public int PendingOrders { get; set; }
}


el mapeo de entidades sera sin librerias externas y te paso un ejemplo de como se hace
// ============================================
// 1. ENTIDADES (Domain Models)
// ============================================
public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Category { get; set; } = string.Empty;
}

// ============================================
// 2. DTOs como RECORDS
// ============================================

// Request DTOs (entrada)
public record CreateUserRequest(
    string Name,
    string Email,
    string Password
);

public record UpdateUserRequest(
    string Name,
    bool IsActive
);

public record CreateOrderRequest(
    int UserId,
    List<OrderItemRequest> Items
);

public record OrderItemRequest(
    int ProductId,
    int Quantity
);

// Response DTOs (salida)
public record UserResponse(
    int Id,
    string Name,
    string Email,
    bool IsActive,
    DateTime? LastLoginAt,
    DateTime CreatedAt
);

public record UserListResponse(
    int Id,
    string Name,
    string Email,
    bool IsActive
);

public record OrderResponse(
    int Id,
    decimal Total,
    string Status,
    DateTime CreatedAt,
    UserSummaryResponse User,
    List<OrderItemResponse> Items
);

public record OrderItemResponse(
    int Id,
    int Quantity,
    decimal Price,
    decimal Subtotal,
    ProductSummaryResponse Product
);

public record ProductSummaryResponse(
    int Id,
    string Name,
    decimal Price
);

public record UserSummaryResponse(
    int Id,
    string Name,
    string Email
);

public record OrderListResponse(
    int Id,
    decimal Total,
    string Status,
    int ItemCount,
    DateTime CreatedAt
);

// ============================================
// 3. EXTENSION METHODS PARA MAPEO
// Convención: ToDto / FromDto / MapTo
// ============================================

public static class UserMappingExtensions
{
    // Entity → DTO (Salida)
    public static UserResponse ToDto(this User user)
    {
        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.IsActive,
            user.LastLoginAt,
            user.CreatedAt
        );
    }

    public static UserListResponse ToListDto(this User user)
    {
        return new UserListResponse(
            user.Id,
            user.Name,
            user.Email,
            user.IsActive
        );
    }

    public static UserSummaryResponse ToSummaryDto(this User user)
    {
        return new UserSummaryResponse(
            user.Id,
            user.Name,
            user.Email
        );
    }

    // DTO → Entity (Creación)
    public static User ToEntity(this CreateUserRequest dto, string passwordHash)
    {
        return new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = passwordHash,
            IsActive = true
        };
    }

    // DTO → Entity (Actualización in-place)
    public static void MapTo(this UpdateUserRequest dto, User entity)
    {
        entity.Name = dto.Name;
        entity.IsActive = dto.IsActive;
    }

    // Collections
    public static List<UserResponse> ToDtoList(this IEnumerable<User> users)
    {
        return users.Select(u => u.ToDto()).ToList();
    }

    public static List<UserListResponse> ToListDtoList(this IEnumerable<User> users)
    {
        return users.Select(u => u.ToListDto()).ToList();
    }
}

public static class OrderMappingExtensions
{
    public static OrderResponse ToDto(this Order order)
    {
        return new OrderResponse(
            order.Id,
            order.Total,
            order.Status.ToString(),
            order.CreatedAt,
            order.User.ToSummaryDto(),
            order.Items.ToDtoList()
        );
    }

    public static OrderListResponse ToListDto(this Order order)
    {
        return new OrderListResponse(
            order.Id,
            order.Total,
            order.Status.ToString(),
            order.Items.Count,
            order.CreatedAt
        );
    }

    public static List<OrderResponse> ToDtoList(this IEnumerable<Order> orders)
    {
        return orders.Select(o => o.ToDto()).ToList();
    }

    public static List<OrderListResponse> ToListDtoList(this IEnumerable<Order> orders)
    {
        return orders.Select(o => o.ToListDto()).ToList();
    }
}

public static class OrderItemMappingExtensions
{
    public static OrderItemResponse ToDto(this OrderItem item)
    {
        return new OrderItemResponse(
            item.Id,
            item.Quantity,
            item.Price,
            item.Quantity * item.Price,
            item.Product.ToSummaryDto()
        );
    }

    public static List<OrderItemResponse> ToDtoList(this IEnumerable<OrderItem> items)
    {
        return items.Select(i => i.ToDto()).ToList();
    }
}

public static class ProductMappingExtensions
{
    public static ProductSummaryResponse ToSummaryDto(this Product product)
    {
        return new ProductSummaryResponse(
            product.Id,
            product.Name,
            product.Price
        );
    }
}

// ============================================
// 4. EJEMPLO DE USO EN SERVICIO
// ============================================
public class UserService
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IAppUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    // ✅ Crear usuario
    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var passwordHash = _passwordHasher.HashPassword(request.Password);
        
        // DTO → Entity
        var user = request.ToEntity(passwordHash);
        
        await _unitOfWork.Repository<User>().AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        // Entity → DTO
        return user.ToDto();
    }

    // ✅ Obtener usuario por ID
    public async Task<UserResponse?> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
        
        // Entity → DTO (null-safe)
        return user?.ToDto();
    }

    // ✅ Listar usuarios
    public async Task<List<UserListResponse>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.Repository<User>().GetAllAsync();
        
        // IEnumerable<Entity> → List<DTO>
        return users.ToListDtoList();
    }

    // ✅ Actualizar usuario
    public async Task<UserResponse?> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
        if (user == null) return null;

        // DTO → Entity (actualización)
        request.MapTo(user);
        
        await _unitOfWork.Repository<User>().UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        return user.ToDto();
    }

    // ✅ Obtener usuarios activos
    public async Task<List<UserListResponse>> GetActiveUsersAsync()
    {
        var users = await _unitOfWork.Repository<User>()
            .FindAsync(u => u.IsActive);
        
        return users.ToListDtoList();
    }
}

public class OrderService
{
    private readonly IAppUnitOfWork _unitOfWork;

    public OrderService(IAppUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // ✅ Obtener órdenes con detalles completos
    public async Task<List<OrderResponse>> GetUserOrdersAsync(int userId, int page = 1)
    {
        var orders = await _unitOfWork.Orders
            .GetOrdersWithDetailsAsync(userId, page, 10);
        
        return orders.ToDtoList();
    }

    // ✅ Listar órdenes (sin detalles completos)
    public async Task<List<OrderListResponse>> GetOrderListAsync(int userId)
    {
        var orders = await _unitOfWork.Repository<Order>()
            .FindAsync(o => o.UserId == userId);
        
        return orders.ToListDtoList();
    }

    // ✅ Crear orden con transacción
    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            var order = new Order
            {
                UserId = request.UserId,
                Status = OrderStatus.Pending,
                Total = 0
            };

            var productRepo = _unitOfWork.Repository<Product>();

            foreach (var itemRequest in request.Items)
            {
                var product = await productRepo.GetByIdAsync(itemRequest.ProductId);
                if (product == null || product.Stock < itemRequest.Quantity)
                    throw new InvalidOperationException($"Stock insuficiente");

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemRequest.Quantity,
                    Price = product.Price
                });

                product.Stock -= itemRequest.Quantity;
                await productRepo.UpdateAsync(product);
                order.Total += product.Price * itemRequest.Quantity;
            }

            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.CommitTransactionAsync();

            // Recargar con navegaciones para el DTO
            var createdOrder = await _unitOfWork.Orders.GetByIdAsync(order.Id);
            return createdOrder!.ToDto();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}

// ============================================
// 5. INTERFACE AUXILIAR
// ============================================
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}


implementacion de patter result
// ============================================
// 1. RESULT PATTERN - Clase Base
// ============================================
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Success result cannot have an error");
        
        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Failure result must have an error");

        IsSuccess = isSuccess;
        Error = error;
    }

    // Factory methods
    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Success<T>(T value) => new(value, true, Error.None);
    public static Result<T> Failure<T>(Error error) => new(default, false, error);
}

// ============================================
// 2. RESULT<T> - Con valor de retorno
// ============================================
public class Result<T> : Result
{
    private readonly T? _value;

    public T Value => IsSuccess 
        ? _value! 
        : throw new InvalidOperationException("Cannot access value of a failed result");

    protected internal Result(T? value, bool isSuccess, Error error) 
        : base(isSuccess, error)
    {
        _value = value;
    }

    // Conversión implícita desde T
    public static implicit operator Result<T>(T value) => Success(value);
    
    // Conversión implícita desde Error
    public static implicit operator Result<T>(Error error) => Failure<T>(error);
}

// ============================================
// 3. ERROR - Representa errores tipados
// ============================================
public sealed record Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

    // Factory methods por tipo
    public static Error NotFound(string code, string message) 
        => new(code, message, ErrorType.NotFound);

    public static Error Validation(string code, string message) 
        => new(code, message, ErrorType.Validation);

    public static Error Conflict(string code, string message) 
        => new(code, message, ErrorType.Conflict);

    public static Error Failure(string code, string message) 
        => new(code, message, ErrorType.Failure);

    public static Error Unauthorized(string code, string message) 
        => new(code, message, ErrorType.Unauthorized);
}

public enum ErrorType
{
    None,
    Validation,
    NotFound,
    Conflict,
    Failure,
    Unauthorized
}

// ============================================
// 4. ERRORES ESPECÍFICOS DEL DOMINIO
// ============================================
public static class UserErrors
{
    public static Error NotFound(int userId) => Error.NotFound(
        "User.NotFound",
        $"Usuario con ID {userId} no fue encontrado");

    public static Error EmailAlreadyExists(string email) => Error.Conflict(
        "User.EmailAlreadyExists",
        $"El email {email} ya está registrado");

    public static Error InvalidEmail(string email) => Error.Validation(
        "User.InvalidEmail",
        $"El email {email} no es válido");

    public static Error InactiveUser => Error.Validation(
        "User.Inactive",
        "El usuario está inactivo");
}

public static class OrderErrors
{
    public static Error NotFound(int orderId) => Error.NotFound(
        "Order.NotFound",
        $"Orden con ID {orderId} no fue encontrada");

    public static Error CannotCancel(string reason) => Error.Validation(
        "Order.CannotCancel",
        $"No se puede cancelar la orden: {reason}");

    public static Error InsufficientStock(string productName) => Error.Conflict(
        "Order.InsufficientStock",
        $"Stock insuficiente para el producto {productName}");

    public static Error EmptyOrder => Error.Validation(
        "Order.EmptyOrder",
        "La orden debe tener al menos un item");
}

public static class ProductErrors
{
    public static Error NotFound(int productId) => Error.NotFound(
        "Product.NotFound",
        $"Producto con ID {productId} no fue encontrado");

    public static Error OutOfStock(string productName) => Error.Conflict(
        "Product.OutOfStock",
        $"El producto {productName} está agotado");
}

// ============================================
// 5. EJEMPLO DE USO EN SERVICIOS
// ============================================
public class UserService
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IAppUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    // ✅ Retorna Result<T> con el DTO
    public async Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest request)
    {
        // Validación de email
        if (!IsValidEmail(request.Email))
            return UserErrors.InvalidEmail(request.Email);

        // Verificar si el email ya existe
        var existingUsers = await _unitOfWork.Repository<User>()
            .FindAsync(u => u.Email == request.Email);
        
        if (existingUsers.Any())
            return UserErrors.EmailAlreadyExists(request.Email);

        // Crear usuario
        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = request.ToEntity(passwordHash);
        
        await _unitOfWork.Repository<User>().AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        // ✅ Retorno exitoso (conversión implícita)
        return user.ToDto();
    }

    public async Task<Result<UserResponse>> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
        
        if (user == null)
            return UserErrors.NotFound(id);

        return user.ToDto();
    }

    public async Task<Result<UserResponse>> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
        
        if (user == null)
            return UserErrors.NotFound(id);

        request.MapTo(user);
        
        await _unitOfWork.Repository<User>().UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        return user.ToDto();
    }

    public async Task<Result> DeleteUserAsync(int id)
    {
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
        
        if (user == null)
            return UserErrors.NotFound(id);

        await _unitOfWork.Repository<User>().DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }

    private bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
}

public class OrderService
{
    private readonly IAppUnitOfWork _unitOfWork;

    public OrderService(IAppUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderRequest request)
    {
        // Validación básica
        if (request.Items.Count == 0)
            return OrderErrors.EmptyOrder;

        // Verificar usuario
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(request.UserId);
        if (user == null)
            return UserErrors.NotFound(request.UserId);

        if (!user.IsActive)
            return UserErrors.InactiveUser;

        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            var order = new Order
            {
                UserId = request.UserId,
                Status = OrderStatus.Pending,
                Total = 0
            };

            var productRepo = _unitOfWork.Repository<Product>();

            foreach (var itemRequest in request.Items)
            {
                var product = await productRepo.GetByIdAsync(itemRequest.ProductId);
                
                if (product == null)
                    return ProductErrors.NotFound(itemRequest.ProductId);

                if (product.Stock < itemRequest.Quantity)
                    return OrderErrors.InsufficientStock(product.Name);

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemRequest.Quantity,
                    Price = product.Price
                });

                product.Stock -= itemRequest.Quantity;
                await productRepo.UpdateAsync(product);
                order.Total += product.Price * itemRequest.Quantity;
            }

            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.CommitTransactionAsync();

            var createdOrder = await _unitOfWork.Orders.GetByIdAsync(order.Id);
            return createdOrder!.ToDto();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Error.Failure("Order.CreateFailed", ex.Message);
        }
    }

    public async Task<Result<OrderResponse>> CancelOrderAsync(int orderId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        
        if (order == null)
            return OrderErrors.NotFound(orderId);

        // Validación de negocio
        if (!await _unitOfWork.Orders.CanCancelOrderAsync(orderId))
            return OrderErrors.CannotCancel("La orden ya no puede ser cancelada");

        order.Status = OrderStatus.Cancelled;
        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return order.ToDto();
    }
}

// ============================================
// 6. CONTROLLER - Manejo de Results
// ============================================
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        var result = await _userService.CreateUserAsync(request);

        return result.Match(
            onSuccess: user => CreatedAtAction(nameof(GetUser), new { id = user.Id }, user),
            onFailure: error => error.ToProblemDetails()
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _userService.GetUserByIdAsync(id);

        return result.Match(
            onSuccess: user => Ok(user),
            onFailure: error => error.ToProblemDetails()
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
    {
        var result = await _userService.UpdateUserAsync(id, request);

        return result.Match(
            onSuccess: user => Ok(user),
            onFailure: error => error.ToProblemDetails()
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);

        return result.Match(
            onSuccess: () => NoContent(),
            onFailure: error => error.ToProblemDetails()
        );
    }
}

// ============================================
// 7. EXTENSION METHODS ÚTILES
// ============================================
public static class ResultExtensions
{
    // Pattern matching para Result<T>
    public static TResult Match<T, TResult>(
        this Result<T> result,
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }

    // Pattern matching para Result
    public static TResult Match<TResult>(
        this Result result,
        Func<TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }

    // Conversión a IActionResult
    public static IActionResult ToProblemDetails(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        return new ObjectResult(new ProblemDetails
        {
            Status = statusCode,
            Title = error.Code,
            Detail = error.Message,
            Type = $"https://httpstatuses.com/{statusCode}"
        })
        {
            StatusCode = statusCode
        };
    }

    // OnSuccess - ejecuta acción si es exitoso
    public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
            action(result.Value);
        
        return result;
    }

    // OnFailure - ejecuta acción si falló
    public static Result<T> OnFailure<T>(this Result<T> result, Action<Error> action)
    {
        if (result.IsFailure)
            action(result.Error);
        
        return result;
    }

    // Map - transforma el valor si es exitoso
    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result, 
        Func<TIn, TOut> mapper)
    {
        return result.IsSuccess 
            ? Result.Success(mapper(result.Value))
            : Result.Failure<TOut>(result.Error);
    }
}

// ============================================
// 8. TIPOS AUXILIARES
// ============================================
public record CreateUserRequest(string Name, string Email, string Password);
public record UpdateUserRequest(string Name, bool IsActive);
public record UserResponse(int Id, string Name, string Email, bool IsActive, DateTime? LastLoginAt, DateTime CreatedAt);

public record CreateOrderRequest(int UserId, List<OrderItemRequest> Items);
public record OrderItemRequest(int ProductId, int Quantity);
public record OrderResponse(int Id, decimal Total, string Status, DateTime CreatedAt);

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum OrderStatus { Pending, Processing, Shipped, Delivered, Cancelled }

public interface IPasswordHasher
{
    string HashPassword(string password);
}

public interface IAppUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : BaseEntity;
    IOrderRepository Orders { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

public interface IOrderRepository : IRepository<Order>
{
    Task<bool> CanCancelOrderAsync(int orderId);
}

// Extension methods de mapeo
public static class UserMappingExtensions
{
    public static UserResponse ToDto(this User user) => 
        new(user.Id, user.Name, user.Email, user.IsActive, user.LastLoginAt, user.CreatedAt);

    public static User ToEntity(this CreateUserRequest dto, string passwordHash) => 
        new() { Name = dto.Name, Email = dto.Email, PasswordHash = passwordHash, IsActive = true };

    public static void MapTo(this UpdateUserRequest dto, User entity)
    {
        entity.Name = dto.Name;
        entity.IsActive = dto.IsActive;
    }
}

public static class OrderMappingExtensions
{
    public static OrderResponse ToDto(this Order order) => 
        new(order.Id, order.Total, order.Status.ToString(), order.CreatedAt);
}



para la ui, utilizaras angular 20 que esta instalada en el pc



Instrucciones adicionales:
- para el manejo de base de datos en .net utilizara entity framework core.
- la configuracion por default sera con mysql.
- sera mono repo asi que el git va a nivel de toda la solucion.
- todo el codigo debe estar en ingles.

LIMITACIONES:
- no podras instalar librerias de terceros sin mi autorizacion del usuario.






