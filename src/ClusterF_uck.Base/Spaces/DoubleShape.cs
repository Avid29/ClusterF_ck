// Adam Dernis © 2022

using ClusterF_ck.Spaces.Interfaces;
using System;

namespace ClusterF_ck.Spaces
{
    /// <summary>
    /// A shape defining how to handle <see cref="double"/>s in a geometric space.
    /// </summary>
    public struct DoubleShape : IGeometricSpace<double, int>
    {
        private const double TOLERANCE = 0.000005;

        /// <inheritdoc/>
        public double Window { get; set; }

        /// <inheritdoc/>
        public bool AreEqual(double it1, double it2)
            => Math.Abs(it1 - it2) < TOLERANCE;

        /// <inheritdoc/>
        public readonly double Average(double[] items)
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
        public readonly double FindDistanceSquared(double it1, double it2)
            => Math.Abs(it1 - it2);

        /// <inheritdoc/>
        public readonly int GetCell(double value)
             => (int)(value / Window);

        /// <inheritdoc/>
        public readonly double GetCellOrigin(int cell)
            => cell * Window;

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
