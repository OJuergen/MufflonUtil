using System;
using UnityEngine;

namespace MufflonUtil
{
    public class ManagedObject : MonoBehaviour
    {
        public static event Action<IManagedObject> Registered;
        public static event Action<IManagedObject> Unregistered;

        private void OnEnable()
        {
            foreach (IManagedObject managedObject in GetComponents<IManagedObject>())
            {
                Registered?.Invoke(managedObject);
            }
        }

        private void OnDisable()
        {
            foreach (IManagedObject managedObject in GetComponents<IManagedObject>())
            {
                Unregistered?.Invoke(managedObject);
            }
        }
    }
}