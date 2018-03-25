using System;
using System.Collections.Generic;
using System.Linq;

namespace LoadTester
{
    public class SampleHistogrammDataFactory
    {
        public readonly int ChartSize;

        public SampleHistogrammDataFactory(int p_chartSize)
        {
            ChartSize = p_chartSize;
        }

        public UniqueValue[] GetHistagrammValues(UniqueValue[] p_uniqueValues)
        {
            UniqueValue[] resultedValues;
            if (p_uniqueValues.Length == ChartSize)
            {
                resultedValues = p_uniqueValues;
            }
            else
            {
                resultedValues = new UniqueValue[ChartSize];
                UniqueValue minValue;
                UniqueValue maxValue;
                FindMinMax(p_uniqueValues, out minValue, out maxValue);

                var step = maxValue.Value / ((double)ChartSize - 1);

                for (int i = 0; i < resultedValues.Length; i++)
                {
                    var resultedValue = resultedValues[i] = new UniqueValue();
                    resultedValue.Count = 0;
                    resultedValue.Value = i * step;
                }
                resultedValues[0].Value = minValue.Value;
                resultedValues[resultedValues.Length - 1].Value = maxValue.Value;


                for (int i = 0; i < p_uniqueValues.Length; i++)
                {
                    var uniqueValue = p_uniqueValues[i];
                    var index = FindIndex(step, uniqueValue.Value);

                    var resultedValue = resultedValues[index];

                    resultedValue.Count += uniqueValue.Count;
                }
            }

            double summ = 0.0;
            for (int i = 0; i < resultedValues.Length; i++)
            {
                summ += resultedValues[i].Count;
            }

            for (int i = 0; i < resultedValues.Length; i++)
            {
                var resultedValue = resultedValues[i];
                resultedValue.Count = 100 * (double)resultedValue.Count / summ;
            }

            return resultedValues;
        }

        private int FindIndex(double p_step, double p_value)
        {
            var position = p_value/p_step;
            int result = (int) Math.Floor(position);
            if (result <0)
            {
                result = 0;
            }
            else if (result >= ChartSize)
            {
                result = ChartSize - 1;
            }
            return result;
        }

        private static void FindMinMax(UniqueValue[] p_uniqueValues, out UniqueValue p_minVal, out UniqueValue p_maxVal)
        {
            p_minVal = null;
            p_maxVal = null;

            if (p_uniqueValues.Length != 0)
            {
                p_minVal = p_uniqueValues[0];
                p_maxVal = p_uniqueValues[0];
            }

            for (int index= 1; index < p_uniqueValues.Length; index++)
            {
                var uniqueValue = p_uniqueValues[index];

                if (uniqueValue.Value < p_minVal.Value)
                    p_minVal = uniqueValue;

                if (uniqueValue.Value > p_maxVal.Value)
                    p_maxVal = uniqueValue;
            }
        }

        public class UniqueValue
        {
            public double Value;
            public double Count;
        }

        public  UniqueValue[] GetSortedUniques(double[] p_values)
        {
            if (p_values == null)
                throw new ArgumentNullException("p_values");

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
            uniqueValues.Add(new UniqueValue() {Count = count + 1, Value = previousValue});

            return uniqueValues.OrderBy(p_value => p_value.Value).ToArray();
        }
    }
}