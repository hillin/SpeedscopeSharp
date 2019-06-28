using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Hillinworks.Speedcore
{
    internal class Frame : IEquatable<Frame>
    {
        [JsonConstructor]
        private Frame(string name, string? file, int? line, int? column)
        {
            this.Name = name;
            this.File = file;
            this.Line = line;
            this.Column = column;
        }

        [JsonProperty("name")] public string Name { get; }

        [JsonProperty("file")] public string? File { get; }

        [JsonProperty("line")] public int? Line { get; }

        [JsonProperty("col")] public int? Column { get; }

        public bool Equals(Frame other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.Name, other.Name, StringComparison.InvariantCulture) &&
                   string.Equals(this.File, other.File, StringComparison.InvariantCulture) && this.Line == other.Line &&
                   this.Column == other.Column;
        }

        public static Frame CreateSimple(string name)
        {
            return new Frame(name, null, null, null);
        }

        public static Frame FromSource(
            [CallerMemberName] string? name = null,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int line = -1)
        {
            return new Frame(name!, file, line, null);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Frame) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Name != null ? StringComparer.InvariantCulture.GetHashCode(this.Name) : 0;
                hashCode = (hashCode * 397) ^
                           (this.File != null ? StringComparer.InvariantCulture.GetHashCode(this.File) : 0);
                hashCode = (hashCode * 397) ^ this.Line.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Column.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Frame left, Frame right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Frame left, Frame right)
        {
            return !Equals(left, right);
        }
    }
}