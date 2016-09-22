using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using GrowBot.API.Controllers.GrowSettings;
using GrowBot.API.Entities.GrowSettings;
using GrowBot.API.Factories.GrowSettings.Interfaces;
using GrowBot.API.Helpers.HelpersInterfaces;
using GrowBot.API.Repository;
using GrowBot.API.Repository.Repositories.GrowSettings;
using GrowBot.DTO.GrowSettings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GrowBot.UnitTests.GrowBot.Api.Controllers.GrowSettings
{
    [TestClass]
    public class GrowPhaseSettingsControllerTests
    {
        private GrowPhaseSettingsController _controller;
        private List<GrowSetting> _data;
        private GrowSettingsMockData _growSettingsMockData;
        private Mock<IGrowSettingsRepository> _growSettingsRepositoryMock;
        private MockRepository _mockRepo;
        private Mock<IGrowPhaseSettingFactory> _GrowPhaseSettingFactoryMock;
        private Mock<IUserHelper> _userHelperMock;


        [TestInitialize]
        public void MyTestInitialize()
        {
            _mockRepo = new MockRepository(MockBehavior.Strict);
            _growSettingsRepositoryMock = _mockRepo.Create<IGrowSettingsRepository>();
            _GrowPhaseSettingFactoryMock = _mockRepo.Create<IGrowPhaseSettingFactory>();
            _userHelperMock = _mockRepo.Create<IUserHelper>();
            _growSettingsMockData = new GrowSettingsMockData();
            _data = _growSettingsMockData.GetMockUserGrowData();
            _controller = new GrowPhaseSettingsController(_growSettingsRepositoryMock.Object,
                _GrowPhaseSettingFactoryMock.Object, _userHelperMock.Object);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            _controller = null;
            _growSettingsRepositoryMock = null;
            _GrowPhaseSettingFactoryMock = null;
            _userHelperMock = null;
            _growSettingsMockData = null;
            _data = null;
        }

        #region GET List

        [TestMethod]
        public void GetAllUserGrowsPhases_ForACertainUsersGrow()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(x => x.GetGrowPhaseSettings(It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(_data.Where(x => x.GrowSettingId == 1).SelectMany(x => x.GrowPhaseSetting).ToList());

            _GrowPhaseSettingFactoryMock.Setup(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()))
                .Returns((GrowPhaseSetting x) => new GrowPhaseSettingDto {GrowPhaseSettingId = x.GrowPhaseSettingId});

            // Act
            var actionResult = _controller.GetGrowPhaseSetting(1) as OkNegotiatedContentResult<List<GrowPhaseSettingDto>>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(3, actionResult.Content.Count);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowPhaseSettings(It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()), Times.Exactly(3));
        }

        [TestMethod]
        public void GetAllUserGrowsPhases_ForACertainUsersGrow_ButUserHasNone()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(x => x.GetGrowPhaseSettings(It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(_data.Where(x => x.GrowSettingId == 100).SelectMany(x => x.GrowPhaseSetting).ToList());

            // Act
            IHttpActionResult actionResult = _controller.GetGrowPhaseSetting(1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowPhaseSettings(It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void GetAllUserGrowsPhases_ErrorCatching_Repository()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(x => x.GetGrowPhaseSettings(It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new IOException());

            // Act
            IHttpActionResult actionResult = _controller.GetGrowPhaseSetting(1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowPhaseSettings(It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void GetAllUserGrowsPhases_ErrorCatching_Factory()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(x => x.GetGrowPhaseSettings(It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(_data.Where(x => x.GrowSettingId == 1).SelectMany(x => x.GrowPhaseSetting).ToList());

            _GrowPhaseSettingFactoryMock.Setup(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()))
                .Throws(new IOException());

            // Act
            IHttpActionResult actionResult = _controller.GetGrowPhaseSetting(1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowPhaseSettings(It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()), Times.Exactly(1));
        }

        #endregion

        #region GET Single

        [TestMethod]
        public void GetUserGrowsPhase_ForACertainUsersGrow()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(_data.Where(x => x.GrowSettingId == 1).SelectMany(x => x.GrowPhaseSetting).FirstOrDefault());

            _GrowPhaseSettingFactoryMock.Setup(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()))
                .Returns((GrowPhaseSetting x) => new GrowPhaseSettingDto {GrowPhaseSettingId = x.GrowPhaseSettingId});

            // Act
            var actionResult = _controller.GetGrowPhaseSetting(1, 1) as OkNegotiatedContentResult<GrowPhaseSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(1, actionResult.Content.GrowPhaseSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetUserGrowsPhase_NoneExistingID()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(_data.Where(x => x.GrowSettingId == 100).SelectMany(x => x.GrowPhaseSetting).FirstOrDefault());

            // Act
            IHttpActionResult actionResult = _controller.GetGrowPhaseSetting(1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetUserGrowsPhase_ErrorCatching_Repository()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new Exception());

            // Act
            IHttpActionResult actionResult = _controller.GetGrowPhaseSetting(1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetUserGrowsPhase_ErrorCatching_Factory()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(_data.Where(x => x.GrowSettingId == 1).SelectMany(x => x.GrowPhaseSetting).FirstOrDefault());

            _GrowPhaseSettingFactoryMock.Setup(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()))
                .Throws(new Exception());

            // Act
            IHttpActionResult actionResult = _controller.GetGrowPhaseSetting(1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()), Times.Exactly(1));
        }

        #endregion

        #region PUT

        [TestMethod]
        public void PutGrowPhaseSetting_Updated()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(_data.Where(x => x.GrowSettingId == 1).SelectMany(x => x.GrowPhaseSetting).FirstOrDefault());

            _GrowPhaseSettingFactoryMock.Setup(
                x => x.PutGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<GrowPhaseSettingDto>()))
                .Returns(new GrowPhaseSetting {GrowPhaseSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x => x.PutGrowPhaseSetting(It.IsAny<int>(), It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowPhaseSetting>(new GrowPhaseSetting {GrowPhaseSettingId = 2},
                    RepositoryActionStatus.Updated));

            _GrowPhaseSettingFactoryMock.Setup(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()))
                .Returns((GrowPhaseSetting x) => new GrowPhaseSettingDto {GrowPhaseSettingId = x.GrowPhaseSettingId});

            // Act
            var actionResult =
                _controller.PutGrowPhaseSetting(1, 1, new GrowPhaseSettingDto()) as
                    OkNegotiatedContentResult<GrowPhaseSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.GrowPhaseSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(
                x => x.PutGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<GrowPhaseSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutGrowPhaseSetting(It.IsAny<int>(), It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutGrowPhaseSetting_ErrorCatching()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(_data.Where(x => x.GrowSettingId == 1).SelectMany(x => x.GrowPhaseSetting).FirstOrDefault());

            _GrowPhaseSettingFactoryMock.Setup(
                x => x.PutGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<GrowPhaseSettingDto>()))
                .Returns(new GrowPhaseSetting {GrowPhaseSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x => x.PutGrowPhaseSetting(It.IsAny<int>(), It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowPhaseSetting>(new GrowPhaseSetting {GrowPhaseSettingId = 2},
                    RepositoryActionStatus.Error));

            // Act
            IHttpActionResult actionResult = _controller.PutGrowPhaseSetting(1, 1, new GrowPhaseSettingDto());

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(
                x => x.PutGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<GrowPhaseSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutGrowPhaseSetting(It.IsAny<int>(), It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutGrowPhaseSetting_NotFound()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(_data.Where(x => x.GrowSettingId == 1).SelectMany(x => x.GrowPhaseSetting).FirstOrDefault());

            _GrowPhaseSettingFactoryMock.Setup(
                x => x.PutGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<GrowPhaseSettingDto>()))
                .Returns(new GrowPhaseSetting {GrowPhaseSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x => x.PutGrowPhaseSetting(It.IsAny<int>(), It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowPhaseSetting>(new GrowPhaseSetting {GrowPhaseSettingId = 2},
                    RepositoryActionStatus.NotFound));

            // Act
            IHttpActionResult actionResult = _controller.PutGrowPhaseSetting(1, 1, new GrowPhaseSettingDto());

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(
                x => x.PutGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<GrowPhaseSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutGrowPhaseSetting(It.IsAny<int>(), It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutGrowPhaseSetting_NotModified()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(_data.Where(x => x.GrowSettingId == 1).SelectMany(x => x.GrowPhaseSetting).FirstOrDefault());

            _GrowPhaseSettingFactoryMock.Setup(
                x => x.PutGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<GrowPhaseSettingDto>()))
                .Returns(new GrowPhaseSetting {GrowPhaseSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x => x.PutGrowPhaseSetting(It.IsAny<int>(), It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowPhaseSetting>(new GrowPhaseSetting {GrowPhaseSettingId = 2},
                    RepositoryActionStatus.NothingModified));

            _GrowPhaseSettingFactoryMock.Setup(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()))
                .Returns((GrowPhaseSetting x) => new GrowPhaseSettingDto {GrowPhaseSettingId = x.GrowPhaseSettingId});

            // Act
            var actionResult =
                _controller.PutGrowPhaseSetting(1, 1, new GrowPhaseSettingDto()) as
                    OkNegotiatedContentResult<GrowPhaseSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.GrowPhaseSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(
                x => x.PutGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<GrowPhaseSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutGrowPhaseSetting(It.IsAny<int>(), It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()), Times.Exactly(1));
        }

        #endregion

        #region POST

        [TestMethod]
        public void PostGrowPhaseSetting_Created()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _GrowPhaseSettingFactoryMock.Setup(x => x.PostGrowPhaseSetting(It.IsAny<GrowPhaseSettingDto>()))
                .Returns(new GrowPhaseSetting {GrowSettingId = 1});

            _growSettingsRepositoryMock.Setup(o => o.PostGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowPhaseSetting>(new GrowPhaseSetting {GrowSettingId = 1},
                    RepositoryActionStatus.Created));

            _GrowPhaseSettingFactoryMock.Setup(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()))
                .Returns(new GrowPhaseSettingDto {GrowSettingId = 1});

            _controller.Request = new HttpRequestMessage {RequestUri = new Uri("http://www.unit-test.com")};

            // Act
            var actionResult =
                _controller.PostGrowPhaseSetting(new GrowPhaseSettingDto {GrowSettingId = 1}) as
                    CreatedNegotiatedContentResult<GrowPhaseSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(1, actionResult.Content.GrowSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(x => x.PostGrowPhaseSetting(It.IsAny<GrowPhaseSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.PostGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(x => x.GetGrowPhaseSetting(It.IsAny<GrowPhaseSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PostGrowPhaseSetting_ErrorCatching()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _GrowPhaseSettingFactoryMock.Setup(x => x.PostGrowPhaseSetting(It.IsAny<GrowPhaseSettingDto>()))
                .Returns(new GrowPhaseSetting {GrowSettingId = 1});

            _growSettingsRepositoryMock.Setup(o => o.PostGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowPhaseSetting>(new GrowPhaseSetting {GrowSettingId = 1},
                    RepositoryActionStatus.Error));

            _controller.Request = new HttpRequestMessage {RequestUri = new Uri("http://www.unit-test.com")};

            // Act
            IHttpActionResult actionResult = _controller.PostGrowPhaseSetting(new GrowPhaseSettingDto {GrowSettingId = 1});

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _GrowPhaseSettingFactoryMock.Verify(x => x.PostGrowPhaseSetting(It.IsAny<GrowPhaseSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.PostGrowPhaseSetting(It.IsAny<GrowPhaseSetting>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        #endregion

        #region DELETE

        [TestMethod]
        public void DeleteGrowPhaseSetting_Deleted()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowPhaseSetting>(null, RepositoryActionStatus.Deleted));

            // Act
            var actionResult = _controller.DeleteGrowPhaseSetting(1, 1) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(actionResult.StatusCode, HttpStatusCode.NoContent);

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void DeleteGrowPhaseSetting_NotFound()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowPhaseSetting>(null, RepositoryActionStatus.NotFound));


            // Act
            IHttpActionResult actionResult = _controller.DeleteGrowPhaseSetting(999, 999);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void DeleteUserGrow_CatchError()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new Exception());


            // Act
            IHttpActionResult actionResult = _controller.DeleteGrowPhaseSetting(1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteGrowPhaseSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        #endregion
    }
}