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
                EditorGUILayout.HelpBox("Some transitions are badly configured for a logical game state machine.",
                    MessageType.Warning);
                if (_autoFix || GUILayout.Button("Fix"))
                {
                    FixTransitions(gameStateMachine);
                }
            }

            if (_statesNeedFixing)
            {
                EditorGUILayout.HelpBox("Some animator states have no game state behaviour or more than one.",
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
            foreach (AnimatorStateTransition transition in animatorController.layers
                         .SelectMany(layer => layer.stateMachine.states.SelectMany(state => state.state.transitions)))
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
            _transitionsNeedFixing = animatorController.layers
                .SelectMany(layer => layer.stateMachine.states.SelectMany(state => state.state.transitions))
                .Any(transition => transition.duration != 0f
                                   || transition.hasExitTime
                                   || !transition.hasFixedDuration
                                   || transition.exitTime < 1f
                                   || transition.offset != 0f);
        }

        private void CheckStates(GameStateMachine gameStateMachine)
        {
            _statesNeedFixing = false;
            var animatorController = gameStateMachine.Animator.runtimeAnimatorController as AnimatorController;
            if (animatorController == null) return;
            _statesNeedFixing = animatorController.layers.SelectMany(layer => layer.stateMachine.states)
                .Any(state => state.state.behaviours.Count(behaviour => behaviour is GameStateBehaviour) != 1);
        }

        private void FixStates(GameStateMachine gameStateMachine)
        {
            var animatorController = gameStateMachine.Animator.runtimeAnimatorController as AnimatorController;
            if (animatorController == null) return;
            foreach (AnimatorState state in animatorController.layers.SelectMany(layer => layer.stateMachine.states)
                         .Select(state => state.state))
            {
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
            }

            _statesNeedFixing = false;
        }
    }
}