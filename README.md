# SDET Portfolio — UI & API Test Automation

A professional test automation portfolio built with **C# and .NET 10**, demonstrating maintainable UI and API testing, BDD, and CI/CD practices.

> **Project status:** In development. Features listed as planned are not yet implemented.

## Goals

This repository demonstrates how I approach software quality as an engineer:

- Design maintainable and readable automated tests.
- Apply BDD with Reqnroll where it improves collaboration and clarity.
- Automate browser workflows with Selenium WebDriver.
- Validate REST APIs, including positive, negative, and boundary scenarios.
- Make test execution repeatable locally and in CI.
- Produce useful test reports and actionable failure diagnostics.

## Planned technology stack

| Area | Technology |
|---|---|
| Language | C# |
| Runtime | .NET 10 |
| UI automation | Selenium WebDriver |
| BDD | Reqnroll |
| Test framework | NUnit |
| API testing | HttpClient |
| CI/CD | GitHub Actions |
| Reporting | Allure or a suitable NUnit-compatible reporter (to be evaluated) |

## Planned solution structure

```text
sdet-portfolio/
├── src/
│   └── SdetPortfolio.Framework/
├── tests/
│   ├── SdetPortfolio.UiTests/
│   └── SdetPortfolio.ApiTests/
├── .github/
│   └── workflows/
├── docs/
├── .gitignore
└── README.md
```

The structure may evolve as the framework is implemented. The goal is to keep test scenarios, automation infrastructure, and reusable application interactions clearly separated.

## Quality principles

- **Maintainability:** small, focused components and clear naming.
- **Reliability:** explicit waits, deterministic test data, and isolated tests.
- **Traceability:** readable scenarios and meaningful test names.
- **Diagnostics:** useful logs, screenshots on UI failures, and clear CI output.
- **Practicality:** avoid abstractions that make tests harder to understand.

## Getting started

Prerequisites and setup instructions will be added with the first runnable test project.

## Roadmap

- [ ] Create the .NET solution and test projects.
- [ ] Add a first UI smoke test using Selenium.
- [ ] Add BDD scenarios with Reqnroll and NUnit.
- [ ] Add REST API tests using HttpClient.
- [ ] Configure GitHub Actions to build and run tests.
- [ ] Publish test results and failure artifacts.
- [ ] Document architecture and key engineering decisions.

## About

This is a hands-on SDET portfolio project focused on C#, .NET, browser automation, API testing, BDD, and continuous integration.
