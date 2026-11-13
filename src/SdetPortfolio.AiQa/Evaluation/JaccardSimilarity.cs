using System;
using System.Linq;

namespace SdetPortfolio.AiQa.Evaluation;

public class JaccardSimilarity
{
	public double Calculate(string textA, string textB)
	{
		textA = textA.ToLowerInvariant();
        textB = textB.ToLowerInvariant();

        var setA = textA.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet();
        var setB = textB.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet();

        var intersection = setA.Intersect(setB).Count();
        var union = setA.Union(setB).Count();

        if (union == 0)
        {
            return 0.0; // Avoid division by zero
        }

        return (double)intersection / union;        
	}
}
