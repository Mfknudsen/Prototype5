#region Packages

using System;
using System.Collections;
using ScriptableVariables.Enums;
using UnityEngine;
using UnityEngine.Events;

#endregion

namespace UI.Book
{
    [Serializable]
    internal struct CloseBookAction
    {
        #region Values

        [SerializeField] private UIBook uiBook;
        [SerializeField] private PlayerStateVariable playerStateVariable;

        public bool IsOperationDone { get; private set; }

        #endregion

        public IEnumerator Operation(UnityAction onEnd)
        {
            this.IsOperationDone = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            this.IsOperationDone = true;

            yield return new WaitForSeconds(0.5f);

            this.playerStateVariable.Value = PlayerStateEnum.Free;

            onEnd?.Invoke();
            
        }
    }
}