using Application;
using Domain;

namespace Tests;

public class ProgressiveFeeCalculatorTests
{
    [Fact]
    public void ExtendedFees_BaseThresholdFeesCorrectlyCalculated()
    {
        var test = GetTestFees();
        var expectedBaseFees = new List<int> {
            0,
            750,
            750 + 2500,
            750 + 2500 + 3000,
            750 + 2500 + 3000 + 4000 };

        var sut = new ProgressiveFeeCalculator(test);
        var actual = sut.ExtendedFees;

        Assert.Equal(5, actual.Count);
        for (var i = 0; i < actual.Count; i++)
        {
            Assert.Equal(expectedBaseFees[i], actual[i].BaseThresholdFee);
        }
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(30_000, 0)]
    [InlineData(45_000, 750)]
    [InlineData(70_000, 750 + 2500)]
    [InlineData(90_000, 750 + 2500 + 3000)]
    [InlineData(110_000, 750 + 2500 + 3000 + 4000)]
    public void CalculateFee1_CorrectThresholdIncomeAmounts(int test, int expected)
    {
        var sut = new ProgressiveFeeCalculator(GetTestFees());

        var actual = sut.CalculateFee1(test);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(28_000, 0)]
    [InlineData(32_000, 100)]
    [InlineData(44_000, 700)]
    [InlineData(47_000, 750 + 200)]
    [InlineData(65_000, 750 + 2000)]
    [InlineData(75_000, 750 + 2500 + 750)]
    [InlineData(95_000, 750 + 2500 + 3000 + 1000)]
    [InlineData(115_000, 750 + 2500 + 3000 + 4000 + 1500)]
    public void CalculateFee1_CorrectNonThresholdIncomeAmounts(int test, int expected)
    {
        var sut = new ProgressiveFeeCalculator(GetTestFees());

        var actual = sut.CalculateFee1(test);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(30_000, 0)]
    [InlineData(45_000, 750)]
    [InlineData(70_000, 750 + 2500)]
    [InlineData(90_000, 750 + 2500 + 3000)]
    [InlineData(110_000, 750 + 2500 + 3000 + 4000)]
    public void StaticCalculateFee2_CorrectThresholdIncomeAmounts(int test, int expected)
    {
        var actual = ProgressiveFeeCalculator.CalculateFee2(GetTestFees(), test);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(28_000, 0)]
    [InlineData(32_000, 100)]
    [InlineData(44_000, 700)]
    [InlineData(47_000, 750 + 200)]
    [InlineData(65_000, 750 + 2000)]
    [InlineData(75_000, 750 + 2500 + 750)]
    [InlineData(95_000, 750 + 2500 + 3000 + 1000)]
    [InlineData(115_000, 750 + 2500 + 3000 + 4000 + 1500)]
    public void StaticCalculateFee2_CorrectNonThresholdIncomeAmounts(int test, int expected)
    {
        var actual = ProgressiveFeeCalculator.CalculateFee2(GetTestFees(), test);

        Assert.Equal(expected, actual);
    }

    private static List<Fee> GetTestFees()
    {
        var fees = new Fee[]
        {
                new()
                {
                    Id = 1,
                    Threshold = 110_000,
                    Amount = 30,
                    FeeGroupId = 1,
                },
                new()
                {
                    Id = 2,
                    Threshold = 30_000,
                    Amount = 5,
                    FeeGroupId = 1,
                },
                new()
                {
                    Id = 3,
                    Threshold = 90_000,
                    Amount = 20,
                    FeeGroupId = 1,
                },
                new()
                {
                    Id = 4,
                    Threshold = 45_000,
                    Amount = 10,
                    FeeGroupId = 1,
                },
                new()
                {
                    Id = 5,
                    Threshold = 70_000,
                    Amount = 15,
                    FeeGroupId = 1,
                },
        };

        return [.. fees];
    }
}
