using UnityEngine;
using UnityEngine.Animations;

namespace MufflonUtil
{
    public class GameStateBehaviour : StateMachineBehaviour<GameStateMachine>
    {
        [SerializeField] private GameState _gameState;

        protected sealed override void OnEnter(GameStateMachine context, Animator animator,
            AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            context.OnEnterState(_gameState);
            OnEnter();
        }

        protected virtual void OnEnter()
        {
            Debug.Log($"Enter {_gameState}");
        }

        protected sealed override void OnExit(GameStateMachine context, Animator animator,
            AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            context.OnExitState(_gameState);
            OnExit();
        }

        protected virtual void OnExit()
        {
            Debug.Log($"Exit {_gameState}");
        }

        protected override void OnInitialize(GameStateMachine context, Animator animator,
            AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            OnInitialize();
        }

        protected virtual void OnInitialize()
        { }

        protected override void OnUpdate(GameStateMachine context, Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex,
            AnimatorControllerPlayable controller)
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        { }

        protected override void OnMachineEnter(GameStateMachine context, Animator animator, int stateMachinePathHash,
            AnimatorControllerPlayable controller)
        {
            OnMachineEnter();
        }

        protected virtual void OnMachineEnter()
        { }

        protected override void OnMachineExit(GameStateMachine context, Animator animator, int stateMachinePathHash,
            AnimatorControllerPlayable controller)
        {
            OnMachineExit();
        }

        protected virtual void OnMachineExit()
        { }
    }
}