
using NUnit.Framework;
using SdetPortfolio.AiQa.Evaluation;

namespace SdetPortfolio.AiQa.Tests;

[TestFixture]
public sealed class FaithfulnessDecisionEvaluatorTests
{
    private FaithfulnessDecisionEvaluator _evaluator = null!;

    [SetUp]
    public void SetUp()
    {
        _evaluator = new FaithfulnessDecisionEvaluator();
    }

    [TestCase(true, 0.90, FaithfulnessDecision.Pass)]
    [TestCase(false, 0.90, FaithfulnessDecision.Fail)]
    [TestCase(true, 0.55, FaithfulnessDecision.Review)]
    [TestCase(false, 0.55, FaithfulnessDecision.Fail)]
    [TestCase(true, 0.70, FaithfulnessDecision.Pass)]
    public void Evaluate_ReturnsExpectedDecision(
        bool isFaithful,
        double confidence,
        FaithfulnessDecision expected)
    {
        var result = new FaithfulnessResult(
            isFaithful,
            "Evaluation reason",
            confidence);

        var decision = _evaluator.Evaluate(result);

        Assert.That(decision, Is.EqualTo(expected));
    }
}
