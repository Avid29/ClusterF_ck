// Adam Dernis © 2022

using ClusterF_ck.Spaces;
using System;

namespace ClusterF_ck.Shapes
{
    /// <summary>
    /// A shape defining how to handle <see cref="float"/>s in a geometric space.
    /// </summary>
    public struct FloatShape : IGeometricSpace<float, int>
    {
        private const double TOLERANCE = 0.000005;

        /// <inheritdoc/>
        public double Window { get; set; }

        /// <inheritdoc/>
        public bool AreEqual(float it1, float it2)
            => Math.Abs(it1 - it2) < TOLERANCE;

        /// <inheritdoc/>
        public float Average(float[] items)
        {
            float sum = 0;
            foreach (var item in items)
            {
                sum += item;
            }
            return sum /= items.Length;
        }

        /// <inheritdoc/>
        public double FindDistanceSquared(float it1, float it2)
        {
            return Math.Abs(it1 - it2);
        }
        /// <inheritdoc/>
        public int GetCell(float value)
            => (int)(value / Window);

        /// <inheritdoc/>
        public float GetCellOrigin(int cell)
            => (float)(cell * Window);

        /// <inheritdoc/>
        public float WeightedAverage((float, double)[] items)
        {
            float sum = 0;
            double totalWeight = 0;
            foreach (var item in items)
            {
                sum += (float)(item.Item1 * item.Item2);
                totalWeight += item.Item2;
            }
            return (float)(sum / totalWeight);
        }
    }
}
