// Adam Dernis © 2023

using ComputeSharp;

namespace ClusterF_ck.GPU.MeanShift.Shaders;

[AutoConstructor]
internal readonly partial struct MeanShiftFloatPointsShader : IComputeShader
{
    private const double ACCEPTED_ERROR = 0.000005;

    private readonly ReadWriteBuffer<float> _pointBuffer;
    private readonly ReadOnlyBuffer<float> _fieldBuffer;
    private readonly ReadWriteTexture2D<float> _fieldWeights;
    private readonly ReadWriteBuffer<int> _flagBuffer;

    private readonly Func<float, float> _kernel;

    public void Execute()
    {
        float point = _pointBuffer[ThreadIds.X];

        // Calculate each point's weight
        for (int i = 0; i < _fieldBuffer.Length; i++)
        {
            float dist = Hlsl.Abs(point - _fieldBuffer[i]);
            _fieldWeights[ThreadIds.X, i] = _kernel(dist);
        }

        // Calculate weighted average
        float sum = 0;
        float totalWeight = 0;
        for (int i = 0; i < _pointBuffer.Length; i++)
        {
            float weight = _fieldWeights[ThreadIds.X, i];
            sum += _pointBuffer[i] * weight;
            totalWeight += weight;
        }

        // Finish calculation and handle write back
        float newPoint = sum / totalWeight;
        _pointBuffer[ThreadIds.X] = newPoint;

        // Flag change
        bool changed = Math.Abs(point - newPoint) > ACCEPTED_ERROR;
        if (changed)
        {
            _flagBuffer[0] = 1;
        }
    }
}
