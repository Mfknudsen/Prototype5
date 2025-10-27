using UI.Book.TextInputField;
using UnityEngine;

namespace UI.Book.Slider
{
    public class BookSliderReference : MonoBehaviour, ICustomGUIElementReference
    {
        private UIBook uiBook;
        private BookTextInputField textInputField;
        
        public void Setup(UIBook uiBook, ICustomGUIElement element)
        {
            this.uiBook = uiBook;
            throw new System.NotImplementedException();
        }
    }
}
