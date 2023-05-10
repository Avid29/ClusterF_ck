// Adam Dernis © 2022

using ClusterF_ck.Tests.Data.DataSet.Abstract;
using ClusterF_ck.Tests.Data.Generation.Gradients;
using ClusterF_ck.Tests.Data.Generation.Gradients.Easing;
using ClusterF_ck.Tests.Data.Generation.Gradients.Easing.Enums;
using ClusterF_ck.Tests.Data.Generation.Gradients.Shapes;
using System.Numerics;

namespace ClusterF_ck.Tests.Data.DataSet
{
    public class GradientSet<T, TShape> : DataSet<T>
        where T : unmanaged
        where TShape : struct, IGradient<T>
    {
        private readonly DimensionSpecs[] _specs;

        public GradientSet(string name, params DimensionSpecs[] specs) :
            base(name)
        {
            _specs = specs;
            Data = GradientGenerator.Generate<T, TShape>(_specs);
        }

        public override string Type => "Gradient";

        public override T[] Data { get; }
    }

    public static class GradientSets
    {
        public static GradientSet<double, DoubleGradientShape> Linear1D_101 =
            new("1D Linear Gradient 101",
            new DimensionSpecs(0, 1, 101, new LinearEase()));

        public static GradientSet<double, DoubleGradientShape> QuadraticEaseIn1D_401 =
            new("1D Quadratic EaseIn Gradient 401",
                new DimensionSpecs(0, 1, 401, new QuadraticEase(EasingMode.EaseIn)));

        public static GradientSet<Vector2, Vector2GradientShape> Linear2D_11x11 =
            new ("2D Linear Gradient 11x11",
                new DimensionSpecs(0, 1, 11, new LinearEase()),
                new DimensionSpecs(0, 1, 11, new LinearEase()));

        public static GradientSet<Vector2, Vector2GradientShape> QuadraticEaseInOut2D_21x21 =
            new ("2D Quadratic EaseInOut Gradient 21x21",
                new DimensionSpecs(0, 1, 21, new QuadraticEase(EasingMode.EaseInOut)),
                new DimensionSpecs(0, 1, 21, new QuadraticEase(EasingMode.EaseInOut)));
    }
}
