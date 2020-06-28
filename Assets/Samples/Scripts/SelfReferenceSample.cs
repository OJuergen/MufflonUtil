using MufflonUtil;
using UnityEngine;

namespace Samples.Scripts
{
    public class SelfReferenceSample : MonoBehaviour
    {
        [SerializeField, SelfReference(true, true)]
        private TagComponent _referenceParentAndChildren;

        [SerializeField, SelfReference(true)]
        private TagComponent _referenceChildren;

        [SerializeField, SelfReference(false, true)]
        private TagComponent _referenceParent;

        [SerializeField, SelfReference()]
        private TagComponent _referenceSelf;
    }
}