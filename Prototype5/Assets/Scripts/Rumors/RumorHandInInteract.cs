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

        private readonly Dialog defaultDialog, trueDialog, falseDialog;

        private readonly TransformVariable playerHandTransformVariable;

        private bool done;

        private Coroutine currentResponse;

        public RumorHandInInteract(RumorHandler handler, Dialog defaultDialog, Dialog trueDialog, Dialog falseDialog,
            TransformVariable playerHandTransformVariable, Rumor correctPotion)
        {
            this.handler = handler;
            this.defaultDialog = defaultDialog;
            this.trueDialog = trueDialog;
            this.falseDialog = falseDialog;
            this.playerHandTransformVariable = playerHandTransformVariable;
            this.correctPotion = correctPotion;
        }

        public override void Trigger(NPCInteract npc)
        {
            if (this.done)
                return;

            if (this.currentResponse != null)
                npc.StopCoroutine(this.currentResponse);

            foreach (Transform t in this.playerHandTransformVariable.Value)
            {
                if (!t.gameObject.activeSelf)
                    continue;

                if (!t.TryGetComponent(out PotionObject potionObject))
                    continue;

                this.done = true;

                this.currentResponse = npc.StartCoroutine(this.correctPotion.CheckCorrect(potionObject.GetValue())
                    ? this.TrueResponse(npc)
                    : this.FalseResponse(npc));

                return;
            }

            this.currentResponse = npc.StartCoroutine(this.FalseResponse(npc));
        }

        public override void DefaultSet(NPCInteract npc)
        {
            npc.SetDialog(this.defaultDialog);
        }

        private IEnumerator TrueResponse(NPCInteract npc)
        {
            this.done = true;
            npc.SetDialog(this.trueDialog);

            yield return new WaitForSeconds(5);

            this.handler.CompleteCurrent();
        }

        private IEnumerator FalseResponse(NPCInteract npc)
        {
            npc.SetDialog(this.falseDialog);

            yield return new WaitForSeconds(5);

            npc.SetDialog(this.defaultDialog);
        }
    }
}