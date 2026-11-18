using System.Net;
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

// Recebe a User Story pelo terminal
Console.WriteLine("Enter the User Story:");
Console.WriteLine("(Press Enter twice to finish)");

var lines = new List<string>();
var consecutiveEmptyLines = 0;

while (true)
{
    var line = Console.ReadLine();

    if (string.IsNullOrEmpty(line))
    {
        consecutiveEmptyLines++;

        if (consecutiveEmptyLines == 2)
        {
            break;
        }
    }
    else
    {
        consecutiveEmptyLines = 0;
        lines.Add(line);
    }
}

var userStory = string.Join(Environment.NewLine, lines).Trim();

if (string.IsNullOrWhiteSpace(userStory))
{
    Console.WriteLine("User Story cannot be empty.");
    return;
}

Console.WriteLine("\nGenerating test cases with AI...");

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

    try
    {
        var report = await evaluationService.EvaluateAsync(
            userStory,
            generatedTest);

        Console.WriteLine($"Faithful: {report.IsFaithful}");
        Console.WriteLine($"Confidence: {report.Confidence}");
        Console.WriteLine($"Decision: {report.Decision}");
        Console.WriteLine($"Reason: {report.Reason}");
    }
    catch (HttpRequestException ex)
        when (ex.StatusCode == HttpStatusCode.TooManyRequests)
    {
        Console.WriteLine("Faithful: Not evaluated");
        Console.WriteLine("Decision: REVIEW");
        Console.WriteLine("Reason: Groq API rate limit exceeded. Try again later.");
    }
}

Console.WriteLine("\nAI QUALITY EVALUATION COMPLETED.");
