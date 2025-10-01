using System.Reflection.Metadata.Ecma335;
using Domain;

namespace Application;

public class ProgressiveFeeCalculator(List<Fee> fees)
{
  public List<ExtendedFee> ExtendedFees { get; } = ToExtendedFees(fees);

  public int CalculateFee1(int income)
  {
    var extendedFee = ExtendedFees
      .LastOrDefault(x => income >= x.Threshold);
    if (extendedFee == null) return 0;

    var thresholdIncomeRange = income - extendedFee.Threshold;
    var thresholdIncomeFee = thresholdIncomeRange * extendedFee.Amount / 100;
    var incomeFee = extendedFee.BaseThresholdFee + thresholdIncomeFee;

    return incomeFee;
  }

  public static int CalculateFee2(List<Fee> fees, int income)
  {
    var incomeFee = 0;

    var relevantFees = fees
      .Where(x => income >= x.Threshold)
      .OrderByDescending(x => x.Threshold)
      .ToList();

    if (relevantFees.Count == 0) return 0;

    var thresholdDifference = income - relevantFees[0].Threshold;
    var thresholdFee = thresholdDifference * relevantFees[0].Amount / 100;
    incomeFee += thresholdFee;

    for (var i = 0; i < relevantFees.Count; i++)
    {
      var currentFee = relevantFees[i];
      if (i < relevantFees.Count - 1)
      {
        var nextFee = relevantFees[i + 1];
        thresholdFee = (currentFee.Threshold - nextFee.Threshold)
          * nextFee.Amount / 100;
        incomeFee += thresholdFee;
      }
    }

    return incomeFee;
  }

  private static List<ExtendedFee> ToExtendedFees(List<Fee> fees)
  {
    var extendedFees = new List<ExtendedFee>();
    var orderedFees = fees.OrderBy(x => x.Threshold).ToList();

    var previousThreshold = 0;
    var previousRate = 0;
    var accumulatedThresholdAmount = 0;

    foreach (var fee in orderedFees)
    {
      var previousThresholdRange = fee.Threshold - previousThreshold;
      var thresholdRangeFee = previousThresholdRange * previousRate / 100;
      accumulatedThresholdAmount += thresholdRangeFee;

      extendedFees.Add(new()
      {
        Id = fee.Id,
        Amount = fee.Amount,
        Threshold = fee.Threshold,
        FeeGroupId = fee.FeeGroupId,
        BaseThresholdFee = accumulatedThresholdAmount,
      });

      previousThreshold = fee.Threshold;
      previousRate = fee.Amount;
    }

    return extendedFees;
  }
}
