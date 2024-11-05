using Atom.MachineLearning.Core;
using Atom.MachineLearning.Core.Maths;
using System;
using UnityEngine;
using UnityEngine.UIElements;


namespace Atom.MachineLearning.Unsupervised.AutoEncoder
{
    public class AutoEncoderModel : IMLModel<NVector, NVector>
    {
        public string ModelName { get; set; } = "auto-encoder";
        public string ModelVersion { get; set; }

        [LearnedParameter, SerializeField] private DenseLayer[] _encoder;
        [LearnedParameter, SerializeField] private CodeLayer _code;
        [LearnedParameter, SerializeField] private DenseLayer[] _decoder;

        public class CodeLayer
        {
            public DenseLayer _enterLayer;
            public DenseLayer _exitLayer;
        }

        public class DenseLayer
        {
            public NVector _input;
            public NMatrix _weigths;
            public NMatrix _weightsInertia;
            public NVector _bias;
            public NVector _biasInertia;
            public NVector _output;
            public NVector _gradient;

            public Func<NVector, NVector> _activationFunction;
            public Func<NVector, NVector> _derivativeFunction;

            public DenseLayer(int input, int output, Func<NVector, NVector> activation = null, Func<NVector, NVector> derivation = null) 
            {
                _weigths = new NMatrix(input, output);

                _weightsInertia = new NMatrix(input, output);

                _bias = new NVector(output);
                _biasInertia = new NVector(output);
                _gradient = new NVector(output);

                _output = new NVector(output);
                _input = new NVector(input);

                _activationFunction = activation;
                _derivativeFunction = derivation;

                if (_activationFunction == null)
                    _activationFunction = (r) =>
                    {
                        for(int i = 0; i < r.Length; ++i)
                            r[i] = MLActivationFunctions.Sigmoid(r[i]);

                        return r;
                    };

                if (_derivativeFunction == null)
                    _derivativeFunction = (r) => {
                        for (int i = 0; i < r.Length; ++i)
                            r[i] = MLActivationFunctions.DSigmoid(r[i]);

                        return r;
                    };
            }

            public DenseLayer Seed()
            {
                for (int i = 0; i < _weigths.Rows; ++i)
                    for(int j = 0; j < _weigths.Columns; ++j)
                        _weigths.Datas[i, j] = MLRandom.Shared.Range(-.01f, .01f);

                for (int i = 0; i < _bias.Length; ++i)
                    _bias.Data[i] += MLRandom.Shared.Range(-.01f, .01f);

                return this;
            }

            public NVector Forward(NVector input)
            {
                _input = input;

                // output is buffered by the layer for backward pass
                NMatrix.MatrixRightMultiplyNonAlloc(_weigths, input, ref _output);
                _output = _activationFunction(_output + _bias);

                return _output;
            }

            /// <summary>
            /// Loss and Gradients are computed by the trainer and reinjected in each layer as this calculus may differ from one training algorithm to another
            /// Derivate the output and matrix mult the gradient never changes
            /// </summary>
            /// <param name="nextlayerGradient"></param>
            /// <returns></returns>
            public NVector Backward(NVector nextlayerGradient)
            {
                var output_derivative = _derivativeFunction(_output);    

                for(int i = 0; i < output_derivative.Length; ++i)
                    _gradient.Data[i] += output_derivative[i] * nextlayerGradient[i];

                return _gradient;
            }

            /// <summary>
            /// This will be done by the trainer in a near future
            /// </summary>
            /// <param name="lr"></param>
            /// <param name="momentum"></param>
            /// <param name="weigthDecay"></param>
            /// <param name="momentumAcc"></param>
            public void UpdateWeights(float lr = .05f, float momentum = .005f, float weigthDecay = .0005f)
            {
                double step = 0.0;
                for(int i = 0; i < _weigths.Rows; ++i)
                    for(int j = 0; j < _weigths.Columns; ++j)
                    {
                        step = _gradient[i] * _input[j] * lr;

                        _weigths[i, j] += step;
                        _weigths[i, j] += _weightsInertia[i, j] * momentum;
                        _weigths[i, j] -= weigthDecay * _weigths[i, j];
                        _weightsInertia[i, j] = step;

                       /* _weigths[i, j] += step + _weightsInertia[i, j] * momentumAcc;
                        _weightsInertia[i, j] += step * momentum;
                        _weightsInertia[i, j] -= _weightsInertia[i, j] * mt_decay;*/
                    }

                for(int i = 0; i < _bias.Length; ++i)
                {
                    step = _gradient[i] * lr;
                    _bias[i] += step;

                    _bias[i] -= weigthDecay * _bias[i];
                    _biasInertia[i] += momentum * _biasInertia[i];
                    _biasInertia[i] = step;

                    /*_bias[i] += step * _biasInertia[i] * momentumAcc;
                    _biasInertia[i] += step * momentum;
                    _biasInertia[i] -= mt_decay * _biasInertia[i];*/
                }

                for (int i = 0; i < _gradient.Length; ++i)
                    _gradient[i] = 0;
            }
        }

        public AutoEncoderModel(int inputDimensions, int[] encoderLayersDimensions, int codeLayerDimension, int[] decoderLayerDimensions, int outputDimensions)
        {
            _encoder = new DenseLayer[encoderLayersDimensions.Length];

            for (int i = 0; i < encoderLayersDimensions.Length; ++i)
            {
                if (i == 0)
                    _encoder[i] = new DenseLayer(inputDimensions, encoderLayersDimensions[i]);
                else 
                    _encoder[i] = new DenseLayer(encoderLayersDimensions[i - 1], encoderLayersDimensions[i]);
            }

            _code = new CodeLayer()
            {
                _enterLayer = new DenseLayer(encoderLayersDimensions[encoderLayersDimensions.Length - 1], codeLayerDimension),
                _exitLayer = new DenseLayer(codeLayerDimension, decoderLayerDimensions[0]),             
            };

            _decoder = new DenseLayer[decoderLayerDimensions.Length];

            for (int i = 0; i < decoderLayerDimensions.Length; ++i)
            {
                if(i < encoderLayersDimensions.Length - 1)
                    _decoder[i] = new DenseLayer(decoderLayerDimensions[i], decoderLayerDimensions[i + 1]);
                else
                    _decoder[i] = new DenseLayer(decoderLayerDimensions[i], outputDimensions);
            }
        }

        public NVector Predict(NVector inputData)
        {
            var temp  = inputData;
            for(int i = 0; i < _encoder.Length; ++i)
            {
                temp = _encoder[i].Forward(temp);
            }

            temp = _code._enterLayer.Forward(temp);
            temp = _code._exitLayer.Forward(temp);

            for (int i = 0; i < _decoder.Length; ++i)
            {
                temp = _decoder[i].Forward(temp);
            }

            return temp;
        }

        public NVector Backpropagate(NVector gradient)
        {
            var temp = gradient;
            for (int i = _decoder.Length -1; i >= 0; ++i)
            {
                temp = _decoder[i].Backward(temp);
            }

            temp = _code._exitLayer.Backward(temp);
            temp = _code._enterLayer.Backward(temp);

            for (int i = _encoder.Length - 1; i >= 0; ++i)
            {
                temp = _encoder[i].Backward(temp);
            }

            return temp;
        }

        public void UpdateWeights(float learningRate = .05f, float momentum = .005f, float weigthDecay = .0005f)
        {
            for (int i = 0; i < _encoder.Length; ++i)
            {
                _encoder[i].UpdateWeights(learningRate, momentum, weigthDecay);
            }

            _code._enterLayer.UpdateWeights(learningRate, momentum, weigthDecay);
            _code._exitLayer.UpdateWeights(learningRate, momentum, weigthDecay);

            for (int i = 0; i < _decoder.Length; ++i)
            {
                _decoder[i].UpdateWeights(learningRate, momentum, weigthDecay);
            }
        }
    }
}