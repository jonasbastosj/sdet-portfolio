using System;
using System.Linq;
using System.Reflection;
using NUnit;
using NUnit.Framework;

namespace SdetPortfolio.AiQa.Tests;

public class AgeValidatorTests
{
    [TestCase(18, true)]
    [TestCase(17, false)]
    [TestCase(25, true)]
    public void ValidateAge_Should_Return_Expected_Result(int age, bool expected)
    {
       // Arrange
        var validator = new AgeValidator();

        // Act
        var result = validator.IsAdult(age);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}
