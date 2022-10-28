// Adam Dernis © 2022

namespace ClusterF_ck.Tests.Data.Generation.Gradients.Shapes
{
    public struct DoubleGradientShape : IGradient<double>
    {
        public int N => 1;

        public double For(double[] coords)
        {
            return coords[0];
        }
    }
}
