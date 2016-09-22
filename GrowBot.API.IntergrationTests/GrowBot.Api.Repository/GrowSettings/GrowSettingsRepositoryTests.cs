using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using GrowBot.API.DbContext;
using GrowBot.API.Entities.GrowSettings;
using GrowBot.API.Repository;
using GrowBot.API.Repository.Repositories.GrowSettings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GrowBot.IntergrationTests.GrowBot.Api.Repository.GrowSettings
{
    [TestClass]
    public class GrowSettingsRepositoryTests
    {
        private readonly DataBaseDataSeed _dataBaseDataSeed = new DataBaseDataSeed();
        private ApplicationDbContext _context;

        [TestInitialize]
        public void MyTestInitialize()
        {
            // file path of the database to create
            var filePath = @"C:\Development\GrowBot\UnitTestDB";
            var databasePath = filePath + @"\GrowBotUnitTestDb.sdf";
            try
            {
                var folderExists = Directory.Exists(filePath);
                if (!folderExists)
                    Directory.CreateDirectory(filePath);

                // delete it if it already exists
                if (File.Exists(databasePath))
                    File.Delete(databasePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            // create the SQL CE connection string - this just points to the file path
            string connectionString = "Datasource = " + databasePath;

            // NEED TO SET THIS TO MAKE DATABASE CREATION WORK WITH SQL CE!!!
            Database.DefaultConnectionFactory =
                new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");

            using (var context = new ApplicationDbContext(connectionString))
            {
                // this will create the database with the schema from the Entity Model
                context.Database.CreateIfNotExists();
            }

            // initialise our DbContext class with the SQL CE connection string, 
            // ready for our tests to use it.
            _context = new ApplicationDbContext(connectionString);

            // Database seed method

            _dataBaseDataSeed.SeedDataBase(_context);


        }

        [TestCleanup]
        public void MyTestCleanup()
        {
        }

        #region UserGrow

        [TestMethod]
        public void GetUserGrows_PublicAndUsersOwnGrows_UserHasOwnGrowSettings()
        {
            var _growSettingsRepository = new GrowSettingsRepository(_context);

            var growSettings = _growSettingsRepository.GetGrowSettings(true, new Guid(_dataBaseDataSeed.ProductOwner1Guid));

            // check the correct product is retrieved
            Assert.AreEqual(4, growSettings.Count);
        }

        [TestMethod]
        public void GetUserGrows_JustUsersOwnGrows_UserHasOwnGrowSettings()
        {
            var _growSettingsRepository = new GrowSettingsRepository(_context);

            var growSettings = _growSettingsRepository.GetGrowSettings(false, new Guid(_dataBaseDataSeed.ProductOwner1Guid));

            // check the correct product is retrieved
            Assert.AreEqual(2, growSettings.Count);
        }

        [TestMethod]
        public void GetUserGrows_PublicAndUsersOwnGrows_UserHasNoGrowSettings()
        {
            var _growSettingsRepository = new GrowSettingsRepository(_context);

            var growSettings = _growSettingsRepository.GetGrowSettings(true, new Guid(_dataBaseDataSeed.GeneralUser1Guid));

            // check the correct product is retrieved
            Assert.AreEqual(2, growSettings.Count);
        }

        [TestMethod]
        public void GetUserGrows_JustUsersOwnGrows_UserHasNoGrowSettings()
        {
            var _growSettingsRepository = new GrowSettingsRepository(_context);

            var growSettings = _growSettingsRepository.GetGrowSettings(false, new Guid(_dataBaseDataSeed.GeneralUser1Guid));

            // check the correct product is retrieved
            Assert.AreEqual(0, growSettings.Count);
        }

        [TestMethod]
        public void GetUserGrow_PublicAndUsersOwnGrows_UserGetsPrivateOwnGrowSettings()
        {
            var _growSettingsRepository = new GrowSettingsRepository(_context);

            var growSetting = _growSettingsRepository.GetGrowSetting(3, new Guid(_dataBaseDataSeed.ProductOwner1Guid));

            // check the correct product is retrieved
            Assert.IsTrue(growSetting != null);
        }

        [TestMethod]
        public void GetUserGrow_JustUsersOwnGrows_UserHasGetsAnotherUsersPublicGrowSettings()
        {
            var _growSettingsRepository = new GrowSettingsRepository(_context);

            var growSetting = _growSettingsRepository.GetGrowSetting(1, new Guid(_dataBaseDataSeed.ProductOwner1Guid));

            // check the correct product is retrieved
            Assert.IsTrue(growSetting != null);
        }

        [TestMethod]
        public void GetUserGrow_JustUsersOwnGrows_UserHasGetsAnotherUsersPrivateGrowSettings()
        {
            var _growSettingsRepository = new GrowSettingsRepository(_context);

            var growSetting = _growSettingsRepository.GetGrowSetting(1000, new Guid(_dataBaseDataSeed.ProductOwner1Guid));

            // check the correct product is retrieved
            Assert.IsTrue(growSetting == null);
        }

        [TestMethod]
        public void PutUserGrow_ModfiedGrowIsOwnedByUser_SucsessfulEdit()
        {
            var _growSettingsRepository = new GrowSettingsRepository(_context);

            // Arrange
            var modifiedUserGrowSetting = _context.GrowSetting.FirstOrDefault(ug => ug.GrowSettingId == 3);


            var nameToChangeTo = "Test Passed";

            modifiedUserGrowSetting.GrowSettingName = nameToChangeTo;

            // Act

            var result = _growSettingsRepository.PutGrowSetting(modifiedUserGrowSetting.GrowSettingId, modifiedUserGrowSetting,
                modifiedUserGrowSetting.UserGuid);

            var resultCheck = _context.GrowSetting.FirstOrDefault(ug => ug.GrowSettingId == modifiedUserGrowSetting.GrowSettingId);

            // Assert

            Assert.IsTrue(result.Status == RepositoryActionStatus.Updated);
            Assert.AreEqual(nameToChangeTo, result.Entity.GrowSettingName);
            Assert.AreEqual(nameToChangeTo, resultCheck.GrowSettingName);
        }

        [TestMethod]
        public void PutUserGrow_ModfiedGrowIsNotOwnedByUser_FailedEdit()
        {
            // Arrange
            var _growSettingsRepository = new GrowSettingsRepository(_context);

            var modifiedUserGrowSetting = _context.GrowSetting.FirstOrDefault(ug => ug.GrowSettingId == 3);

            const string nameToChangeTo = "Test Passed";

            modifiedUserGrowSetting.GrowSettingName = nameToChangeTo;

            // Act

            var result = _growSettingsRepository.PutGrowSetting(modifiedUserGrowSetting.GrowSettingId, modifiedUserGrowSetting,
                new Guid(_dataBaseDataSeed.GeneralUser1Guid));
            var resultCheck = _context.GrowSetting.FirstOrDefault(ug => ug.GrowSettingId == modifiedUserGrowSetting.GrowSettingId);

            // Assert

            Assert.IsTrue(result.Status == RepositoryActionStatus.NotFound);
            Assert.AreNotEqual(nameToChangeTo, resultCheck.GrowSettingName);
        }


        #endregion
    }
}