using System;
using System.Collections.Generic;
using NPCs;
using ScriptableVariables.Objects;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Rumors
{
    public sealed class RumorHandler : MonoBehaviour
    {
        #region Values

        [SerializeField] private TransformVariable playerHandTransformVariable;

        [SerializeField] private bool selectByListOrder;

        [SerializeField] private List<RumorApply> rumorApplies;

        private int index;

        private readonly List<int> completedIndex = new List<int>();

        #endregion

        private void Start()
        {
            if (this.rumorApplies == null || this.rumorApplies.Count == 0)
            {
                Debug.LogError("NO RUMORS TO APPLY");
                return;
            }

            if (!this.selectByListOrder)
                this.index = Random.Range(0, this.rumorApplies.Count);

            this.completedIndex.Add(this.index);

            this.rumorApplies[this.index].Apply(this, this.playerHandTransformVariable);
        }

        public void CompleteCurrent()
        {
            this.rumorApplies[this.index].Complete();

            while (this.completedIndex.Count < this.rumorApplies.Count)
            {
                this.index = Random.Range(0, this.rumorApplies.Count);

                RumorApply r = this.rumorApplies[this.index];
                if (r.IsComplete())
                    continue;

                r.Apply(this, this.playerHandTransformVariable);

                break;
            }
        }

        [Serializable]
        private struct RumorApply
        {
            [SerializeField] private Rumor toActivate;

            [SerializeField] private NPCInteract npcToHandIn;

            [SerializeField] private Dialog defaultDialog;
            [FormerlySerializedAs("onCorrectDialog")] [SerializeField] private Dialog onTrueDialog;
            [FormerlySerializedAs("onFailDialog")] [SerializeField] private Dialog onFalseDialog;

            [SerializeField] private List<NPCWithDialog> npcWithDialogs;

            private bool completed;

            [Serializable]
            private struct NPCWithDialog
            {
                [SerializeField] private NPCInteract npc;

                [SerializeField] private Dialog dialog;

                public void Set()
                {
                    this.npc.SetDialog(this.dialog);
                }

                public void Clear()
                {
                    this.npc.SetDialog(null);
                }
            }

            public void Apply(RumorHandler handler, TransformVariable playerHandTransformVariable)
            {
                this.npcToHandIn.SetCurrentTrigger(new RumorHandInInteract(handler, this.defaultDialog,
                    this.onTrueDialog,
                    this.onFalseDialog,
                    playerHandTransformVariable,
                    this.toActivate));

                foreach (NPCWithDialog npcWithDialog in this.npcWithDialogs)
                    npcWithDialog.Set();
            }

            public void Complete()
            {
                this.completed = true;

                foreach (NPCWithDialog npcWithDialog in this.npcWithDialogs)
                    npcWithDialog.Clear();
            }

            public bool IsComplete()
            {
                return this.completed;
            }
        }
    }
}