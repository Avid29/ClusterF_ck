﻿// Adam Dernis © 2022

using ClusterF_ck.Spaces;
using System.Numerics;

namespace ClusterF_ck.Shapes
{/// <summary>
 /// A shape defining how to handle <see cref="Vector3"/>s in a geometric space.
 /// </summary>
    public struct Vector3Shape : IGeometricSpace<Vector3, (int, int, int)>
    {
        /// <inheritdoc/>
        public bool AreEqual(Vector3 it1, Vector3 it2)
        {
            return it1 == it2;
        }

        /// <inheritdoc/>
        public Vector3 Average(Vector3[] items)
        {
            Vector3 sumVector = Vector3.Zero;
            foreach (var item in items)
            {
                sumVector += item;
            }
            sumVector /= items.Length;
            return sumVector;
        }

        /// <inheritdoc/>
        public double FindDistanceSquared(Vector3 it1, Vector3 it2)
        {
            return (it1 - it2).LengthSquared();
        }

        /// <inheritdoc/>
        public (int, int, int) GetCell(Vector3 value, double window)
        {
            var cell = value / (float)window;
            return ((int)cell.X, (int)cell.Y, (int)cell.Z);
        }

        /// <inheritdoc/>
        public Vector3 GetCellCenter((int, int, int) cell, double window)
        {
            Vector3 cellScale = new(cell.Item1, cell.Item2, cell.Item3);
            cellScale *= (float)window;

            var offset = new Vector3((float)(window / 2));

            return cellScale + offset;
        }

        /// <inheritdoc/>
        public Vector3 WeightedAverage((Vector3, double)[] items)
        {
            Vector3 sumVector = Vector3.Zero;
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
