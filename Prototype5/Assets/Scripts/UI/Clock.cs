using System;
using DayNightCycle;
using TMPro;
using UnityEngine;

namespace UI
{
    public sealed class Clock : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI clockText, timeStateText;

        private void Update()
        {
            (int, int) time = DayNight.GetCurrentHourMinutes();
            this.clockText.text =
                $"{(time.Item1 < 10 ? "0" : "")}{time.Item1} : {(time.Item2 < 10 ? "0" : "")}{time.Item2}";
            this.timeStateText.text = DayNight.GetCurrentDayNightTime().ToString();
        }
    }
}