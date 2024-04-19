using System;
using UnityEngine;

namespace MufflonUtil.Mecanim.StateMachine
{
    /// <summary>
    /// Utility class that ensures all transitions in the AnimatorController assigned to the Animator Component
    /// have a transition duration of zero and no exit time. This makes sure the state machine can act as a logic
    /// state machine that is only ever in one state and exits the current state before it enters the next.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class MecanimStateMachine : MonoBehaviour
    {
        [field: SerializeField, NotEditable] public Animator Animator { get; private set; }
        public bool IsValid { get; private set; }
        public MecanimGameState State { get; private set; }

        protected void OnValidate()
        {
            Animator = GetComponent<Animator>();
        }

        public void OnEnterState(MecanimGameState mecanimGameState)
        {
            State = mecanimGameState;
            IsValid = mecanimGameState != null;
        }

        public void OnExitState(MecanimGameState mecanimGameState)
        {
            if (!IsValid)
            {
                throw new InvalidOperationException($"Cannot exit state {mecanimGameState}:" +
                                                    "State machine is not in a valid state. " +
                                                    "This might be because of badly configured transitions.");
            }

            if (!mecanimGameState.Equals(State))
            {
                throw new ArgumentException($"Cannot exit {mecanimGameState}, " +
                                            $"because state machine is currently in state {State}. " +
                                            "This might be because of badly configured transitions.");
            }

            IsValid = false;
        }
    }
}