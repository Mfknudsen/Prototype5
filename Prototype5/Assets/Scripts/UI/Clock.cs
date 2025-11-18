using System.Text;
using DayNightCycle;
using TMPro;
using UnityEngine;

namespace UI
{
    public sealed class Clock : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI clockText, timeStateText;

        private const string a = "", b = "0", c = " : ";

        private void Update()
        {
            (int, int) time = DayNight.GetCurrentHourMinutes();
            this.clockText.text =
                new StringBuilder().Append(time.Item1 < 10 ? b : a)
                    .Append(time.Item1)
                    .Append(c)
                    .Append(time.Item2 < 10 ? b : a)
                    .Append(time.Item2)
                    .ToString();
            this.timeStateText.text = DayNight.GetCurrentDayNightTime().ToString();
        }

        public void DeactivateClock()
        {
            this.gameObject.SetActive(false);
        }
    }
}