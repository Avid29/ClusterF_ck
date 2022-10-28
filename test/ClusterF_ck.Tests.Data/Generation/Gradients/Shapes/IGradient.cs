// Adam Dernis © 2022

namespace ClusterF_ck.Tests.Data.Generation.Gradients.Shapes
{
    public interface IGradient<out T>
    {
        int N { get; }

        T For(double[] coords);
    }
}
