using SdetPortfolio.AiQa;
using SdetPortfolio.AiQa.Evaluation;

var apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY");

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("GROQ_API_KEY não encontrada.");
    return;
}

using var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(60)
};

var options = new AiTestCaseGeneratorOptions(
    "https://api.groq.com/openai/v1",
    apiKey,
    "openai/gpt-oss-20b");

// 1. Gera os casos de teste
var generator = new AiTestCaseGenerator(httpClient, options);

// 2. Avalia a fidelidade
var faithfulnessEvaluator = new FaithfulnessEvaluator(
    httpClient,
    options);

var decisionEvaluator = new FaithfulnessDecisionEvaluator();

var evaluationService = new FaithfulnessEvaluationService(
    faithfulnessEvaluator,
    decisionEvaluator);

const string userStory = """
As a user, I want to create an account
so that I can access the application.

The password must contain at least 12 characters.
The email address must have a valid format.
""";

Console.WriteLine("Generating test cases with AI...");

var testCases = await generator.GenerateAsync(userStory);

Console.WriteLine($"\nGenerated {testCases.Count} test cases.");

foreach (var testCase in testCases)
{
    Console.WriteLine("\n==================================");
    Console.WriteLine($"TEST: {testCase.Title}");
    Console.WriteLine($"OBJECTIVE: {testCase.Objective}");
    Console.WriteLine($"EXPECTED: {testCase.ExpectedResult}");

    var generatedTest = $"""
        Title: {testCase.Title}
        Objective: {testCase.Objective}
        Steps:
        {string.Join("\n", testCase.Steps)}
        Expected result: {testCase.ExpectedResult}
        """;

    Console.WriteLine("\nEvaluating test faithfulness...");

    var report = await evaluationService.EvaluateAsync(
        userStory,
        generatedTest);

    Console.WriteLine($"Faithful: {report.IsFaithful}");
    Console.WriteLine($"Confidence: {report.Confidence}");
    Console.WriteLine($"Decision: {report.Decision}");
    Console.WriteLine($"Reason: {report.Reason}");
}

Console.WriteLine("\nAI QUALITY EVALUATION COMPLETED.");