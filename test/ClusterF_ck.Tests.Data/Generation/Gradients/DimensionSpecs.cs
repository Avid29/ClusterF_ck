// Adam Dernis © 2022

using ClusterF_ck.Tests.Data.Generation.Gradients.Easing.Abstract;

namespace ClusterF_ck.Tests.Data.Generation.Gradients
{
    public struct DimensionSpecs
    {
        public DimensionSpecs(double start, double end, int steps, EasingBase ease)
        {
            Start = start;
            End = end;
            Steps = steps;
            Ease = ease;
        }

        public double Start { get; }

        public double End { get; }

        public int Steps { get; }

        public EasingBase Ease { get; }
    }
}
