using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MufflonUtil
{
    public class ObjectManager<T> : Singleton<ObjectManager<T>> where T : Component, IManagedObject
    {
        private readonly Dictionary<int, T> _registeredObjects = new Dictionary<int, T>();
        private int _nextID;

        public delegate void ObjectDelegate(T obj);

        public static event ObjectDelegate ObjectRegistered;
        public static event ObjectDelegate ObjectUnregistered;

        public ObjectManager()
        {
            ManagedObject.Registered += OnObjectRegistered;
            ManagedObject.Unregistered += OnObjectUnregistered;
            _nextID = 0;
            foreach (T managedObject in Object.FindObjectsOfType<T>())
            {
                OnObjectRegistered(managedObject);
            }
        }

        ~ObjectManager()
        {
            ManagedObject.Registered -= OnObjectRegistered;
            ManagedObject.Unregistered -= OnObjectUnregistered;
        }

        private void OnObjectRegistered(IManagedObject obj)
        {
            if (!(obj is T t)) return;
            t.ID = _nextID++;
            _registeredObjects[t.ID] = t;
            ObjectRegistered?.Invoke(t);
        }

        private void OnObjectUnregistered(IManagedObject obj)
        {
            if (!(obj is T t)) return;
            _registeredObjects.Remove(t.ID);
            ObjectUnregistered?.Invoke(t);
        }

        public T Get(int id)
        {
            return _registeredObjects.TryGetValue(id, out T t) ? t : null;
        }

        public IEnumerable<T> GetAll()
        {
            return _registeredObjects.Values;
        }
        
        public T GetClosest(Vector3 position, float maxDistance = float.PositiveInfinity)
        {
            return GetAll()
                .Select(obj => (obj, Distance: Vector3.Distance(obj.transform.position, position)))
                .Where(args => args.Distance < maxDistance)
                .OrderBy(args => args.Distance)
                .Select(args => args.obj)
                .FirstOrDefault();
        }

        public T GetClosest(Ray ray, float maxDistance = float.PositiveInfinity)
        {
            return GetAll()
                .Select(obj => (obj, Distance: Vector3.ProjectOnPlane(obj.transform.position - ray.origin, ray.direction).magnitude))
                .Where(args => args.Distance < maxDistance)
                .OrderBy(args => args.Distance)
                .Select(args => args.obj)
                .FirstOrDefault();
        }
    }
}