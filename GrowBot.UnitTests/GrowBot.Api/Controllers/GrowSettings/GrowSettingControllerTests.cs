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
    public class GrowSettingControllerTests
    {
        private List<GrowSetting> _data;
        private GrowSettingsMockData _growSettingsMockData;
        private Mock<IGrowSettingsRepository> _growSettingsRepositoryMock;
        private MockRepository _mockRepo;
        private Mock<IGrowSettingFactory> _userGrowFactoryMock;
        private Mock<IUserHelper> _userHelperMock;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _mockRepo = new MockRepository(MockBehavior.Strict);
            _growSettingsRepositoryMock = _mockRepo.Create<IGrowSettingsRepository>();
            _userGrowFactoryMock = _mockRepo.Create<IGrowSettingFactory>();
            _userHelperMock = _mockRepo.Create<IUserHelper>();
            _growSettingsMockData = new GrowSettingsMockData();
            _data = _growSettingsMockData.GetMockUserGrowData();
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            _growSettingsRepositoryMock = null;
            _userGrowFactoryMock = null;
            _userHelperMock = null;
            _data = null;
        }

        #region GET List

        [TestMethod]
        public void GetAllUserGrows_PersonalGrows()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(o => o.GetGrowSettings(false, _growSettingsMockData._userGuid1))
                .Returns(() => _data.Where(x => x.UserGuid == _growSettingsMockData._userGuid1).ToList());

            _userGrowFactoryMock.Setup(x => x.GetGrowSetting(It.IsAny<GrowSetting>()))
                .Returns((GrowSetting x) => new GrowSettingDto {GrowSettingId = x.GrowSettingId});

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            var actionResult = controller.GetUserGrow() as OkNegotiatedContentResult<List<GrowSettingDto>>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.Count);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowSettings(false, _growSettingsMockData._userGuid1),
                Times.Exactly(1));
            _userGrowFactoryMock.Verify(x => x.GetGrowSetting(It.IsAny<GrowSetting>()), Times.Exactly(2));
        }

        [TestMethod]
        public void GetAllUserGrows_PersonalAndPublicGrows()
        {
            // Arrange

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid2);

            _growSettingsRepositoryMock.Setup(o => o.GetGrowSettings(true, _growSettingsMockData._userGuid2))
                .Returns(() => _data.Where(x => x.UserGuid == _growSettingsMockData._userGuid2).ToList());

            _userGrowFactoryMock.Setup(x => x.GetGrowSetting(It.IsAny<GrowSetting>()))
                .Returns((GrowSetting x) => new GrowSettingDto {GrowSettingId = x.GrowSettingId});

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            var actionResult = controller.GetUserGrow(true) as OkNegotiatedContentResult<List<GrowSettingDto>>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(4, actionResult.Content.Count);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowSettings(true, _growSettingsMockData._userGuid2),
                Times.Exactly(1));
            _userGrowFactoryMock.Verify(x => x.GetGrowSetting(It.IsAny<GrowSetting>()), Times.Exactly(4));
        }

        [TestMethod]
        public void GetAllUserGrows_PersonalGrows_UserHasNone()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid4);

            _growSettingsRepositoryMock.Setup(o => o.GetGrowSettings(false, It.IsAny<Guid>()))
                .Returns(new List<GrowSetting>());

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            IHttpActionResult actionResult = controller.GetUserGrow();

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowSettings(false, It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetAllUserGrows_ErrorCatching_Repository()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid2);

            _growSettingsRepositoryMock.Setup(o => o.GetGrowSettings(It.IsAny<bool>(), It.IsAny<Guid>()))
                .Throws(new Exception());

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            IHttpActionResult actionResult = controller.GetUserGrow(false);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowSettings(It.IsAny<bool>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetAllUserGrows_ErrorCatching_Factory()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid2);

            _growSettingsRepositoryMock.Setup(o => o.GetGrowSettings(It.IsAny<bool>(), It.IsAny<Guid>()))
                .Returns(() => _data.Where(x => x.UserGuid == _growSettingsMockData._userGuid1).ToList());
            //.Throws(new Exception());

            _userGrowFactoryMock.Setup(x => x.GetGrowSetting(It.IsAny<GrowSetting>()))
                .Throws(new Exception());

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            IHttpActionResult actionResult = controller.GetUserGrow(false);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowSettings(It.IsAny<bool>(), It.IsAny<Guid>()), Times.Exactly(1));
            _userGrowFactoryMock.Verify(x => x.GetGrowSetting(It.IsAny<GrowSetting>()), Times.Exactly(1));
        }

        #endregion

        #region GET Single

        [TestMethod]
        public void GetUserGrow()
        {
            // Arrange
            int GrowIdBelongingToUser =
                _data.FirstOrDefault(ug => ug.UserGuid == _growSettingsMockData._userGuid1).GrowSettingId;

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.GetGrowSetting(GrowIdBelongingToUser, _growSettingsMockData._userGuid1))
                .Returns(() => _data.FirstOrDefault(x => x.GrowSettingId == GrowIdBelongingToUser));

            _userGrowFactoryMock.Setup(x => x.GetGrowSetting(It.IsAny<GrowSetting>()))
                .Returns((GrowSetting x) => new GrowSettingDto {GrowSettingId = x.GrowSettingId});

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            var actionResult = controller.GetUserGrow(GrowIdBelongingToUser) as OkNegotiatedContentResult<GrowSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(GrowIdBelongingToUser, actionResult.Content.GrowSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowSetting(GrowIdBelongingToUser, _growSettingsMockData._userGuid1), Times.Exactly(1));
            _userGrowFactoryMock.Verify(x => x.GetGrowSetting(It.IsAny<GrowSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetUserGrow_NoneExistingID()
        {
            // Arrange
            int NoneExistingID = 99999;

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(o => o.GetGrowSetting(NoneExistingID, It.IsAny<Guid>()))
                .Returns(() => _data.FirstOrDefault(x => x.GrowSettingId == NoneExistingID));

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            IHttpActionResult actionResult = controller.GetUserGrow(NoneExistingID);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowSetting(NoneExistingID, It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetUserGrow_ErrorCatching_Repository()
        {
            // Arrange

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid2);

            _growSettingsRepositoryMock.Setup(o => o.GetGrowSetting(It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new Exception());

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            IHttpActionResult actionResult = controller.GetUserGrow(1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowSetting(It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetUserGrow_ErrorCatching_Factory()
        {
            // Arrange
            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid2);

            _growSettingsRepositoryMock.Setup(o => o.GetGrowSetting(It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(() => _data.FirstOrDefault(x => x.UserGuid == _growSettingsMockData._userGuid1));
            //.Throws(new Exception());

            _userGrowFactoryMock.Setup(x => x.GetGrowSetting(It.IsAny<GrowSetting>()))
                .Throws(new Exception());


            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            IHttpActionResult actionResult = controller.GetUserGrow(1);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.GetGrowSetting(It.IsAny<int>(), It.IsAny<Guid>()), Times.Exactly(1));
            _userGrowFactoryMock.Verify(x => x.GetGrowSetting(It.IsAny<GrowSetting>()), Times.Exactly(1));
        }

        #endregion

        #region PUT

        [TestMethod]
        public void PutUserGrow_Updated()
        {
            // Arrange
            var modifiedGrow = new GrowSettingDto
            {
                GrowSettingId = 1
            };

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.GetGrowSetting(modifiedGrow.GrowSettingId, _growSettingsMockData._userGuid1))
                .Returns(() => _data.FirstOrDefault(x => x.GrowSettingId == modifiedGrow.GrowSettingId));

            _growSettingsRepositoryMock.Setup(
                o => o.PutGrowSetting(It.IsAny<int>(), It.IsAny<GrowSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowSetting>(new GrowSetting(), RepositoryActionStatus.Updated));

            _userGrowFactoryMock.Setup(x => x.PutGrowSetting(It.IsAny<GrowSetting>(), It.IsAny<GrowSettingDto>()))
                .Returns(new GrowSetting {GrowSettingId = modifiedGrow.GrowSettingId});

            _userGrowFactoryMock.Setup(x => x.GetGrowSetting(It.IsAny<GrowSetting>()))
                .Returns(new GrowSettingDto {GrowSettingId = modifiedGrow.GrowSettingId});

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            var actionResult =
                controller.PutUserGrow(modifiedGrow.GrowSettingId, modifiedGrow) as OkNegotiatedContentResult<GrowSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(modifiedGrow.GrowSettingId, actionResult.Content.GrowSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowSetting(modifiedGrow.GrowSettingId, _growSettingsMockData._userGuid1), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutGrowSetting(It.IsAny<int>(), It.IsAny<GrowSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
            _userGrowFactoryMock.Verify(x => x.PutGrowSetting(It.IsAny<GrowSetting>(), It.IsAny<GrowSettingDto>()),
                Times.Exactly(1));
            _userGrowFactoryMock.Verify(x => x.GetGrowSetting(It.IsAny<GrowSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PutUserGrow_ErrorCatching()
        {
            // Arrange
            var modifiedGrow = new GrowSettingDto
            {
                GrowSettingId = 1
            };

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.GetGrowSetting(modifiedGrow.GrowSettingId, _growSettingsMockData._userGuid1))
                .Returns(() => _data.FirstOrDefault(x => x.GrowSettingId == modifiedGrow.GrowSettingId));

            _userGrowFactoryMock.Setup(x => x.PutGrowSetting(It.IsAny<GrowSetting>(), It.IsAny<GrowSettingDto>()))
                .Returns(new GrowSetting {GrowSettingId = modifiedGrow.GrowSettingId});

            _growSettingsRepositoryMock.Setup(
                o => o.PutGrowSetting(It.IsAny<int>(), It.IsAny<GrowSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowSetting>(new GrowSetting(), RepositoryActionStatus.Error));


            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            IHttpActionResult actionResult = controller.PutUserGrow(modifiedGrow.GrowSettingId, modifiedGrow);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowSetting(modifiedGrow.GrowSettingId, _growSettingsMockData._userGuid1), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutGrowSetting(It.IsAny<int>(), It.IsAny<GrowSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
            _userGrowFactoryMock.Verify(x => x.PutGrowSetting(It.IsAny<GrowSetting>(), It.IsAny<GrowSettingDto>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void PutUserGrow_NotFound()
        {
            // Arrange
            var modifiedGrow = new GrowSettingDto
            {
                GrowSettingId = 1
            };

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.GetGrowSetting(modifiedGrow.GrowSettingId, _growSettingsMockData._userGuid1))
                .Returns(() => _data.FirstOrDefault(x => x.GrowSettingId == modifiedGrow.GrowSettingId));

            _userGrowFactoryMock.Setup(x => x.PutGrowSetting(It.IsAny<GrowSetting>(), It.IsAny<GrowSettingDto>()))
                .Returns(new GrowSetting {GrowSettingId = modifiedGrow.GrowSettingId});

            _growSettingsRepositoryMock.Setup(
                o => o.PutGrowSetting(It.IsAny<int>(), It.IsAny<GrowSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowSetting>(new GrowSetting(), RepositoryActionStatus.NotFound));

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            IHttpActionResult actionResult = controller.PutUserGrow(modifiedGrow.GrowSettingId, modifiedGrow);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowSetting(modifiedGrow.GrowSettingId, _growSettingsMockData._userGuid1), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutGrowSetting(It.IsAny<int>(), It.IsAny<GrowSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
            _userGrowFactoryMock.Verify(x => x.PutGrowSetting(It.IsAny<GrowSetting>(), It.IsAny<GrowSettingDto>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void PutUserGrow_NothingModified()
        {
            // Arrange
            var modifiedGrow = new GrowSettingDto
            {
                GrowSettingId = 1
            };

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(
                o => o.GetGrowSetting(modifiedGrow.GrowSettingId, _growSettingsMockData._userGuid1))
                .Returns(() => _data.FirstOrDefault(x => x.GrowSettingId == modifiedGrow.GrowSettingId));

            _growSettingsRepositoryMock.Setup(
                o => o.PutGrowSetting(It.IsAny<int>(), It.IsAny<GrowSetting>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowSetting>(new GrowSetting(), RepositoryActionStatus.NothingModified));

            _userGrowFactoryMock.Setup(x => x.PutGrowSetting(It.IsAny<GrowSetting>(), It.IsAny<GrowSettingDto>()))
                .Returns(new GrowSetting {GrowSettingId = modifiedGrow.GrowSettingId});

            _userGrowFactoryMock.Setup(x => x.GetGrowSetting(It.IsAny<GrowSetting>()))
                .Returns(new GrowSettingDto {GrowSettingId = modifiedGrow.GrowSettingId});

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            var actionResult =
                controller.PutUserGrow(modifiedGrow.GrowSettingId, modifiedGrow) as OkNegotiatedContentResult<GrowSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(modifiedGrow.GrowSettingId, actionResult.Content.GrowSettingId);

            _mockRepo.VerifyAll();
            _userHelperMock.Verify(x => x.GetUserGuid(), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.GetGrowSetting(modifiedGrow.GrowSettingId, _growSettingsMockData._userGuid1), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(
                x => x.PutGrowSetting(It.IsAny<int>(), It.IsAny<GrowSetting>(), It.IsAny<Guid>()), Times.Exactly(1));
            _userGrowFactoryMock.Verify(x => x.PutGrowSetting(It.IsAny<GrowSetting>(), It.IsAny<GrowSettingDto>()),
                Times.Exactly(1));
        }

        #endregion

        #region POST

        [TestMethod]
        public void PostUserGrow_Created()
        {
            // Arrange
            var createdGrow = new GrowSettingDto
            {
                GrowSettingId = 1
            };

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _userGrowFactoryMock.Setup(x => x.PostGrowSetting(It.IsAny<GrowSettingDto>(), It.IsAny<Guid>()))
                .Returns(new GrowSetting {GrowSettingId = createdGrow.GrowSettingId});

            _growSettingsRepositoryMock.Setup(o => o.PostGrowSetting(It.IsAny<GrowSetting>()))
                .Returns(new RepositoryActionResult<GrowSetting>(new GrowSetting {GrowSettingId = createdGrow.GrowSettingId},
                    RepositoryActionStatus.Created));

            _userGrowFactoryMock.Setup(x => x.GetGrowSetting(It.IsAny<GrowSetting>()))
                .Returns(new GrowSettingDto {GrowSettingId = createdGrow.GrowSettingId});

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);
            controller.Request = new HttpRequestMessage {RequestUri = new Uri("http://www.unit-test.com")};

            // Act
            var actionResult = controller.PostUserGrow(createdGrow) as CreatedNegotiatedContentResult<GrowSettingDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(createdGrow.GrowSettingId, actionResult.Content.GrowSettingId);

            _mockRepo.VerifyAll();
            _userGrowFactoryMock.Verify(x => x.PostGrowSetting(It.IsAny<GrowSettingDto>(), It.IsAny<Guid>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.PostGrowSetting(It.IsAny<GrowSetting>()), Times.Exactly(1));
            _userGrowFactoryMock.Verify(x => x.GetGrowSetting(It.IsAny<GrowSetting>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PostUserGrow_ErrorCatching()
        {
            // Arrange
            var createdGrow = new GrowSettingDto
            {
                GrowSettingId = 1
            };

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _userGrowFactoryMock.Setup(x => x.PostGrowSetting(It.IsAny<GrowSettingDto>(), It.IsAny<Guid>()))
                .Returns(new GrowSetting {GrowSettingId = createdGrow.GrowSettingId});

            _growSettingsRepositoryMock.Setup(o => o.PostGrowSetting(It.IsAny<GrowSetting>()))
                .Returns(new RepositoryActionResult<GrowSetting>(new GrowSetting {GrowSettingId = createdGrow.GrowSettingId},
                    RepositoryActionStatus.Error));

            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            IHttpActionResult actionResult = controller.PostUserGrow(createdGrow);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _userGrowFactoryMock.Verify(x => x.PostGrowSetting(It.IsAny<GrowSettingDto>(), It.IsAny<Guid>()), Times.Exactly(1));
            _growSettingsRepositoryMock.Verify(x => x.PostGrowSetting(It.IsAny<GrowSetting>()), Times.Exactly(1));
        }

        #endregion

        #region DELETE

        [TestMethod]
        public void DeleteUserGrow_Deleted()
        {
            // Arrange
            int deletegrowSettingId = 1;

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(o => o.DeleteGrowSetting(It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowSetting>(null, RepositoryActionStatus.Deleted));


            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            var actionResult = controller.DeleteUserGrow(deletegrowSettingId) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(actionResult.StatusCode, HttpStatusCode.NoContent);

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(x => x.DeleteGrowSetting(It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void DeleteUserGrow_NotFound()
        {
            // Arrange
            int deletegrowSettingId = 1;

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(o => o.DeleteGrowSetting(It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(new RepositoryActionResult<GrowSetting>(null, RepositoryActionStatus.NotFound));


            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            IHttpActionResult actionResult = controller.DeleteUserGrow(deletegrowSettingId);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (NotFoundResult));

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(x => x.DeleteGrowSetting(It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void DeleteUserGrow_CatchError()
        {
            // Arrange
            int deletegrowSettingId = 1;

            _userHelperMock.Setup(o => o.GetUserGuid())
                .Returns(_growSettingsMockData._userGuid1);

            _growSettingsRepositoryMock.Setup(o => o.DeleteGrowSetting(It.IsAny<int>(), It.IsAny<Guid>()))
                .Throws(new Exception());


            var controller = new GrowSettingController(_growSettingsRepositoryMock.Object, _userGrowFactoryMock.Object,
                _userHelperMock.Object);

            // Act
            IHttpActionResult actionResult = controller.DeleteUserGrow(deletegrowSettingId);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof (InternalServerErrorResult));

            _mockRepo.VerifyAll();
            _growSettingsRepositoryMock.Verify(x => x.DeleteGrowSetting(It.IsAny<int>(), It.IsAny<Guid>()),
                Times.Exactly(1));
        }

        #endregion
    }
}