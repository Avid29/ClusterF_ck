// Adam Dernis © 2022

using ClusterF_ck.Tests.Data.Generation.Gradients.Easing.Enums;

namespace ClusterF_ck.Tests.Data.Generation.Gradients.Easing.Abstract
{
    public abstract class AsymetricEasingBase : EasingBase
    {
        protected AsymetricEasingBase(EasingMode easingMode)
        {
            EasingMode = easingMode;
        }

        public EasingMode EasingMode { get; }
    }
}
