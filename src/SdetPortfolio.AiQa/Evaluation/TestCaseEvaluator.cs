namespace SdetPortfolio.AiQa.Evaluation;

/// <summary>
/// Performs deterministic structural checks on an AI-generated test case.
/// These checks do not replace semantic review by a QA engineer.
/// </summary>
public sealed class TestCaseEvaluator
{
    public TestCaseStructureEvaluation EvaluateStructure(GeneratedTestCase testCase)
    {
        ArgumentNullException.ThrowIfNull(testCase);

        var hasTitle = !string.IsNullOrWhiteSpace(testCase.Title);
        var hasObjective = !string.IsNullOrWhiteSpace(testCase.Objective);
        var hasSteps = testCase.Steps is { Count: > 0 } &&
                       testCase.Steps.All(step => !string.IsNullOrWhiteSpace(step));
        var hasExpectedResult = !string.IsNullOrWhiteSpace(testCase.ExpectedResult);
        var hasPriority = !string.IsNullOrWhiteSpace(testCase.Priority);

        return new TestCaseStructureEvaluation(
            HasTitle: hasTitle,
            HasObjective: hasObjective,
            HasSteps: hasSteps,
            HasExpectedResult: hasExpectedResult,
            HasPriority: hasPriority);
    }
}

/// <summary>
/// Reports objective structural checks only. A structurally valid test case
/// can still be irrelevant, incomplete, or factually incorrect.
/// </summary>
public sealed record TestCaseStructureEvaluation(
    bool HasTitle,
    bool HasObjective,
    bool HasSteps,
    bool HasExpectedResult,
    bool HasPriority)
{
    public bool IsStructurallyValid =>
        HasTitle && HasObjective && HasSteps && HasExpectedResult && HasPriority;

    public bool RequiresHumanReview => true;
}
