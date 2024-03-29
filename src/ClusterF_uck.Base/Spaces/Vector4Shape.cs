﻿// Adam Dernis © 2022

using ClusterF_ck.Spaces.Interfaces;
using System.Numerics;

namespace ClusterF_ck.Spaces
{
    /// <summary>
    /// A shape defining how to handle <see cref="Vector4"/>s in a geometric space.
    /// </summary>
    public struct Vector4Shape : IGeometricSpace<Vector4, (int, int, int, int)>
    {
        /// <inheritdoc/>
        public double Window { get; set; }

        /// <inheritdoc/>
        public bool AreEqual(Vector4 it1, Vector4 it2)
        {
            return it1 == it2;
        }

        /// <inheritdoc/>
        public Vector4 Average(Vector4[] items)
        {
            Vector4 sumVector = Vector4.Zero;
            foreach (var item in items)
            {
                sumVector += item;
            }
            sumVector /= items.Length;
            return sumVector;
        }

        /// <inheritdoc/>
        public double FindDistanceSquared(Vector4 it1, Vector4 it2)
        {
            return (it1 - it2).LengthSquared();
        }

        /// <inheritdoc/>
        public (int, int, int, int) GetCell(Vector4 value)
        {
            var cell = value / (float)Window;
            return ((int)cell.W, (int)cell.X, (int)cell.Y, (int)cell.Z);
        }

        /// <inheritdoc/>
        public Vector4 GetCellOrigin((int, int, int, int) cell)
        {
            Vector4 cellScale = new(cell.Item1, cell.Item2, cell.Item3, cell.Item4);
            return cellScale * (float)Window;
        }

        /// <inheritdoc/>
        public Vector4 WeightedAverage((Vector4, double)[] items)
        {
            Vector4 sumVector = Vector4.Zero;
            double totalWeight = 0;
            foreach (var item in items)
            {
                sumVector += item.Item1 * (float)item.Item2;
                totalWeight += item.Item2;
            }
            sumVector /= (float)totalWeight;
            return sumVector;
        }
    }
}
