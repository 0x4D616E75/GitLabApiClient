using GitLabApiClient.Internal.Utilities;

namespace GitLabApiClient.Models.Releases.Requests
{
    /// <summary>
    /// Used to delete a link in a release.
    /// </summary>
    public sealed class DeleteLinkRequest
    {
        public string ProjectId { get; }
        public string TagName { get; }
        public string LinkId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteLinkRequest"/> class
        /// </summary>
        /// <param name="projectId">The ID or URL-encoded path of the project.</param>
        /// <param name="tagName">The name of the tag which corresponds to the release</param>
        /// <param name="linkId">The id of the link.</param>
        public DeleteLinkRequest(string projectId, string tagName, string linkId)
        {
            Guard.NotEmpty(projectId, nameof(projectId));
            Guard.NotEmpty(tagName, nameof(tagName));
            Guard.NotEmpty(linkId, nameof(linkId));

            ProjectId = projectId;
            TagName = tagName;
            LinkId = linkId;
        }
    }
}
