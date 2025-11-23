using Potions;
using UnityEngine;

public class HoverHighlighter : MonoBehaviour
{
    public float maxDistance = 5f;
    private Highlightable current;

    public LayerMask selectableLayer;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, selectableLayer))
        {
            Highlightable h = hit.collider.GetComponentInParent<Highlightable>();
            if (h != current)
            {
                current?.Unhighlight();
                current = h;
                current.Highlight();
            }
        }
        else
        {
            current?.Unhighlight();
            current = null;
        }
    }

}