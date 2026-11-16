namespace SdetPortfolio.AiQa.Evaluation;

public class FaithfulnessResult
{
    public bool IsFaithful { get; set; }

    public string Reason { get; set; } = string.Empty;

    public double Confidence { get; set; }
}