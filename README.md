# SDET Portfolio — AI-Assisted Quality Engineering

A hands-on portfolio demonstrating how I combine **software quality engineering, test automation, and AI** using C# and .NET 10.

> **Project status:** In development. The AI-assisted test design MVP is implemented; UI automation, API test suites, BDD, and additional CI capabilities are planned.

## What this project demonstrates

- Applying AI to accelerate test design without treating model output as truth.
- Validating and constraining AI-generated test cases before they enter a QA workflow.
- Testing AI integrations deterministically with a mocked HTTP provider.
- Building maintainable automation in C#/.NET.
- Keeping a human QA engineer responsible for review, risk analysis, and final test decisions.

## Implemented: AI-assisted test case design

The `SdetPortfolio.AiQa` component sends a user story to an OpenAI-compatible chat completions endpoint and converts the response into structured test cases.

**Current safeguards**
- User-story input validation and size limits.
- Explicit instructions to treat story content as untrusted data.
- Structured JSON response parsing.
- Validation of required fields, test-case count, and output size.
- No execution of AI-generated code or test steps.
- Unit tests use a stub HTTP handler, so they do not require an API key or network access.

AI-generated cases are suggestions. A QA engineer must review them for correctness, coverage, and product risk before using them.

## Technology

| Area | Technology |
|---|---|
| Language / runtime | C# / .NET 10 |
| AI integration | OpenAI-compatible Chat Completions API via HttpClient |
| Serialization | System.Text.Json |
| Unit testing | NUnit |
| Planned UI automation | Selenium WebDriver |
| Planned BDD | Reqnroll |
| Planned CI/CD | GitHub Actions |

## Repository structure

```text
sdet-portfolio/
├── src/
│   └── SdetPortfolio.AiQa/
│       ├── AiTestCaseGenerator.cs
│       ├── AiTestCaseGeneratorOptions.cs
│       └── GeneratedTestCase.cs
├── tests/
│   └── SdetPortfolio.AiQa.Tests/
│       └── AiTestCaseGeneratorTests.cs
├── .github/workflows/
├── .gitignore
└── README.md
```

## Run the tests

Requires the .NET 10 SDK.

```bash
dotnet test tests/SdetPortfolio.AiQa.Tests/SdetPortfolio.AiQa.Tests.csproj
```

The test suite uses a fake HTTP response and does not call an external AI service.

## Using the AI integration

Create an `AiTestCaseGenerator` with an `HttpClient` and provider settings:

```csharp
var options = new AiTestCaseGeneratorOptions(
    BaseUrl: Environment.GetEnvironmentVariable("AI_BASE_URL")!,
    ApiKey: Environment.GetEnvironmentVariable("AI_API_KEY")!,
    Model: Environment.GetEnvironmentVariable("AI_MODEL")!);

var generator = new AiTestCaseGenerator(new HttpClient(), options);
var testCases = await generator.GenerateAsync(
    "As a registered user, I want to reset my password so that I can regain access.");
```

Keep API keys in environment variables or a secret manager. Never commit credentials to the repository. The configured base URL should be the provider's API base URL (for example, ending in `/v1`).

## Quality principles

- **Human in the loop:** AI proposes; QA reviews and decides.
- **Trust boundaries:** model output is external, untrusted input.
- **Determinism:** automated tests do not depend on a live model.
- **Traceability:** test cases have explicit objectives, steps, expected results, and priority.
- **Pragmatism:** use AI where it reduces repetitive work, not as a replacement for engineering judgment.

## Roadmap

- [x] Implement an AI-assisted test case generator.
- [x] Add deterministic unit tests for valid and invalid provider responses.
- [ ] Add prompt-injection and malformed-response test coverage.
- [ ] Add a small UI automation suite with Selenium.
- [ ] Add BDD scenarios with Reqnroll and NUnit.
- [ ] Add REST API tests using HttpClient.
- [ ] Add GitHub Actions build and test workflow.
- [ ] Add test reports and failure diagnostics.
- [ ] Document architecture decisions and example QA workflows.

## About

This project explores practical AI-enabled quality engineering: combining automation, critical thinking, and responsible AI integration in a maintainable .NET codebase.
