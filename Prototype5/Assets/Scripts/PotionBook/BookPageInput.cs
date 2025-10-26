using UnityEngine;
using UnityEngine.EventSystems;

public class BookPageInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public BookRaw book;
    private bool draggingRight = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 screenPos = eventData.position;
        draggingRight = screenPos.x > Screen.width / 2;

        SendDrag(screenPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SendDrag(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        book.OnMouseRelease();
    }

    private void SendDrag(Vector3 screenPos)
    {
        // Convert mouse pos to a world position on the book’s plane
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        // Use a Plane at the book's transform position, facing its forward direction
        Plane bookPlane = new Plane(book.transform.forward, book.transform.position);

        float distance;
        if (bookPlane.Raycast(ray, out distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);

            // Feed the worldPoint to BookRaw so it moves correctly in 3D
            if (draggingRight)
                book.OnMouseDragRightPage(worldPoint);
            else
                book.OnMouseDragLeftPage(worldPoint);
        }
    }
}