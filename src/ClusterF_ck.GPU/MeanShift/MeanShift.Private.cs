// Adam Dernis © 2023

using ClusterF_ck.GPU.MeanShift.Shaders;
using ComputeSharp;

namespace ClusterF_ck.GPU.MeanShift;

public partial class MeanShift
{
    internal static void ClusterRaw(
        ReadWriteBuffer<float> pointsBuffer,
        ReadOnlyBuffer<float> fieldBuffer,
        Func<float, float> kernel)
    {
        var device = pointsBuffer.GraphicsDevice;
        var fieldWeights = device.AllocateReadWriteTexture2D<float>(pointsBuffer.Length, fieldBuffer.Length);
        var flagBuffer = device.AllocateReadWriteBuffer<int>(1);
        var source = device.AllocateReadOnlyBuffer(new [] { 0 });
        var readBack = device.AllocateReadBackBuffer<int>(1);

        var shader = new MeanShiftFloatPointsShader(pointsBuffer, fieldBuffer, fieldWeights, flagBuffer, kernel);

        do
        {
            // Reset the flag
            flagBuffer.CopyFrom(source);

            device.For(pointsBuffer.Length, shader);

            // Retrieve the flag
            flagBuffer.CopyTo(readBack);

        } while (readBack.Span[0] != 0);

        // TODO: Connected components
    }
}
