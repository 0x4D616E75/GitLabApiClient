using System.Collections.Generic;
using GitLabApiClient.Internal.Utilities;
using GitLabApiClient.Models.Releases.Responses;

namespace GitLabApiClient.Models.Releases.Requests
{
    /// <summary>
    /// Used to create a link in a release.
    /// </summary>
    public sealed class CreateLinkRequest : Link
    {
        public string ProjectId { get; }

        public string TagName { get; }

        public IEnumerable<KeyValuePair<string,string>> GetContent() =>
            new [] { new KeyValuePair<string,string>("name", Name), new KeyValuePair<string,string>("url", Url)};

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateLinkRequest"/> class.
        /// </summary>
        /// <param name="projectId">The ID or URL-encoded path of the project.</param>
        /// <param name="tagName">The name of the tag the release correspons to.</param>
        /// <param name="name">The name of the link.</param>
        /// <param name="url">The URL of the link.</param>
        public CreateLinkRequest(string projectId, string tagName, string name, string url)
        {
            Guard.NotEmpty(projectId, nameof(projectId));
            Guard.NotEmpty(tagName, nameof(tagName));
            Guard.NotEmpty(name, nameof(name));
            Guard.NotEmpty(url, nameof(url));

            ProjectId = projectId;
            TagName = tagName;
            Name = name;
            Url = url;
        }
    }
}
