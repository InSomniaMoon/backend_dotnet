namespace GestionMateriel.Domain.Enums;

public sealed class IssueStatusEnum
{
    public static readonly IssueStatusEnum Open = new("Open");
    public static readonly IssueStatusEnum Resolved = new("Resolved");

    public static IEnumerable<IssueStatusEnum> List() => [Open, Resolved];

    public string Value { get; }

    private IssueStatusEnum(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;

    public static IssueStatusEnum FromString(string value)
    {
        return List().FirstOrDefault(r => r.Value == value)
               ?? throw new ArgumentException($"Invalid issue status value: {value}");
    }

    public override bool Equals(object? obj)
    {
        return obj is IssueStatusEnum other && Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}

public sealed class ItemStatusEnum
{
    public static readonly ItemStatusEnum OK = new("OK");
    public static readonly ItemStatusEnum NOK = new("NOK");
    public static readonly ItemStatusEnum KO = new("KO");

    public static IEnumerable<ItemStatusEnum> List() => [OK, NOK, KO];


    public string Value { get; }

    private ItemStatusEnum(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;

    public static ItemStatusEnum FromString(string value)
    {
        return List().FirstOrDefault(r => r.Value == value)
               ?? throw new ArgumentException($"Invalid item status value: {value}");
    }

    public override bool Equals(object? obj)
    {
        return obj is ItemStatusEnum other && Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}
