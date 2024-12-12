using Prototype.Utils;

namespace Prototype.UI
{
    public sealed class SceneTransitionUIAnimation : Singleton<SceneTransitionUIAnimation>
    {
        public static SceneTransitionUI SceneTransitionUI { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            SceneTransitionUI = GetComponentInChildren<SceneTransitionUI>();
        }
    }
}