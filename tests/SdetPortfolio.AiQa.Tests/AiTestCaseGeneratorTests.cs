using System.Net;
using System.Text;
using NUnit.Framework;

namespace SdetPortfolio.AiQa.Tests;

[TestFixture]
public sealed class AiTestCaseGeneratorTests
{
    [Test]
    public async Task GenerateAsync_WhenProviderReturnsValidJson_ReturnsTestCases()
    {
        const string providerResponse = """
            {
              "choices": [{
                "message": {
                  "content": "{\"testCases\":[{\"title\":\"Reject invalid password\",\"objective\":\"Verify authentication validation\",\"preconditions\":\"A registered user exists\",\"steps\":[\"Open the login page\",\"Enter a valid email and invalid password\",\"Submit the form\"],\"expectedResult\":\"Access is denied and a safe error is displayed\",\"priority\":\"High\"}]}"
                }
              }]
            }
            """;

        using var client = new HttpClient(new StubHttpMessageHandler(providerResponse));
        var generator = new AiTestCaseGenerator(
            client,
            new AiTestCaseGeneratorOptions("https://api.example.test/v1", "test-key", "test-model"));

        var result = await generator.GenerateAsync("As a user, I want to sign in securely.");

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Title, Is.EqualTo("Reject invalid password"));
        Assert.That(result[0].Steps, Has.Count.EqualTo(3));
    }

    [Test]
    public void GenerateAsync_WhenStoryIsEmpty_ThrowsBeforeCallingProvider()
    {
        using var client = new HttpClient(new StubHttpMessageHandler("{}"));
        var generator = new AiTestCaseGenerator(
            client,
            new AiTestCaseGeneratorOptions("https://api.example.test/v1", "test-key", "test-model"));

        Assert.ThrowsAsync<ArgumentException>(() => generator.GenerateAsync(" "));
    }

    [Test]
    public void GenerateAsync_WhenAiReturnsNoTestCases_RejectsTheResponse()
    {
        const string providerResponse = """
            {
              "choices": [{
                "message": {
                  "content": "{\"testCases\":[]}"
                }
              }]
            }
            """;

        using var client = new HttpClient(new StubHttpMessageHandler(providerResponse));
        var generator = new AiTestCaseGenerator(
            client,
            new AiTestCaseGeneratorOptions("https://api.example.test/v1", "test-key", "test-model"));

        Assert.ThrowsAsync<InvalidOperationException>(
            () => generator.GenerateAsync("As a user, I want to sign in."));
    }

    private sealed class StubHttpMessageHandler(string responseBody) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
            Assert.That(request.Headers.Authorization?.Scheme, Is.EqualTo("Bearer"));

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
            });
        }
    }
}
