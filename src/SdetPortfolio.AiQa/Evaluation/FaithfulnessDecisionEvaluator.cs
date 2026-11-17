namespace SdetPortfolio.AiQa.Evaluation;

public sealed class FaithfulnessDecisionEvaluator
{
    private const double MinimumConfidence = 0.70;


    public FaithfulnessDecision Evaluate(FaithfulnessResult result)
    {
        if (!result.IsFaithful)
            return FaithfulnessDecision.Fail;

        if (result.Confidence >= MinimumConfidence)
            return FaithfulnessDecision.Pass;

        return FaithfulnessDecision.Review;
    }

}