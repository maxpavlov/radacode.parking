using System.Data.Entity;

namespace RadaCode.Web.Data.EF
{
    public class RadaCodeWebStoreContextInitializer : DropCreateDatabaseAlways<RadaCodeWebStoreContext> //DropCreateDatabaseIfModelChanges<FilexStoreContext>
    {
        protected override void Seed(RadaCodeWebStoreContext context)
        {
            //new List<WebUserRole>
            //{
            //    new WebUserRole() { RoleName = "Thinker"
            //             },
            //    new WebUserRole() { RoleName = "ThoughtRouter"}
            //}.ForEach(r => context.WebUserRoles.Add(r));

            //var nimda = new WebUser()
            //    {
            //        CreateDate = DateTime.Now,
            //        UserName = "Nimda", Password = Crypto.HashPassword("jackPecker"),
            //        Roles = context.WebUserRoles.Local.Where(rl => rl.RoleName == "ThoughtRouter").ToList()
            //    };

            //context.WebUsers.Add(nimda);

            base.Seed(context);
        }
    }
}
