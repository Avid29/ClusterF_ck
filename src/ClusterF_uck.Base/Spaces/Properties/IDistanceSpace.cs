// Adam Dernis © 2022

namespace ClusterF_ck.Spaces.Properties;

/// <summary>
/// An <see cref="ISpace{T}"/> where the distance between objects is determinable.
/// </summary>
public interface IDistanceSpace<T> : ISpace<T>
{
    /// <summary>
    /// Gets the distance squared between <paramref name="it1"/> and <paramref name="it2"/>.
    /// </summary>
    /// <param name="it1">Point A.</param>
    /// <param name="it2">Point B.</param>
    /// <returns>The distance squared between point A and point B.</returns>
    double FindDistanceSquared(T it1, T it2);
}