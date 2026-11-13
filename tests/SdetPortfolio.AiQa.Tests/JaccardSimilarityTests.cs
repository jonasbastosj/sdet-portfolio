using NUnit.Framework;
using SdetPortfolio.AiQa.Evaluation;

namespace SdetPortfolio.AiQa.Tests;

[TestFixture]
public class JaccardSimilarityTests
{
    [Test]
    public void ShouldReturnOne_WhenTextsAreIdentical()
    {
        var jaccard = new JaccardSimilarity();
        var result = jaccard.Calculate("login password", "login password");
        Assert.That(result, Is.EqualTo(1.0));    }
    
    [Test]
    public void ShouldReturnPointThreeThree_WhenOneWordMatches()
    {
        var jaccard = new JaccardSimilarity();
        var result = jaccard.Calculate("login password", "login username");
        Assert.That(result, Is.EqualTo(0.3333333333333333));
    }       

    [Test]
    public void ShouldReturnZero_WhenTextsAreDifferent()
    {
        var jaccard = new JaccardSimilarity();
        var result = jaccard.Calculate("apple", "orange");
        Assert.That(result, Is.EqualTo(0.0));
    }

    [Test]
    public void ShouldReturnZero_WhenTextsAreEmpty()
    {
        var jaccard = new JaccardSimilarity();
        var result = jaccard.Calculate("", "");
        Assert.That(result, Is.EqualTo(0.0));
    }

    [Test]
    public void ShouldReturnOne_WhenTextsHaveExtraSpaces()
    {
        var jaccard = new JaccardSimilarity();
        var result = jaccard.Calculate("  login   password  ", "login password");
        Assert.That(result, Is.EqualTo(1.0));
    }
}
