// Adam Dernis © 2022

using ClusterF_ck.Spaces;
using System.Numerics;

namespace ClusterF_ck.Shapes
{
    /// <summary>
    /// A shape defining how to handle <see cref="Vector4"/>s in a geometric space.
    /// </summary>
    public struct Vector4Shape : IGeometricSpace<Vector4, (int, int, int, int)>
    {
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
        public (int, int, int, int) GetCell(Vector4 value, double window)
        {
            var cell = value / (float)window;
            return ((int)cell.W, (int)cell.X, (int)cell.Y, (int)cell.Z);
        }

        /// <inheritdoc/>
        public Vector4 GetCellCenter((int, int, int, int) cell, double window)
        {
            Vector4 cellScale = new(cell.Item1, cell.Item2, cell.Item3, cell.Item4);
            cellScale *= (float)window;

            var offset = new Vector4((float)(window / 2));

            return cellScale + offset;
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
