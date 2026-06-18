namespace GestionMateriel.Domain.Enums;

public sealed class StructureTypeEnum
{
    public static readonly StructureTypeEnum National = new("NATIONAL");
    public static readonly StructureTypeEnum Territoire = new("TERRITOIRE");
    public static readonly StructureTypeEnum Groupe = new("GROUPE");
    public static readonly StructureTypeEnum Unite = new("UNITE");

    public static IEnumerable<StructureTypeEnum> List() => [National, Territoire, Groupe, Unite];

    public string Value { get; }

    private StructureTypeEnum(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;

    public static StructureTypeEnum FromString(string value)
    {
        return List().FirstOrDefault(r => r.Value == value)
               ?? throw new ArgumentException($"Invalid structure type value: {value}");
    }

    public override bool Equals(object? obj)
    {
        return obj is StructureTypeEnum other && Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public string ComputeCodeStructureMask(string codeStructure)
    {
        if (Equals(Unite))
        {
            return codeStructure;
        }

        if (Equals(Groupe))
        {
            return codeStructure[..2];
        }

        if (Equals(National))
        {
            return codeStructure[..4];
        }

        if (Equals(Territoire))
        {
            return codeStructure[..6];
        }

        throw new InvalidOperationException($"Unknown structure type: {this}");
    }
}

