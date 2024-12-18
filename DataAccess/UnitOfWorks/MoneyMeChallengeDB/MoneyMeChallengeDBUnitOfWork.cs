using Common.Constants;
using Common.DataTransferObjects.AuditTrail;
using DataAccess.DBContexts.MoneyMeChallengeDB;
using DataAccess.DBContexts.MoneyMeChallengeDB.Models;
using DataAccess.Repositories.MoneyMeChallengeDB;
using DataAccess.Repositories.MoneyMeChallengeDB.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.UnitOfWorks.MoneyMeChallengeDB
{
    public sealed class MoneyMeChallengeDBUnitOfWork : IMoneyMeChallengeDBUnitOfWork
    {
        private readonly MoneyMeChallengeDBContext _context;
        public MoneyMeChallengeDBUnitOfWork(MoneyMeChallengeDBContext context)
        {
            _context = context;
            ErrorLogRepository = new ErrorLogRepository(_context);
            AuditTrailRepository = new AuditTrailRepository(_context);
        }

        public IErrorLogRepository ErrorLogRepository { get; private set; }

        public IAuditTrailRepository AuditTrailRepository { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync(string transactionBy)
        {
            List<Tuple<ContextChangeTrackingDetail, EntityEntry>> contextChangeTrackingDetail = TrackRevisionDetails();
            int mainChangeResult = await _context.SaveChangesAsync();


            if (contextChangeTrackingDetail.Any())
            {
                AuditTrail auditTrail = new AuditTrail()
                {
                    TransactionBy = transactionBy,
                    TransactionDate = DateTime.Now
                };

                foreach (var changeTrackingTuple in contextChangeTrackingDetail)
                {
                    ContextChangeTrackingDetail changeTrackingDetail = changeTrackingTuple.Item1;
                    EntityEntry entityEntry = changeTrackingTuple.Item2;

                    object[] primaryKeys = entityEntry.Metadata.FindPrimaryKey()
                                    .Properties
                                    .Select(p => entityEntry.Property(p.Name).CurrentValue)
                                    .ToArray();

                    string concatenatedPrimaryKeys = string.Join(',', primaryKeys);

                    AuditTrailDetail auditTrailDetail = new AuditTrailDetail()
                    {
                        EntityField = changeTrackingDetail.EntityField,
                        EntityId = changeTrackingDetail.EntityId != concatenatedPrimaryKeys ? concatenatedPrimaryKeys : changeTrackingDetail.EntityId,
                        TableName = changeTrackingDetail.TableName,
                        OldValue = changeTrackingDetail.OldValue,
                        NewValue = changeTrackingDetail.NewValue
                    };
                    auditTrail.AuditTrailDetails.Add(auditTrailDetail);
                }

                await _context.AuditTrails.AddAsync(auditTrail);
                await _context.SaveChangesAsync();
            }

            return mainChangeResult;
        }

        private List<Tuple<ContextChangeTrackingDetail, EntityEntry>> TrackRevisionDetails()
        {
            //GET Changes Entities and Exclude those do not need audit trail
            IEnumerable<EntityEntry> entityEntries = _context.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() != typeof(AuditTrail)
                && e.Entity.GetType() != typeof(AuditTrailDetail)
                && e.Entity.GetType() != typeof(ErrorLog));

            List<string> excludedFields = DbContextConstant.NoAuditColumns.Split(',').ToList();

            List<Tuple<ContextChangeTrackingDetail, EntityEntry>> contextChangeTrackingDetail = new List<Tuple<ContextChangeTrackingDetail, EntityEntry>>();

            foreach (EntityEntry entityEntry in entityEntries)
            {
                object[] primaryKeys = entityEntry.Metadata.FindPrimaryKey()
                                    .Properties
                                    .Select(p => entityEntry.Property(p.Name).CurrentValue)
                                    .ToArray();
                foreach (PropertyEntry propertyEntry in entityEntry.Properties
                    .Where(p => !excludedFields.Contains(p.Metadata.Name)))
                {
                    if (propertyEntry.IsModified || entityEntry.State == EntityState.Added)
                    {
                        ContextChangeTrackingDetail changeTrackingDetail = new ContextChangeTrackingDetail()
                        {
                            EntityId = string.Join(',', primaryKeys),
                            EntityField = propertyEntry.Metadata.Name,
                            OldValue = propertyEntry.OriginalValue?.ToString(),
                            NewValue = propertyEntry.CurrentValue?.ToString(),
                            TableName = entityEntry.Entity.GetType().Name
                        };

                        Tuple<ContextChangeTrackingDetail, EntityEntry> changeTrackingTuple = new Tuple<ContextChangeTrackingDetail, EntityEntry>(changeTrackingDetail, entityEntry);
                        contextChangeTrackingDetail.Add(changeTrackingTuple);
                    }
                }
            }

            return contextChangeTrackingDetail;
        }
    }
}
