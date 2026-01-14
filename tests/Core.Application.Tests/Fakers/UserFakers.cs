namespace Core.Application.Tests.Fakers;

/// <summary>
/// Fakers para generación de datos de prueba de usuarios.
/// </summary>
public static class UserFakers
{
    /// <summary>
    /// Faker para CreateUserDto con datos válidos.
    /// </summary>
    public static Faker<CreateUserDto> CreateUserDtoFaker => new Faker<CreateUserDto>()
        .CustomInstantiator(f => new CreateUserDto(
            f.Name.FirstName(),
            f.Name.LastName(),
            f.Internet.Email(),
            $"{f.Internet.Password(8)}A1!"
        ));

    /// <summary>
    /// Faker para UpdateUserDto con datos válidos.
    /// </summary>
    public static Faker<UpdateUserDto> UpdateUserDtoFaker => new Faker<UpdateUserDto>()
        .CustomInstantiator(f => new UpdateUserDto(
            f.Name.FirstName(),
            f.Name.LastName(),
            true
        ));

    /// <summary>
    /// Faker para entidad User con datos válidos.
    /// </summary>
    public static Faker<User> UserFaker => new Faker<User>()
        .RuleFor(u => u.Id, f => f.IndexFaker + 1)
        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
        .RuleFor(u => u.LastName, f => f.Name.LastName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.PasswordHash, _ => "hashed_password")
        .RuleFor(u => u.IsActive, true)
        .RuleFor(u => u.CreatedAt, f => f.Date.Past());
}
