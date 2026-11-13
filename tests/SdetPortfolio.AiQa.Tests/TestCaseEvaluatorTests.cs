using NUnit.Framework;
using SdetPortfolio.AiQa.Evaluation;

namespace SdetPortfolio.AiQa.Tests;

[TestFixture]
public sealed class TestCaseEvaluatorTests
{
    [Test]
    public void Passed_ShouldBeTrue_WhenAllScoresAreMaximum()
    {
        var evaluation = new TestCaseEvaluation(
            relevance: 2,
            coverage: 2,
            clarity: 2,
            verifiability: 2,
            factualAccuracy: 2);

        Assert.That(evaluation.Total, Is.EqualTo(10));
        Assert.That(evaluation.Passed, Is.True);
    }

    [Test]
    public void Passed_ShouldBeFalse_WhenTotalScoreIsBelowEight()
    {
        var evaluation = new TestCaseEvaluation(
            relevance: 2,
            coverage: 1,
            clarity: 2,
            verifiability: 1,
            factualAccuracy: 1);

        Assert.That(evaluation.Total, Is.EqualTo(7));
        Assert.That(evaluation.Passed, Is.False);
    }

    [Test]
    public void Passed_ShouldBeFalse_WhenFactualAccuracyIsBelowTwo()
    {
        var evaluation = new TestCaseEvaluation(
            relevance: 2,
            coverage: 2,
            clarity: 2,
            verifiability: 2,
            factualAccuracy: 0);

        Assert.That(evaluation.Total, Is.EqualTo(8));
        Assert.That(evaluation.Passed, Is.False);
    }

    [Test]
    public void Constructor_ShouldRejectScoresOutsideZeroToTwo()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new TestCaseEvaluation(
                relevance: 3,
                coverage: 2,
                clarity: 2,
                verifiability: 2,
                factualAccuracy: 2));
    }
}
