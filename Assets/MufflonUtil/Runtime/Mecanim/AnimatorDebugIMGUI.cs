using UnityEngine;

namespace MufflonUtil.Mecanim
{
    public class AnimatorDebugIMGUI : MonoBehaviour
    {
        private Animator _animator;
        private float _speed = 1;
        private int _layer;
        private string _clipName;
        private float _transitionDuration = 0.25f;
        private float _animationOffset;
        private bool _relativeTime;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.speed = _speed;
        }

        [ContextMenu("Play Animation Direct")]
        private void PlayAnimation()
        {
            _animator.Play(_clipName, 0, _animationOffset);
        }

        [ContextMenu("Cross Fade Animation")]
        private void CrossFadeAnimation()
        {
            _animator.CrossFade(_clipName, _transitionDuration, 0, _animationOffset);
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, 500, 500));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Speed");
            _speed = float.Parse(GUILayout.TextField($"{_speed}"));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Layer");
            _layer = int.Parse(GUILayout.TextField($"{_layer}"));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Clip name");
            _clipName = GUILayout.TextField(_clipName);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Offset");
            _animationOffset = float.Parse(GUILayout.TextField($"{_animationOffset}"));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Transition duration");
            _transitionDuration = float.Parse(GUILayout.TextField($"{_transitionDuration}"));
            GUILayout.EndHorizontal();

            _relativeTime = GUILayout.Toggle(_relativeTime, "Relative Time");

            if (GUILayout.Button("Play Direct"))
            {
                if (_relativeTime)
                    _animator.Play(_clipName, _layer, _animationOffset);
                else
                    _animator.PlayInFixedTime(_clipName, _layer, _animationOffset);
            }
            if (GUILayout.Button("Fade In"))
            {
                if (_relativeTime)
                    _animator.CrossFade(_clipName, _transitionDuration, _layer, _animationOffset);
                else
                    _animator.CrossFadeInFixedTime(_clipName, _transitionDuration, _layer, _animationOffset);
            }

            AnimatorStateInfo currentStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextStateInfo = _animator.GetNextAnimatorStateInfo(0);
            bool isInTransition = _animator.IsInTransition(0);
            AnimatorTransitionInfo animatorTransitionInfo = _animator.GetAnimatorTransitionInfo(0);

            GUILayout.Label($"current state: {currentStateInfo.normalizedTime}");
            if (isInTransition)
            {
                GUILayout.Label($"next state state: {nextStateInfo.normalizedTime}");
                GUILayout.Label($"transition duration: {animatorTransitionInfo.duration}");
            }
            GUILayout.EndArea();
        }
    }
}