namespace SdetPortfolio.AiQa.Evaluation;

public sealed class FaithfulnessDecisionEvaluator
{
    private const double MinimumConfidence = 0.70;


    public FaithfulnessDecision Evaluate(FaithfulnessResult result)
    {
        if (result.IsFaithful && result.Confidence >= MinimumConfidence)
        {
            return FaithfulnessDecision.Pass;
        }
        else if (!result.IsFaithful && result.Confidence >= MinimumConfidence)
        {
            return FaithfulnessDecision.Fail;
        }
        else
        {
            return FaithfulnessDecision.Review;
        }
    }

}