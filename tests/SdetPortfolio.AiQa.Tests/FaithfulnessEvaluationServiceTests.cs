using System.Net;
using System.Text;
using NUnit.Framework;
using SdetPortfolio.AiQa;
using SdetPortfolio.AiQa.Evaluation;

namespace SdetPortfolio.AiQa.Tests;

[TestFixture]
public sealed class FaithfulnessEvaluationServiceTests
{
    [Test]
    public async Task EvaluateAsync_WhenTestIsNotFaithful_ReturnsFailReport()
    {
        const string providerResponse = """
            {
              "choices": [{
                "message": {
                  "content": "{\"isFaithful\":false,\"reason\":\"The test contradicts the requirement.\",\"confidence\":0.98}"
                }
              }]
            }
            """;

        using var client = new HttpClient(
            new StubHttpMessageHandler(providerResponse));

        var evaluator = new FaithfulnessEvaluator(
            client,
            new AiTestCaseGeneratorOptions(
                "https://api.example.test/v1",
                "test-key",
                "test-model"));

        var decisionEvaluator = new FaithfulnessDecisionEvaluator();

        var service = new FaithfulnessEvaluationService(
            evaluator,
            decisionEvaluator);

        var report = await service.EvaluateAsync(
            "Passwords must contain at least 12 characters.",
            "Verify that passwords with 8 characters are accepted.");

        Assert.That(report.IsFaithful, Is.False);
        Assert.That(report.Reason, Is.EqualTo("The test contradicts the requirement."));
        Assert.That(report.Confidence, Is.EqualTo(0.98));
        Assert.That(report.Decision, Is.EqualTo(FaithfulnessDecision.Fail));
    }

    [Test]
public async Task EvaluateAsync_WhenTestIsFaithful_ReturnsPassReport()
{
    const string providerResponse = """
        {
          "choices": [{
            "message": {
              "content": "{\"isFaithful\":true,\"reason\":\"The test correctly represents the requirement.\",\"confidence\":0.95}"
            }
          }]
        }
        """;

    using var client = new HttpClient(
        new StubHttpMessageHandler(providerResponse));

    var evaluator = new FaithfulnessEvaluator(
        client,
        new AiTestCaseGeneratorOptions(
            "https://api.example.test/v1",
            "test-key",
            "test-model"));

    var service = new FaithfulnessEvaluationService(
        evaluator,
        new FaithfulnessDecisionEvaluator());

    var report = await service.EvaluateAsync(
        "Passwords must contain at least 12 characters.",
        "Verify that passwords with 12 characters are accepted.");

    Assert.That(report.IsFaithful, Is.True);
    Assert.That(report.Confidence, Is.EqualTo(0.95));
    Assert.That(report.Decision, Is.EqualTo(FaithfulnessDecision.Pass));
}

    private sealed class StubHttpMessageHandler(string responseBody) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    responseBody,
                    Encoding.UTF8,
                    "application/json")
            });
        }
    }
}