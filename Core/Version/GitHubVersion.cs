using Octokit;
using System;
using System.Collections.Generic;

namespace ReedBooks.Core.Version
{
    public class GitHubVersion : Version
    {
        private const string GITHUB_REPOSITORY_NAME = "ReedBooks";
        private const string GITHUB_REPOSITORY_OWNER = "xueaaaa";

        /// <summary>
        /// Release Notes
        /// </summary>
        public string Body { get; private set; }
        /// <summary>
        /// Date and time of publication
        /// </summary>
        public DateTimeOffset? PublishedAt { get; private set; }
        /// <summary>
        /// Links to update materials
        /// </summary>
        public IReadOnlyList<ReleaseAsset> Assets { get; private set; }

        public GitHubVersion() 
        {
            var github = new GitHubClient(new ProductHeaderValue(GITHUB_REPOSITORY_NAME));
            var latestRelease = github.Repository.Release.GetLatest(GITHUB_REPOSITORY_OWNER, GITHUB_REPOSITORY_NAME).Result;
            
            var versionAsStr = latestRelease.Name;
            var versionParts = versionAsStr.Split('.');
            if (versionParts.Length != 4) throw new ArgumentException("This string does not represents a version");

            Major = Convert.ToByte(versionParts[0]);
            Minor = Convert.ToByte(versionParts[1]);
            Patch = Convert.ToByte(versionParts[2]);
            Revision = Convert.ToByte(versionParts[3]);

            Body = latestRelease.Body;
            PublishedAt = latestRelease.PublishedAt;
            Assets = latestRelease.Assets;
        }
    }
}
