﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atom.MachineLearning.Core
{
    public struct TrainingResult : ITrainingResult
    {
        public float Accuracy { get; set; }
    }
}