using System;
using UnityEngine;

namespace DayNightCycle
{
    [CreateAssetMenu(fileName = "SkyboxSetting", menuName = "Scriptable Objects/SkyboxSetting")]
    public sealed class SkyboxSetting : ScriptableObject
    {
        [SerializeField] private float minutesPerDay;

        [Header("Morning")] [SerializeField] private Color morningColor = Color.white;
        [SerializeField] private float morningIntensity = 1;
        [SerializeField] private float morningLightRotation;

        [Header("Noon")] [SerializeField] private Color noonColor = Color.white;
        [SerializeField] private float noonIntensity = 1;
        [SerializeField] private float noonLightRotation;

        [Header("Evening")] [SerializeField] private Color eveningColor = Color.white;
        [SerializeField] private float eveningIntensity = 1;
        [SerializeField] private float eveningLightRotation;

        [Header("Night")] [SerializeField] private Color nightColor = Color.white;
        [SerializeField] private float nightIntensity = 1;
        [SerializeField] private float nightLightRotation;

        [Header("Midnight")] [SerializeField] private Color midnightColor = Color.white;
        [SerializeField] private float midnightIntensity = 1;
        [SerializeField] private float midnightLightRotation;

        public (Color, float, float) Get(DayNightTime dayNightTime, float time)
        {
            return dayNightTime switch
            {
                DayNightTime.Midnight => (Color.Lerp(this.midnightColor, this.morningColor, time),
                    Mathf.Lerp(this.midnightIntensity, this.morningIntensity, time),
                    Mathf.Lerp(this.midnightLightRotation - 180, this.morningLightRotation, time)),
                DayNightTime.Morning => (Color.Lerp(this.morningColor, this.noonColor, time),
                    Mathf.Lerp(this.morningIntensity, this.noonIntensity, time),
                    Mathf.Lerp(this.morningLightRotation, this.noonLightRotation, time)),
                DayNightTime.Noon => (Color.Lerp(this.noonColor, this.eveningColor, time),
                    Mathf.Lerp(this.noonIntensity, this.eveningIntensity, time),
                    Mathf.Lerp(this.noonLightRotation, this.eveningLightRotation, time)),
                DayNightTime.Evening => (Color.Lerp(this.eveningColor, this.nightColor, time),
                    Mathf.Lerp(this.eveningIntensity, this.nightIntensity, time),
                    Mathf.Lerp(this.eveningLightRotation, this.nightLightRotation, time)),
                DayNightTime.Night => (Color.Lerp(this.nightColor, this.midnightColor, time),
                    Mathf.Lerp(this.nightIntensity, this.midnightIntensity, time),
                    Mathf.Lerp(this.nightLightRotation, this.midnightLightRotation, time)),
                _ => throw new ArgumentOutOfRangeException(nameof(dayNightTime), dayNightTime, null)
            };
        }

        public float GetCycleTime()
        {
            return this.minutesPerDay;
        }
    }
}