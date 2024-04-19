using System;
using UnityEngine;

namespace MufflonUtil
{
    /// <summary>
    /// Utility class that ensures all transitions in the AnimatorController assigned to the Animator Component
    /// have a transition duration of zero and no exit time. This makes sure the state machine can act as a logic
    /// state machine that is only ever in one state and exits the current state before it enters the next.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class GameStateMachine : MonoBehaviour
    {
        [field: SerializeField, NotEditable] public Animator Animator { get; private set; }
        public bool IsValid { get; private set; }
        public GameState State { get; private set; }

        protected void OnValidate()
        {
            Animator = GetComponent<Animator>();
        }

        public void OnEnterState(GameState gameState)
        {
            State = gameState;
            IsValid = gameState != null;
        }

        public void OnExitState(GameState gameState)
        {
            if (!IsValid)
            {
                throw new InvalidOperationException($"Cannot exit state {gameState}:" +
                                                    "State machine is not in a valid state.");
            }

            if (!gameState.Equals(State))
            {
                throw new ArgumentException($"Cannot exit {gameState}, " +
                                            $"because state machine is currently in state {State}");
            }

            IsValid = false;
        }
    }
}