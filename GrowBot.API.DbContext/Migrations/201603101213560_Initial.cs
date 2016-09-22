namespace GrowBot.API.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FanSettings",
                c => new
                    {
                        FanSettingId = c.Int(nullable: false, identity: true),
                        MaxHeatCelsius = c.Int(nullable: false),
                        MinHeatCelsius = c.Int(nullable: false),
                        MinFanSpeedPercent = c.Int(nullable: false),
                        GrowPhaseSettingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FanSettingId)
                .ForeignKey("dbo.GrowPhaseSettings", t => t.GrowPhaseSettingId, cascadeDelete: true)
                .Index(t => t.GrowPhaseSettingId);
            
            CreateTable(
                "dbo.GrowPhaseSettings",
                c => new
                    {
                        GrowPhaseSettingId = c.Int(nullable: false, identity: true),
                        GrowPhaseName = c.String(maxLength: 4000),
                        Duration = c.Int(nullable: false),
                        PhaseNotes = c.String(),
                        PhaseOrder = c.Int(nullable: false),
                        GrowSettingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GrowPhaseSettingId)
                .ForeignKey("dbo.GrowSettings", t => t.GrowSettingId, cascadeDelete: true)
                .Index(t => t.GrowSettingId);
            
            CreateTable(
                "dbo.GrowSettings",
                c => new
                    {
                        GrowSettingId = c.Int(nullable: false, identity: true),
                        UserGuid = c.Guid(nullable: false),
                        GrowSettingName = c.String(maxLength: 4000),
                        GrowSettingNotes = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                        SystemDefaultGrowSetting = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GrowSettingId);
            
            CreateTable(
                "dbo.LightSettings",
                c => new
                    {
                        LightSettingId = c.Int(nullable: false, identity: true),
                        TurnOnLightTimeTicks = c.Long(nullable: false),
                        TurnOffLightTimeTicks = c.Long(nullable: false),
                        GrowPhaseSettingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LightSettingId)
                .ForeignKey("dbo.GrowPhaseSettings", t => t.GrowPhaseSettingId, cascadeDelete: true)
                .Index(t => t.GrowPhaseSettingId);
            
            CreateTable(
                "dbo.WaterSettings",
                c => new
                    {
                        WaterSettingId = c.Int(nullable: false, identity: true),
                        TurnOnWaterTimeTicks = c.Long(nullable: false),
                        TurnOffWaterTimeTicks = c.Long(nullable: false),
                        GrowPhaseSettingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WaterSettingId)
                .ForeignKey("dbo.GrowPhaseSettings", t => t.GrowPhaseSettingId, cascadeDelete: true)
                .Index(t => t.GrowPhaseSettingId);
            
            CreateTable(
                "dbo.Grows",
                c => new
                    {
                        GrowId = c.Int(nullable: false, identity: true),
                        GrowName = c.String(maxLength: 4000),
                        GrowNotes = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                        UserGuid = c.Guid(nullable: false),
                        SystemDefaultGrow = c.Boolean(nullable: false),
                        PublicGrow = c.Boolean(nullable: false),
                        GrowSettingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GrowId)
                .ForeignKey("dbo.GrowSettings", t => t.GrowSettingId, cascadeDelete: true)
                .Index(t => t.GrowSettingId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Grows", "GrowSettingId", "dbo.GrowSettings");
            DropForeignKey("dbo.WaterSettings", "GrowPhaseSettingId", "dbo.GrowPhaseSettings");
            DropForeignKey("dbo.LightSettings", "GrowPhaseSettingId", "dbo.GrowPhaseSettings");
            DropForeignKey("dbo.GrowPhaseSettings", "GrowSettingId", "dbo.GrowSettings");
            DropForeignKey("dbo.FanSettings", "GrowPhaseSettingId", "dbo.GrowPhaseSettings");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Grows", new[] { "GrowSettingId" });
            DropIndex("dbo.WaterSettings", new[] { "GrowPhaseSettingId" });
            DropIndex("dbo.LightSettings", new[] { "GrowPhaseSettingId" });
            DropIndex("dbo.GrowPhaseSettings", new[] { "GrowSettingId" });
            DropIndex("dbo.FanSettings", new[] { "GrowPhaseSettingId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Grows");
            DropTable("dbo.WaterSettings");
            DropTable("dbo.LightSettings");
            DropTable("dbo.GrowSettings");
            DropTable("dbo.GrowPhaseSettings");
            DropTable("dbo.FanSettings");
        }
    }
}
