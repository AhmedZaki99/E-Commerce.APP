using E_Commerce.App.Application.Abstruction;
using E_Commerce.App.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace E_Commerce.App.Infrastructre.presistent._Data.Interceptor
{
    internal class CustomSaveChangesInterceptor(ILoggedInUserService loggedInUserService) : SaveChangesInterceptor
    {
        public ILoggedInUserService LoggedInUserService { get; } = loggedInUserService;

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }


        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
      

        private void UpdateEntities(DbContext? context)
        {
            if (context is null) return;

            foreach (var entry in context.ChangeTracker.Entries<BaseEntity<int>>()
                .Where((entity) => entity.State is EntityState.Added or EntityState.Modified))
            {
                if (entry.State is EntityState.Added)
                {
                    entry.Entity.CreatedBy = LoggedInUserService.UserId ?? "UserAdmin";
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                }
                entry.Entity.lastModifiedBy = LoggedInUserService.UserId ?? "UserAdmin";
                entry.Entity.LastModifideOn = DateTime.UtcNow;

            }
        }

       
    }
}
