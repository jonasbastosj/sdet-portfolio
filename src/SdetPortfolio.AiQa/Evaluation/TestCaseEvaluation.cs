namespace SdetPortfolio.AiQa.Evaluation;

/// <summary>
/// Stores the deterministic quality scores for one AI-generated test case.
/// Each dimension is scored from 0 (does not meet the criterion)
/// to 2 (fully meets the criterion).
/// </summary>
public sealed record TestCaseEvaluation
{
    public TestCaseEvaluation(
        int relevance,
        int coverage,
        int clarity,
        int verifiability,
        int factualAccuracy)
    {
        Relevance = ValidateScore(nameof(relevance), relevance);
        Coverage = ValidateScore(nameof(coverage), coverage);
        Clarity = ValidateScore(nameof(clarity), clarity);
        Verifiability = ValidateScore(nameof(verifiability), verifiability);
        FactualAccuracy = ValidateScore(nameof(factualAccuracy), factualAccuracy);
    }

    public int Relevance { get; }

    public int Coverage { get; }

    public int Clarity { get; }

    public int Verifiability { get; }

    public int FactualAccuracy { get; }

    public int Total =>
        Relevance + Coverage + Clarity + Verifiability + FactualAccuracy;

    // A high total cannot compensate for unsupported business assumptions.
    public bool Passed => Total >= 8 && FactualAccuracy == 2;

    private static int ValidateScore(string parameterName, int score)
    {
        if (score is < 0 or > 2)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                score,
                "Each evaluation score must be between 0 and 2.");
        }

        return score;
    }
}
