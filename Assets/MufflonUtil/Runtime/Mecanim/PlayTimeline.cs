using UnityEngine;
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
        
        protected override void OnStateEntered(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Context.Play(_playableAsset);
            Context.stopped += OnStopped;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(_stopOnExit) Context.Stop();
            Context.stopped -= OnStopped;
        }

        private void OnStopped(PlayableDirector obj)
        {
            if(_setParameterWhenStopped) Animator.SetBool(_parameter, _parameterValueWhenStopped);
        }
    }
}