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
    public class WaterSettingsControllerTests
    {
        private WaterSettingController _controller;
        private List<GrowSetting> _data;
        private GrowSettingsMockData _growSettingsMockData;
        private Mock<IGrowSettingsRepository> _growSettingsRepositoryMock;
        private MockRepository _mockRepo;
        private Mock<IUserHelper> _userHelperMock;
        private Mock<IWaterSettingFactory> _waterSettingFactory;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _mockRepo = new MockRepository(MockBehavior.Strict);
            _growSettingsRepositoryMock = _mockRepo.Create<IGrowSettingsRepository>();
            _waterSettingFactory = _mockRepo.Create<IWaterSettingFactory>();
            _userHelperMock = _mockRepo.Create<IUserHelper>();
            _growSettingsMockData = new GrowSettingsMockData();
            _data = _growSettingsMockData.GetMockUserGrowData();
            _controller = new WaterSettingController(_growSettingsRepositoryMock.Object, _waterSettingFactory.Object,
                _userHelperMock.Object);
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            _controller = null;
            _growSettingsRepositoryMock = null;
            _waterSettingFactory = null;
            _userHelperMock = null;
            _growSettingsMockData = null;
            _data = null;
        }

        #region GET List

        [TestMethod]
        public void GetAllWaterSettings_ForACertainUsersGrowPhase()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.WaterSetting)
                        .ToList());

            _waterSettingFactory.Setup(x => x.GetWaterSetting(It.IsAny<WaterSetting>()))
                .Returns((WaterSetting x) => new WaterSettingDto {WaterSettingId = x.WaterSettingId});

            // Act
            var actionResult = _controller.GetWaterSettings(1, 1) as OkNegotiatedContentResult<List<WaterSettingDto>>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(4, actionResult.Content.Count);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.GetWaterSetting(It.IsAny<WaterSetting>()), Times.Exactly(4));
        }

        [TestMethod]
        public void GetAllWaterSettings_ForACertainUsersGrowPhase_ButUserHasNone()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1000)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.WaterSetting)
                        .ToList());

            // Act
            IHttpActionResult actionResult = _controller.GetWaterSettings(1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetAllWaterSettings_ErrorCatching_Repository()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new IOException());

            // Act
            IHttpActionResult actionResult = _controller.GetWaterSettings(1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetAllWaterSettings_ErrorCatching_Factory()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.WaterSetting)
                        .ToList());

            _waterSettingFactory.Setup(x => x.GetWaterSetting(It.IsAny<WaterSetting>()))
                .Throws(new IOException());

            // Act
            IHttpActionResult actionResult = _controller.GetWaterSettings(1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSettings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.GetWaterSetting(It.IsAny<WaterSetting>()), Times.Exactly(1));
        }

        #endregion

        #region GET Single

        [TestMethod]
        public void GetWaterSetting_ForACertainUsersGrowPhase()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.WaterSetting)
                        .FirstOrDefault(x => x.WaterSettingId == 2)
                );

            _waterSettingFactory.Setup(x => x.GetWaterSetting(It.IsAny<WaterSetting>()))
                .Returns((WaterSetting x) => new WaterSettingDto {WaterSettingId = x.WaterSettingId});

            // Act
            var actionResult = _controller.GetWaterSetting(1, 1, 1) as OkNegotiatedContentResult<WaterSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.WaterSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.GetWaterSetting(It.IsAny<WaterSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetWaterSetting_ForACertainUsersGrowPhase_ButUserHasNone()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.WaterSetting)
                        .FirstOrDefault(x => x.WaterSettingId == 2000)
                );

            // Act
            IHttpActionResult actionResult = _controller.GetWaterSetting(1, 1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void GetWaterSetting_ErrorCatching_Repository()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new Exception());

            // Act
            IHttpActionResult actionResult = _controller.GetWaterSetting(1, 1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void GetWaterSettings_ErrorCatching_Factory()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.WaterSetting)
                        .FirstOrDefault(x => x.WaterSettingId == 2)
                );

            _waterSettingFactory.Setup(x => x.GetWaterSetting(It.IsAny<WaterSetting>()))
                .Throws(new Exception());

            // Act
            IHttpActionResult actionResult = _controller.GetWaterSetting(1, 1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.GetWaterSetting(It.IsAny<WaterSetting>()), Times.Exactly(1));
        }

        #endregion

        #region PUT

        [TestMethod]
        public void PutWaterSettings_Updated()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.WaterSetting)
                        .FirstOrDefault(x => x.WaterSettingId == 2)
                );

            _waterSettingFactory.Setup(
                x => x.PutWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<WaterSettingDto>()))
                .Returns(new WaterSetting {WaterSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x =>
                    x.PutWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<WaterSetting>(),
                        It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<WaterSetting>(new WaterSetting {WaterSettingId = 2},
                    RepositoryActionStatus.Updated));

            _waterSettingFactory.Setup(x => x.GetWaterSetting(It.IsAny<WaterSetting>()))
                .Returns((WaterSetting x) => new WaterSettingDto {WaterSettingId = x.WaterSettingId});

            // Act
            var actionResult =
                _controller.PutWaterSetting(1, 1, 1, new WaterSettingDto()) as
                    OkNegotiatedContentResult<WaterSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.WaterSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.PutWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<WaterSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x =>
                    x.PutWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<WaterSetting>(),
                        It.IsAny<Guid>()), Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.GetWaterSetting(It.IsAny<WaterSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutWaterSettings_ErrorCatching()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.WaterSetting)
                        .FirstOrDefault(x => x.WaterSettingId == 2)
                );

            _waterSettingFactory.Setup(
                x => x.PutWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<WaterSettingDto>()))
                .Returns(new WaterSetting {WaterSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x =>
                    x.PutWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<WaterSetting>(),
                        It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<WaterSetting>(new WaterSetting {WaterSettingId = 2},
                    RepositoryActionStatus.Error));

            // Act
            IHttpActionResult actionResult = _controller.PutWaterSetting(1, 1, 1, new WaterSettingDto());

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.PutWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<WaterSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x =>
                    x.PutWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<WaterSetting>(),
                        It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutWaterSettings_NotFound()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.WaterSetting)
                        .FirstOrDefault(x => x.WaterSettingId == 2)
                );

            _waterSettingFactory.Setup(
                x => x.PutWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<WaterSettingDto>()))
                .Returns(new WaterSetting {WaterSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x =>
                    x.PutWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<WaterSetting>(),
                        It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<WaterSetting>(new WaterSetting {WaterSettingId = 2},
                    RepositoryActionStatus.NotFound));

            // Act
            IHttpActionResult actionResult = _controller.PutWaterSetting(1, 1, 1, new WaterSettingDto());

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.PutWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<WaterSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x =>
                    x.PutWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<WaterSetting>(),
                        It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutWaterSettings_NotModified()
        {
            // Arrange
            _userHelperMock.Setup(x => x.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(
                    _data.Where(x => x.GrowSettingId == 1)
                        .SelectMany(x => x.GrowPhaseSetting)
                        .Where(x => x.GrowPhaseSettingId == 1)
                        .SelectMany(x => x.WaterSetting)
                        .FirstOrDefault(x => x.WaterSettingId == 2)
                );

            _waterSettingFactory.Setup(
                x => x.PutWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<WaterSettingDto>()))
                .Returns(new WaterSetting {WaterSettingId = 2});

            _growSettingsRepositoryMock.Setup(
                x =>
                    x.PutWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<WaterSetting>(),
                        It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<WaterSetting>(new WaterSetting {WaterSettingId = 2},
                    RepositoryActionStatus.NothingModified));

            _waterSettingFactory.Setup(x => x.GetWaterSetting(It.IsAny<WaterSetting>()))
                .Returns((WaterSetting x) => new WaterSettingDto {WaterSettingId = x.WaterSettingId});

            // Act
            var actionResult =
                _controller.PutWaterSetting(1, 1, 1, new WaterSettingDto()) as
                    OkNegotiatedContentResult<WaterSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.WaterSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.PutWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<WaterSettingDto>()),
                Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x =>
                    x.PutWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<WaterSetting>(),
                        It.IsAny<Guid>()), Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.GetWaterSetting(It.IsAny<WaterSetting>()), Times.Exactly(1));
        }

        #endregion

        #region POST

        [TestMethod]
        public void PostWatterSettings_Created()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _waterSettingFactory.Setup(x => x.PostWaterSetting(It.IsAny<WaterSettingDto>()))
                .Returns(new WaterSetting {WaterSettingId = 1});

            _growSettingsRepositoryMock.Setup(o => o.PostWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<WaterSetting>(new WaterSetting {WaterSettingId = 1},
                    RepositoryActionStatus.Created));

            _waterSettingFactory.Setup(x => x.GetWaterSetting(It.IsAny<WaterSetting>()))
                .Returns(new WaterSettingDto {WaterSettingId = 1});

            _controller.Request = new HttpRequestMessage {RequestUri = new Uri("http://www.unit-test.com")};

            // Act
            var actionResult =
                _controller.PostWaterSetting(It.IsAny<int>(), It.IsAny<int>(), new WaterSettingDto {WaterSettingId = 1})
                    as CreatedNegotiatedContentResult<WaterSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(1, actionResult.Content.WaterSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.PostWaterSetting(It.IsAny<WaterSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.PostWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<Guid>()),
                Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.GetWaterSetting(It.IsAny<WaterSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PostWatterSettings_ErrorCatching()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _waterSettingFactory.Setup(x => x.PostWaterSetting(It.IsAny<WaterSettingDto>()))
                .Returns(new WaterSetting {WaterSettingId = 1});

            _growSettingsRepositoryMock.Setup(o => o.PostWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<WaterSetting>(new WaterSetting {WaterSettingId = 1},
                    RepositoryActionStatus.Error));

            _controller.Request = new HttpRequestMessage {RequestUri = new Uri("http://www.unit-test.com")};

            // Act
            IHttpActionResult actionResult = _controller.PostWaterSetting(It.IsAny<int>(), It.IsAny<int>(),
                new WaterSettingDto {WaterSettingId = 1});

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _waterSettingFactory.Verify(x => x.PostWaterSetting(It.IsAny<WaterSettingDto>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.PostWaterSetting(It.IsAny<WaterSetting>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        #endregion

        #region DELETE

        [TestMethod]
        public void DeleteWatterSettings_Deleted()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<WaterSetting>(null, RepositoryActionStatus.Deleted));

            // Act
            var actionResult = _controller.DeleteWaterSetting(1, 1, 1) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(actionResult.StatusCode, HttpStatusCode.NoContent);

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void DeleteWatterSettings_NotFound()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<WaterSetting>(null, RepositoryActionStatus.NotFound));


            // Act
            IHttpActionResult actionResult = _controller.DeleteWaterSetting(999, 999, 999);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void DeleteWatterSettings_CatchError()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.DeleteWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new Exception());


            // Act
            IHttpActionResult actionResult = _controller.DeleteWaterSetting(1, 1, 1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(
                x => x.DeleteWaterSetting(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        #endregion
    }
}