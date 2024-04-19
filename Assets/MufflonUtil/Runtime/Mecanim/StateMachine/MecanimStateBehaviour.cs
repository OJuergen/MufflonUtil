using UnityEngine;
using UnityEngine.Animations;

namespace MufflonUtil.Mecanim.StateMachine
{
    public class MecanimStateBehaviour : StateMachineBehaviour<MecanimStateMachine>
    {
        [SerializeField] private MecanimGameState _mecanimGameState;

        protected sealed override void OnEnter(MecanimStateMachine context, Animator animator,
            AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            context.OnEnterState(_mecanimGameState);
            OnEnter();
        }

        protected virtual void OnEnter()
        {
            Debug.Log($"Enter {_mecanimGameState}");
        }

        protected sealed override void OnExit(MecanimStateMachine context, Animator animator,
            AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            context.OnExitState(_mecanimGameState);
            OnExit();
        }

        protected virtual void OnExit()
        {
            Debug.Log($"Exit {_mecanimGameState}");
        }

        protected override void OnInitialize(MecanimStateMachine context, Animator animator,
            AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            OnInitialize();
        }

        protected virtual void OnInitialize()
        { }

        protected override void OnUpdate(MecanimStateMachine context, Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex,
            AnimatorControllerPlayable controller)
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        { }

        protected override void OnMachineEnter(MecanimStateMachine context, Animator animator, int stateMachinePathHash,
            AnimatorControllerPlayable controller)
        {
            OnMachineEnter();
        }

        protected virtual void OnMachineEnter()
        { }

        protected override void OnMachineExit(MecanimStateMachine context, Animator animator, int stateMachinePathHash,
            AnimatorControllerPlayable controller)
        {
            OnMachineExit();
        }

        protected virtual void OnMachineExit()
        { }
    }
}