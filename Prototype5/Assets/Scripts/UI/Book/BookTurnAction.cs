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

        #endregion

        public void SetDirection(bool set)
        {
            this.fromLeftToRight = set;
        }

        public IEnumerator Operation(UnityAction onEnd)
        {
            this.SetOpens(false);
            this.SetTurns(true);

            const float animationTime = 0.5f;

            yield return new WaitForSeconds(animationTime * 0.1f);

            if (this.fromLeftToRight)
                this.openRight.SetActive(true);
            else
                this.openLeft.SetActive(true);

            yield return new WaitForSeconds(animationTime * 0.825f);

            this.SetTurns(false);
            this.SetOpens(true);

            this.uiBook.ConstructUI();

            yield return new WaitForSeconds(animationTime * 0.075f);

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