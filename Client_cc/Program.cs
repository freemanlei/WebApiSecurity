// 从元数据中发现端点
using Api_cc.Configs;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

AppSettings.Init();

Task.Run((Func<Task?>)(async () =>
{
    // See https://aka.ms/new-console-template for more information



    var client = new HttpClient();
    var disco = await client.GetDiscoveryDocumentAsync(AppSettings.WebApiSettings.AuthorityUrl);

    if (disco.IsError)
    {
        Console.Error.WriteLine(disco.Error);
        return;
    }


    // 请求令牌

    var tokenResponsse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
    {
        Address = disco.TokenEndpoint,
        ClientId = AppSettings.WebApiSettings.ClientId,
        ClientSecret = "secret",
        Scope = AppSettings.WebApiSettings.Scope
    });

    if (tokenResponsse.IsError)
    {
        Console.WriteLine(tokenResponsse.Error);
        return;
    }
    Console.WriteLine(tokenResponsse.Json);

    Console.WriteLine("\n\n");

    //call api
    var apiClient = new HttpClient();
    apiClient.SetBearerToken(tokenResponsse.AccessToken);


    await GetAsync(apiClient, AppSettings.WebApiSettings.ApiIdentityUrl);

    await GetAsync(apiClient, AppSettings.WebApiSettings.WeatherForecastUrl);

})

);

Console.ReadLine();

static async Task GetAsync(HttpClient apiClient, string url)
{
    var response = await apiClient.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine(response.StatusCode);
    }
    else
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(JArray.Parse(content));
    }
}