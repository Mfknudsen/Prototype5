using System;
using System.Collections;
using ScriptableVariables.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Book
{
    [Serializable]
    internal struct BookOpenAction
    {
        #region Values

        [SerializeField] private UIBook uiBook;
        [SerializeField] private PlayerStateVariable playerStateVariable;

        public bool IsOperationDone { get; private set; }

        #endregion

        public IEnumerator Operation(UnityAction onEnd)
        {
            this.playerStateVariable.Value = PlayerStateEnum.InMenu;

            yield return null;

            this.uiBook.ConstructUI();
            this.IsOperationDone = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            onEnd?.Invoke();

            yield return null;
        }
    }
}