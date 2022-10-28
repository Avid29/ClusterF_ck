// Adam Dernis © 2022

using System.Numerics;

namespace ClusterF_ck.Tests.Data.Generation.Gradients.Shapes
{
    public struct Vector2GradientShape : IGradient<Vector2>
    {
        public int N => 2;

        public Vector2 For(double[] coords)
        {
            return new Vector2((float)coords[0], (float)coords[1]);
        }
    }
}
