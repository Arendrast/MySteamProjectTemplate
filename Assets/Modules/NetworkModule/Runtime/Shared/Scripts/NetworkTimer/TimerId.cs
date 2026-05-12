using System;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.NetworkTimer
{
    [Serializable]
    public readonly struct TimerId : IEquatable<TimerId>
    {
        public readonly string Id;

        public TimerId(string id)
        {
            Id = id;
        }

        public bool CompareTag(string tag)
        {
            return Id != null && tag != null && Id.StartsWith(tag);
        }

        public override string ToString()
        {
            return Id ?? string.Empty;
        }

        public bool Equals(TimerId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is TimerId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id != null ? Id.GetHashCode() : 0;
        }

        public static bool operator ==(TimerId left, TimerId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TimerId left, TimerId right)
        {
            return !left.Equals(right);
        }
    }
}