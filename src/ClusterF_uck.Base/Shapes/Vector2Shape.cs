﻿// Adam Dernis © 2022

using ClusterF_ck.Spaces;
using System.Numerics;

namespace ClusterF_ck.Shapes
{
    /// <summary>
    /// A shape defining how to handle <see cref="Vector2"/>s in a geometric space.
    /// </summary>
    public struct Vector2Shape : IGeometricSpace<Vector2, (int, int)>
    {
        /// <inheritdoc/>
        public double Window { get; set; }

        /// <inheritdoc/>
        public bool AreEqual(Vector2 it1, Vector2 it2)
            => it1 == it2;

        /// <inheritdoc/>
        public Vector2 Average(Vector2[] items)
        {
            Vector2 sumVector = Vector2.Zero;
            foreach (var item in items)
            {
                sumVector += item;
            }
            sumVector /= items.Length;
            return sumVector;
        }

        /// <inheritdoc/>
        public double FindDistanceSquared(Vector2 it1, Vector2 it2)
        {
            return (it1 - it2).LengthSquared();
        }

        /// <inheritdoc/>
        public (int, int) GetCell(Vector2 value)
        {
            var cell = value / (float)Window;
            return ((int)cell.X, (int)cell.Y);
        }

        /// <inheritdoc/>
        public Vector2 GetCellOrigin((int, int) cell)
        {
            Vector2 cellScale = new(cell.Item1, cell.Item2);
            return cellScale * (float)Window;
        }

        /// <inheritdoc/>
        public Vector2 WeightedAverage((Vector2, double)[] items)
        {
            Vector2 sumVector = Vector2.Zero;
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
