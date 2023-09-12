using UnityEngine;

namespace MufflonUtil
{
    /// <summary>
    /// Used to add informational comments and references to a game object.
    /// </summary>
    [DisallowMultipleComponent]
    public class Comment : MonoBehaviour
    {
        [SerializeField] private string _name = "Name";
        [SerializeField] private string _description = "Description supports multiple lines\n" +
                                                       "and <color=#FF0000><b><i>rich-</i></b></color>text formatting.";
    }
}