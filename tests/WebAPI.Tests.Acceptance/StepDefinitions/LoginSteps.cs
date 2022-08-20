using System.Net;
using System.Net.Http.Json;
using CleanArchitecture.WebApi.Tests.Acceptance.Models;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.WebApi.Tests.Acceptance.StepDefinitions;

[Binding]
public class LoginSteps
{
    private readonly HttpClient _httpClient;
    private readonly ScenarioContext _scenarioContext;

    public LoginSteps(HttpClient httpClient, ScenarioContext scenarioContext)
    {
        _httpClient = httpClient;
        _scenarioContext = scenarioContext;
    }

    [When(@"a logged out user logs in with valid credentials")]
    public async Task WhenALoggedOutUserLogsInWithValidCredentials(Table table)
    {
        await Task.Delay(2000);
        
        var adminLoginRequest = table.CreateInstance<LoginRequest>();
        
        _scenarioContext.Add("adminLoginRequest", adminLoginRequest);

        var response = await _httpClient.PostAsJsonAsync("api/Authentication/login", adminLoginRequest);
        
        _scenarioContext.Add("adminLoginResponse", response);
    }

    [Then(@"they log in successfully")]
    public async Task ThenTheyLogInSuccessfully()
    {
        var adminLoginRequest = _scenarioContext.Get<LoginRequest>("adminLoginRequest");
        var adminLoginResponse = _scenarioContext.Get<HttpResponseMessage>("adminLoginResponse");
        
        var authenticateResponse = await adminLoginResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();

        adminLoginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        authenticateResponse!.UserResult.Email.Should().Be(adminLoginRequest.Email);
        authenticateResponse.Token.Should().NotBeEmpty();
    }

    [When(@"a logged out user logs in with invalid credentials")]
    public async Task WhenALoggedOutUserLogsInWithInvalidCredentials(Table table)
    {
        var hackerLoginRequest = table.CreateInstance<LoginRequest>();

        var response = await _httpClient.PostAsJsonAsync("api/Authentication/login", hackerLoginRequest);
        
        _scenarioContext.Add("hackerLoginResponse", response);
    }

    [Then(@"an error is display")]
    public void ThenAnErrorIsDisplay()
    {
        var hackerLoginResponse = _scenarioContext.Get<HttpResponseMessage>("hackerLoginResponse");
        
        hackerLoginResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}