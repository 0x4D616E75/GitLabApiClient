using GitLabApiClient.Models.Releases.Requests;

namespace GitLabApiClient.Internal.Queries
{
    class LinkQueryBuilder : QueryBuilder<LinkQueryOptions>
    {
        protected override void BuildCore(LinkQueryOptions options)
        {
            if (!string.IsNullOrEmpty(options.ProjectId))
                Add("id", options.ProjectId);

            if (!string.IsNullOrEmpty(options.TagName))
                Add("tag_name", options.TagName);
        }
    }
}
