using System;
using System.Linq;

namespace SdetPortfolio.AiQa.Evaluation;

public class JaccardSimilarity
{
	public double Calculate(string textA, string textB)
	{
		textA = textA.ToLower();
        textB = textB.ToLower();

        var setA = textA.Split(' ').ToHashSet();
        var setB = textB.Split(' ').ToHashSet();

        var intersection = setA.Intersect(setB).Count();
        var union = setA.Union(setB).Count();

        if (union == 0)
        {
            return 0.0; // Avoid division by zero
        }

        return (double)intersection / union;
	}
}
