There are two approaches developed using TDD for calculating the total fees for a given income amount. 
Fees are calculated progressively for income ranges in the same way that income tax is calculated.
The Amount column in the Fee object is percent fee for the fee range.


Approach 1- CalculateFee1(income)

Calculate the BaseThresholdFee as the accumulated fees for each of the previous range fees for all the fees. 

Select the relevant Fee item which is the maximum Threshold value less than or equal to the income amount.
 
Calculate the fee for the difference between the income amount and the Threshold vale for this maximum Fee item.

The total fee is equal to this fee plus the BaseThresholdFee for the maximum Fee item.

An ExtendedFee object extends the Fee object with the BaseThresholdFee property.
The Fee list is passed into the ProgressiveFeeCalculator constructor which calculates the BaseThresholdFee for each Fee and sets the result in the BaseThresholdFee property.


Approach 2 - Static CalculateFee2(fees, income)

The relevant Fee objects with Threshold values less than or equal to the income amount are sorted in descending Threshold value order.

The first Fee item in the descending ordered list is the maximum Fee item. 

The fee on the difference between the income amount and the Threshold value on the maximum Fee item is calculated.

Then the list is iterated calculating the fee for each Fee object based on the difference in Threshold values with the next Fee in the list multiplied by the next Fee item Amount as a percentage rate. The Fee range fee result is added to the income fee.

The accumulated income fee is returned when there are no more next Fee items in the list. 
