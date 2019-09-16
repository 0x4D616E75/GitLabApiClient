using System;
using System.Collections.Generic;
using GitLabApiClient.Internal.Utilities;

namespace GitLabApiClient.Models.Releases.Requests
{
    /// <summary>
    /// Used to update a link in a release.
    /// </summary>
    public sealed class UpdateLinkRequest
    {
        public string ProjectId { get; }

        public string TagName { get; }
        public string LinkId { get; }
        public string Name { get; set; }
        public string Url { get; set; }

        public IEnumerable<KeyValuePair<string,string>> GetContent()
        {
            var content = new List<KeyValuePair<string,string>>();
            if(!string.IsNullOrEmpty(Name))
                content.Add(new KeyValuePair<string,string>("name", Name));
            if(!string.IsNullOrEmpty(Name))
                content.Add(new KeyValuePair<string,string>("url", Url));
            return content;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLinkRequest"/> class.
        /// </summary>
        /// <param name="projectId">The ID or URL-encoded path of the project.</param>
        /// <param name="tagName">The name of the tag the release correspons to.</param>
        /// <param name="linkId">The id of the link.</param>
        /// <param name="name">The name of the link.</param>
        /// <param name="url">The URL of the link.</param>
        public UpdateLinkRequest(string projectId, string tagName, string linkId, string name, string url)
        {
            Guard.NotEmpty(projectId, nameof(projectId));
            Guard.NotEmpty(tagName, nameof(tagName));
            Guard.NotEmpty(linkId, nameof(linkId));
            Guard.OneIsNotEmpty(new [] { Tuple.Create(name, nameof(name)), Tuple.Create(url, nameof(url)) });

            ProjectId = projectId;
            TagName = tagName;
            LinkId = linkId;
            Name = name;
            Url = url;
        }
    }
}
