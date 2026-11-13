using NUnit.Framework;
using SdetPortfolio.AiQa;
using SdetPortfolio.AiQa.Evaluation;

namespace SdetPortfolio.AiQa.Tests;

[TestFixture]
public sealed class TestCaseEvaluatorStructureTests
{
    private readonly TestCaseEvaluator _evaluator = new();

    [Test]
    public void EvaluateStructure_ShouldPass_WhenAllRequiredFieldsArePresent()
    {
        var result = _evaluator.EvaluateStructure(CreateTestCase());

        Assert.That(result.IsStructurallyValid, Is.True);
        Assert.That(result.RequiresHumanReview, Is.True);
    }

    [Test]
    public void EvaluateStructure_ShouldFail_WhenTitleIsMissing()
    {
        var result = _evaluator.EvaluateStructure(CreateTestCase() with { Title = " " });

        Assert.That(result.HasTitle, Is.False);
        Assert.That(result.IsStructurallyValid, Is.False);
    }

    [Test]
    public void EvaluateStructure_ShouldFail_WhenStepsAreEmpty()
    {
        var result = _evaluator.EvaluateStructure(
            CreateTestCase() with { Steps = Array.Empty<string>() });

        Assert.That(result.HasSteps, Is.False);
        Assert.That(result.IsStructurallyValid, Is.False);
    }

    [Test]
    public void EvaluateStructure_ShouldFail_WhenAnyStepIsBlank()
    {
        var result = _evaluator.EvaluateStructure(
            CreateTestCase() with { Steps = new[] { "Open the page", " " } });

        Assert.That(result.HasSteps, Is.False);
        Assert.That(result.IsStructurallyValid, Is.False);
    }

    [Test]
    public void EvaluateStructure_ShouldFail_WhenExpectedResultIsMissing()
    {
        var result = _evaluator.EvaluateStructure(
            CreateTestCase() with { ExpectedResult = "" });

        Assert.That(result.HasExpectedResult, Is.False);
        Assert.That(result.IsStructurallyValid, Is.False);
    }

    [Test]
    public void EvaluateStructure_ShouldRejectNullTestCase()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _evaluator.EvaluateStructure(null!));
    }

    private static GeneratedTestCase CreateTestCase() =>
        new(
            Title: "Registered user can reset password",
            Objective: "Verify that a registered user can request a password reset.",
            Preconditions: "The user has a registered account.",
            Steps: new[] { "Open the sign-in page", "Request a password reset" },
            ExpectedResult: "A password reset confirmation is displayed.",
            Priority: "High");
}
