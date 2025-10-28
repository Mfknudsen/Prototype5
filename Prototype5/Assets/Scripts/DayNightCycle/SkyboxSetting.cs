using System;
using UnityEngine;

namespace DayNightCycle
{
    [CreateAssetMenu(fileName = "SkyboxSetting", menuName = "Scriptable Objects/SkyboxSetting")]
    public sealed class SkyboxSetting : ScriptableObject
    {
        [Header("Morning")] [SerializeField] private Color morningColor = Color.white;
        [SerializeField] private float morningIntensity = 1;

        [Header("Evening")] [SerializeField] private Color eveningColor = Color.white;
        [SerializeField] private float eveningIntensity = 1;

        [Header("Afternoon")] [SerializeField] private Color afternoonColor = Color.white;
        [SerializeField] private float afternoonIntensity = 1;

        [Header("Night")] [SerializeField] private Color nightColor = Color.white;
        [SerializeField] private float nightIntensity = 1;

        [Header("Midnight")] [SerializeField] private Color midnightColor = Color.white;
        [SerializeField] private float midnightIntensity = 1;

        public (Color, float) Get(DayNightTime dayNightTime, float time)
        {
            return dayNightTime switch
            {
                DayNightTime.Midnight => (Color.Lerp(this.midnightColor, this.morningColor, time),
                    Mathf.Lerp(this.midnightIntensity, this.morningIntensity, time)),
                DayNightTime.Morning => (Color.Lerp(this.morningColor, this.eveningColor, time),
                    Mathf.Lerp(this.morningIntensity, this.eveningIntensity, time)),
                DayNightTime.Evening => (Color.Lerp(this.eveningColor, this.afternoonColor, time),
                    Mathf.Lerp(this.eveningIntensity, this.afternoonIntensity, time)),
                DayNightTime.Afternoon => (Color.Lerp(this.afternoonColor, this.nightColor, time),
                    Mathf.Lerp(this.afternoonIntensity, this.nightIntensity, time)),
                DayNightTime.Night => (Color.Lerp(this.nightColor, this.midnightColor, time),
                    Mathf.Lerp(this.nightIntensity, this.midnightIntensity, time)),
                _ => throw new ArgumentOutOfRangeException(nameof(dayNightTime), dayNightTime, null)
            };
        }
    }
}