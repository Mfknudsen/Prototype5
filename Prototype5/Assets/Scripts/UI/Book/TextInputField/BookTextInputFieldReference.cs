using UnityEngine;

namespace UI.Book.TextInputField
{
    public class BookTextInputFieldReference : MonoBehaviour, ICustomGUIElementReference
    {
        private UIBook uiBook;
        private BookTextInputField bookField;

        public void Setup(UIBook setUIBook, ICustomGUIElement element)
        {
            this.uiBook = setUIBook;
            if (element is BookTextInputField field) this.bookField = field;
        }
    }
}