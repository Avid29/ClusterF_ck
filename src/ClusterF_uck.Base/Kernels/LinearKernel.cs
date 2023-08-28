// Adam Dernis © 2022

namespace ClusterF_ck.Kernels;

/// The shape of a linear distribution
/// 
///               *
///             *   *
///           *       *
///         *           *
///       *                *
///     *                    *
/// * *                        * *
/// ------------------------------
/// <summary>
/// A kernel with a linear falloff.
/// </summary>
public struct LinearKernel : IKernel
{
    private double _windowSquared;
    private double _window;

    /// <summary>
    /// Initializes a new instance of the <see cref="LinearKernel"/> struct.
    /// </summary>
    /// <param name="window">The window size of the Kernel.</param>
    public LinearKernel(double window)
    {
        // These will be set in WindowSize
        _window = 0;
        _windowSquared = 0;
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
        double rawWeight = _windowSquared - distanceSquared;
        return rawWeight < 0 ? 0 : rawWeight;
    }
}