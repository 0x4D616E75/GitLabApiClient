using FluentAssertions;
using System.Threading.Tasks;
using GitLabApiClient.Internal.Queries;
using GitLabApiClient.Models.Releases.Requests;
using Xunit;
using static GitLabApiClient.Test.Utilities.GitLabApiHelper;
using GitLabApiClient.Models.Releases.Responses;
using System;
using System.Collections.Generic;
using GitLabApiClient.Models.Tags.Requests;
using System.Net;

namespace GitLabApiClient.Test
{
    [Trait("Category", "LinuxIntegration")]
    [Collection("GitLabContainerFixture")]
    public class ReleasesTest
    {
        private readonly TagClient _tclient = new TagClient(GetFacade(), new TagQueryBuilder());
        private readonly ReleaseClient _sut = new ReleaseClient(GetFacade(), new ReleaseQueryBuilder());

        [Fact]
        public async Task CreatedReleaseCanBeUpdated()
        {
            //arrange
            var releaseName = $"{nameof(CreatedReleaseCanBeUpdated)}-{Guid.NewGuid()}";
            var tagName = releaseName;
            var tag = await _tclient.CreateAsync(new CreateTagRequest(TestProjectTextId, tagName, "master", null, null));
            var createdRelease = await _sut.CreateAsync(new CreateReleaseRequest(TestProjectTextId, releaseName, tagName, TestDescription, DateTime.MinValue));

            //act
            var updatedRelease = await _sut.UpdateAsync(new UpdateReleaseRequest(TestProjectTextId, releaseName, tagName, "Updated Description", DateTime.MinValue));

            //assert
            updatedRelease.Should().Match<Release>(i =>
                // Project id isn't part of response
                i.ReleaseName == releaseName &&
                i.TagName == tagName &&
                i.ReleasedAt == DateTime.MinValue);
        }

        [Fact]
        public async Task CreatedReleaseCanBeFetched()
        {
            //arrange
            var releaseName = $"{nameof(CreatedReleaseCanBeFetched)}-{Guid.NewGuid()}";
            var tagName = releaseName;
            var tag = await _tclient.CreateAsync(new CreateTagRequest(TestProjectTextId, tagName, "master", null, null));
            var createdRelease = await _sut.CreateAsync(new CreateReleaseRequest(TestProjectTextId, releaseName, tagName, TestDescription, DateTime.MinValue));

            //act
            var fetchedRelease = await _sut.GetAsync(TestProjectTextId, tagName);

            //assert
            fetchedRelease.Should().Match<Release>(i =>
                // Project id isn't part of response
                i.ReleaseName == releaseName &&
                i.TagName == tagName &&
                i.ReleasedAt == DateTime.MinValue);
        }

        [Fact]
        public async Task CreatedReleaseCanBeListed()
        {
            //arrange
            var releaseName = $"{nameof(CreatedReleaseCanBeListed)}-{Guid.NewGuid()}";
            var tagName = releaseName;
            var tag = await _tclient.CreateAsync(new CreateTagRequest(TestProjectTextId, tagName, "master", null, null));
            var createdRelease = await _sut.CreateAsync(new CreateReleaseRequest(TestProjectTextId, releaseName, tagName, TestDescription, DateTime.MinValue));

            //act
            var releaseList = await _sut.GetAsync(TestProjectTextId);

            //assert
            releaseList[0].Should().Match<Release>(i =>
                i.ProjectId == TestProjectTextId &&
                i.ReleaseName == releaseName &&
                i.TagName == tagName &&
                i.ReleasedAt == DateTime.MinValue);
        }

        [Fact]
        public async Task CreatedReleaseCanBeDeleted()
        {
            //arrange
            var releaseName = $"{nameof(CreatedReleaseCanBeDeleted)}-{Guid.NewGuid()}";
            var tagName = releaseName;
            var tag = await _tclient.CreateAsync(new CreateTagRequest(TestProjectTextId, tagName, "master", null, null));
            var createdRelease = await _sut.CreateAsync(new CreateReleaseRequest(TestProjectTextId, releaseName, tagName, TestDescription, DateTime.MinValue));

            //act
            await _sut.DeleteAsync(new DeleteReleaseRequest(TestProjectTextId, tagName));

            //assert
            Func<Task<Release>> getAction = () => _sut.GetAsync(TestProjectTextId, tagName);
            getAction.ShouldThrow<GitLabException>().
                WithMessage("{\"message\":\"403 Forbidden\"}").
                Where(e => e.HttpStatusCode == HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LinkCanBeAddedToRelease()
        {
            //arrange
            var releaseName = $"{nameof(LinkCanBeAddedToRelease)}-{Guid.NewGuid()}";
            var tagName = releaseName;
            var tag = await _tclient.CreateAsync(new CreateTagRequest(TestProjectTextId, tagName, "master", null, null));
            var createdRelease = await _sut.CreateAsync(new CreateReleaseRequest(TestProjectTextId, releaseName, tagName, TestDescription, DateTime.MinValue));

            //act
            var createdLink = await _sut.CreateLinkAsync(new CreateLinkRequest(TestProjectTextId, tagName, TestLinkName, TestLinkUrl));
            var releaseWithLink = await _sut.GetAsync(TestProjectTextId, tagName);

            //assert
            createdLink.Should().Match<Link>(i =>
                i.Name == TestLinkName &&
                i.Url == TestLinkUrl);
            releaseWithLink.Should().Match<Release>(i =>
                i.Assets != null &&
                i.Assets.Links != null &&
                i.Assets.Links.Length == 1 &&
                i.Assets.Links[0].Id == createdLink.Id &&
                i.Assets.Links[0].Name == createdLink.Name &&
                i.Assets.Links[0].Url == createdLink.Url &&
                i.Assets.Links[0].External == createdLink.External);
        }
    }
}
