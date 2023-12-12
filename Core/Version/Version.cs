using System;
using System.Reflection;

namespace ReedBooks.Core.Version
{
    public class Version
    {
        /// <summary>
        /// Program version recorded in Assembly
        /// </summary>
        public static Version Local
        {
            get
            {
                string version = Assembly.GetExecutingAssembly()
                    .GetName()
                    .Version
                    .ToString();
                var versionParts = version.Split ('.');
                if (versionParts.Length != 4) throw new ArgumentException("This string does not represents a version");

                return new Version(Convert.ToByte(versionParts[0]),
                    Convert.ToByte(versionParts[1]),
                    Convert.ToByte(versionParts[2]),
                    Convert.ToByte(versionParts[3]));
            }
        }

        /// <summary>
        /// First digit in the sequence (x.0.0.0)
        /// </summary>
        public byte Major;
        /// <summary>
        /// Second digit in the sequence (0.x.0.0)
        /// </summary>
        public byte Minor;
        /// <summary>
        /// Third digit in the sequence (0.0.x.0)
        /// </summary>
        public byte Patch;
        /// <summary>
        /// Fourth digit in the sequence (0.0.0.x)
        /// </summary>
        public byte Revision;

        public Version(byte major = 0, byte minor = 0, byte patch = 0, byte revision = 0)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Revision = revision;
        }

        public int CompareTo(Version other)
        {
            if (Major != other.Major)
                return Major.CompareTo(other.Major);

            if (Minor != other.Minor)
                return Minor.CompareTo(other.Minor);

            if (Patch != other.Patch)
                return Patch.CompareTo(other.Patch);

            return Revision.CompareTo(other.Revision);
        }

        public static bool operator >(Version version1, Version version2)
        {
            return version1.CompareTo(version2) > 0;
        }

        public static bool operator <(Version version1, Version version2)
        {
            return version1.CompareTo(version2) < 0;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}.{Revision}";
        }
    }
}
