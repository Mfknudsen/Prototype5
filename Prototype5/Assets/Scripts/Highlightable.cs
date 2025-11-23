using UnityEngine;
using System.Collections.Generic;

public class Highlightable : MonoBehaviour
{
    private Renderer[] renderers;

    // Store original materials for each renderer
    private Dictionary<Renderer, Material[]> originalMaterials = new();

    public Material outlineMaterial;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        // Save original materials for all renderers
        foreach (Renderer r in renderers)
        {
            originalMaterials[r] = r.materials;
        }
    }

    public void Highlight()
    {
        foreach (Renderer r in renderers)
        {
            var mats = r.materials;

            // Already outlined?
            if (mats.Length > 0 && mats[mats.Length - 1] == outlineMaterial)
                continue;

            // Append the outline material
            Material[] newMats = new Material[mats.Length + 1];
            for (int i = 0; i < mats.Length; i++)
                newMats[i] = mats[i];

            newMats[mats.Length] = outlineMaterial;

            r.materials = newMats;
        }
    }

    public void Unhighlight()
    {
        foreach (Renderer r in renderers)
        {
            // Restore original materials EXACTLY as they were
            if (originalMaterials.TryGetValue(r, out var mats))
            {
                r.materials = mats;
            }
        }
    }
}