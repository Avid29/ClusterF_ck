﻿// Adam Dernis © 2022

using ClusterF_ck.Spaces;
using System;

namespace ClusterF_ck.Shapes
{
    /// <summary>
    /// A shape defining how to handle <see cref="double"/>s in a geometric space.
    /// </summary>
    public struct DoubleShape : IGeometricSpace<double, int>
    {
        /// <inheritdoc/>
        public bool AreEqual(double it1, double it2)
        {
            return it1 == it2;
        }

        /// <inheritdoc/>
        public double Average(double[] items)
        {
            double sum = 0;
            int count = 0;
            foreach (var item in items)
            {
                sum += item;
                count++;
            }
            return sum /= count;
        }

        /// <inheritdoc/>
        public double FindDistanceSquared(double it1, double it2)
        {
            return Math.Abs(it1 - it2);
        }

        /// <inheritdoc/>
        public int GetCell(double value, double window)
             => (int)(value / window);

        /// <inheritdoc/>
        public double GetCellCenter(int cell, double window)
            => (cell * window) + (window / 2);

        /// <inheritdoc/>
        public double WeightedAverage((double, double)[] items)
        {
            double sum = 0;
            double totalWeight = 0;
            foreach (var item in items)
            {
                sum += item.Item1 * item.Item2;
                totalWeight += item.Item2;
            }
            return sum / totalWeight;
        }
    }
}
