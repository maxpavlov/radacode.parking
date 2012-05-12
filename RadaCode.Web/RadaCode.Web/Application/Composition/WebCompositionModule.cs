using Ninject.Modules;
using RadaCode.Web.Application.Membership;

namespace RadaCode.Web.Application.Composition
{
    public class WebCompositionModule: NinjectModule
    {
        public override void Load()
        {
            Bind<WebUserMembershipProvider>().ToSelf();
            Bind<WebUserRoleProvider>().ToSelf();
        }
    }
}