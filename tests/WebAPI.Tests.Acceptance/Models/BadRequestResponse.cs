namespace CleanArchitecture.WebApi.Tests.Acceptance.Models;

public class BadRequestResponse
{
    public string Title { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Detail { get; set; } = null!;
}