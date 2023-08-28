// Adam Dernis © 2022

namespace ClusterF_ck.Kernels;

/// In order to be a valid Kernel, it must meet two requirements.
/// #1: The integral of K(u)du = 1
/// #2: K(u) = K(|u|)
/// 
/// In other words,
/// It must be normalized and symmetric
/// <summary>
/// An <see langword="interface"/> for a kernel distribution.
/// </summary>
public interface IKernel
{
    /// <summary>
    /// Gets or sets the windows size of the kernel.
    /// </summary>
    double WindowSize { get; set; }

    /// <summary>
    /// Gets the weighted relevance of a point at sqrt(<paramref name="distanceSquared"/>) away.
    /// </summary>
    /// <remarks>
    /// Distance square is used for optimization purposes. The resulting weight should not be squared.
    /// </remarks>
    /// <param name="distanceSquared">The distance^2 of the point to be weighted.</param>
    /// <returns>The weight of a point at sqrt(<paramref name="distanceSquared"/>) away.</returns>
    double WeightDistance(double distanceSquared);
}