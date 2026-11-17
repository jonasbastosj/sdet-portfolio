namespace SdetPortfolio.AiQa.Evaluation;

public sealed class FaithfulnessEvaluationService(
    FaithfulnessEvaluator faithfulnessEvaluator,
    FaithfulnessDecisionEvaluator decisionEvaluator)
{
    public async Task<FaithfulnessEvaluationReport> EvaluateAsync(
        string requirement,
        string generatedTest,
        CancellationToken cancellationToken = default)
    {
        var result = await faithfulnessEvaluator.EvaluateAsync(
            requirement,
            generatedTest,
            cancellationToken);

        var decision = decisionEvaluator.Evaluate(result);

        return new FaithfulnessEvaluationReport(
            result.IsFaithful,
            result.Reason,
            result.Confidence,
            decision);
    }
}