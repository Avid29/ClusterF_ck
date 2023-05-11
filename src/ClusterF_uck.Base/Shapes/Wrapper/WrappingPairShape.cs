// Adam Dernis © 2022

using ClusterF_ck.Spaces.Properties;

namespace ClusterF_ck.Shapes.Wrapper;

/// <summary>
/// A shape for wrapping a paired point list to cluster in a metric space. 
/// </summary>
/// <typeparam name="T">The type of the main point.</typeparam>
/// <typeparam name="T2">The type of the pair.</typeparam>
/// <typeparam name="TShape">The type of the child shape for <typeparamref name="T"/>.</typeparam>
internal struct WrappingPairShape<T, T2, TShape> : IDistanceSpace<(T, T2)>
    where T : unmanaged
    where T2 : unmanaged
    where TShape : struct, IDistanceSpace<T>
{
    // Don't make readonly.
    // This would make method calls require cloning before invocation.
    private TShape _shape;

    /// <summary>
    /// Initializes a new instance of the <see cref="WrappingPairShape{T, T2, TShape}"/> struct.
    /// </summary>
    /// <param name="shape">The child shape for <typeparamref name="T"/>.</param>
    public WrappingPairShape(TShape shape)
    {
        _shape = shape;
    }

    public double FindDistanceSquared((T, T2) it1, (T, T2) it2)
    {
        return _shape.FindDistanceSquared(it1.Item1, it2.Item1);
    }
}