// Adam Dernis © 2022

namespace ClusterF_ck.Kernels;

/// The shape of a Uniform Distribution
///
///     * * * * * * * * *
///     *               *
///     *               *
///     *               *
/// * * *               * * * 
/// -------------------------
/// <summary>
/// A Kernel with a rectangular shape and flat cutoff on its window size.
/// </summary>
public struct UniformKernel : IKernel
{
    private double _windowSquared;
    private double _window;

    /// <summary>
    /// Initializes a new instance of the <see cref="UniformKernel"/> struct.
    /// </summary>
    /// <param name="window">The window size of the Kernel.</param>
    public UniformKernel(double window)
    {
        // These will be set in WindowSize
        _window = 0;
        _windowSquared = 0;

        WindowSize = window;
    }

    /// <inheritdoc/>
    public double WindowSize
    {
        get => _window;
        set
        {
            _window = value;
            _windowSquared = value * value;
        }
    }

    /// <inheritdoc/>
    public double WeightDistance(double distanceSquared)
    {
        return distanceSquared < _windowSquared ? 1 : 0;
    }
}