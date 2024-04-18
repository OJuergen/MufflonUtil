using System.Linq;
using UnityEngine;

namespace MufflonUtil
{
    /// <summary>
    /// Utility class that ensures all transitions in the AnimatorController assigned to the Animator Component
    /// have a transition duration of zero and no exit time. This makes sure the state machine can act as a logic
    /// state machine that is only ever in one state and exits the current state before it enters the next.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class StateMachineAnimator : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _animator = GetComponent<Animator>();
            var animatorController = _animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
            if (animatorController == null) return;
            foreach (UnityEditor.Animations.AnimatorStateTransition transition in animatorController.layers
                .SelectMany(layer => layer.stateMachine.states.SelectMany(state => state.state.transitions)))
            {
                transition.duration = 0f;
                transition.hasExitTime = false;
                transition.hasFixedDuration = true;
                transition.exitTime = 1f;
                transition.offset = 0f;
            }
            // todo visualize, warn and offer manual fix options instead of automatic approach
        }
#endif
    }
}