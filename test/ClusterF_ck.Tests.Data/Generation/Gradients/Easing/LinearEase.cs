// Adam Dernis © 2022

using ClusterF_ck.Tests.Data.Generation.Gradients.Easing.Abstract;

namespace ClusterF_ck.Tests.Data.Generation.Gradients.Easing
{
    public class LinearEase : EasingBase
    {
        public override double Ease(double pos)
        {
            return pos;
        }
    }
}
