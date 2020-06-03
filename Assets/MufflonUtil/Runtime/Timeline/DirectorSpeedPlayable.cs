using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public class DirectorSpeedPlayable : PlayableDirectorTrack.PlayableAsset<DirectorSpeedPlayable.PlayableBehaviour>
    {
        [Serializable]
        public class PlayableBehaviour : PlayableDirectorTrack.Behaviour
        {
            [SerializeField] private float _speed;

            protected override void OnBehaviourUpdate(Playable playable, FrameData info, PlayableDirector playerData)
            {
                Context.playableGraph.GetRootPlayable(0).SetSpeed(_speed);
            }
        }
    }
}