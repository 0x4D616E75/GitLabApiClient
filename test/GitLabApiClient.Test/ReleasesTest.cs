using FluentAssertions;
using System.Threading.Tasks;
using GitLabApiClient.Internal.Queries;
using GitLabApiClient.Models.Releases.Requests;
using Xunit;
using static GitLabApiClient.Test.Utilities.GitLabApiHelper;
using GitLabApiClient.Models.Releases.Responses;
using System;
using System.Collections.Generic;

namespace GitLabApiClient.Test
{
    [Trait("Category", "LinuxIntegration")]
    [Collection("GitLabContainerFixture")]
    public class ReeasesTest
    {
        private readonly ReleaseClient _sut = new ReleaseClient(GetFacade(), new ReleaseQueryBuilder());

        [Fact]
        public async Task CreatedReleaseCanBeUpdated()
        {
            //arrange
            var createdRelease = await _sut.CreateAsync(new CreateReleaseRequest(TestProjectTextId, TestRelease, TestTagName, TestDescription, DateTime.MinValue));

            //act
            var updatedRelease = await _sut.UpdateAsync(new UpdateReleaseRequest(TestProjectTextId, TestRelease, TestTagName, "Updated Description", DateTime.MinValue));

            //assert
            updatedRelease.Should().Match<Release>(i =>
                i.ProjectId == TestProjectTextId &&
                i.ReleaseName == TestRelease &&
                i.TagName == TestTagName &&
                i.ReleasedAt == DateTime.MinValue);
        }

        [Fact]
        public async Task CreatedReleaseCanBeFetched()
        {
            //arrange
            var createdRelease = await _sut.CreateAsync(new CreateReleaseRequest(TestProjectTextId, TestRelease, TestTagName, TestDescription, DateTime.MinValue));

            //act
            var fetchedRelease = await _sut.GetAsync(TestProjectTextId, TestTagName);

            //assert
            fetchedRelease.Should().Match<Release>(i =>
                i.ProjectId == TestProjectTextId &&
                i.ReleaseName == TestRelease &&
                i.TagName == TestTagName &&
                i.ReleasedAt == DateTime.MinValue);
        }

        [Fact]
        public async Task CreatedReleaseCanBeListed()
        {
            //arrange
            var createdRelease = await _sut.CreateAsync(new CreateReleaseRequest(TestProjectTextId, TestRelease, TestTagName, TestDescription, DateTime.MinValue));

            //act
            var releaseList = await _sut.GetAsync(TestProjectTextId, o => o.TagName = TestTagName);

            //assert
            releaseList[0].Should().Match<Release>(i =>
                i.ProjectId == TestProjectTextId &&
                i.ReleaseName == TestRelease &&
                i.TagName == TestTagName &&
                i.ReleasedAt == DateTime.MinValue);
        }

        [Fact]
        public async Task CreatedReleaseCanBeDeleted()
        {
            //arrange
            var createdRelease = await _sut.CreateAsync(new CreateReleaseRequest(TestProjectTextId, TestRelease, TestTagName, TestDescription, DateTime.MinValue));

            //act
            await _sut.DeleteAsync(new DeleteReleaseRequest(TestProjectTextId, TestTagName));

            //assert
            var fetcheRelease = await _sut.GetAsync(TestProjectTextId, TestTagName);
            fetcheRelease.Should().BeNull();
        }

        [Fact]
        public async Task LinkCanBeAddedToRelease()
        {
            //arrange
            var createdRelease = await _sut.CreateAsync(new CreateReleaseRequest(TestProjectTextId, TestRelease, TestTagName, TestDescription, DateTime.MinValue));

            //act
            var createdLink = await _sut.CreateLinkAsync(new CreateLinkRequest(TestProjectTextId, TestTagName, TestLinkName, TestLinkUrl));
            var releaseWithLink = await _sut.GetAsync(TestProjectTextId, TestTagName);

            //assert
            createdLink.Should().Match<Link>(i =>
                i.Name == TestLinkUrl &&
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
