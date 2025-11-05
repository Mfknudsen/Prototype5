using UI.Book.TextInputField;
using UnityEngine;

namespace UI.Book.Slider
{
    public class BookSliderReference : MonoBehaviour, ICustomGUIElementReference
    {
        private UIBook uiBook;
        private BookTextInputField textInputField;
        
        public void Setup(UIBook setUIBook, ICustomGUIElement element)
        {
            this.uiBook = setUIBook;
            throw new System.NotImplementedException();
        }
    }
}
