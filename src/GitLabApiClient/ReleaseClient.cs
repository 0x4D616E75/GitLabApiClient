using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitLabApiClient.Internal.Http;
using GitLabApiClient.Internal.Queries;
using GitLabApiClient.Models.Releases.Requests;
using GitLabApiClient.Models.Releases.Responses;

namespace GitLabApiClient
{
    public sealed class ReleaseClient
    {
        private readonly GitLabHttpFacade _httpFacade;
        private readonly ReleaseQueryBuilder _releaseQueryBuilder;

        internal ReleaseClient(
            GitLabHttpFacade httpFacade,
            ReleaseQueryBuilder releaseQueryBuilder)
        {
            _httpFacade = httpFacade;
            _releaseQueryBuilder = releaseQueryBuilder;
        }

        public async Task<Release> GetAsync(string projectId, string tagName) =>
            await _httpFacade.Get<Release>($"projects/{projectId}/releases/{tagName}");

        public async Task<IList<Release>> GetAsync(string projectId, Action<ReleaseQueryOptions> options = null)
        {
            var queryOptions = new ReleaseQueryOptions();
            options?.Invoke(queryOptions);

            string url = _releaseQueryBuilder.Build($"projects/{projectId}/releases", queryOptions);
            return await _httpFacade.GetPagedList<Release>(url);
        }

        public async Task<Release> CreateAsync(CreateReleaseRequest request) =>
            await _httpFacade.Post<Release>($"projects/{request.ProjectId}/releases/", request);

        public async Task<Release> UpdateAsync(UpdateReleaseRequest request) =>
            await _httpFacade.Put<Release>($"projects/{request.ProjectId}/releases/", request);

        public async Task DeleteAsync(DeleteReleaseRequest request) =>
            await _httpFacade.Delete($"projects/{request.ProjectId}/releases/{request.TagName}");





        public async Task<Link> GetLinkAsync(string projectId, string tagName, string linkId) =>
            await _httpFacade.Get<Link>($"projects/{projectId}/releases/{tagName}/assets/links/{linkId}");

        public async Task<IList<Link>> GetLinksAsync(string projectId, string tagName) =>
            await _httpFacade.GetPagedList<Link>($"projects/{projectId}/releases/{tagName}/assets/links");

        public async Task<Link> CreateLinkAsync(CreateLinkRequest request) =>
            await _httpFacade.PostLink($"projects/{request.ProjectId}/releases/{request.TagName}/assets/links/", request);

        public async Task<Link> UpdateLinkAsync(UpdateLinkRequest request) =>
            await _httpFacade.PutLink($"projects/{request.ProjectId}/releases/{request.TagName}/assets/links/", request);

        public async Task DeleteAsync(DeleteLinkRequest request) =>
            await _httpFacade.Delete($"projects/{request.ProjectId}/releases/{request.TagName}/assets/links/{request.LinkId}");
    }
}
