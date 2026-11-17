using System.Net;
using System.Text;
using NUnit.Framework;
using SdetPortfolio.AiQa;
using SdetPortfolio.AiQa.Evaluation;

namespace SdetPortfolio.AiQa.Tests;

[TestFixture]
public sealed class FaithfulnessEvaluatorTests
{
    [Test]
    public async Task EvaluateAsync_WhenProviderReturnsValidEvaluation_ReturnsFaithfulnessResult()
    {
        const string providerResponse = """
            {
              "choices": [{
                "message": {
                  "content": "{\"isFaithful\":false,\"reason\":\"The test accepts an 8-character password, but the requirement demands at least 12 characters.\",\"confidence\":0.98}"
                }
              }]
            }
            """;

        using var client = new HttpClient(new StubHttpMessageHandler(providerResponse));
        var evaluator = new FaithfulnessEvaluator(
            client,
            new AiTestCaseGeneratorOptions("https://api.example.test/v1", "test-key", "test-model"));

        var result = await evaluator.EvaluateAsync(
            "Passwords must contain at least 12 characters.",
            "Verify that passwords with 8 characters are accepted.");

        Assert.That(result.IsFaithful, Is.False);
        Assert.That(result.Reason, Does.Contain("8-character"));
        Assert.That(result.Confidence, Is.EqualTo(0.98));
    }

    [Test]
    public void EvaluateAsync_WhenRequirementIsEmpty_ThrowsBeforeCallingProvider()
    {
        using var client = new HttpClient(new StubHttpMessageHandler("{}"));
        var evaluator = new FaithfulnessEvaluator(
            client,
            new AiTestCaseGeneratorOptions("https://api.example.test/v1", "test-key", "test-model"));

        Assert.ThrowsAsync<ArgumentException>(
            () => evaluator.EvaluateAsync(" ", "A generated test."));
    }

    [Test]
    public void EvaluateAsync_WhenConfidenceIsOutsideAllowedRange_RejectsResponse()
    {
        const string providerResponse = """
            {
              "choices": [{
                "message": {
                  "content": "{\"isFaithful\":true,\"reason\":\"Looks correct.\",\"confidence\":1.5}"
                }
              }]
            }
            """;

        using var client = new HttpClient(new StubHttpMessageHandler(providerResponse));
        var evaluator = new FaithfulnessEvaluator(
            client,
            new AiTestCaseGeneratorOptions("https://api.example.test/v1", "test-key", "test-model"));

        Assert.ThrowsAsync<InvalidOperationException>(
            () => evaluator.EvaluateAsync("A requirement.", "A generated test."));
    }

    private sealed class StubHttpMessageHandler(string responseBody) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
            Assert.That(request.RequestUri?.AbsoluteUri,
                Is.EqualTo("https://api.example.test/v1/chat/completions"));
            Assert.That(request.Headers.Authorization?.Scheme, Is.EqualTo("Bearer"));

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
            });
        }
    }
}
