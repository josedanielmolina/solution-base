using Testcontainers.MySql;

namespace Infrastructure.Persistence.Tests.Fixtures;

/// <summary>
/// Fixture compartido para contenedor MySQL usando Testcontainers.
/// Proporciona una base de datos real para tests de integración.
/// </summary>
public class DatabaseFixture : IAsyncLifetime
{
    private readonly MySqlContainer _container = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithDatabase("testdb")
        .WithUsername("test")
        .WithPassword("test")
        .Build();

    /// <summary>
    /// Connection string para conectar a la base de datos del contenedor.
    /// </summary>
    public string ConnectionString => _container.GetConnectionString();

    /// <summary>
    /// Inicia el contenedor MySQL.
    /// </summary>
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    /// <summary>
    /// Detiene y elimina el contenedor MySQL.
    /// </summary>
    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    /// <summary>
    /// Crea un nuevo DbContext configurado para el contenedor.
    /// </summary>
    public ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString))
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}

/// <summary>
/// Collection definition para compartir el contenedor entre tests.
/// </summary>
[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}
