// Adam Dernis © 2022

namespace ClusterF_ck.Spaces.Properties
{
    /// <summary>
    /// An <see cref="ISpace{T}"/> where objects can be considered equal.
    /// </summary>
    public interface IEquatableSpace<T> : ISpace<T>
    {
        /// <summary>
        /// Checks equality of two items.
        /// </summary>
        /// <param name="it1">The item to compare.</param>
        /// <param name="it2">The item to compare it to.</param>
        /// <returns>Whether or not the items are equal.</returns>
        bool AreEqual(T it1, T it2);
    }
}
