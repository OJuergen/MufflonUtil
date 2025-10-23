using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public class
        DirectorSpeedClip : PlayableDirectorTrack.PlayableDirectorClipPlayableAsset<DirectorSpeedClip.ClipBehaviour>
    {
        [SerializeField] private ClipBehaviour _behaviour;
        protected override ClipBehaviour BehaviourTemplate => _behaviour;

        [Serializable]
        public class ClipBehaviour : PlayableDirectorTrack.ClipBehaviour
        {
            [SerializeField] private float _speed;

            protected override void OnUpdate(Playable playable, FrameData info, PlayableDirector playerData)
            {
                Context.playableGraph.GetRootPlayable(0).SetSpeed(_speed);
            }
        }
    }
}