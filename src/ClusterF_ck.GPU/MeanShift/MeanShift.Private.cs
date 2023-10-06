// Adam Dernis © 2023

using ClusterF_ck.GPU.MeanShift.Shaders;
using CommunityToolkit.Diagnostics;
using ComputeSharp;

namespace ClusterF_ck.GPU.MeanShift;

public partial class MeanShift
{
    internal static void ClusterRaw<T>(
        ReadWriteBuffer<T> pointsBuffer,
        ReadOnlyBuffer<T> fieldBuffer,
        Func<float, float> kernel)
        where T : unmanaged
    {
        var device = pointsBuffer.GraphicsDevice;
        var fieldWeights = device.AllocateReadWriteTexture2D<T>(pointsBuffer.Length, fieldBuffer.Length);
        var flagBuffer = device.AllocateReadWriteBuffer<int>(1);
        var source = device.AllocateReadOnlyBuffer(new [] { 0 });
        var readBack = device.AllocateReadBackBuffer<int>(1);

        // Create a shader to match the method type
        T type = default;
        Action? step = type switch
        {
            float _ => () => device.For(pointsBuffer.Length,
                new MeanShiftFloatPointsShader(
                    pointsBuffer as ReadWriteBuffer<float>,
                    fieldBuffer as ReadOnlyBuffer<float>,
                    fieldWeights as ReadWriteTexture2D<float>,
                    flagBuffer, kernel)),
            _ => null,
        };

        // The step method could not be created because an invalid type T was supplied
        if (step is null)
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(T));

        do
        {
            // Reset the flag
            flagBuffer.CopyFrom(source);

            // Execute step
            step();

            // Retrieve the flag
            flagBuffer.CopyTo(readBack);

        } while (readBack.Span[0] != 0);

        // TODO: Connected components
    }
}
