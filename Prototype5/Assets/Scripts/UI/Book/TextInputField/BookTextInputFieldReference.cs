using UnityEngine;

namespace UI.Book.TextInputField
{
    public class BookTextInputFieldReference : MonoBehaviour, ICustomGUIElementReference
    {
        private UIBook uiBook;
        private BookTextInputField bookField;

        public void Setup(UIBook uiBook, ICustomGUIElement element)
        {
            this.uiBook = uiBook;
            if (element is BookTextInputField field) this.bookField = field;
        }
    }
}