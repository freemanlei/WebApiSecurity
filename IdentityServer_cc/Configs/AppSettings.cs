namespace IdentityServer_cc.Configs
{

    public class AppSettings
    {
        public static WebApiSettings WebApiSettings { get; set; }

        public static void Init(IConfiguration configuration)
        {
            WebApiSettings = new WebApiSettings();
            configuration.GetSection("WebApiSettings").Bind(WebApiSettings);
        }
    }
}
