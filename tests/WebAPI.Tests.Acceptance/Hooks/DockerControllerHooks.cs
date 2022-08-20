using System.Net;
using BoDi;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace CleanArchitecture.WebApi.Tests.Acceptance.Hooks;

[Binding]
public class DockerControllerHooks
{
    private static ICompositeService _compositeService;
    private IObjectContainer _objectContainer;

    public DockerControllerHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeTestRun]
    public static void DockerComposeUp()
    {
        var config = LoadConfiguration();

        var dockerComposeFileName = config["DockerComposeFileName"];
        var dockerComposePath = GetDockerComposeLocation(dockerComposeFileName);

        var confirmationUrl = config["WebApi:BaseAddress"];
        
        _compositeService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(dockerComposePath)
            .RemoveOrphans()
            // This WaitForHttp() method is never work, and this library have no docs for how to use it.
            // .WaitForHttp("clean_architecture.webapi", $"{confirmationUrl}/api/authentication/login",
            //     continuation: (response, cnt) => response.Code != HttpStatusCode.OK ? 100000 : 0,
            //     method: HttpMethod.Post, 
            //     body: @"{""email"":""administrator@localhost"", ""password"":""Administrator1!""}")
            .Build()
            .Start();
    }

    [AfterTestRun]
    public static void DockerComposeDown()
    {
        _compositeService.Stop();
        _compositeService.Dispose();
    }

    [BeforeScenario]
    public void AddHttpClient()
    {
        var config = LoadConfiguration();

        var httpClient = new HttpClient { BaseAddress = new Uri(config["WebApi:BaseAddress"]) };
        
        _objectContainer.RegisterInstanceAs(httpClient);
    }

    private static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }

    private static string GetDockerComposeLocation(string dockerComposeFileName)
    {
        var directory = Directory.GetCurrentDirectory();
        
        while (!Directory.EnumerateFiles(directory, "*.yml").Any(s => s.EndsWith(dockerComposeFileName)))
        {
            directory = directory.Substring(0, directory.LastIndexOf(Path.DirectorySeparatorChar));
        }

        return Path.Combine(directory, dockerComposeFileName);
    }
}