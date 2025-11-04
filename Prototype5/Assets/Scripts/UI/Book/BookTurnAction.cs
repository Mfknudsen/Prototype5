#region Packages

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#endregion

namespace UI.Book
{
    [Serializable]
    internal struct BookTurnAction
    {
        #region Values

        [SerializeField] private UIBook uiBook;
        [SerializeField] private GameObject turnLeftPaper, turnRightPaper, openLeft, openRight;

        private bool fromLeftToRight;
        private bool done;
        private static readonly int InvertPageID = Shader.PropertyToID("InvertPage");

        #endregion

        public void SetDirection(bool set)
        {
            this.fromLeftToRight = set;
        }

        public bool IsOperationDone => this.done;

        public IEnumerator Operation(UnityAction onEnd)
        {
            this.done = false;

            this.SetOpens(false);
            this.SetTurns(true);

            this.turnLeftPaper.GetComponent<Renderer>().material.SetInt(InvertPageID, this.fromLeftToRight ? 0 : 1);
            this.turnRightPaper.GetComponent<Renderer>().material.SetInt(InvertPageID, this.fromLeftToRight ? 1 : 0);

            const float animationTime = 0.5f;

            yield return new WaitForSeconds(animationTime * 0.1f);

            if (!this.fromLeftToRight)
                this.openRight.SetActive(true);
            else
                this.openLeft.SetActive(true);

            yield return new WaitForSeconds(animationTime * 0.9f);

            this.SetTurns(false);
            this.SetOpens(true);

            this.uiBook.ConstructUI();
            
            this.done = true;
            
            onEnd?.Invoke();
        }

        #region Internal

        private void SetOpens(bool set)
        {
            this.openLeft.SetActive(set);
            this.openRight.SetActive(set);
        }

        private void SetTurns(bool set)
        {
            this.turnLeftPaper.SetActive(set);
            this.turnRightPaper.SetActive(set);
        }

        #endregion
    }
}