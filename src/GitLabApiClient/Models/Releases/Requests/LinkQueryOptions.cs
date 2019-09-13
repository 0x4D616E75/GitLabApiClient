namespace GitLabApiClient.Models.Releases.Requests
{
    public sealed class LinkQueryOptions
    {
        public string ProjectId { get; set; }
        public string TagName { get; set; }

        internal LinkQueryOptions(string projectId = null) => ProjectId = projectId;
    }
}
