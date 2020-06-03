using System;
using UnityEngine;

namespace MufflonUtil
{
    [Serializable]
    public struct AnimatorParameterId
    {
        [SerializeField, NotEditable] private string _name;
        [SerializeField, HideInInspector] private int _hash;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _hash = Animator.StringToHash(_name);
            }
        }

        public AnimatorParameterId(string name)
        {
            _name = name;
            _hash = Animator.StringToHash(_name);
        }

        public static implicit operator int(AnimatorParameterId id)
        {
            if(id._hash == 0) id._hash = Animator.StringToHash(id._name);
            return id._hash;
        }

        public static implicit operator AnimatorParameterId(string s)
        {
            return new AnimatorParameterId(s);
        }
    }
}