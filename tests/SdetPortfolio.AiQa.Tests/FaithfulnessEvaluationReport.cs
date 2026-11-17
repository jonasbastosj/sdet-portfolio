namespace SdetPortfolio.AiQa.Evaluation;

public sealed record FaithfulnessEvaluationReport(
    bool IsFaithful,
    string Reason,
    double Confidence,
    FaithfulnessDecision Decision);