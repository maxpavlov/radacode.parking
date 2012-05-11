using System.Data.Entity;
using RadaCode.Web.Data.Entities;

namespace RadaCode.Web.Data.EF
{
    public class RadaCodeWebStoreContext : DbContext
    {
        public DbSet<WebUser> WebUsers { get; set; }
        public DbSet<WebUserRole> WebUserRoles { get; set; }
    }
}
