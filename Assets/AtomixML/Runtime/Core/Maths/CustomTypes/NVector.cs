﻿using Atom.MachineLearning.Core.Maths;
using Sirenix.OdinInspector;
using System;
using System.Linq;

namespace Atom.MachineLearning.Core
{
    [Serializable]
    public struct NVector : IMLInOutData
    {
        [ShowInInspector, ReadOnly] public double[] Data { get; set; }

        public int Length => Data.Length;

        public double x => Data[0];
        public double y => Data[1];
        public double z => Data[2];
        public double w => Data[3];

        public double this[int index]
        {
            get
            {
                return Data[index];
            }
            set
            {
                Data[index] = value;
            }
        }

        public static NVector operator +(NVector a, NVector b)
        {
            if (a.Length != b.Length) throw new ArgumentException($"Vector dimensions aren't equals. A is {a.Length} and B is {b.Length}");

            double[] temp = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                temp[i] = a[i] + b[i];
            }

            return new NVector(temp);
        }

        public static NVector operator -(NVector a, NVector b)
        {
            if (a.Length != b.Length) throw new ArgumentException($"Vector dimensions aren't equals. A is {a.Length} and B is {b.Length}");

            double[] temp = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                temp[i] = a[i] - b[i];
            }

            return new NVector(temp);
        }

        public static bool operator ==(NVector a, NVector b)
        {
            if (a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }

        public static bool operator !=(NVector a, NVector b)
        {
            if (a.Length != b.Length) return true;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return true;
            }

            return false;
        }

        public static NVector operator *(NVector a, double b)
        {
            double[] temp = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                temp[i] = a[i] * b;
            }

            return new NVector(temp);
        }

        public static NVector operator /(NVector a, double b)
        {
            double[] temp = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                temp[i] = a[i] / b;
            }

            return new NVector(temp);
        }

        public NVector(int dimensions)
        {
            Data = new double[dimensions];
        }

        public NVector(double[] arr)
        {
            Data = new double[arr.Length];

            for (int i = 0; i < arr.Length; ++i)
                Data[i] = arr[i];
        }

        /// <summary>
        /// scalar
        /// </summary>
        /// <param name="x"></param>
        public NVector(double x)
        {
            Data = new double[] { x };
        }

        public NVector(double x, double y)
        {
            Data = new double[] { x, y };
        }

        public NVector(double x, double y, double z)
        {
            Data = new double[] { x, y, z };
        }

        public NVector(double x, double y, double z, double w)
        {
            Data = new double[] { x, y, z, w };
        }

        public override string ToString()
        {
            return string.Join(", ", Data);
        }

        /// <summary>
        /// Returns the magnitude of the vector
        /// </summary>
        public double magnitude
        {
            get
            {
                double magn = 0.0;
                for(int i = 0; i < Data.Length; ++i)
                {
                    magn += Math.Pow(Data[i], 2);
                }

                return Math.Sqrt(magn);
            }
        }

        /// <summary>
        /// Returns a normalized duplicate of the vector
        /// </summary>
        public NVector normalized
        {
            get
            {
                var vect = new NVector(Data);
                vect.Normalize();
                return vect;
            }
        }

        public NVector Random(double min = 0, double max = 1)
        {
            for(int i = 0; i < Data.Length; ++i)
            {
                Data[i] = MLRandom.Shared.Range(min, max);
            }

            return this;
        }

        /// <summary>
        /// Normalizes the vector
        /// </summary>
        public NVector Normalize()
        {
            double mag = magnitude;

            if (mag > 0.0)
                for (int i = 0; i < Data.Length; ++i)
                    Data[i] /= mag;

            return this;
        }

        /// <summary>
        /// Dot product of the two n-dimensional vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns> The dot product, a scalar </returns>
        /// <exception cref="ArgumentException"></exception>
        public static double Dot(NVector a, NVector b)
        {
            if (a.Length != b.Length) throw new ArgumentException($"Vector dimensions aren't equals. A is {a.Length} and B is {b.Length}");

            double dot = 0.0;
            for (int i = 0; i < a.Length; ++i)
                dot += a[i] * b[i];

            return dot;
        }

        /// <summary>
        /// Srt euclidian distance between two multidimensionnal vectors represented by float arrays
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Manhattan(NVector a, NVector b)
        {
            if (a.Length != b.Length) throw new ArgumentException($"Vector dimensions aren't equals. A is {a.Length} and B is {b.Length}");

            double result = 0;
            for (int i = 0; i < a.Length; ++i)
            {
                result += Math.Abs(a[i] - b[i]);
            }

            return result;
        }

        /// <summary>
        /// Srt euclidian distance between two multidimensionnal vectors represented by float arrays
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Euclidian(NVector a, NVector b)
        {
            if (a.Length != b.Length) throw new ArgumentException($"Vector dimensions aren't equals. A is {a.Length} and B is {b.Length}");

            double result = 0;
            for (int i = 0; i < a.Length; ++i)
            {
                result += Math.Pow(a[i] - b[i], 2);
            }

            return Math.Sqrt(result);
        }

        /// <summary>
        /// Euclidian (without square root) distance between two multidimensionnal vectors represented by float arrays
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double SquaredEuclidian(NVector a, NVector b)
        {
            if (a.Length != b.Length) throw new ArgumentException($"Vector dimensions aren't equals. A is {a.Length} and B is {b.Length}");

            double result = 0;
            for (int i = 0; i < a.Length; ++i)
            {
                result += Math.Pow(a[i] - b[i], 2);
            }

            return result;
        }

        public static double Mnkowski(NVector a, NVector b, double power)
        {
            if (a.Length != b.Length) throw new ArgumentException($"Vector dimensions aren't equals. A is {a.Length} and B is {b.Length}");

            double result = 0;
            for (int i = 0; i < a.Length; ++i)
            {
                result += Math.Pow(Math.Abs(a[i] - b[i]), power);
            }

            double z = 1 / power;

            return Math.Pow(result, z);
        }

        /// <summary>
        /// WIP
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static NVector Cross(NVector a, NVector b)
        {
            if (a.Length != b.Length) throw new ArgumentException($"Vector dimensions aren't equals. A is {a.Length} and B is {b.Length}");
            if (a.Length < 3) throw new ArgumentException($"Vector should have at least 3 dimensions");

            int columns = a.Length;
            for (int i = 0; i < columns; ++i)
            {
                for (int j = 0; j < columns; ++j)
                {
                    UnityEngine.Debug.Log("/" + a[j] + " < j > " + b[j]);

                    if (j == i) continue;

                    UnityEngine.Debug.Log(a[j] + " < j > " + b[j]);
                    UnityEngine.Debug.Log(a[i] + " < i > " + b[i]);
                }

                UnityEngine.Debug.Log("----");
            }

            return new NVector();
        }


        public static NVector Average(NVector[] vectors)
        {
            int dimensions = vectors[0].Length;

            var mean = new NVector(dimensions);
            for (int i = 0; i < vectors.Length; ++i)
            {
                mean += vectors[i];
            }

            return mean /= vectors.Length;
        }

        public double FeaturesMin()
        {
            double result = 0.0;
            for (int i = 0; i < Data.Length; ++i)
            {
                result = Math.Min(result, Data[i]);
            }
            return result;
        }

        public double FeaturesMax()
        {
            double result = 0.0;
            for (int i = 0; i < Data.Length; ++i)
            {
                result = Math.Max(result, Data[i]);
            }
            return result;
        }

        /// <summary>
        /// Compute the mean of a column of the array of vectors at the featureIndex (aka the feature of the vector we want to sum)
        /// </summary>
        /// <param name="vectors"></param>
        /// <param name="featureIndex"></param>
        /// <returns></returns>
        public static double FeatureAverage(NVector[] vectors, int featureIndex)
        {
            double sum = 0.0;
            for (int i = 0; i < vectors.Length; ++i)
            {
                sum += vectors[i][featureIndex];
            }

            return sum / vectors.Length;
        }

        public static double FeatureStandardDeviation(NVector[] vectors, double feature_mean, int featureIndex)
        {
            var sum = 0.0;
            for (int i = 0; i < vectors.Length; ++i)
            {
                sum += Math.Pow(vectors[i][featureIndex] - feature_mean, 2);
            }

            return Math.Sqrt((sum / vectors.Length));
        }

        public static double Covariance(NVector a, NVector b)
        {
            return Covariance(a.Data, b.Data);
        }

        public static double Covariance(double[] featureA, double[] featureB)
        {
            if (featureA.Length != featureB.Length)
                throw new ArgumentException("Feature arrays must have the same length.");

            double meanA = featureA.Average();
            double meanB = featureB.Average();

            double sum = 0.0;

            for (int i = 0; i < featureA.Length; i++)
            {
                sum += (featureA[i] - meanA) * (featureB[i] - meanB);
            }

            return sum / (featureA.Length - 1);  // Using n-1 for sample covariance
        }

        /// <summary>
        /// Covariance matrix of the array of n-dimensional vectors
        /// <param name="datas"></param>
        /// <returns></returns>
        public static double[,] CovarianceMatrix(NVector[] datas)
        {
            int dimensions = datas[0].Length;
            var matrix = new double[datas[0].Length, datas[0].Length];

            // Iterate over each vector arrays column
            for (int i = 0; i < dimensions; ++i)
            {
                for (int j = 0; j < dimensions; ++j)
                {
                    // Collect all values for features i and j across all vectors
                    double[] featureIValues = new double[datas.Length];
                    double[] featureJValues = new double[datas.Length];

                    // summing each column into two arrays
                    for (int k = 0; k < datas.Length; k++)
                    {
                        featureIValues[k] = datas[k][i];
                        featureJValues[k] = datas[k][j];
                    }

                    // Compute covariance matrix between features i and j
                    // covariance matrix is a matrix of all couple feature i - j
                    matrix[i, j] = Covariance(featureIValues, featureJValues);
                }
            }

            return matrix;
        }

        public static NVector[] Standardize(NVector[] vectors, out NVector meanVector, out NVector stdDeviationsVector, out double mean_std_dev)
        {
            int dimensions = vectors[0].Length;

            meanVector = new NVector(dimensions);
            stdDeviationsVector = new NVector(dimensions);

            // compute mean for each feature of the n-dimensional vector array
            for (int i = 0; i < dimensions; ++i)
            {
                meanVector.Data[i] = NVector.FeatureAverage(vectors, i);
            }

            // compute standardDeviation for each feature of the n-dimensional vector array
            mean_std_dev = 0.0;
            for (int i = 0; i < dimensions; ++i)
            {
                stdDeviationsVector.Data[i] = NVector.FeatureStandardDeviation(vectors, meanVector[i], i);
                mean_std_dev += stdDeviationsVector.Data[i];
            }
            mean_std_dev /= dimensions;

            // apply standardisation to ech n-vector
            NVector[] result = new NVector[vectors.Length];
            for (int i = 0; i < vectors.Length; ++i)
            {
                result[i] = Standardize(vectors[i], meanVector, stdDeviationsVector, mean_std_dev);
            }

            return result;
        }

        public static NVector Standardize(NVector vector, NVector meanVector, NVector stdDeviations, double mean_std_dev)
        {
            var result = new NVector(vector.Length);

            for (int j = 0; j < vector.Length; ++j)
            {
                result.Data[j] = (vector[j] - meanVector[j]) / (stdDeviations[j] != 0f ? stdDeviations[j] : mean_std_dev);
            }

            return result;
        }

        public static NVector[] Normalize(NVector[] datas)
        {
            int dimension = datas[0].Length;
            NVector[] normalizedData = new NVector[datas.Length];
            double minValue = int.MaxValue;
            double maxValue = 0;

            for (int i = 0; i < normalizedData.Length; ++i)
            {
                double minTemp = datas[i].FeaturesMax();
                minValue = minTemp < minValue ? minTemp : minValue;

                double maxTemp = datas[i].FeaturesMin();
                maxValue = maxTemp > maxValue ? maxTemp : maxValue;
            }

            double delta = maxValue - minValue;

            for (int i = 0; i < normalizedData.Length; ++i)
            {
                NVector normalized = new NVector(dimension);
                for (int j = 0; j < dimension; ++j)
                {
                    normalized[j] = (datas[i][j] - minValue) / delta;                       
                }

                normalizedData[i] = normalized;
            }

            return normalizedData;
        }
    }

    public static class NVectorExtensions
    {
        /// <summary>
        /// Srt euclidian distance between two multidimensionnal vectors represented by float arrays
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double SqrtEuclidian(this NVector a, NVector b)
        {
            return NVector.Euclidian(a, b);
        }

        /// <summary>
        /// Euclidian distance between two multidimensionnal vectors represented by float arrays
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Euclidian(this NVector a, NVector b)
        {
            return NVector.SquaredEuclidian(a, b);
        }

        public static double Average(this NVector vector)
        {
            double val = 0;
            for (int i = 0; i < vector.Length; ++i)
            {
                val += vector[i];
            }

            return val / vector.Length;
        }

        /// <summary>
        /// Transform the input matrix into an array of row n-vectors
        /// </summary>
        /// <param name="matrix2D"></param>
        /// <returns></returns>
        public static NVector[] ToNVectorRowsArray(this double[,] matrix2D)
        {
            var result = new NVector[matrix2D.GetLength(0)];
            double[] temp = new double[matrix2D.GetLength(1)];

            for (int i = 0; i < result.Length; ++i)
            {
                for (int j = 0; j < temp.Length; ++j)
                {
                    temp[j] = matrix2D[i, j];
                }
                result[i] = new NVector(temp);
            }

            return result;
        }
    }
}