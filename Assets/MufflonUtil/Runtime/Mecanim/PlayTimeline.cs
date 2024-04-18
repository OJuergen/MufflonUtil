using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public class PlayTimeline : StateMachineBehaviour<PlayableDirector>
    {
        [SerializeField] private bool _stopOnExit;
        [SerializeField] private PlayableAsset _playableAsset;
        [SerializeField] private bool _setParameterWhenStopped;
        [SerializeField] private bool _parameterValueWhenStopped;
        [SerializeField] private AnimatorParameterId _parameter;

        protected override void OnStateEntered(PlayableDirector context, Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex, AnimatorControllerPlayable controller)
        {
            Context.Play(_playableAsset);
            Context.stopped += OnStopped;
        }

        protected override void OnStateExit(PlayableDirector context, Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex, AnimatorControllerPlayable controller)
        {
            if (_stopOnExit) Context.Stop();
            Context.stopped -= OnStopped;
        }

        private void OnStopped(PlayableDirector playableDirector)
        {
            if (_setParameterWhenStopped) Animator.SetBool(_parameter, _parameterValueWhenStopped);
        }
    }
}