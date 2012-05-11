using Ninject.Modules;
using Ninject.Web.Common;
using RadaCode.Web.Data.EF;
using RadaCode.Web.Data.Repositories;

namespace RadaCode.Web.Data
{
    public class WebDataDependenciesModule: NinjectModule
    {
        public override void Load()
        {
            Bind<WebStoreContext>().ToSelf().InRequestScope();
            Bind<IWebUserRepository>().To<WebUserRepository>().InRequestScope();
        }
    }
}
