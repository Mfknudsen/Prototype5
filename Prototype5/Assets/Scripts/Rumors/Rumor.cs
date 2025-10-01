using System.Collections.Generic;
using Potions;
using UnityEngine;

namespace Rumors
{
    [CreateAssetMenu(fileName = "Rumor", menuName = "Scriptable Objects/Rumor")]
    public sealed class Rumor : ScriptableObject
    {
        [SerializeField] [TextArea] private string displayText;

        [SerializeField] private List<PotionValue> correctPotionOptions;

        public bool CheckCorrect(PotionValue toCheck)
        {
            if (this.correctPotionOptions.Count == 0)
            {
                Debug.LogError("NO CORRECT POTION OPTIONS");
                return false;
            }

            foreach (PotionValue correctPotionOption in this.correctPotionOptions)
            {
                if (toCheck.Equals(correctPotionOption))
                    return true;
            }

            return false;
        }

        public string GetDisplayText()
        {
            return this.displayText;
        }
    }
}