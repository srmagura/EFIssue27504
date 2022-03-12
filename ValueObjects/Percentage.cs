using System.ComponentModel.DataAnnotations.Schema;

namespace ValueObjects;

public record Percentage : DbValueObject, IComparable<Percentage>
{
    private const int DecimalPrecision = 4;

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Value { get; protected init; }

    public Percentage(decimal value)
    {
        Value = Math.Round(value, DecimalPrecision);
    }

    public Percentage(int value)
    {
        Value = value;
    }

    public Percentage(double value)
    {
        Value = Math.Round((decimal)value, DecimalPrecision);
    }

    public Percentage(Percentage other) : base(other)
    {
    }

    //

    public static Percentage operator +(Percentage d) => d;
    public static Percentage operator -(Percentage d) => new Percentage(-1.0m * d.Value);

    public static Percentage operator +(Percentage a, Percentage b) => new Percentage(a.Value + b.Value);
    public static Percentage operator -(Percentage a, Percentage b) => new Percentage(a.Value - b.Value);
    public static Percentage operator *(Percentage a, Percentage b) => new Percentage(a.Value * b.Value);
    public static Percentage operator /(Percentage a, Percentage b) => new Percentage(a.Value / b.Value);

    public static Percentage operator +(Percentage a, decimal b) => new Percentage(a.Value + b);
    public static Percentage operator -(Percentage a, decimal b) => new Percentage(a.Value - b);
    public static Percentage operator *(Percentage a, decimal b) => new Percentage(a.Value * b);
    public static Percentage operator /(Percentage a, decimal b) => new Percentage(a.Value / b);
    public static Percentage operator +(decimal a, Percentage b) => new Percentage(a + b.Value);
    public static Percentage operator -(decimal a, Percentage b) => new Percentage(a - b.Value);
    public static Percentage operator *(decimal a, Percentage b) => new Percentage(a * b.Value);
    public static Percentage operator /(decimal a, Percentage b) => new Percentage(a / b.Value);

    public static Percentage operator +(Percentage a, double b) => new Percentage((double)a.Value + b);
    public static Percentage operator -(Percentage a, double b) => new Percentage((double)a.Value - b);
    public static Percentage operator *(Percentage a, double b) => new Percentage((double)a.Value * b);
    public static Percentage operator /(Percentage a, double b) => new Percentage((double)a.Value / b);
    public static Percentage operator +(double a, Percentage b) => new Percentage(a + (double)b.Value);
    public static Percentage operator -(double a, Percentage b) => new Percentage(a - (double)b.Value);
    public static Percentage operator *(double a, Percentage b) => new Percentage(a * (double)b.Value);
    public static Percentage operator /(double a, Percentage b) => new Percentage(a / (double)b.Value);

    public static bool operator <=(Percentage a, Percentage b) => a.Value <= b.Value;
    public static bool operator >=(Percentage a, Percentage b) => a.Value >= b.Value;

    public static bool operator <(Percentage a, Percentage b) => a.Value < b.Value;
    public static bool operator >(Percentage a, Percentage b) => a.Value > b.Value;

    //

    public static implicit operator decimal(Percentage m) => m.Value;
    public static implicit operator decimal?(Percentage m) => m.Value;

    public static implicit operator double(Percentage m) => (double)m.Value;
    public static implicit operator double?(Percentage m) => (double?)m.Value;

    //

    public static explicit operator Percentage(decimal d) => new(d);
    public static explicit operator Percentage(double d) => new(d);

    public int CompareTo(Percentage other)
    {
        if (other == null) return 1;

        return Value.CompareTo(other.Value);
    }

    public override string ToString() => $"{Value:0.00}";
}
