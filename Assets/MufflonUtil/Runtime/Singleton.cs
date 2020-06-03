using JetBrains.Annotations;

namespace MufflonUtil
{
    public interface ISingleton
    { }

    public class Singleton<T> : ISingleton where T : Singleton<T>, new()
    {
        private static T _instance;
        [NotNull] public static T Instance => _instance ?? (_instance = new T());
    }
}