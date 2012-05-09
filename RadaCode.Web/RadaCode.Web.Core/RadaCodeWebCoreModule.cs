using Ninject.Modules;
using RadaCode.Web.Core.Setttings;

namespace RadaCode.Web.Core
{
    public class RadaCodeWebCoreModule: NinjectModule
    {
        public override void Load()
        {
            Bind<IRadaCodeWebSettings>().To<ProductionRadaCodeWebSettings>().InSingletonScope();
        }
    }
}
