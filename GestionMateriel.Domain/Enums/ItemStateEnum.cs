namespace GestionMateriel.Domain.Enums;

public sealed class ItemState
{
    public static readonly ItemState OK = new("OK");
    public static readonly ItemState NOK = new("NOK");
    public static readonly ItemState KO = new("KO");

    public string Value { get; }

    private ItemState(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;

    public static ItemState FromString(string value)
    {
        return List().FirstOrDefault(r => r.Value == value)
               ?? throw new ArgumentException($"Invalid item state value: {value}");
    }

    public static IEnumerable<ItemState> List() => [OK, NOK, KO];

    public override bool Equals(object? obj)
    {
        return obj is ItemState other && Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}