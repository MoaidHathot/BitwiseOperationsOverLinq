using System;

namespace OzCodeLinqArticle
{
    public sealed class Bit : IEquatable<Bit>, IComparable, IComparable<Bit>
    {
        public static Bit Off = new Bit(false, "Off");
        public static Bit On = new Bit(true, "On");

        private readonly int _value;
        private readonly bool _isOn;
        private readonly string _name;

        private static readonly string[] Names = { Off._name, On._name };
        private static readonly Bit[] Values = { Off, On };

        private Bit(bool isOn, string name)
        {
            _isOn = isOn;
            _name = name;

            _value = Convert.ToInt32(_isOn);
        }

        public static implicit operator bool(Bit bit) => bit._isOn;

        public static implicit operator Bit(bool bit) => bit ? On : Off;

        public static implicit operator int(Bit bit) => bit._value;
        public static implicit operator string(Bit bit) => bit._name;

        public static Bit operator |(Bit first, Bit second) => first ? On : second;
        public static Bit operator &(Bit first, Bit second) => first ? second : On;
        public static Bit operator ~(Bit bit) => bit ? Off : On;

        public static Bit operator !(Bit bit) => ~bit;

        public static bool operator ==(Bit first, Bit second) => first != null && first.Equals(second);

        public static bool operator !=(Bit first, Bit second) => (object)first != null && ((object)second != null && !first.Equals(second));

        public static string[] GetNames() => Names;
        public static Bit[] GetValues() => Values;

        public override string ToString() => _name;
        public override int GetHashCode() => _value;

        public override bool Equals(object obj) => Equals(obj as Bit);

        public bool Equals(Bit other) => null != other && _isOn == other._isOn;

        public int CompareTo(object obj) => CompareTo((Bit)obj);

        public int CompareTo(Bit other) => _value - other._value;
    }

}