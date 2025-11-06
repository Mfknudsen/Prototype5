using UnityEngine;

namespace Rumors
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "Scriptable Objects/Dialog")]
    public sealed class Dialog : ScriptableObject
    {
        [SerializeField] [TextArea] private string description;

        [TextArea] public string text;
    }
}