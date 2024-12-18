﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Atom.MachineLearning.NeuralNetwork
{
    [CreateAssetMenu(menuName = "TrainingSets/FlowClassification")]
    public class FlowerClassification : TrainingSettingBase
    {
        public double[][] normalized_datas;

        public override void Init()
        {
            normalized_datas = data();
            //DATA = NormalizeData(DATA, 4);
            Normalize(normalized_datas, new int[] { 0, 1, 2, 3 });
        }

        public override void GetTrainDatas(out double[][] x_datas, out double[][] t_datas)
        {
            x_datas = new double[TrainingDataLenght][];
            t_datas = new double[TrainingDataLenght][];

            for (int i = 0; i < x_datas.GetLength(0); ++i)
            {
                GetNextValues(out x_datas[i], out t_datas[i]);
            }
        }

        public override bool ValidateRun(double[] y_values, double[] t_values)
        {
            int index = NeuralNetworkMathHelper.MaxIndex(y_values);
            int tMaxIndex = NeuralNetworkMathHelper.MaxIndex(t_values);
            if (index.Equals(tMaxIndex))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void GetNextValues(out double[] x_val, out double[] t_val)
        {
            int index = UnityEngine.Random.Range(0, normalized_datas.Length);
            x_val = new double[4];
            t_val = new double[3];

            for (int j = 0; j < 4; ++j)
            {
                x_val[j] = normalized_datas[index][j];
            }

            for (int k = 0; k < 3; ++k)
            {
                t_val[k] = normalized_datas[index][4 + k];
            }
        }

        static void Normalize(double[][] dataMatrix, int[] cols)
        {
            // normalize specified cols by computing (x - mean) / sd for each value
            foreach (int col in cols)
            {
                double sum = 0.0;
                for (int i = 0; i < dataMatrix.Length; ++i)
                    sum += dataMatrix[i][col];
                double mean = sum / dataMatrix.Length;
                sum = 0.0;
                for (int i = 0; i < dataMatrix.Length; ++i)
                    sum += (dataMatrix[i][col] - mean) * (dataMatrix[i][col] - mean);
                // thanks to Dr. W. Winfrey, Concord Univ., for catching bug in original code
                double sd = Math.Sqrt(sum / (dataMatrix.Length - 1));
                for (int i = 0; i < dataMatrix.Length; ++i)
                    dataMatrix[i][col] = (dataMatrix[i][col] - mean) / sd;
            }
        }

        double[][] data()
        {
            double[][] allData = new double[150][];
            allData[0] = new double[] { 5.1, 3.5, 1.4, 0.2, 0, 0, 1 }; // sepal length, width, petal length, width
            allData[1] = new double[] { 4.9, 3.0, 1.4, 0.2, 0, 0, 1 }; // Iris setosa = 0 0 1
            allData[2] = new double[] { 4.7, 3.2, 1.3, 0.2, 0, 0, 1 }; // Iris versicolor = 0 1 0
            allData[3] = new double[] { 4.6, 3.1, 1.5, 0.2, 0, 0, 1 }; // Iris virginica = 1 0 0
            allData[4] = new double[] { 5.0, 3.6, 1.4, 0.2, 0, 0, 1 };
            allData[5] = new double[] { 5.4, 3.9, 1.7, 0.4, 0, 0, 1 };
            allData[6] = new double[] { 4.6, 3.4, 1.4, 0.3, 0, 0, 1 };
            allData[7] = new double[] { 5.0, 3.4, 1.5, 0.2, 0, 0, 1 };
            allData[8] = new double[] { 4.4, 2.9, 1.4, 0.2, 0, 0, 1 };
            allData[9] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };

            allData[10] = new double[] { 5.4, 3.7, 1.5, 0.2, 0, 0, 1 };
            allData[11] = new double[] { 4.8, 3.4, 1.6, 0.2, 0, 0, 1 };
            allData[12] = new double[] { 4.8, 3.0, 1.4, 0.1, 0, 0, 1 };
            allData[13] = new double[] { 4.3, 3.0, 1.1, 0.1, 0, 0, 1 };
            allData[14] = new double[] { 5.8, 4.0, 1.2, 0.2, 0, 0, 1 };
            allData[15] = new double[] { 5.7, 4.4, 1.5, 0.4, 0, 0, 1 };
            allData[16] = new double[] { 5.4, 3.9, 1.3, 0.4, 0, 0, 1 };
            allData[17] = new double[] { 5.1, 3.5, 1.4, 0.3, 0, 0, 1 };
            allData[18] = new double[] { 5.7, 3.8, 1.7, 0.3, 0, 0, 1 };
            allData[19] = new double[] { 5.1, 3.8, 1.5, 0.3, 0, 0, 1 };

            allData[20] = new double[] { 5.4, 3.4, 1.7, 0.2, 0, 0, 1 };
            allData[21] = new double[] { 5.1, 3.7, 1.5, 0.4, 0, 0, 1 };
            allData[22] = new double[] { 4.6, 3.6, 1.0, 0.2, 0, 0, 1 };
            allData[23] = new double[] { 5.1, 3.3, 1.7, 0.5, 0, 0, 1 };
            allData[24] = new double[] { 4.8, 3.4, 1.9, 0.2, 0, 0, 1 };
            allData[25] = new double[] { 5.0, 3.0, 1.6, 0.2, 0, 0, 1 };
            allData[26] = new double[] { 5.0, 3.4, 1.6, 0.4, 0, 0, 1 };
            allData[27] = new double[] { 5.2, 3.5, 1.5, 0.2, 0, 0, 1 };
            allData[28] = new double[] { 5.2, 3.4, 1.4, 0.2, 0, 0, 1 };
            allData[29] = new double[] { 4.7, 3.2, 1.6, 0.2, 0, 0, 1 };

            allData[30] = new double[] { 4.8, 3.1, 1.6, 0.2, 0, 0, 1 };
            allData[31] = new double[] { 5.4, 3.4, 1.5, 0.4, 0, 0, 1 };
            allData[32] = new double[] { 5.2, 4.1, 1.5, 0.1, 0, 0, 1 };
            allData[33] = new double[] { 5.5, 4.2, 1.4, 0.2, 0, 0, 1 };
            allData[34] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };
            allData[35] = new double[] { 5.0, 3.2, 1.2, 0.2, 0, 0, 1 };
            allData[36] = new double[] { 5.5, 3.5, 1.3, 0.2, 0, 0, 1 };
            allData[37] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };
            allData[38] = new double[] { 4.4, 3.0, 1.3, 0.2, 0, 0, 1 };
            allData[39] = new double[] { 5.1, 3.4, 1.5, 0.2, 0, 0, 1 };

            allData[40] = new double[] { 5.0, 3.5, 1.3, 0.3, 0, 0, 1 };
            allData[41] = new double[] { 4.5, 2.3, 1.3, 0.3, 0, 0, 1 };
            allData[42] = new double[] { 4.4, 3.2, 1.3, 0.2, 0, 0, 1 };
            allData[43] = new double[] { 5.0, 3.5, 1.6, 0.6, 0, 0, 1 };
            allData[44] = new double[] { 5.1, 3.8, 1.9, 0.4, 0, 0, 1 };
            allData[45] = new double[] { 4.8, 3.0, 1.4, 0.3, 0, 0, 1 };
            allData[46] = new double[] { 5.1, 3.8, 1.6, 0.2, 0, 0, 1 };
            allData[47] = new double[] { 4.6, 3.2, 1.4, 0.2, 0, 0, 1 };
            allData[48] = new double[] { 5.3, 3.7, 1.5, 0.2, 0, 0, 1 };
            allData[49] = new double[] { 5.0, 3.3, 1.4, 0.2, 0, 0, 1 };

            allData[50] = new double[] { 7.0, 3.2, 4.7, 1.4, 0, 1, 0 };
            allData[51] = new double[] { 6.4, 3.2, 4.5, 1.5, 0, 1, 0 };
            allData[52] = new double[] { 6.9, 3.1, 4.9, 1.5, 0, 1, 0 };
            allData[53] = new double[] { 5.5, 2.3, 4.0, 1.3, 0, 1, 0 };
            allData[54] = new double[] { 6.5, 2.8, 4.6, 1.5, 0, 1, 0 };
            allData[55] = new double[] { 5.7, 2.8, 4.5, 1.3, 0, 1, 0 };
            allData[56] = new double[] { 6.3, 3.3, 4.7, 1.6, 0, 1, 0 };
            allData[57] = new double[] { 4.9, 2.4, 3.3, 1.0, 0, 1, 0 };
            allData[58] = new double[] { 6.6, 2.9, 4.6, 1.3, 0, 1, 0 };
            allData[59] = new double[] { 5.2, 2.7, 3.9, 1.4, 0, 1, 0 };

            allData[60] = new double[] { 5.0, 2.0, 3.5, 1.0, 0, 1, 0 };
            allData[61] = new double[] { 5.9, 3.0, 4.2, 1.5, 0, 1, 0 };
            allData[62] = new double[] { 6.0, 2.2, 4.0, 1.0, 0, 1, 0 };
            allData[63] = new double[] { 6.1, 2.9, 4.7, 1.4, 0, 1, 0 };
            allData[64] = new double[] { 5.6, 2.9, 3.6, 1.3, 0, 1, 0 };
            allData[65] = new double[] { 6.7, 3.1, 4.4, 1.4, 0, 1, 0 };
            allData[66] = new double[] { 5.6, 3.0, 4.5, 1.5, 0, 1, 0 };
            allData[67] = new double[] { 5.8, 2.7, 4.1, 1.0, 0, 1, 0 };
            allData[68] = new double[] { 6.2, 2.2, 4.5, 1.5, 0, 1, 0 };
            allData[69] = new double[] { 5.6, 2.5, 3.9, 1.1, 0, 1, 0 };

            allData[70] = new double[] { 5.9, 3.2, 4.8, 1.8, 0, 1, 0 };
            allData[71] = new double[] { 6.1, 2.8, 4.0, 1.3, 0, 1, 0 };
            allData[72] = new double[] { 6.3, 2.5, 4.9, 1.5, 0, 1, 0 };
            allData[73] = new double[] { 6.1, 2.8, 4.7, 1.2, 0, 1, 0 };
            allData[74] = new double[] { 6.4, 2.9, 4.3, 1.3, 0, 1, 0 };
            allData[75] = new double[] { 6.6, 3.0, 4.4, 1.4, 0, 1, 0 };
            allData[76] = new double[] { 6.8, 2.8, 4.8, 1.4, 0, 1, 0 };
            allData[77] = new double[] { 6.7, 3.0, 5.0, 1.7, 0, 1, 0 };
            allData[78] = new double[] { 6.0, 2.9, 4.5, 1.5, 0, 1, 0 };
            allData[79] = new double[] { 5.7, 2.6, 3.5, 1.0, 0, 1, 0 };

            allData[80] = new double[] { 5.5, 2.4, 3.8, 1.1, 0, 1, 0 };
            allData[81] = new double[] { 5.5, 2.4, 3.7, 1.0, 0, 1, 0 };
            allData[82] = new double[] { 5.8, 2.7, 3.9, 1.2, 0, 1, 0 };
            allData[83] = new double[] { 6.0, 2.7, 5.1, 1.6, 0, 1, 0 };
            allData[84] = new double[] { 5.4, 3.0, 4.5, 1.5, 0, 1, 0 };
            allData[85] = new double[] { 6.0, 3.4, 4.5, 1.6, 0, 1, 0 };
            allData[86] = new double[] { 6.7, 3.1, 4.7, 1.5, 0, 1, 0 };
            allData[87] = new double[] { 6.3, 2.3, 4.4, 1.3, 0, 1, 0 };
            allData[88] = new double[] { 5.6, 3.0, 4.1, 1.3, 0, 1, 0 };
            allData[89] = new double[] { 5.5, 2.5, 4.0, 1.3, 0, 1, 0 };

            allData[90] = new double[] { 5.5, 2.6, 4.4, 1.2, 0, 1, 0 };
            allData[91] = new double[] { 6.1, 3.0, 4.6, 1.4, 0, 1, 0 };
            allData[92] = new double[] { 5.8, 2.6, 4.0, 1.2, 0, 1, 0 };
            allData[93] = new double[] { 5.0, 2.3, 3.3, 1.0, 0, 1, 0 };
            allData[94] = new double[] { 5.6, 2.7, 4.2, 1.3, 0, 1, 0 };
            allData[95] = new double[] { 5.7, 3.0, 4.2, 1.2, 0, 1, 0 };
            allData[96] = new double[] { 5.7, 2.9, 4.2, 1.3, 0, 1, 0 };
            allData[97] = new double[] { 6.2, 2.9, 4.3, 1.3, 0, 1, 0 };
            allData[98] = new double[] { 5.1, 2.5, 3.0, 1.1, 0, 1, 0 };
            allData[99] = new double[] { 5.7, 2.8, 4.1, 1.3, 0, 1, 0 };

            allData[100] = new double[] { 6.3, 3.3, 6.0, 2.5, 1, 0, 0 };
            allData[101] = new double[] { 5.8, 2.7, 5.1, 1.9, 1, 0, 0 };
            allData[102] = new double[] { 7.1, 3.0, 5.9, 2.1, 1, 0, 0 };
            allData[103] = new double[] { 6.3, 2.9, 5.6, 1.8, 1, 0, 0 };
            allData[104] = new double[] { 6.5, 3.0, 5.8, 2.2, 1, 0, 0 };
            allData[105] = new double[] { 7.6, 3.0, 6.6, 2.1, 1, 0, 0 };
            allData[106] = new double[] { 4.9, 2.5, 4.5, 1.7, 1, 0, 0 };
            allData[107] = new double[] { 7.3, 2.9, 6.3, 1.8, 1, 0, 0 };
            allData[108] = new double[] { 6.7, 2.5, 5.8, 1.8, 1, 0, 0 };
            allData[109] = new double[] { 7.2, 3.6, 6.1, 2.5, 1, 0, 0 };

            allData[110] = new double[] { 6.5, 3.2, 5.1, 2.0, 1, 0, 0 };
            allData[111] = new double[] { 6.4, 2.7, 5.3, 1.9, 1, 0, 0 };
            allData[112] = new double[] { 6.8, 3.0, 5.5, 2.1, 1, 0, 0 };
            allData[113] = new double[] { 5.7, 2.5, 5.0, 2.0, 1, 0, 0 };
            allData[114] = new double[] { 5.8, 2.8, 5.1, 2.4, 1, 0, 0 };
            allData[115] = new double[] { 6.4, 3.2, 5.3, 2.3, 1, 0, 0 };
            allData[116] = new double[] { 6.5, 3.0, 5.5, 1.8, 1, 0, 0 };
            allData[117] = new double[] { 7.7, 3.8, 6.7, 2.2, 1, 0, 0 };
            allData[118] = new double[] { 7.7, 2.6, 6.9, 2.3, 1, 0, 0 };
            allData[119] = new double[] { 6.0, 2.2, 5.0, 1.5, 1, 0, 0 };

            allData[120] = new double[] { 6.9, 3.2, 5.7, 2.3, 1, 0, 0 };
            allData[121] = new double[] { 5.6, 2.8, 4.9, 2.0, 1, 0, 0 };
            allData[122] = new double[] { 7.7, 2.8, 6.7, 2.0, 1, 0, 0 };
            allData[123] = new double[] { 6.3, 2.7, 4.9, 1.8, 1, 0, 0 };
            allData[124] = new double[] { 6.7, 3.3, 5.7, 2.1, 1, 0, 0 };
            allData[125] = new double[] { 7.2, 3.2, 6.0, 1.8, 1, 0, 0 };
            allData[126] = new double[] { 6.2, 2.8, 4.8, 1.8, 1, 0, 0 };
            allData[127] = new double[] { 6.1, 3.0, 4.9, 1.8, 1, 0, 0 };
            allData[128] = new double[] { 6.4, 2.8, 5.6, 2.1, 1, 0, 0 };
            allData[129] = new double[] { 7.2, 3.0, 5.8, 1.6, 1, 0, 0 };

            allData[130] = new double[] { 7.4, 2.8, 6.1, 1.9, 1, 0, 0 };
            allData[131] = new double[] { 7.9, 3.8, 6.4, 2.0, 1, 0, 0 };
            allData[132] = new double[] { 6.4, 2.8, 5.6, 2.2, 1, 0, 0 };
            allData[133] = new double[] { 6.3, 2.8, 5.1, 1.5, 1, 0, 0 };
            allData[134] = new double[] { 6.1, 2.6, 5.6, 1.4, 1, 0, 0 };
            allData[135] = new double[] { 7.7, 3.0, 6.1, 2.3, 1, 0, 0 };
            allData[136] = new double[] { 6.3, 3.4, 5.6, 2.4, 1, 0, 0 };
            allData[137] = new double[] { 6.4, 3.1, 5.5, 1.8, 1, 0, 0 };
            allData[138] = new double[] { 6.0, 3.0, 4.8, 1.8, 1, 0, 0 };
            allData[139] = new double[] { 6.9, 3.1, 5.4, 2.1, 1, 0, 0 };

            allData[140] = new double[] { 6.7, 3.1, 5.6, 2.4, 1, 0, 0 };
            allData[141] = new double[] { 6.9, 3.1, 5.1, 2.3, 1, 0, 0 };
            allData[142] = new double[] { 5.8, 2.7, 5.1, 1.9, 1, 0, 0 };
            allData[143] = new double[] { 6.8, 3.2, 5.9, 2.3, 1, 0, 0 };
            allData[144] = new double[] { 6.7, 3.3, 5.7, 2.5, 1, 0, 0 };
            allData[145] = new double[] { 6.7, 3.0, 5.2, 2.3, 1, 0, 0 };
            allData[146] = new double[] { 6.3, 2.5, 5.0, 1.9, 1, 0, 0 };
            allData[147] = new double[] { 6.5, 3.0, 5.2, 2.0, 1, 0, 0 };
            allData[148] = new double[] { 6.2, 3.4, 5.4, 2.3, 1, 0, 0 };
            allData[149] = new double[] { 5.9, 3.0, 5.1, 1.8, 1, 0, 0 };

            return allData;

        }

        private void stock()
        {
            double[][] allData = new double[150][];
            allData[0] = new double[] { 25.6, 17.6, 7.1, 1.1, 0, 0, 1 };
            allData[1] = new double[] { 24.6, 15.1, 7.1, 1.1, 0, 0, 1 };
            allData[2] = new double[] { 23.6, 16.1, 6.6, 1.1, 0, 0, 1 };
            allData[3] = new double[] { 23.1, 15.6, 7.6, 1.1, 0, 0, 1 };
            allData[4] = new double[] { 25.1, 18.1, 7.1, 1.1, 0, 0, 1 };
            allData[5] = new double[] { 27.1, 19.6, 8.6, 2.1, 0, 0, 1 };
            allData[6] = new double[] { 23.1, 17.1, 7.1, 1.6, 0, 0, 1 };
            allData[7] = new double[] { 25.1, 17.1, 7.6, 1.1, 0, 0, 1 };
            allData[8] = new double[] { 22.1, 14.6, 7.1, 1.1, 0, 0, 1 };
            allData[9] = new double[] { 24.6, 15.6, 7.6, 0.6, 0, 0, 1 };
            allData[10] = new double[] { 27.1, 18.6, 7.6, 1.1, 0, 0, 1 };
            allData[11] = new double[] { 24.1, 17.1, 8.1, 1.1, 0, 0, 1 };
            allData[12] = new double[] { 24.1, 15.1, 7.1, 0.6, 0, 0, 1 };
            allData[13] = new double[] { 21.6, 15.1, 5.6, 0.6, 0, 0, 1 };
            allData[14] = new double[] { 29.1, 20.1, 6.1, 1.1, 0, 0, 1 };
            allData[15] = new double[] { 28.6, 22.1, 7.6, 2.1, 0, 0, 1 };
            allData[16] = new double[] { 27.1, 19.6, 6.6, 2.1, 0, 0, 1 };
            allData[17] = new double[] { 25.6, 17.6, 7.1, 1.6, 0, 0, 1 };
            allData[18] = new double[] { 28.6, 19.1, 8.6, 1.6, 0, 0, 1 };
            allData[19] = new double[] { 25.6, 19.1, 7.6, 1.6, 0, 0, 1 };
            allData[20] = new double[] { 27.1, 17.1, 8.6, 1.1, 0, 0, 1 };
            allData[21] = new double[] { 25.6, 18.6, 7.6, 2.1, 0, 0, 1 };
            allData[22] = new double[] { 23.1, 18.1, 5.1, 1.1, 0, 0, 1 };
            allData[23] = new double[] { 25.6, 16.6, 8.6, 2.6, 0, 0, 1 };
            allData[24] = new double[] { 24.1, 17.1, 9.6, 1.1, 0, 0, 1 };
            allData[25] = new double[] { 25.1, 15.1, 8.1, 1.1, 0, 0, 1 };
            allData[26] = new double[] { 25.1, 17.1, 8.1, 2.1, 0, 0, 1 };
            allData[27] = new double[] { 26.1, 17.6, 7.6, 1.1, 0, 0, 1 };
            allData[28] = new double[] { 26.1, 17.1, 7.1, 1.1, 0, 0, 1 };
            allData[29] = new double[] { 23.6, 16.1, 8.1, 1.1, 0, 0, 1 };
            allData[30] = new double[] { 24.1, 15.6, 8.1, 1.1, 0, 0, 1 };
            allData[31] = new double[] { 27.1, 17.1, 7.6, 2.1, 0, 0, 1 };
            allData[32] = new double[] { 26.1, 20.6, 7.6, 0.6, 0, 0, 1 };
            allData[33] = new double[] { 27.6, 21.1, 7.1, 1.1, 0, 0, 1 };
            allData[34] = new double[] { 24.6, 15.6, 7.6, 0.6, 0, 0, 1 };
            allData[35] = new double[] { 25.1, 16.1, 6.1, 1.1, 0, 0, 1 };
            allData[36] = new double[] { 27.6, 17.6, 6.6, 1.1, 0, 0, 1 };
            allData[37] = new double[] { 24.6, 15.6, 7.6, 0.6, 0, 0, 1 };
            allData[38] = new double[] { 22.1, 15.1, 6.6, 1.1, 0, 0, 1 };
            allData[39] = new double[] { 25.6, 17.1, 7.6, 1.1, 0, 0, 1 };
            allData[40] = new double[] { 25.1, 17.6, 6.6, 1.6, 0, 0, 1 };
            allData[41] = new double[] { 22.6, 11.6, 6.6, 1.6, 0, 0, 1 };
            allData[42] = new double[] { 22.1, 16.1, 6.6, 1.1, 0, 0, 1 };
            allData[43] = new double[] { 25.1, 17.6, 8.1, 3.1, 0, 0, 1 };
            allData[44] = new double[] { 25.6, 19.1, 9.6, 2.1, 0, 0, 1 };
            allData[45] = new double[] { 24.1, 15.1, 7.1, 1.6, 0, 0, 1 };
            allData[46] = new double[] { 25.6, 19.1, 8.1, 1.1, 0, 0, 1 };
            allData[47] = new double[] { 23.1, 16.1, 7.1, 1.1, 0, 0, 1 };
            allData[48] = new double[] { 26.6, 18.6, 7.6, 1.1, 0, 0, 1 };
            allData[49] = new double[] { 25.1, 16.6, 7.1, 1.1, 0, 0, 1 };
            allData[50] = new double[] { 35.1, 16.1, 23.6, 7.1, 0, 1, 0 };
            allData[51] = new double[] { 32.1, 16.1, 22.6, 7.6, 0, 1, 0 };
            allData[52] = new double[] { 34.6, 15.6, 24.6, 7.6, 0, 1, 0 };
            allData[53] = new double[] { 27.6, 11.6, 20.1, 6.6, 0, 1, 0 };
            allData[54] = new double[] { 32.6, 14.1, 23.1, 7.6, 0, 1, 0 };
            allData[55] = new double[] { 28.6, 14.1, 22.6, 6.6, 0, 1, 0 };
            allData[56] = new double[] { 31.6, 16.6, 23.6, 8.1, 0, 1, 0 };
            allData[57] = new double[] { 24.6, 12.1, 16.6, 5.1, 0, 1, 0 };
            allData[58] = new double[] { 33.1, 14.6, 23.1, 6.6, 0, 1, 0 };
            allData[59] = new double[] { 26.1, 13.6, 19.6, 7.1, 0, 1, 0 };
            allData[60] = new double[] { 25.1, 10.1, 17.6, 5.1, 0, 1, 0 };
            allData[61] = new double[] { 29.6, 15.1, 21.1, 7.6, 0, 1, 0 };
            allData[62] = new double[] { 30.1, 11.1, 20.1, 5.1, 0, 1, 0 };
            allData[63] = new double[] { 30.6, 14.6, 23.6, 7.1, 0, 1, 0 };
            allData[64] = new double[] { 28.1, 14.6, 18.1, 6.6, 0, 1, 0 };
            allData[65] = new double[] { 33.6, 15.6, 22.1, 7.1, 0, 1, 0 };
            allData[66] = new double[] { 28.1, 15.1, 22.6, 7.6, 0, 1, 0 };
            allData[67] = new double[] { 29.1, 13.6, 20.6, 5.1, 0, 1, 0 };
            allData[68] = new double[] { 31.1, 11.1, 22.6, 7.6, 0, 1, 0 };
            allData[69] = new double[] { 28.1, 12.6, 19.6, 5.6, 0, 1, 0 };
            allData[70] = new double[] { 29.6, 16.1, 24.1, 9.1, 0, 1, 0 };
            allData[71] = new double[] { 30.6, 14.1, 20.1, 6.6, 0, 1, 0 };
            allData[72] = new double[] { 31.6, 12.6, 24.6, 7.6, 0, 1, 0 };
            allData[73] = new double[] { 30.6, 14.1, 23.6, 6.1, 0, 1, 0 };
            allData[74] = new double[] { 32.1, 14.6, 21.6, 6.6, 0, 1, 0 };
            allData[75] = new double[] { 33.1, 15.1, 22.1, 7.1, 0, 1, 0 };
            allData[76] = new double[] { 34.1, 14.1, 24.1, 7.1, 0, 1, 0 };
            allData[77] = new double[] { 33.6, 15.1, 25.1, 8.6, 0, 1, 0 };
            allData[78] = new double[] { 30.1, 14.6, 22.6, 7.6, 0, 1, 0 };
            allData[79] = new double[] { 28.6, 13.1, 17.6, 5.1, 0, 1, 0 };
            allData[80] = new double[] { 27.6, 12.1, 19.1, 5.6, 0, 1, 0 };
            allData[81] = new double[] { 27.6, 12.1, 18.6, 5.1, 0, 1, 0 };
            allData[82] = new double[] { 29.1, 13.6, 19.6, 6.1, 0, 1, 0 };
            allData[83] = new double[] { 30.1, 13.6, 25.6, 8.1, 0, 1, 0 };
            allData[84] = new double[] { 27.1, 15.1, 22.6, 7.6, 0, 1, 0 };
            allData[85] = new double[] { 30.1, 17.1, 22.6, 8.1, 0, 1, 0 };
            allData[86] = new double[] { 33.6, 15.6, 23.6, 7.6, 0, 1, 0 };
            allData[87] = new double[] { 31.6, 11.6, 22.1, 6.6, 0, 1, 0 };
            allData[88] = new double[] { 28.1, 15.1, 20.6, 6.6, 0, 1, 0 };
            allData[89] = new double[] { 27.6, 12.6, 20.1, 6.6, 0, 1, 0 };
            allData[90] = new double[] { 27.6, 13.1, 22.1, 6.1, 0, 1, 0 };
            allData[91] = new double[] { 30.6, 15.1, 23.1, 7.1, 0, 1, 0 };
            allData[92] = new double[] { 29.1, 13.1, 20.1, 6.1, 0, 1, 0 };
            allData[93] = new double[] { 25.1, 11.6, 16.6, 5.1, 0, 1, 0 };
            allData[94] = new double[] { 28.1, 13.6, 21.1, 6.6, 0, 1, 0 };
            allData[95] = new double[] { 28.6, 15.1, 21.1, 6.1, 0, 1, 0 };
            allData[96] = new double[] { 28.6, 14.6, 21.1, 6.6, 0, 1, 0 };
            allData[97] = new double[] { 31.1, 14.6, 21.6, 6.6, 0, 1, 0 };
            allData[98] = new double[] { 25.6, 12.6, 15.1, 5.6, 0, 1, 0 };
            allData[99] = new double[] { 28.6, 14.1, 20.6, 6.6, 0, 1, 0 };
            allData[100] = new double[] { 31.6, 16.6, 30.1, 12.6, 1, 0, 0 };
            allData[101] = new double[] { 29.1, 13.6, 25.6, 9.6, 1, 0, 0 };
            allData[102] = new double[] { 35.6, 15.1, 29.6, 10.6, 1, 0, 0 };
            allData[103] = new double[] { 31.6, 14.6, 28.1, 9.1, 1, 0, 0 };
            allData[104] = new double[] { 32.6, 15.1, 29.1, 11.1, 1, 0, 0 };
            allData[105] = new double[] { 38.1, 15.1, 33.1, 10.6, 1, 0, 0 };
            allData[106] = new double[] { 24.6, 12.6, 22.6, 8.6, 1, 0, 0 };
            allData[107] = new double[] { 36.6, 14.6, 31.6, 9.1, 1, 0, 0 };
            allData[108] = new double[] { 33.6, 12.6, 29.1, 9.1, 1, 0, 0 };
            allData[109] = new double[] { 36.1, 18.1, 30.6, 12.6, 1, 0, 0 };
            allData[110] = new double[] { 32.6, 16.1, 25.6, 10.1, 1, 0, 0 };
            allData[111] = new double[] { 32.1, 13.6, 26.6, 9.6, 1, 0, 0 };
            allData[112] = new double[] { 34.1, 15.1, 27.6, 10.6, 1, 0, 0 };
            allData[113] = new double[] { 28.6, 12.6, 25.1, 10.1, 1, 0, 0 };
            allData[114] = new double[] { 29.1, 14.1, 25.6, 12.1, 1, 0, 0 };
            allData[115] = new double[] { 32.1, 16.1, 26.6, 11.6, 1, 0, 0 };
            allData[116] = new double[] { 32.6, 15.1, 27.6, 9.1, 1, 0, 0 };
            allData[117] = new double[] { 38.6, 19.1, 33.6, 11.1, 1, 0, 0 };
            allData[118] = new double[] { 38.6, 13.1, 34.6, 11.6, 1, 0, 0 };
            allData[119] = new double[] { 30.1, 11.1, 25.1, 7.6, 1, 0, 0 };
            allData[120] = new double[] { 34.6, 16.1, 28.6, 11.6, 1, 0, 0 };
            allData[121] = new double[] { 28.1, 14.1, 24.6, 10.1, 1, 0, 0 };
            allData[122] = new double[] { 38.6, 14.1, 33.6, 10.1, 1, 0, 0 };
            allData[123] = new double[] { 31.6, 13.6, 24.6, 9.1, 1, 0, 0 };
            allData[124] = new double[] { 33.6, 16.6, 28.6, 10.6, 1, 0, 0 };
            allData[125] = new double[] { 36.1, 16.1, 30.1, 9.1, 1, 0, 0 };
            allData[126] = new double[] { 31.1, 14.1, 24.1, 9.1, 1, 0, 0 };
            allData[127] = new double[] { 30.6, 15.1, 24.6, 9.1, 1, 0, 0 };
            allData[128] = new double[] { 32.1, 14.1, 28.1, 10.6, 1, 0, 0 };
            allData[129] = new double[] { 36.1, 15.1, 29.1, 8.1, 1, 0, 0 };
            allData[130] = new double[] { 37.1, 14.1, 30.6, 9.6, 1, 0, 0 };
            allData[131] = new double[] { 39.6, 19.1, 32.1, 10.1, 1, 0, 0 };
            allData[132] = new double[] { 32.1, 14.1, 28.1, 11.1, 1, 0, 0 };
            allData[133] = new double[] { 31.6, 14.1, 25.6, 7.6, 1, 0, 0 };
            allData[134] = new double[] { 30.6, 13.1, 28.1, 7.1, 1, 0, 0 };
            allData[135] = new double[] { 38.6, 15.1, 30.6, 11.6, 1, 0, 0 };
            allData[136] = new double[] { 31.6, 17.1, 28.1, 12.1, 1, 0, 0 };
            allData[137] = new double[] { 32.1, 15.6, 27.6, 9.1, 1, 0, 0 };
            allData[138] = new double[] { 30.1, 15.1, 24.1, 9.1, 1, 0, 0 };
            allData[139] = new double[] { 34.6, 15.6, 27.1, 10.6, 1, 0, 0 };
            allData[140] = new double[] { 33.6, 15.6, 28.1, 12.1, 1, 0, 0 };
            allData[141] = new double[] { 34.6, 15.6, 25.6, 11.6, 1, 0, 0 };
            allData[142] = new double[] { 29.1, 13.6, 25.6, 9.6, 1, 0, 0 };
            allData[143] = new double[] { 34.1, 16.1, 29.6, 11.6, 1, 0, 0 };
            allData[144] = new double[] { 33.6, 16.6, 28.6, 12.6, 1, 0, 0 };
            allData[145] = new double[] { 33.6, 15.1, 26.1, 11.6, 1, 0, 0 };
            allData[146] = new double[] { 31.6, 12.6, 25.1, 9.6, 1, 0, 0 };
            allData[147] = new double[] { 32.6, 15.1, 26.1, 10.1, 1, 0, 0 };
            allData[148] = new double[] { 31.1, 17.1, 27.1, 11.6, 1, 0, 0 };
            allData[149] = new double[] { 29.6, 15.1, 25.6, 9.1, 1, 0, 0 };         
        }

    }
}
