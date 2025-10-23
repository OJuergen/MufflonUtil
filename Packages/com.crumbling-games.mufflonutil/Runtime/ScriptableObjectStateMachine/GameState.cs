using System.Linq;
using UnityEngine;

namespace MufflonUtil.ScriptableObjectStateMachine
{
    [CreateAssetMenu(menuName = "MufflonUtil/Game State")]
    public class GameState : ScriptableObject
    {
        public bool IsInitialized { get; private set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool IsActive { get; private set; }
        [SerializeField, Tooltip("Limit transitions to these target states. " +
                                 "Allow all transitions by leaving this empty.")]
        private GameState[] _canTransitionTo;

        private void OnEnable()
        {
            IsInitialized = false;
            IsActive = false;
        }

        public void InitializeState(GameStateMachine gameStateMachine)
        {
            OnInitialize(gameStateMachine);
            IsInitialized = true;
        }

        protected virtual void OnInitialize(GameStateMachine gameStateMachine)
        { }

        public void EnterState(GameStateMachine gameStateMachine)
        {
            OnEnter(gameStateMachine);
            IsActive = true;
        }

        protected virtual void OnEnter(GameStateMachine gameStateMachine)
        { }

        public void UpdateState(GameStateMachine gameStateMachine)
        {
            OnUpdate(gameStateMachine);
        }

        protected virtual void OnUpdate(GameStateMachine gameStateMachine)
        { }

        public void ExitState(GameStateMachine gameStateMachine)
        {
            OnExit(gameStateMachine);
            IsActive = false;
        }

        protected virtual void OnExit(GameStateMachine gameStateMachine)
        { }

        public bool CanTransitionTo(GameState targetState)
        {
            return _canTransitionTo.Length == 0 || _canTransitionTo.Contains(targetState);
        }
    }
}