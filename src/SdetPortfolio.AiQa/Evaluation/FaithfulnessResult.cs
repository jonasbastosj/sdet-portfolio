namespace SdetPortfolio.AiQa.Evaluation;

public sealed record FaithfulnessResult(
    bool IsFaithful,
    string Reason,
    double Confidence);
