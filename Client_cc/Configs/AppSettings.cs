
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Api_cc.Configs
{
    public static class AppSettings
    {
        public static WebApiSettings WebApiSettings { get; set; }


        public static void Init()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                     .AddJsonFile("appsettings.json").Build();
            WebApiSettings = new WebApiSettings();
            configurationRoot.GetSection("WebApiSettings").Bind(WebApiSettings);

        }

    }
}
