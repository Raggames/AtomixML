﻿using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Atom.MachineLearning.NeuralNetwork
{
    public class CNNTrainer : NeuralNetworkTrainer
    {
        [Header("----CNN Trainer----")]
        public Vector2Int ImageDimensions;

        public int Padding = 1;
        public int Stride = 2;

        public ConvolutionnalNeuralNetwork NeuralNetwork;

        private WaitForSeconds delay;
        private Coroutine ExecutionCoroutine;

        [Header("----RUNTIME----")]

        public double[][,] x_datas;
        public double[][] t_datas;

        public double[,] run_inputs;
        public double[] run_outputs;
        public double[] run_test_outputs;

        private void Start()
        {
            Initialize();
        }

        private void OnGUI()
        {

            if (GUI.Button(new Rect(10, 100, 100, 30), "Load"))
            {
                //PrepareExecution();
            }

            if (GUI.Button(new Rect(10, 150, 100, 30), "Test"))
            {
                ExecutionCoroutine = StartCoroutine(Test());
            }

            if (GUI.Button(new Rect(10, 50, 100, 30), "Save"))
            {
                SaveCurrentModel(NeuralNetwork);
            }
        }

        [Button]
        /// <summary>
        /// 95% Accuracy training with 0.043 learning rate / P1, S2 and 2 filters to flatten => dense = flatten count / 2 + output
        /// </summary>

        public void Initialize()
        {
            TrainingSetting.Init();

            NeuralNetwork = new ConvolutionnalNeuralNetwork();

            // Convolute from 28x28 input to 27x27 feature map
            // Convolute from 28x28 input to 13x13 feature map with P1 / S2
            ConvolutionLayer convolutionLayer = new ConvolutionLayer(ImageDimensions.x, ImageDimensions.y, Padding, Stride)
                .AddFilter(KernelType.Random)
                .AddFilter(KernelType.Random);

            convolutionLayer.Initialize();

            NeuralNetwork.CNNLayers.Add(convolutionLayer);

            // *************************

            /* ConvolutionLayer convolutionLayer2 = new ConvolutionLayer(convolutionLayer.OutputWidth, convolutionLayer.OutputHeight, Padding, Stride * 2)
                 .AddFilter(KernelType.Random)
                 .AddFilter(KernelType.Random);

             convolutionLayer2.Initialize();*/

            //NeuralNetwork.CNNLayers.Add(convolutionLayer2);

            // *************************

            // Pooling layer matrix out is 13x13 for 1 filter = 169 neurons

            //WithPooling(convolutionLayer);

            NeuralNetwork.FlattenLayer = new FlattenLayer(convolutionLayer.OutputWidth, convolutionLayer.OutputHeight, 2);

            /*NeuralNetwork.DenseLayers.Add(new DenseLayer(LayerType.DenseHidden, ActivationFunctions.ReLU, NeuralNetwork.FlattenLayer.NodeCount, 60));
            NeuralNetwork.DenseLayers.Add(new DenseLayer(LayerType.Output, ActivationFunctions.Softmax, 60, 10));*/
            
            NeuralNetwork.DenseLayers.Add(new DenseLayer(LayerType.Output, ActivationFunctions.Softmax, NeuralNetwork.FlattenLayer.NodeCount, 10));

            NeuralNetwork.SeedRandomWeights(InitialWeightRange.x, InitialWeightRange.y);

            InitializeTrainingBestWeightSet(NeuralNetwork);
        }

        /// <summary>
        /// Can't converge
        /// </summary>
        /// <param name="convolutionLayer"></param>
        private void WithPooling(ConvolutionLayer convolutionLayer)
        {
            PoolingLayer poolingLayer = new PoolingLayer(
                            convolutionLayer.OutputWidth,
                            convolutionLayer.OutputHeight,
                            convolutionLayer.Depth, // depth = features map length
                            2, // filter size > how much we 'loose' data 
                            Padding,
                            PoolingRule.Max);

            NeuralNetwork.CNNLayers.Add(poolingLayer);

            NeuralNetwork.FlattenLayer = new FlattenLayer(poolingLayer.OutputWidth, poolingLayer.OutputHeight, 2);
        }

        private CancellationTokenSource _tokenSource;
        [Button]
        private async void TrainAsync(int delay = 1)
        {
            Initialize();

            _tokenSource = new CancellationTokenSource();
            Debug.Log("Start training");
            await Task.Run(() => Train(delay), _tokenSource.Token);
            Debug.Log("End training");
        }

        [Button]
        private async void StopTrain()
        {
            _tokenSource.Cancel();
        }

        [Button]
        private void Save()
        {
            SaveCurrentModel(NeuralNetwork);
        }

        private IEnumerator Test()
        {
            CurrentEpoch = 0;

            double current_time = 0;

            // Get training datas from the setting
            TrainingSetting.GetMatrixTrainDatas(out x_datas, out t_datas);

            // Compute number of iterations 
            // Batchsize shouldn't be 0
            int iterations_per_epoch = x_datas.Length / BatchSize;

            int[] sequence_indexes = new int[x_datas.Length];
            for (int i = 0; i < sequence_indexes.Length; ++i)
                sequence_indexes[i] = i;

            for (int i = 0; i < Epochs; ++i)
            {
                int dataIndex = 0;

                // Shuffle datas each epoch
                Shuffle(sequence_indexes);

                // Going through all data batched, mini-batch or stochastic, depending on BatchSize value
                double mean_error_sum = 0;
                for (int d = 0; d < iterations_per_epoch; ++d)
                {
                    for (int j = 0; j < BatchSize; ++j)
                    {
                        run_inputs = x_datas[sequence_indexes[dataIndex]];
                        run_test_outputs = t_datas[sequence_indexes[dataIndex]];

                        run_outputs = NeuralNetwork.ComputeForward(run_inputs);

                        ComputeAccuracy(run_test_outputs, run_outputs);

                        mean_error_sum += ComputeLossFunction(run_outputs, run_test_outputs);
                        dataIndex++;

                        current_time += Time.deltaTime;

                        yield return null;                        
                    }
                }

                // Computing the mean error
                currentMeanError = mean_error_sum / x_datas.Length;

                CurrentEpoch++;
            }
        }

        private async void Train(int delay = 50)
        {
            try
            {
                CurrentEpoch = 0;

                double current_time = 0;

                // Get training datas from the setting
                TrainingSetting.GetMatrixTrainDatas(out x_datas, out t_datas);

                // Compute number of iterations 
                // Batchsize shouldn't be 0
                int iterations_per_epoch = x_datas.Length / BatchSize;

                int[] sequence_indexes = new int[x_datas.Length];
                for (int i = 0; i < sequence_indexes.Length; ++i)
                    sequence_indexes[i] = i;

                for (int i = 0; i < Epochs; ++i)
                {
                    int dataIndex = 0;

                    // Shuffle datas each epoch
                    Shuffle(sequence_indexes);

                    // Going through all data batched, mini-batch or stochastic, depending on BatchSize value
                    double mean_error_sum = 0;
                    for (int d = 0; d < iterations_per_epoch; ++d)
                    {
                        for (int j = 0; j < BatchSize; ++j)
                        {
                            run_inputs = x_datas[sequence_indexes[dataIndex]];
                            run_test_outputs = t_datas[sequence_indexes[dataIndex]];

                            run_outputs = NeuralNetwork.ComputeForward(run_inputs);

                            NeuralNetwork.ComputeGradients(run_test_outputs, run_outputs);

                            ComputeAccuracy(run_test_outputs, run_outputs);

                            mean_error_sum += ComputeLossFunction(run_outputs, run_test_outputs);
                            dataIndex++;

                            await Task.Delay(delay);
                        }

                        // Computing gradients average over batchsize
                        NeuralNetwork.MeanGradients(BatchSize);

                        // NeuralNetwork.MeanGradients
                        // Computing new weights and reseting gradients to 0 for next batch
                        NeuralNetwork.UpdateWeights(LearningRate, Momentum, WeightDecay, BiasRate);
                    }

                    double last_mean_error = currentMeanError;
                    // Computing the mean error
                    currentMeanError = mean_error_sum / x_datas.Length;

                    if (currentMeanError < last_mean_error)
                    {
                        // Keeping a trace of the set
                        MemorizeBestSet(NeuralNetwork, currentMeanError);
                    }

                    // If under target error, stop
                    if (currentMeanError < Target_Mean_Error)
                    {
                        break;
                    }

                    //DecayLearningRate();

                    CurrentEpoch++;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            
        }
    }
}
