using UnityEngine;

public class BookPageManager : MonoBehaviour
{
    [SerializeField] private BookRaw book;
    [SerializeField] private PotionPageContent[] pageContents;

    void Start()
    {
        // Ensure we have all potion pages
        if (pageContents == null || pageContents.Length == 0)
            pageContents = GetComponentsInChildren<PotionPageContent>(true);

        // Generate textures for each page
        UpdateBookTextures();
    }

    public void UpdateBookTextures()
    {
        Texture[] textures = new Texture[pageContents.Length];

        for (int i = 0; i < pageContents.Length; i++)
        {
            // Make sure the camera renders its current content
            var cam = pageContents[i].GetComponentInChildren<Camera>();
            if (cam != null && cam.targetTexture != null)
            {
                cam.Render(); // refresh the RenderTexture
                textures[i] = cam.targetTexture;
            }
        }

        book.bookPages = textures;
        book.currentPage = 0;
    }
}