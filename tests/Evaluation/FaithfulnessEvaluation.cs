namespace SdetPortfolio.AiQa.Tests;

public sealed class FaithfulnessEvaluation
{
	public bool IsFaithful { get; init; }

	public string Reason { get; init; } = string.Empty;

	public double Confidence { get; init; }

	public string Decision { get; init; } = string.Empty;
}
