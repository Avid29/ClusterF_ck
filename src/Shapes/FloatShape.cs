// Adam Dernis © 2022

using ClusterF_ck.Spaces;
using System;

namespace ClusterF_ck.Shapes
{
    /// <summary>
    /// A shape defining how to handle <see cref="float"/>s in a geometric space.
    /// </summary>
    public struct FloatShape : IGeometricSpace<float>
    {
        /// <inheritdoc/>
        public bool AreEqual(float it1, float it2)
        {
            return it1 == it2;
        }

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
