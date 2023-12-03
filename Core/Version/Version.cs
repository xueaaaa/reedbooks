using Octokit;
using System;
using System.Reflection;

namespace ReedBooks.Core.Version
{
    public struct Version
    {
        private const string GITHUB_REPOSITORY_NAME = "ReedBooks";
        private const string GITHUB_REPOSITORY_OWNER = "xueaaaa";

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

                return new Version(version);
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

        public Version(byte major, byte minor = 0, byte patch = 0, byte revision = 0)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Revision = revision;
        }

        public Version(string stringVersion)
        {
            var versionParts = stringVersion.Split('.');
            if (versionParts.Length != 4) this = new Version(0);

            Major = Convert.ToByte(versionParts[0]);
            Minor = Convert.ToByte(versionParts[1]);
            Patch = Convert.ToByte(versionParts[2]);
            Revision = Convert.ToByte(versionParts[3]);
        }

        /// <summary>
        /// Gets the latest version of the program from GitHub
        /// </summary>
        /// <returns>Version</returns>
        public static Version GetFromGitHub()
        {
            var github = new GitHubClient(new ProductHeaderValue(GITHUB_REPOSITORY_NAME));
            var tags = github.Repository.GetAllTags(GITHUB_REPOSITORY_OWNER, GITHUB_REPOSITORY_NAME).Result;
            var latestTag = tags[tags.Count - 1];
            var versionAsStr = latestTag.Name;
            var version = new Version(versionAsStr);

            return version;
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
