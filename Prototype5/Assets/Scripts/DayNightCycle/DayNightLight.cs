using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DayNightCycle
{
    [RequireComponent(typeof(Light))]
    public sealed class DayNightLight : MonoBehaviour
    {
        [SerializeField] private Light lightComponent;

        [Header("Morning")] [SerializeField] private Color morningColor = Color.white;
        [SerializeField] private bool morningOnState = true;
        [SerializeField] private float morningIntensity = 1;

        [Header("Noon")] [SerializeField] private Color noonColor = Color.white;
        [SerializeField] private bool noonOnState = true;
        [SerializeField] private float noonIntensity = 1;

        [Header("Evening")] [SerializeField] private Color eveningColor = Color.white;
        [SerializeField] private bool eveningOnState = true;
        [SerializeField] private float eveningIntensity = 1;

        [Header("Night")] [SerializeField] private Color nightColor = Color.white;
        [SerializeField] private bool nightOnState = true;
        [SerializeField] private float nightIntensity = 1;

        [Header("Midnight")] [SerializeField] private Color midnightColor = Color.white;
        [SerializeField] private bool midnightOnState = true;
        [SerializeField] private float midnightIntensity = 1;
        
        private void OnValidate()
        {
            this.lightComponent = this.GetComponent<Light>();
        }

        private void OnEnable()
        {
            DayNight.AddLight(this);
        }

        private void OnDisable()
        {
            DayNight.RemoveLight(this);
        }

        internal void UpdateLight(DayNightTime dayNightTime, float time)
        {
            switch (dayNightTime)
            {
                case DayNightTime.Midnight:
                    this.lightComponent.enabled = this.midnightOnState;
                    this.lightComponent.color = Color.Lerp(this.midnightColor, this.morningColor, time);
                    this.lightComponent.intensity = Mathf.Lerp(this.midnightIntensity, this.morningIntensity, time);
                    return;
                case DayNightTime.Morning:
                    this.lightComponent.enabled = this.morningOnState;
                    this.lightComponent.color = Color.Lerp(this.morningColor, this.noonColor, time);
                    this.lightComponent.intensity = Mathf.Lerp(this.morningIntensity, this.noonIntensity, time);
                    return;
                case DayNightTime.Noon:
                    this.lightComponent.enabled = this.noonOnState;
                    this.lightComponent.color = Color.Lerp(this.noonColor, this.eveningColor, time);
                    this.lightComponent.intensity = Mathf.Lerp(this.noonIntensity, this.eveningIntensity, time);
                    return;
                case DayNightTime.Evening:
                    this.lightComponent.enabled = this.eveningOnState;
                    this.lightComponent.color = Color.Lerp(this.eveningColor, this.nightColor, time);
                    this.lightComponent.intensity = Mathf.Lerp(this.eveningIntensity, this.nightIntensity, time);
                    return;
                case DayNightTime.Night:
                    this.lightComponent.enabled = this.nightOnState;
                    this.lightComponent.color = Color.Lerp(this.nightColor, this.midnightColor, time);
                    this.lightComponent.intensity = Mathf.Lerp(this.nightIntensity, this.midnightIntensity, time);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayNightTime), dayNightTime, null);
            }
        }
    }
}