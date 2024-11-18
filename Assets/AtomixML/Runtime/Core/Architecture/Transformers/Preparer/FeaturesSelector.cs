﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Atom.MachineLearning.Core.Transformers
{
    public class FeaturesSelector : IMLTransformer<NStringVector, NStringVector>
    {
        [SerializeField, LearnedParameter] private int[] _selectedColumns;

        public FeaturesSelector(int[] selectedColumns)
        {
            _selectedColumns = selectedColumns;
        }

        public NStringVector Predict(NStringVector inputData)
        {
            var row = new NStringVector(_selectedColumns.Length);

            for (int j = 0; j < _selectedColumns.Length; j++)
            {
                row[j] = inputData[_selectedColumns[j]];
            }

            return row;
        }

        public NStringVector[] Transform(string[,] input) => Transform(input.ToNStringVectorArray());

        public NStringVector[] Transform(NStringVector[] input)
        {
            NStringVector[] result = new NStringVector[input.Length];
            for (int i  = 0; i < input.Length; i++)
            {
                var row = Predict(input[i]);
                result[i] = row;
            }

            return result;
        }

    }
}