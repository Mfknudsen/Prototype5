using TMPro;
using UnityEngine;

public class PotionPageContent : MonoBehaviour
{
    [SerializeField] private RenderTexture rt;

    [SerializeField] private Camera renderCamera;

    [SerializeField] private TextMeshProUGUI textMesh;

    [SerializeField] private GameObject potionPrefab;

    public string potionName;
    public Color backgroundColor = Color.yellow;
    
    void Start()
    {
        // Validate the Render Texture
        if (rt == null)
        {
            Debug.LogError("Render texture could not be set");
            return;
        }
        if (renderCamera == null)
        {
            Debug.LogError("Render camera could not be set");
            return;
        }

        // Configure the properties
        ConfigurePotionPage();
    }
    
    private void ConfigurePotionPage()
    {
        // Set the potion name in the TextMeshPro component
        textMesh.text = potionName;

        // Set the camera's background color
        renderCamera.backgroundColor = backgroundColor;

        // Clear the existing potion instance if it exists
        foreach (Transform child in renderCamera.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate and configure the potion prefab
        if (potionPrefab != null)
        {
            GameObject potionInstance = Instantiate(potionPrefab, renderCamera.transform, false);
            potionInstance.transform.localPosition = new Vector3(0, 0, 5); // Adjust position
            potionInstance.transform.rotation = Quaternion.identity; // Reset rotation
        }
        else
        {
            Debug.LogWarning("Potion prefab not set");
        }
    }
}
