using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Store.Data.EF
{
    public  class DbContextFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public DbContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        
        public  StoreDbContext Create(Type repositoryType)
        {
            var services = httpContextAccessor.HttpContext.RequestServices;

            var dbContexts = services.GetRequiredService<Dictionary<Type, StoreDbContext>>();
            if (dbContexts.ContainsKey(repositoryType))
            {
                return dbContexts[repositoryType];
            }

            dbContexts[repositoryType] = services.GetRequiredService<StoreDbContext>();
            return dbContexts[repositoryType];
        }
    }
}
