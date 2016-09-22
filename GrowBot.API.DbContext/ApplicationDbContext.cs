using System.Data.Entity;
using GrowBot.API.Entities.Basics;
using GrowBot.API.Entities.GrowResults;
using GrowBot.API.Entities.GrowSettings;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GrowBot.API.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
        }

        public ApplicationDbContext(string connectionString)
            : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        #region Grow Settings

        public DbSet<GrowSetting> GrowSetting { get; set; }
        public DbSet<GrowPhaseSetting> GrowPhaseSetting { get; set; }
        public DbSet<LightSetting> LightSetting { get; set; }
        public DbSet<WaterSetting> WaterSetting { get; set; }
        public DbSet<FanSetting> FanSetting { get; set; }

        #endregion

        #region Grow Results

        public DbSet<Grow> Grow { get; set; }

        #endregion
    }
}