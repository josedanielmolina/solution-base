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
            f.Random.AlphaNumeric(10),
            f.Phone.PhoneNumber(),
            $"{f.Internet.Password(8)}A1!",
            null
        ));

    /// <summary>
    /// Faker para UpdateUserDto con datos válidos.
    /// </summary>
    public static Faker<UpdateUserDto> UpdateUserDtoFaker => new Faker<UpdateUserDto>()
        .CustomInstantiator(f => new UpdateUserDto(
            f.Name.FirstName(),
            f.Name.LastName(),
            f.Random.AlphaNumeric(10),
            f.Phone.PhoneNumber(),
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
        .RuleFor(u => u.Document, f => f.Random.AlphaNumeric(10))
        .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
        .RuleFor(u => u.PasswordHash, _ => "hashed_password")
        .RuleFor(u => u.IsActive, true)
        .RuleFor(u => u.RequiresPasswordChange, false)
        .RuleFor(u => u.CreatedAt, f => f.Date.Past());
}

