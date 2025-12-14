using ScriptableVariables.Objects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DayNightCycle
{
    public class DirectionalLight : MonoBehaviour
    {
        private void Awake()
        {
            Addressables.LoadAssetAsync<TransformVariable>(
                "Assets/ScriptableObjects/Variables/DirectionalLightTransform.asset").Completed += t =>
            {
                if (t.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError("Failed to load directional light transform");
                    return;
                }

                t.Result.Value = transform;
            };
        }
    }
}
