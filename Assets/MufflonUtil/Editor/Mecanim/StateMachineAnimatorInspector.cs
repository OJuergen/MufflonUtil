using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace MufflonUtil
{
    [CustomEditor(typeof(GameStateMachine), editorForChildClasses: true)]
    public class StateMachineAnimatorInspector : Editor
    {
        private bool _transitionsNeedFixing;
        private bool _statesNeedFixing;
        private bool _autoCheck;
        private bool _autoFix;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var gameStateMachine = (GameStateMachine)target;
            if (gameStateMachine == null) return;

            _autoCheck = EditorPrefsUtil.Checkbox("Auto Check", gameStateMachine);
            _autoFix = EditorPrefsUtil.Checkbox("Auto Fix", gameStateMachine);

            if (_autoCheck || GUILayout.Button("Run Checks"))
            {
                RunChecks(gameStateMachine);
            }

            if (_transitionsNeedFixing)
            {
                EditorGUILayout.HelpBox("Some transitions are badly configured for a logical game state machine. " +
                                        "All transitions must have zero duration and no exit time.",
                    MessageType.Warning);
                if (_autoFix || GUILayout.Button("Fix"))
                {
                    FixTransitions(gameStateMachine);
                }
            }

            if (_statesNeedFixing)
            {
                EditorGUILayout.HelpBox("Some animator states have no game state behaviour or more than one. " +
                                        "Or there are sub state machines with game state behaviours. " +
                                        "This is not supported",
                    MessageType.Warning);
                if (_autoFix || GUILayout.Button("Fix"))
                {
                    FixStates(gameStateMachine);
                }
            }
        }

        private void RunChecks(GameStateMachine gameStateMachine)
        {
            CheckStates(gameStateMachine);
            CheckTransitions(gameStateMachine);
        }

        private void FixTransitions(GameStateMachine gameStateMachine)
        {
            var animatorController = gameStateMachine.Animator.runtimeAnimatorController as AnimatorController;
            if (animatorController == null) return;
            foreach (AnimatorStateTransition transition in GetAllTransitions(animatorController))
            {
                transition.duration = 0f;
                transition.hasExitTime = false;
                transition.hasFixedDuration = true;
                transition.exitTime = 1f;
                transition.offset = 0f;
            }

            _transitionsNeedFixing = false;
        }

        private void CheckTransitions(GameStateMachine gameStateMachine)
        {
            _transitionsNeedFixing = false;
            var animatorController = gameStateMachine.Animator.runtimeAnimatorController as AnimatorController;
            if (animatorController == null) return;

            _transitionsNeedFixing = GetAllTransitions(animatorController)
                .Any(transition => transition.duration != 0f
                                   || transition.hasExitTime
                                   || !transition.hasFixedDuration
                                   || transition.exitTime < 1f
                                   || transition.offset != 0f);
        }

        private HashSet<AnimatorStateMachine> GetAllStateMachines(AnimatorController animatorController)
        {
            HashSet<AnimatorStateMachine> stateMachines = animatorController.layers
                .Select(layer => layer.stateMachine)
                .ToHashSet();

            bool newElementsAdded;
            do
            {
                int initialCount = stateMachines.Count;
                HashSet<AnimatorStateMachine> newMachines = stateMachines.SelectMany(stateMachine =>
                    stateMachine.stateMachines.Select(child => child.stateMachine)).ToHashSet();
                stateMachines.UnionWith(newMachines);
                newElementsAdded = stateMachines.Count > initialCount;
            } while (newElementsAdded);

            return stateMachines;
        }

        private IEnumerable<AnimatorState> GetAllStates(AnimatorController animatorController)
        {
            return GetAllStateMachines(animatorController)
                .SelectMany(stateMachine => stateMachine.states.Select(s => s.state));
        }

        private IEnumerable<AnimatorStateTransition> GetAllTransitions(AnimatorController animatorController)
        {
            HashSet<AnimatorStateMachine> stateMachines = GetAllStateMachines(animatorController);
            return stateMachines
                .SelectMany(stateMachine => stateMachine.states.SelectMany(state => state.state.transitions))
                .Union(stateMachines.SelectMany(sm => sm.anyStateTransitions));
        }

        private void CheckStates(GameStateMachine gameStateMachine)
        {
            _statesNeedFixing = false;
            var animatorController = gameStateMachine.Animator.runtimeAnimatorController as AnimatorController;
            if (animatorController == null) return;
            _statesNeedFixing = GetAllStates(animatorController)
                .Any(state => state.behaviours.Count(behaviour => behaviour is GameStateBehaviour) != 1);
            _statesNeedFixing |= GetAllStateMachines(animatorController)
                .Any(machine => machine.behaviours.Any(behaviour => behaviour is GameStateBehaviour));
        }

        private void FixStates(GameStateMachine gameStateMachine)
        {
            var animatorController = gameStateMachine.Animator.runtimeAnimatorController as AnimatorController;
            if (animatorController == null) return;
            foreach (AnimatorState state in GetAllStates(animatorController))
            {
                // very state should have exactly one identifying GameStateBehaviour
                if (state.behaviours.Count(behaviour => behaviour is GameStateBehaviour) > 1)
                {
                    StateMachineBehaviour keep = state.behaviours.First(behaviour => behaviour is GameStateBehaviour);
                    state.behaviours = state.behaviours
                        .Where(behaviour => behaviour is not GameStateBehaviour || behaviour == keep)
                        .ToArray();
                }

                if (!state.behaviours.Any(behaviour => behaviour is GameStateBehaviour))
                {
                    state.AddStateMachineBehaviour(typeof(GameStateBehaviour));
                }
                
                // SubStateMachines should not have GameStateBehaviours. They are StateMachines and not GameStates.
                // Internal state changes will trigger OnExit and OnEnter events on Behaviours attached
                // to the SubStateMachine, which game states shouldn't do.
                foreach (AnimatorStateMachine stateMachine in GetAllStateMachines(animatorController))
                {
                    stateMachine.behaviours = stateMachine.behaviours
                        .Where(behaviour => behaviour is not GameStateBehaviour).ToArray();
                }
            }

            _statesNeedFixing = false;
        }
    }
}