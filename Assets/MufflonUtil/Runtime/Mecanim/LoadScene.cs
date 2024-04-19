using UnityEngine;
using UnityEngine.SceneManagement;

namespace MufflonUtil.Mecanim
{
    public class LoadScene : StateMachineBehaviour
    {
        [SerializeField] private SceneReference _scene;
        private AsyncOperation _sceneLoadAsyncOp;
        private bool _isActive;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isActive = true;
            _sceneLoadAsyncOp = SceneManager.LoadSceneAsync(_scene.SceneName, LoadSceneMode.Additive);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isActive = false;

            if (SceneManager.GetSceneByName(_scene.SceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(_scene.SceneName);
            }
            else if (_sceneLoadAsyncOp is { isDone: false })
            {
                _sceneLoadAsyncOp.completed += OnLoaded;
            }
        }

        private void OnLoaded(AsyncOperation operation)
        {
            if (!_isActive && SceneManager.GetSceneByName(_scene.SceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(_scene.SceneName);
            }
        }
    }
}