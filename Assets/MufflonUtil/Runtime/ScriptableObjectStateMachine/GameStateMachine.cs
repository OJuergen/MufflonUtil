using System;
using UnityEngine;

namespace MufflonUtil.ScriptableObjectStateMachine
{
    public class GameStateMachine : MonoBehaviour
    {
        public GameState GameState { get; private set; }
        public bool IsValid => GameState != null;

        [SerializeField] private GameState _initialState;

        private void Awake()
        {
            if (_initialState != null) ChangeState(_initialState);
        }

        public void ChangeState(GameState targetState)
        {
            if (targetState == null)
            {
                throw new ArgumentNullException(nameof(targetState));
            }

            if (GameState != null)
            {
                if (targetState != null && !GameState.CanTransitionTo(targetState))
                {
                    throw new InvalidOperationException($"Cannot transition from {GameState} to {targetState}");
                }

                GameState.ExitState(this);
            }

            GameState = targetState;
            if (targetState != null && !targetState.IsInitialized) targetState.InitializeState(this);
            if (targetState != null) targetState.EnterState(this);
        }

        private void Update()
        {
            if (GameState != null) GameState.UpdateState(this);
        }
    }
}