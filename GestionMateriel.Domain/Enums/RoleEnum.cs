namespace GestionMateriel.Domain.Enums;

public sealed class RoleEnum
{
    public string Value { get; }

    private RoleEnum(string value)
    {
        Value = value;
    }

    public static readonly RoleEnum User = new("user");
    public static readonly RoleEnum Admin = new("admin");

    public static IEnumerable<RoleEnum> List() => [User, Admin];

    public override string ToString() => Value;

    public static RoleEnum FromString(string value)
    {
        return List().FirstOrDefault(r => r.Value == value)
               ?? throw new ArgumentException($"Invalid role value: {value}");
    }

    public override bool Equals(object? obj)
    {
        return obj is RoleEnum other && Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}
