using NPCs.Enemies;
using UnityEngine;

namespace ScriptableVariables.Objects
{
    [CreateAssetMenu(fileName = "EnemySpawnerReference", menuName = "Scriptable Objects/EnemySpawnerReference")]
    public class EnemySpawnerReference : ScriptableObject
    {
        [HideInInspector] public EnemySpawner value;
    }
}
