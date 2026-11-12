namespace SdetPortfolio.AiQa;

public sealed record GeneratedTestCase(
    string Title,
    string Objective,
    string Preconditions,
    IReadOnlyList<string> Steps,
    string ExpectedResult,
    string Priority);
