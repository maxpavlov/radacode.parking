namespace RadaCode.Web.Core.Setttings
{
    class ProductionRadaCodeWebSettings : IRadaCodeWebSettings
    {
        private readonly string _host;

        public ProductionRadaCodeWebSettings()
        {
            var rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");

            var host = rootWebConfig.AppSettings.Settings["deployedHost"];
            _host = host != null ? host.Value : "localhost";
        }

        public string CurrentHost
        {
            get { return _host; }
        }
    }
}