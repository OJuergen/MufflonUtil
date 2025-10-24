using JetBrains.Annotations;
using UnityEngine;

namespace MufflonUtil.Samples.Basics
{
    public class SelfReferenceSample : MonoBehaviour
    {
        [SerializeField, SelfReference(true, true), UsedImplicitly]
        private TagComponent _referenceParentAndChildren;
        
        [SerializeField, SelfReference(true, true), UsedImplicitly]
        private GameObject _referenceGameObjectParentAndChildren;

        [SerializeField, SelfReference(true), UsedImplicitly]
        private TagComponent _referenceChildren;

        [SerializeField, SelfReference(false, true), UsedImplicitly]
        private TagComponent _referenceParent;

        [SerializeField, SelfReference, UsedImplicitly]
        private TagComponent _referenceSelf;

        [SerializeField, UsedImplicitly] private TagComponent _regularReference;
    }
}