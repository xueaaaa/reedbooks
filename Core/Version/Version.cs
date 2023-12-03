namespace ReedBooks.Core.Version
{
    public struct Version
    {
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

        public Version(byte major)
        {
            Major = major;
            Minor = 0;
            Patch = 0;
            Revision = 0;
        }

        public Version(byte major, byte minor)
        {
            Major = major;
            Minor = minor;
            Patch = 0;
            Revision = 0;
        }

        public Version(byte major, byte minor, byte patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Revision = 0;
        }

        public Version(byte major, byte minor, byte patch, byte revision)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Revision = revision;
        }

        public static bool operator >(Version a, Version b)
        {
            if (a.Major > b.Major) return true;
            else if (a.Minor > b.Minor) return true;
            else if(a.Patch > b.Patch) return true;
            else if(a.Revision > b.Revision) return true;
            else return false;
        }

        public static bool operator <(Version a, Version b)
        {
            if (a.Major < b.Major) return true;
            else if (a.Minor < b.Minor) return true;
            else if (a.Patch < b.Patch) return true;
            else if (a.Revision < b.Revision) return true;
            else return false;
        }
    }
}
