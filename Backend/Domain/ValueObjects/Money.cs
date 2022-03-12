using System.ComponentModel.DataAnnotations.Schema;
using ITI.DDD.Domain;

namespace ValueObjects;

public record Money : DbValueObject, IComparable<Money>
{
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Value { get; protected set; }

    public Money(decimal value)
    {
        Value = Math.Round(value, 2);
    }

    public Money(double value) : this((decimal)value)
    {
    }

    public Money(int value) : this((decimal)value)
    {
    }

    public static Money operator +(Money d) => d;
    public static Money operator -(Money d) => new(-d.Value);

    public static Money operator +(Money a, Money b) => new(a.Value + b.Value);
    public static Money operator -(Money a, Money b) => new(a.Value - b.Value);
    public static Money operator *(Money a, Money b) => new(a.Value * b.Value);
    public static Money operator /(Money a, Money b) => new(a.Value / b.Value);

    public static Money operator +(Money a, decimal b) => new(a.Value + b);
    public static Money operator -(Money a, decimal b) => new(a.Value - b);
    public static Money operator *(Money a, decimal b) => new(a.Value * b);
    public static Money operator /(Money a, decimal b) => new(a.Value / b);
    public static Money operator +(decimal a, Money b) => new(a + b.Value);
    public static Money operator -(decimal a, Money b) => new(a - b.Value);
    public static Money operator *(decimal a, Money b) => new(a * b.Value);
    public static Money operator /(decimal a, Money b) => new(a / b.Value);

    public static bool operator <=(Money a, Money b) => a.Value <= b.Value;
    public static bool operator >=(Money a, Money b) => a.Value >= b.Value;

    public static bool operator <(Money a, Money b) => a.Value < b.Value;
    public static bool operator >(Money a, Money b) => a.Value > b.Value;

    public static bool operator <(Money a, decimal b) => a.Value < b;
    public static bool operator >(Money a, decimal b) => a.Value > b;

    public int CompareTo(Money? other)
    {
        if (other == null) return 1;

        return Value.CompareTo(other.Value);
    }

    public override string ToString()
    {
        return $"{Value:0.00}";
    }
}
