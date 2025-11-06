using System.Collections;
using NPCs;
using Potions;
using ScriptableVariables.Objects;
using UnityEngine;

namespace Rumors
{
    public sealed class RumorHandInInteract : NPCInteractBase
    {
        private readonly RumorHandler handler;

        private readonly Rumor correctPotion;

        private readonly Dialog defaultDialog, correctDialog, failDialog;

        private readonly TransformVariable playerHandTransformVariable;

        private bool done;

        public RumorHandInInteract(RumorHandler handler, Dialog defaultDialog, Dialog correctDialog, Dialog failDialog,
            TransformVariable playerHandTransformVariable, Rumor correctPotion)
        {
            this.handler = handler;
            this.defaultDialog = defaultDialog;
            this.correctDialog = correctDialog;
            this.failDialog = failDialog;
            this.playerHandTransformVariable = playerHandTransformVariable;
            this.correctPotion = correctPotion;
        }

        public override void Trigger(NPCInteract npc)
        {
            if (this.done)
                return;

            foreach (Transform t in this.playerHandTransformVariable.Value)
            {
                if (!t.gameObject.activeSelf)
                    continue;

                if (!t.TryGetComponent(out PotionObject potionObject))
                    continue;

                this.done = true;

                npc.StartCoroutine(this.correctPotion.CheckCorrect(potionObject.GetValue())
                    ? this.CorrectResponse(npc)
                    : this.FailResponse(npc));
            }
        }

        private IEnumerator CorrectResponse(NPCInteract npc)
        {
            npc.SetDialog(this.correctDialog);

            yield return new WaitForSeconds(5);

            this.handler.CompleteCurrent();
        }

        private IEnumerator FailResponse(NPCInteract npc)
        {
            npc.SetDialog(this.failDialog);

            yield return new WaitForSeconds(5);

            npc.SetDialog(this.defaultDialog);

            this.done = false;
        }
    }
}