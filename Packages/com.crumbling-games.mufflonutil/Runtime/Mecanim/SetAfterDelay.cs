using UnityEngine;

namespace MufflonUtil.Mecanim
{
    /// <summary>
    /// Sets a bool animator parameter a certain time after the state is entered.
    /// </summary>
    public class SetAfterDelay : StateMachineBehaviour
    {
        [SerializeField] private bool _valueAfterDelay;
        [SerializeField] private AnimatorParameterId _boolParameter = new AnimatorParameterId("Parameter");
        [SerializeField] private MinMaxFloat _delaySecondsRange = new MinMaxFloat {Min = 0, Max = 1};
        private float _delaySeconds;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _delaySeconds = _delaySecondsRange.Random;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime * stateInfo.length < _delaySeconds) return;
            animator.SetBool(_boolParameter, _valueAfterDelay);
        }
    }
}