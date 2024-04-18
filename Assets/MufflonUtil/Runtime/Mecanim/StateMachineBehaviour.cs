using System;
using UnityEngine;
using UnityEngine.Animations;

namespace MufflonUtil
{
    /// <summary>
    /// A <see cref="StateMachineBehaviour"/> with a context MonoBehaviour.
    /// </summary>
    /// <typeparam name="T">Type of the context</typeparam>
    public class StateMachineBehaviour<T> : StateMachineBehaviour where T : Component
    {
        /// <summary>
        /// The context MonoBehaviour owning the state machine.
        /// </summary>
        protected T Context { get; private set; }

        protected Animator Animator { get; private set; }

        /// <summary>
        /// The transform of the owning GameObject.
        /// </summary>
        protected Transform Transform { get; private set; }

        /// <summary>
        /// True if this state was entered before and it was successfully initialized.
        /// Check to see if you can expect Context, Animator and Transform properties to be set.
        /// </summary>
        protected bool IsInitialized { get; private set; }
        
        /// <summary>
        /// True if this state is currently the active state of the state machine.
        /// </summary>
        protected bool IsActive { get; private set; }

        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (!IsInitialized)
            {
                Animator = animator;
                Context = animator.GetComponentInParent<T>();
                if (Context == null)
                    throw new InvalidOperationException(
                        $"State machine behaviour needs sibling/parent component of type {typeof(T)}");
                Transform = Context.transform;
                IsInitialized = true;
                OnInitialize(Context, animator, stateInfo, layerIndex, controller);
            }

            IsActive = true;
            OnStateEntered(Context, animator, stateInfo, layerIndex, controller);
        }

        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // see overload with controller parameter
        }

        /// <summary>
        /// Initialize is called when the state is entered for the first time. Called before <see cref="OnStateEntered"/>
        /// </summary>
        protected virtual void OnInitialize(T context, Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        { }

        /// <summary>
        /// Called every time the state is entered.
        /// </summary>
        protected virtual void OnStateEntered(T context, Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        { }

        public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (Context != null && Animator != null)
            {
                OnStateUpdate(Context, Animator, stateInfo, layerIndex, controller);
            }
            else
            {
                throw new InvalidOperationException("Context or Animator is not initialized.");
            }
        }

        public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // see overload with controller parameter
        }

        /// <summary>
        /// Called every frame while the state is active.
        /// </summary>
        protected virtual void OnStateUpdate(T context, Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex, AnimatorControllerPlayable controller)
        { }

        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (Context != null && Animator != null)
            {
                IsActive = false;
                OnStateExit(Context, animator, stateInfo, layerIndex, controller);
            }
            else
            {
                throw new InvalidOperationException("Context or Animator is not initialized.");
            }
        }


        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // see overload with controller parameter
        }

        /// <summary>
        /// Called when the state is being exited.
        /// </summary>
        protected virtual void OnStateExit(T context, Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        { }

        public sealed override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // not applicable for state machine
        }

        public sealed override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            // not applicable for state machine
        }

        public sealed override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // not applicable for state machine
        }

        public sealed override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            // not applicable for state machine
        }

        public sealed override void OnStateMachineEnter(Animator animator, int stateMachinePathHash,
            AnimatorControllerPlayable controller)
        {
            OnStateMachineEnter(Context, animator, stateMachinePathHash, controller);
        }

        public sealed override void OnStateMachineExit(Animator animator, int stateMachinePathHash,
            AnimatorControllerPlayable controller)
        {
            IsInitialized = false;
            IsActive = false;
            OnStateMachineExit(Context, animator, stateMachinePathHash, controller);
        }

        public sealed override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            // see overload with controller parameter
        }

        protected virtual void OnStateMachineEnter(T context, Animator animator, int stateMachinePathHash,
            AnimatorControllerPlayable controller)
        { }

        public sealed override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            // see overload with controller parameter
        }

        protected virtual void OnStateMachineExit(T context, Animator animator, int stateMachinePathHash,
            AnimatorControllerPlayable controller)
        { }
    }
}