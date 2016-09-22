using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using GrowBot.API.DbContext;
using GrowBot.API.Entities.GrowResults;
using GrowBot.API.Entities.GrowSettings;

namespace GrowBot.API.Repository.Repositories.GrowResults
{
    class GrowResultsRepository
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public GrowResultsRepository()
        {

        }

        public GrowResultsRepository(ApplicationDbContext dbContext = null)
        {
            if (dbContext == null)
            {
                _dbContext = new ApplicationDbContext();
            }
            else
            {
                _dbContext = dbContext;
            }
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        #region Grow


        public List<Grow> GetGrow(bool publicGrows, Guid currentUserGuid)
        {
            if (publicGrows)
            {
                return
                    _dbContext.Grow.Where(g => g.UserGuid == currentUserGuid || g.PublicGrow)
                        .ToList();
            }
            return _dbContext.Grow.Where(g => g.UserGuid == currentUserGuid).ToList();
        }

        public Grow GetGrow(int growId, Guid currentUserGuid)
        {
            return
                _dbContext.Grow.FirstOrDefault(g => g.GrowId == growId && (g.UserGuid == currentUserGuid));
        }

        public RepositoryActionResult<Grow> PutGrow(int growId, Grow modifiedGrow,
            Guid currentUserGuid)
        {
            if (modifiedGrow.UserGuid != currentUserGuid)
            {
                _dbContext.Entry(modifiedGrow).State = EntityState.Unchanged;
                return new RepositoryActionResult<Grow>(modifiedGrow, RepositoryActionStatus.NotFound);
            }

            //_dbContext.Entry(modifiedUserGrow).State = EntityState.Modified;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!GrowExists(growId))
                {
                    return new RepositoryActionResult<Grow>(null, RepositoryActionStatus.Error, ex);
                }
                throw;
            }
            return new RepositoryActionResult<Grow>(modifiedGrow, RepositoryActionStatus.Updated);
        }

        public RepositoryActionResult<Grow> PostGrowSetting(Grow entity)
        {
            try
            {
                _dbContext.Grow.Add(entity);
                int result = _dbContext.SaveChanges();
                if (result > 0)
                {
                    return new RepositoryActionResult<Grow>(entity, RepositoryActionStatus.Created);
                }
                return new RepositoryActionResult<Grow>(entity, RepositoryActionStatus.NothingModified, null);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Grow>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public RepositoryActionResult<Grow> DeleteGrow(int growId, Guid currentUserGuid)
        {
            try
            {
                Grow grow =
                    _dbContext.Grow.FirstOrDefault(
                        ug => ug.GrowId == growId && ug.UserGuid == currentUserGuid);
                if (grow != null)
                {
                    _dbContext.Grow.Remove(grow);

                    _dbContext.SaveChanges();
                    return new RepositoryActionResult<Grow>(null, RepositoryActionStatus.Deleted);
                }
                return new RepositoryActionResult<Grow>(null, RepositoryActionStatus.NotFound);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Grow>(null, RepositoryActionStatus.Error, ex);
            }
        }

        private bool GrowExists(int id)
        {
            return _dbContext.Grow.Count(e => e.GrowId == id) > 0;
        }
        #endregion

    }
}
