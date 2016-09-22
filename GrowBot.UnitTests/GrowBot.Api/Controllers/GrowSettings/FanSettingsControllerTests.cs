using System;
using System.Collections.Generic;
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
    public class FanSettingsControllerTests
    {
        private FanSettingController _controller;
        private List<GrowSetting> _data;
        private Mock<IFanSettingFactory> _fanSettingFactory;
        private GrowSettingsMockData _growSettingsMockData;
        private Mock<IGrowSettingsRepository> _growSettingsRepositoryMock;
        private MockRepository _mockRepo;
        private Mock<IUserHelper> _userHelperMock;


        [TestInitialize]
        public void MyTestInitialize()
        {
            _mockRepo = new MockRepository(MockBehavior.Strict);
            _growSettingsRepositoryMock = _mockRepo.Create<IGrowSettingsRepository>();
            _fanSettingFactory = _mockRepo.Create<IFanSettingFactory>();
            _userHelperMock = _mockRepo.Create<IUserHelper>();
            _growSettingsMockData = new GrowSettingsMockData();
            _data = _growSettingsMockData.GetMockUserGrowData();
            _controller = new FanSettingController(_growSettingsRepositoryMock.Object, _fanSettingFactory.Object,
                _userHelperMock.Object);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            _controller = null;
            _growSettingsRepositoryMock = null;
            _fanSettingFactory = null;
            _userHelperMock = null;
            _growSettingsMockData = null;
            _data = null;
        }

        #region GET Single

        [TestMethod]
        public void GetFanSettings_ForACertainUsersGrowPhase()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 2)
                        .SelectMany(x => x.FanSetting)
                        .FirstOrDefault(x => x.FanSettingId == 2)
                );

            _fanSettingFactory.Setup(x => x.GetFanSetting(It.IsAny<FanSetting>()))
                .Returns((FanSetting x) => new FanSettingDto {FanSettingId = x.FanSettingId});

            // Act
            var actionResult = _controller.GetFanSetting(1, 1, 1) as OkNegotiatedContentResult<FanSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.FanSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _fanSettingFactory.Verify(x => x.GetFanSetting(It.IsAny<FanSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetFanSettings_ForACertainUsersGrowPhase_ButUserHasNone()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.FanSetting)
                        .FirstOrDefault(x => x.FanSettingId == 2000)
                );

            // Act
            IHttpActionResult actionResult = _controller.GetFanSetting(1, 1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void GetFanSettings_ErrorCatching_Repository()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new Exception());

            // Act
            IHttpActionResult actionResult = _controller.GetFanSetting(1, 1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void GetFanSettings_ErrorCatching_Factory()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 2)
                        .SelectMany(x => x.FanSetting)
                        .FirstOrDefault(x => x.FanSettingId == 2)
                );

            _fanSettingFactory.Setup(x => x.GetFanSetting(It.IsAny<FanSetting>()))
                .Throws(new Exception());

            // Act
            IHttpActionResult actionResult = _controller.GetFanSetting(1, 1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _fanSettingFactory.Verify(x => x.GetFanSetting(It.IsAny<FanSetting>()), Times.Exactly(1));
        }

        #endregion

        #region PUT

        [TestMethod]
        public void PutFanSettings_Updated()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 2)
                        .SelectMany(x => x.FanSetting)
                        .FirstOrDefault(x => x.FanSettingId == 2)
                );

            _fanSettingFactory.Setup(
                x => x.PutFanSetting(It.IsAny<FanSetting>(), It.IsAny<FanSettingDto>()))
                .Returns(new FanSetting {FanSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x => x.PutFanSetting(It.IsAny<int>(), It.IsAny<FanSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<FanSetting>(new FanSetting {FanSettingId = 2},
                    RepositoryActionStatus.Updated));

            _fanSettingFactory.Setup(x => x.GetFanSetting(It.IsAny<FanSetting>()))
                .Returns((FanSetting x) => new FanSettingDto {FanSettingId = x.FanSettingId});

            // Act
            var actionResult =
                _controller.PutFanSetting(1, 1, 1, new FanSettingDto()) as OkNegotiatedContentResult<FanSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.FanSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _fanSettingFactory.Verify(x => x.PutFanSetting(It.IsAny<FanSetting>(), It.IsAny<FanSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutFanSetting(It.IsAny<int>(), It.IsAny<FanSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
            _fanSettingFactory.Verify(x => x.GetFanSetting(It.IsAny<FanSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutFanSettings_ErrorCatching()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 2)
                        .SelectMany(x => x.FanSetting)
                        .FirstOrDefault(x => x.FanSettingId == 2)
                );

            _fanSettingFactory.Setup(
                x => x.PutFanSetting(It.IsAny<FanSetting>(), It.IsAny<FanSettingDto>()))
                .Returns(new FanSetting {FanSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x => x.PutFanSetting(It.IsAny<int>(), It.IsAny<FanSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<FanSetting>(new FanSetting {FanSettingId = 2},
                    RepositoryActionStatus.Error));

            // Act
            IHttpActionResult actionResult = _controller.PutFanSetting(1, 1, 1, new FanSettingDto());

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _fanSettingFactory.Verify(x => x.PutFanSetting(It.IsAny<FanSetting>(), It.IsAny<FanSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutFanSetting(It.IsAny<int>(), It.IsAny<FanSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutFanSettings_NotFound()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 2)
                        .SelectMany(x => x.FanSetting)
                        .FirstOrDefault(x => x.FanSettingId == 2)
                );

            _fanSettingFactory.Setup(
                x => x.PutFanSetting(It.IsAny<FanSetting>(), It.IsAny<FanSettingDto>()))
                .Returns(new FanSetting {FanSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x => x.PutFanSetting(It.IsAny<int>(), It.IsAny<FanSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<FanSetting>(new FanSetting {FanSettingId = 2},
                    RepositoryActionStatus.NotFound));

            // Act
            IHttpActionResult actionResult = _controller.PutFanSetting(1, 1, 1, new FanSettingDto());

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _fanSettingFactory.Verify(x => x.PutFanSetting(It.IsAny<FanSetting>(), It.IsAny<FanSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutFanSetting(It.IsAny<int>(), It.IsAny<FanSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutFanSettings_NotModified()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 2)
                        .SelectMany(x => x.FanSetting)
                        .FirstOrDefault(x => x.FanSettingId == 2)
                );

            _fanSettingFactory.Setup(
                x => x.PutFanSetting(It.IsAny<FanSetting>(), It.IsAny<FanSettingDto>()))
                .Returns(new FanSetting {FanSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x => x.PutFanSetting(It.IsAny<int>(), It.IsAny<FanSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<FanSetting>(new FanSetting {FanSettingId = 2},
                    RepositoryActionStatus.NothingModified));

            _fanSettingFactory.Setup(x => x.GetFanSetting(It.IsAny<FanSetting>()))
                .Returns((FanSetting x) => new FanSettingDto {FanSettingId = x.FanSettingId});

            // Act
            var actionResult =
                _controller.PutFanSetting(1, 1, 1, new FanSettingDto()) as OkNegotiatedContentResult<FanSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.FanSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _fanSettingFactory.Verify(x => x.PutFanSetting(It.IsAny<FanSetting>(), It.IsAny<FanSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutFanSetting(It.IsAny<int>(), It.IsAny<FanSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
            _fanSettingFactory.Verify(x => x.GetFanSetting(It.IsAny<FanSetting>()), Times.Exactly(1));
        }

        #endregion

        #region POST

        [TestMethod]
        public void PostFanSettings_Created()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _fanSettingFactory.Setup(x => x.PostFanSetting(It.IsAny<FanSettingDto>()))
                .Returns(new FanSetting {FanSettingId = 1});

            _growSettingsRepositoryMock.Setup(o => o.PostFanSetting(It.IsAny<FanSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<FanSetting>(new FanSetting {FanSettingId = 1},
                    RepositoryActionStatus.Created));

            _fanSettingFactory.Setup(x => x.GetFanSetting(It.IsAny<FanSetting>()))
                .Returns(new FanSettingDto {FanSettingId = 1});

            _controller.Request = new HttpRequestMessage {RequestUri = new Uri("http://www.unit-test.com")};

            // Act
            var actionResult =
                _controller.PostFanSetting(new FanSettingDto {FanSettingId = 1}) as
                    CreatedNegotiatedContentResult<FanSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(1, actionResult.Content.FanSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _fanSettingFactory.Verify(x => x.PostFanSetting(It.IsAny<FanSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.PostFanSetting(It.IsAny<FanSetting>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _fanSettingFactory.Verify(x => x.GetFanSetting(It.IsAny<FanSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PostFanSettings_ErrorCatching()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _fanSettingFactory.Setup(x => x.PostFanSetting(It.IsAny<FanSettingDto>()))
                .Returns(new FanSetting {FanSettingId = 1});

            _growSettingsRepositoryMock.Setup(o => o.PostFanSetting(It.IsAny<FanSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<FanSetting>(new FanSetting {FanSettingId = 1},
                    RepositoryActionStatus.Error));

            _controller.Request = new HttpRequestMessage {RequestUri = new Uri("http://www.unit-test.com")};

            // Act
            IHttpActionResult actionResult = _controller.PostFanSetting(new FanSettingDto {FanSettingId = 1});

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _fanSettingFactory.Verify(x => x.PostFanSetting(It.IsAny<FanSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.PostFanSetting(It.IsAny<FanSetting>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        #endregion

        #region DELETE

        [TestMethod]
        public void DeleteFanSettings_Deleted()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<FanSetting>(null, RepositoryActionStatus.Deleted));

            // Act
            var actionResult = _controller.DeleteFanSetting(1, 1) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(actionResult.StatusCode, HttpStatusCode.NoContent);

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void DeleteFanSettings_NotFound()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<FanSetting>(null, RepositoryActionStatus.NotFound));


            // Act
            IHttpActionResult actionResult = _controller.DeleteFanSetting(999, 999);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void DeleteFanSettings_CatchError()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new Exception());


            // Act
            IHttpActionResult actionResult = _controller.DeleteFanSetting(1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteFanSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        #endregion
    }
}