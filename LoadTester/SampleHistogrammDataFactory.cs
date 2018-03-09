using System;
using System.Collections.Generic;
using System.Linq;

namespace LoadTester
{
    public class SampleHistogrammDataFactory
    {
        public static int ChartSize = 500;

        public static UniqueValue[] GetResultedValues(UniqueValue[] p_uniqueValues)
        {
            UniqueValue[] resultedValues;
            if (p_uniqueValues.Length == ChartSize)
            {
                resultedValues = p_uniqueValues;
            }
            else
            {
                resultedValues = new UniqueValue[ChartSize];
                if (ChartSize > p_uniqueValues.Length)
                {
                    double stepSize = ((double) ChartSize)/(double) p_uniqueValues.Length;
                    
                    int targetIndex = 0;
                    double currentStep = 0.0;
                    int previousStep = 0;

                    for (int i = 0; i < p_uniqueValues.Length; i++)
                    {
                        var uniqueValue = p_uniqueValues[i];

                        targetIndex = (int) Math.Floor(currentStep);
                        SimpleFillValues(resultedValues, uniqueValue, previousStep, targetIndex);

                        previousStep = targetIndex;
                        currentStep += stepSize;
                    }
                }
                else if (p_uniqueValues.Length > ChartSize)
                {
                    var stepSize = ((double) ChartSize)/p_uniqueValues.Length;
                    double currentStep = 0.0;
                    for (int index = 0; index < p_uniqueValues.Length; index++)
                    {
                        var uniqueValue = p_uniqueValues[index];
                        currentStep += stepSize;
                        var j = (int) Math.Floor(currentStep);
                        if (j > ChartSize - 1)
                            j = ChartSize - 1;

                        resultedValues[j] = new UniqueValue();
                        resultedValues[j].Value = uniqueValue.Value;
                        resultedValues[j].Count = uniqueValue.Count;
                    }
                }
            }
            return resultedValues;
        }

        private static void SimpleFillValues(IList<UniqueValue> p_targetCollection, UniqueValue p_value, int p_startIndex, int p_endIndex)
        {
            for (int index = p_startIndex; index < p_endIndex; index++)
            {
                p_targetCollection[index] = p_value;
            }
        }

        public class UniqueValue
        {
            public double Value;
            public int Count;
        }

        public static UniqueValue[] GetSortedUniques(double[] p_values)
        {
            if (p_values == null)
                throw new ArgumentNullException(nameof(p_values));

            if (p_values.Length == 0)
                return new UniqueValue[0];

            if (p_values.Length == 1)
                return new UniqueValue[] {new UniqueValue() {Count = 1, Value = p_values[0]}};


            var uniqueValues = new List<UniqueValue>();

            Array.Sort(p_values);

            double previousValue = 0.0;

            var count = 0;
            previousValue = p_values[0];
            for (int index = 1; index < p_values.Length; index++)
            {
                var value = p_values[index];

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value == previousValue)
                {
                    ++count;
                }
                else
                {
                    uniqueValues.Add(new UniqueValue() {Count = count + 1, Value = previousValue});

                    count = 0;
                }

                previousValue = value;
            }
            if (count != 0)
            {
                uniqueValues.Add(new UniqueValue() {Count = count + 1, Value = previousValue});
            }
            return uniqueValues.OrderBy(p_value => p_value.Value).ToArray();
        }
    }
}