using IdentityServer4.Models;

namespace IdentityServer_cc.Configs
{
    public class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
       new List<ApiScope>
       {
            new ApiScope( AppSettings.WebApiSettings.Scope, "My API")
       };

        public static IEnumerable<Client> Clients => new List<Client>
            {
                new Client{
                    ClientId=AppSettings.WebApiSettings.ClientId,
                    // 没有交互式用户，使用 clientid/secret 进行身份验证
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                     // 用于身份验证的密钥
                    ClientSecrets =
                    {
                         new Secret("secret".Sha256())
                    },

                    //客户端有权访问的范围
                    AllowedScopes ={ AppSettings.WebApiSettings.Scope }
                }
            };

    }


}
