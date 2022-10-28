// Adam Dernis © 2022

namespace ClusterF_ck.Spaces.Properties
{
    /// <summary>
    /// An <see cref="ISpace{T}"/> where the weighted average of a set of objects can be taken.
    /// </summary>
    public interface IWeightedAverageSpace<T> : ISpace<T>
    {
        /// <summary>
        /// Gets the weighted average value of a list of (T, double) by point and weight.
        /// </summary>
        /// <param name="items">A weighted list of points.</param>
        /// <returns>The weighted center of the points.</returns>
        T WeightedAverage((T, double)[] items);
    }
}
