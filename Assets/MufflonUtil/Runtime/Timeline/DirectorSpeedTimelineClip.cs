using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public class DirectorSpeedClip : PlayableDirectorTrack.PlayableDirectorClipPlayableAsset<DirectorSpeedClip.Behaviour>
    {
        [field:SerializeField] protected override Behaviour Template { get; set; }

        [Serializable]
        public class Behaviour : PlayableDirectorTrack.Behaviour
        {
            [SerializeField] private float _speed;

            protected override void OnUpdate(Playable playable, FrameData info, PlayableDirector playerData)
            {
                Context.playableGraph.GetRootPlayable(0).SetSpeed(_speed);
            }
        }
    }
}