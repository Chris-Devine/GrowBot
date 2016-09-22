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
    public class LightSettingsControllerTests
    {
        private LightSettingController _controller;
        private List<GrowSetting> _data;
        private GrowSettingsMockData _growSettingsMockData;
        private Mock<IGrowSettingsRepository> _growSettingsRepositoryMock;
        private Mock<ILightSettingFactory> _lightSettingFactory;
        private MockRepository _mockRepo;
        private Mock<IUserHelper> _userHelperMock;


        [TestInitialize]
        public void MyTestInitialize()
        {
            _mockRepo = new MockRepository(MockBehavior.Strict);
            _growSettingsRepositoryMock = _mockRepo.Create<IGrowSettingsRepository>();
            _lightSettingFactory = _mockRepo.Create<ILightSettingFactory>();
            _userHelperMock = _mockRepo.Create<IUserHelper>();
            _growSettingsMockData = new GrowSettingsMockData();
            _data = _growSettingsMockData.GetMockUserGrowData();
            _controller = new LightSettingController(_growSettingsRepositoryMock.Object, _lightSettingFactory.Object,
                _userHelperMock.Object);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            _controller = null;
            _growSettingsRepositoryMock = null;
            _lightSettingFactory = null;
            _userHelperMock = null;
            _growSettingsMockData = null;
            _data = null;
        }

        #region GET List

        [TestMethod]
        public void GetAllLightSettings_ForACertainUsersGrowPhase()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.LightSetting)
                        .ToList());

            _lightSettingFactory.Setup(x => x.GetLightSetting(It.IsAny<LightSetting>()))
                .Returns((LightSetting x) => new LightSettingDto {LightSettingId = x.LightSettingId});

            // Act
            var actionResult = _controller.GetLightSettings(1, 1) as OkNegotiatedContentResult<List<LightSettingDto>>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.Count);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.GetLightSetting(It.IsAny<LightSetting>()), Times.Exactly(2));
        }

        [TestMethod]
        public void GetAllLightSettings_ForACertainUsersGrowPhase_ButUserHasNone()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1000)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.LightSetting)
                        .ToList());

            // Act
            IHttpActionResult actionResult = _controller.GetLightSettings(1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetAllLightSettings_ErrorCatching_Repository()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new IOException());

            // Act
            IHttpActionResult actionResult = _controller.GetLightSettings(1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetAllLightSettings_ErrorCatching_Factory()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.LightSetting)
                        .ToList());

            _lightSettingFactory.Setup(x => x.GetLightSetting(It.IsAny<LightSetting>()))
                .Throws(new IOException());

            // Act
            IHttpActionResult actionResult = _controller.GetLightSettings(1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.GetLightSetting(It.IsAny<LightSetting>()), Times.Exactly(1));
        }

        #endregion

        #region GET Single

        [TestMethod]
        public void GetLightSettings_ForACertainUsersGrowPhase()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.LightSetting)
                        .FirstOrDefault(x => x.LightSettingId == 2)
                );

            _lightSettingFactory.Setup(x => x.GetLightSetting(It.IsAny<LightSetting>()))
                .Returns((LightSetting x) => new LightSettingDto {LightSettingId = x.LightSettingId});

            // Act
            var actionResult = _controller.GetLightSetting(1, 1, 1) as OkNegotiatedContentResult<LightSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.LightSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.GetLightSetting(It.IsAny<LightSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetLightSettings_ForACertainUsersGrowPhase_ButUserHasNone()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.LightSetting)
                        .FirstOrDefault(x => x.LightSettingId == 2000)
                );

            // Act
            IHttpActionResult actionResult = _controller.GetLightSetting(1, 1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void GetLightSettings_ErrorCatching_Repository()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new Exception());

            // Act
            IHttpActionResult actionResult = _controller.GetLightSetting(1, 1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void GetLightSettings_ErrorCatching_Factory()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.LightSetting)
                        .FirstOrDefault(x => x.LightSettingId == 2)
                );

            _lightSettingFactory.Setup(x => x.GetLightSetting(It.IsAny<LightSetting>()))
                .Throws(new Exception());

            // Act
            IHttpActionResult actionResult = _controller.GetLightSetting(1, 1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.GetLightSetting(It.IsAny<LightSetting>()), Times.Exactly(1));
        }

        #endregion

        #region PUT

        [TestMethod]
        public void PutLightSettings_Updated()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.LightSetting)
                        .FirstOrDefault(x => x.LightSettingId == 2)
                );

            _lightSettingFactory.Setup(
                x => x.PutLightSetting(It.IsAny<LightSetting>(), It.IsAny<LightSettingDto>()))
                .Returns(new LightSetting {LightSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x =>
                    x.PutLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LightSetting>(),
                        It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<LightSetting>(new LightSetting {LightSettingId = 2},
                    RepositoryActionStatus.Updated));

            _lightSettingFactory.Setup(x => x.GetLightSetting(It.IsAny<LightSetting>()))
                .Returns((LightSetting x) => new LightSettingDto {LightSettingId = x.LightSettingId});

            // Act
            var actionResult =
                _controller.PutLightSetting(1, 1, 1, new LightSettingDto()) as
                    OkNegotiatedContentResult<LightSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.LightSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.PutLightSetting(It.IsAny<LightSetting>(), It.IsAny<LightSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x =>
                    x.PutLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LightSetting>(),
                        It.IsAny<Guid>()), Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.GetLightSetting(It.IsAny<LightSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutLightSettings_ErrorCatching()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.LightSetting)
                        .FirstOrDefault(x => x.LightSettingId == 2)
                );

            _lightSettingFactory.Setup(
                x => x.PutLightSetting(It.IsAny<LightSetting>(), It.IsAny<LightSettingDto>()))
                .Returns(new LightSetting {LightSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x =>
                    x.PutLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LightSetting>(),
                        It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<LightSetting>(new LightSetting {LightSettingId = 2},
                    RepositoryActionStatus.Error));

            // Act
            IHttpActionResult actionResult = _controller.PutLightSetting(1, 1, 1, new LightSettingDto());

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.PutLightSetting(It.IsAny<LightSetting>(), It.IsAny<LightSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x =>
                    x.PutLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LightSetting>(),
                        It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutLightSettings_NotFound()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.LightSetting)
                        .FirstOrDefault(x => x.LightSettingId == 2)
                );

            _lightSettingFactory.Setup(
                x => x.PutLightSetting(It.IsAny<LightSetting>(), It.IsAny<LightSettingDto>()))
                .Returns(new LightSetting {LightSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x =>
                    x.PutLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LightSetting>(),
                        It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<LightSetting>(new LightSetting {LightSettingId = 2},
                    RepositoryActionStatus.NotFound));

            // Act
            IHttpActionResult actionResult = _controller.PutLightSetting(1, 1, 1, new LightSettingDto());

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.PutLightSetting(It.IsAny<LightSetting>(), It.IsAny<LightSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x =>
                    x.PutLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LightSetting>(),
                        It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutLightSettings_NotModified()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.LightSetting)
                        .FirstOrDefault(x => x.LightSettingId == 2)
                );

            _lightSettingFactory.Setup(
                x => x.PutLightSetting(It.IsAny<LightSetting>(), It.IsAny<LightSettingDto>()))
                .Returns(new LightSetting {LightSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x =>
                    x.PutLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LightSetting>(),
                        It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<LightSetting>(new LightSetting {LightSettingId = 2},
                    RepositoryActionStatus.NothingModified));

            _lightSettingFactory.Setup(x => x.GetLightSetting(It.IsAny<LightSetting>()))
                .Returns((LightSetting x) => new LightSettingDto {LightSettingId = x.LightSettingId});

            // Act
            var actionResult =
                _controller.PutLightSetting(1, 1, 1, new LightSettingDto()) as
                    OkNegotiatedContentResult<LightSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.LightSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.PutLightSetting(It.IsAny<LightSetting>(), It.IsAny<LightSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x =>
                    x.PutLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<LightSetting>(),
                        It.IsAny<Guid>()), Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.GetLightSetting(It.IsAny<LightSetting>()), Times.Exactly(1));
        }

        #endregion

        #region POST

        [TestMethod]
        public void PostLightSettings_Created()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _lightSettingFactory.Setup(x => x.PostLightSetting(It.IsAny<LightSettingDto>()))
                .Returns(new LightSetting {LightSettingId = 1});

            _growSettingsRepositoryMock.Setup(o => o.PostLightSetting(It.IsAny<LightSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<LightSetting>(new LightSetting {LightSettingId = 1},
                    RepositoryActionStatus.Created));

            _lightSettingFactory.Setup(x => x.GetLightSetting(It.IsAny<LightSetting>()))
                .Returns(new LightSettingDto {LightSettingId = 1});

            _controller.Request = new HttpRequestMessage {RequestUri = new Uri("http://www.unit-test.com")};

            // Act
            var actionResult =
                _controller.PostLightSetting(It.IsAny<int>(), It.IsAny<int>(), new LightSettingDto {LightSettingId = 1})
                    as CreatedNegotiatedContentResult<LightSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(1, actionResult.Content.LightSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.PostLightSetting(It.IsAny<LightSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.PostLightSetting(It.IsAny<LightSetting>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.GetLightSetting(It.IsAny<LightSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PostLightSettings_ErrorCatching()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _lightSettingFactory.Setup(x => x.PostLightSetting(It.IsAny<LightSettingDto>()))
                .Returns(new LightSetting {LightSettingId = 1});

            _growSettingsRepositoryMock.Setup(o => o.PostLightSetting(It.IsAny<LightSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<LightSetting>(new LightSetting {LightSettingId = 1},
                    RepositoryActionStatus.Error));

            _controller.Request = new HttpRequestMessage {RequestUri = new Uri("http://www.unit-test.com")};

            // Act
            IHttpActionResult actionResult = _controller.PostLightSetting(It.IsAny<int>(), It.IsAny<int>(),
                new LightSettingDto {LightSettingId = 1});

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _lightSettingFactory.Verify(x => x.PostLightSetting(It.IsAny<LightSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.PostLightSetting(It.IsAny<LightSetting>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        #endregion

        #region DELETE

        [TestMethod]
        public void DeleteLightSettings_Deleted()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<LightSetting>(null, RepositoryActionStatus.Deleted));

            // Act
            var actionResult = _controller.DeleteLightSetting(1, 1, 1) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(actionResult.StatusCode, HttpStatusCode.NoContent);

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void DeleteLightSettings_NotFound()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<LightSetting>(null, RepositoryActionStatus.NotFound));


            // Act
            IHttpActionResult actionResult = _controller.DeleteLightSetting(999, 999, 999);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void DeleteLightSettings_CatchError()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new Exception());


            // Act
            IHttpActionResult actionResult = _controller.DeleteLightSetting(1, 1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteLightSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        #endregion
    }
}