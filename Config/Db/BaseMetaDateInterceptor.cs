using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MaterialDemo.Config.Db
{
    public class BaseMetaDateInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
        {
            if (eventData.Context == null) return result;
            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity entity) {
                    // 修改插入对象的数据    
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreateBy = MaterialDemo.Security.SecurityContext.GetUserLoginName();
                        entity.CreateTime = DateTime.Now;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entity.UpdateBy = MaterialDemo.Security.SecurityContext.GetUserLoginName();
                        entity.UpdateTime = DateTime.Now;
                    }
                }
            }

            return result;
        }
    }
}
