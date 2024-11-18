﻿using Atom.MachineLearning.Core.Maths;
using Atom.MachineLearning.Core.Visualization.VisualElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Atom.MachineLearning.Core.Visualization.VisualElements
{
    /// <summary>
    /// A simple graphic to input
    /// </summary>
    public class SimpleLineChart : AtomMLChart
    {
        private double[] _pointsY;
        private double[,] _pointsXY;
        private float _lineWidth;

        /// <summary>
        /// Unidimensional mode, the points will be placed by the maximum avalaible interval on X axis
        /// If 500 px and 500 points, 1 point per pixel on X
        /// </summary>
        /// <param name="getPoints"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public SimpleLineChart(double[] pointsY, float lineWidth = 2f, int width = 300, int height = 300)
        {
            _pointsY = pointsY;
            _lineWidth = lineWidth;

            style.width = width;
            style.height = height;
            style.backgroundColor = new StyleColor(Color.white);

            generateVisualContent += GenerateLineY;
            generateVisualContent += DrawOrthonormalLines;
        }

        public SimpleLineChart(double[,] pointXY, float lineWidth = 2f, int width = 300, int height = 300)
        {
            _pointsXY = pointXY;
            _lineWidth = lineWidth;

            style.width = width;
            style.height = height;
            style.backgroundColor = new StyleColor(Color.white);

            generateVisualContent += GenerateLineXY;
            generateVisualContent += DrawOrthonormalLines;
        }

        /// <summary>
        /// Generate the line without knowing any x value, so we assume a equal distribution of points on x and just compute the interval by pointsCount / avalaibleWidth 
        /// </summary>
        /// <param name="ctx"></param>
        protected void GenerateLineY(MeshGenerationContext ctx)
        {
            var painter2D = ctx.painter2D;

            painter2D.lineWidth = _lineWidth;
            painter2D.strokeColor = Color.black;

            MLMath.ColumnMinMax(_pointsY, out y_min, out y_max);

            x_min = 0;
            x_max = _pointsY.Length;

            painter2D.BeginPath();

            var relative_position_x = 0.0;
            var relative_position_y = 1 - MLMath.Lerp(_pointsY[0], y_min, y_max);

            painter2D.MoveTo(Plot(relative_position_x, relative_position_y));

            for (int i = 0; i < _pointsY.Length; i++)
            {
                relative_position_x = MLMath.Lerp(i, x_min, x_max);
                relative_position_y = 1 - MLMath.Lerp(_pointsY[i], y_min, y_max);

                painter2D.LineTo(Plot(relative_position_x, relative_position_y));

            }

            painter2D.Stroke();
        }

        protected void GenerateLineXY(MeshGenerationContext ctx)
        {
            var painter2D = ctx.painter2D;

            painter2D.lineWidth = _lineWidth;
            painter2D.strokeColor = Color.black;


        }
    }
}