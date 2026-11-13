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
        var validatorType = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .FirstOrDefault(type => type.Name.Equals("AgeValidator", StringComparison.OrdinalIgnoreCase));

        Assert.That(validatorType, Is.Not.Null);

        var method = validatorType!
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
            .FirstOrDefault(candidate =>
                candidate.ReturnType == typeof(bool) &&
                candidate.GetParameters().Length == 1 &&
                candidate.GetParameters()[0].ParameterType == typeof(int));

        Assert.That(method, Is.Not.Null);

        object? result;

        if (method!.IsStatic)
        {
            result = method.Invoke(null, new object[] { age });
        }
        else
        {
            var instance = Activator.CreateInstance(validatorType!);
            result = method.Invoke(instance, new object[] { age });
        }

        Assert.That(result, Is.EqualTo(expected));
    }
}
