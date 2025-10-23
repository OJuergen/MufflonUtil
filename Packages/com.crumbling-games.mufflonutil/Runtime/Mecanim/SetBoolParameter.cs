using UnityEngine;

namespace MufflonUtil.Mecanim
{
    /// <summary>
    /// Sets a boolean state machine parameter on enter and on exit.
    /// </summary>
    public class SetBoolParameter : StateMachineBehaviour
    {
        [SerializeField] private bool _valueOnEnter = true;
        [SerializeField] private bool _valueOnExit;
        [SerializeField] private AnimatorParameterId _boolParameter = new AnimatorParameterId("State");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(_boolParameter, _valueOnEnter);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(_boolParameter, _valueOnExit);
        }
    }
}