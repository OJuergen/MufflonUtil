using UnityEngine;

namespace MufflonUtil
{
    /// <summary>
    /// Sets a float animator parameter.
    /// </summary>
    public class SetFloatParameter : StateMachineBehaviour
    {
        [SerializeField] private bool _setValueOnEnter;
        [SerializeField] private float _valueOnEnter;
        [SerializeField] private bool _setValueOnExit;
        [SerializeField] private float _valueOnExit;
        [SerializeField] private bool _setValueOnUpdate;
        [SerializeField] private float _valueSpeed;
        [SerializeField] private AnimatorParameterId _floatParameter = new AnimatorParameterId("Parameter");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_setValueOnEnter) animator.SetFloat(_floatParameter, _valueOnEnter);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_setValueOnUpdate)
                animator.SetFloat(_floatParameter, animator.GetFloat(_floatParameter) + _valueSpeed * Time.deltaTime);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_setValueOnExit) animator.SetFloat(_floatParameter, _valueOnExit);
        }
    }
}