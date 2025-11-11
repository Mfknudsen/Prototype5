using System;
using System.Collections.Generic;
using NPCs.Enemies;
using ScriptableVariables.Objects;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DayNightCycle
{
    public enum DayNightTime
    {
        Morning = 5,
        Evening = 8,
        Afternoon = 14,
        Night = 18,
        Midnight = 22
    }

    public static class DayNight
    {
        private static float CycleTime = 20; //In Minutes
        private static float _currentTime, _timeOffset;

        private static List<DayNightLight> _allLights;

        private static DayNightTime _currentDayNightTime;

        private static UnityEvent<DayNightTime> _onTimeChangeEvent;

        private static SkyboxSetting skyboxSetting;

        private static Camera playerCamera;

        private static TransformVariable cameraTransformVariable;

        private static EnemySpawner _enemySpawner;
        private static bool _enemiesSpawned;

        #region Getters

        public static int GetCurrentHour()
        {
            return (int)_currentTime;
        }

        public static (int, int) GetCurrentHourMinutes()
        {
            return ((int)_currentTime, (int)(_currentTime % 1 * 60));
        }

        public static DayNightTime GetCurrentDayNightTime()
        {
            return _currentDayNightTime;
        }

        #endregion

        #region Setters

        public static void AddListener(UnityAction<DayNightTime> toAdd)
        {
            _onTimeChangeEvent.AddListener(toAdd);
        }

        public static void RemoveListener(UnityAction<DayNightTime> toRemove)
        {
            _onTimeChangeEvent.RemoveListener(toRemove);
        }

        public static void SetTime(float timeInMinutes)
        {
            _currentTime = timeInMinutes * _timeOffset % 24;
        }

        public static void SetTime(DayNightTime dayNightTime)
        {
            _currentDayNightTime = dayNightTime;
            _currentTime = (float)dayNightTime * _timeOffset;
        }

        #endregion

        #region In

        internal static void AddLight(DayNightLight light)
        {
            _allLights.Add(light);
        }

        internal static void RemoveLight(DayNightLight light)
        {
            _allLights.Remove(light);
        }

        #endregion

        #region Internal

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            _allLights = new List<DayNightLight>(32);
            _onTimeChangeEvent = new UnityEvent<DayNightTime>();

            AsyncOperationHandle<SkyboxSetting> loadSkyboxSetting = Addressables
                .LoadAssetAsync<SkyboxSetting>("Assets/ScriptableObjects/DayNight/SkyboxSetting.asset");
            loadSkyboxSetting.Completed += t =>
            {
                skyboxSetting = t.Result;

                CycleTime = skyboxSetting.GetCycleTime();

                //System works on a 24hour basis but offset to match the desired cycle time
                _timeOffset = 24.0f / CycleTime;
            };

            AsyncOperationHandle<TransformVariable> loadTransformVariable =
                Addressables.LoadAssetAsync<TransformVariable>(
                    "Assets/ScriptableObjects/Variables/CameraTransform.asset");
            cameraTransformVariable = loadTransformVariable.Result;
            loadTransformVariable.Completed += t =>
            {
                cameraTransformVariable = t.Result;

                if (cameraTransformVariable.Value != null)
                    playerCamera = cameraTransformVariable.Value.GetComponent<Camera>();

                cameraTransformVariable.AddListener(OnCameraTransformUpdate);
            };

            PlayerLoopSystem playerLoopSystem = PlayerLoop.GetCurrentPlayerLoop();
            for (int i = 0; i < playerLoopSystem.subSystemList.Length; i++)
            {
                if (playerLoopSystem.subSystemList[i].type == typeof(Update))
                    playerLoopSystem.subSystemList[i].updateDelegate += Update;
            }

            PlayerLoop.SetPlayerLoop(playerLoopSystem);

            _currentDayNightTime = DayNightTime.Evening;
            _currentTime = (float)DayNightTime.Evening;

            AsyncOperationHandle<EnemySpawnerReference> loadEnemySpawner = Addressables
                .LoadAssetAsync<EnemySpawnerReference>("Assets/ScriptableObjects/Spawners/EnemySpawnerReference.asset");
            loadEnemySpawner.Completed += t =>
            {
                if (loadEnemySpawner.Result.value != null)
                    _enemySpawner = loadEnemySpawner.Result.value;

                _onTimeChangeEvent.AddListener(CheckSpawnEnemies);
            };

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnExitPlayMode;
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Clean up on exiting play mode.
        /// </summary>
        /// <param name="state">State giving by Unity</param>
        private static void OnExitPlayMode(PlayModeStateChange state)
        {
            if (!state.Equals(PlayModeStateChange.ExitingPlayMode))
                return;

            cameraTransformVariable?.RemoveListener(OnCameraTransformUpdate);

            PlayerLoopSystem playerLoopSystem = PlayerLoop.GetCurrentPlayerLoop();
            for (int i = 0; i < playerLoopSystem.subSystemList.Length; i++)
            {
                if (playerLoopSystem.subSystemList[i].type == typeof(Update))
                    playerLoopSystem.subSystemList[i].updateDelegate -= Update;
            }

            PlayerLoop.SetPlayerLoop(playerLoopSystem);

            EditorApplication.playModeStateChanged -= OnExitPlayMode;
        }
#endif

        private static void Update()
        {
            _currentTime += Time.deltaTime / 60.0f * _timeOffset; // Seconds to minutes

            DayNightTime previous = _currentDayNightTime;
            _currentDayNightTime = _currentTime switch
            {
                < (float)DayNightTime.Morning => DayNightTime.Midnight,
                < (float)DayNightTime.Evening => DayNightTime.Morning,
                < (float)DayNightTime.Afternoon => DayNightTime.Evening,
                < (float)DayNightTime.Night => DayNightTime.Afternoon,
                < (float)DayNightTime.Midnight => DayNightTime.Night,
                _ => DayNightTime.Midnight
            };

            if (_currentDayNightTime != previous)
                _onTimeChangeEvent?.Invoke(_currentDayNightTime);

            if (_currentTime > 24)
                _currentTime -= 24;

            float t = _currentDayNightTime switch
            {
                DayNightTime.Morning => (_currentTime - (float)DayNightTime.Morning) /
                                        ((float)DayNightTime.Evening - (float)DayNightTime.Morning),
                DayNightTime.Evening => (_currentTime - (float)DayNightTime.Evening) /
                                        ((float)DayNightTime.Afternoon - (float)DayNightTime.Evening),
                DayNightTime.Afternoon => (_currentTime - (float)DayNightTime.Afternoon) /
                                          ((float)DayNightTime.Night - (float)DayNightTime.Afternoon),
                DayNightTime.Night => (_currentTime - (float)DayNightTime.Night) /
                                      ((float)DayNightTime.Midnight - (float)DayNightTime.Night),
                DayNightTime.Midnight => (_currentTime + (24 - (float)DayNightTime.Midnight)) /
                                         ((float)DayNightTime.Morning + (24 - (float)DayNightTime.Midnight)),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (skyboxSetting != null)
            {
                (Color color, float intensity) = skyboxSetting.Get(_currentDayNightTime, t);
                //RenderSettings.skybox.color = color;
                RenderSettings.ambientLight = color;
                RenderSettings.ambientIntensity = intensity;
                if (playerCamera != null)
                    playerCamera.backgroundColor = color;
            }

            foreach (DayNightLight dayNightLight in _allLights)
                dayNightLight.UpdateLight(_currentDayNightTime, t);
        }

        private static void OnCameraTransformUpdate(Transform transform)
        {
            playerCamera = transform?.GetComponent<Camera>();
        }

        private static void CheckSpawnEnemies(DayNightTime dayNightTime)
        {
            var spawnTime = _enemySpawner.useNightMobs ? DayNightTime.Night : DayNightTime.Morning;
            var despawnTime = _enemySpawner.useNightMobs ? DayNightTime.Morning : DayNightTime.Night;

            if (dayNightTime == spawnTime && !_enemiesSpawned)
            {
                _enemySpawner.SpawnMobs();
                _enemiesSpawned = true;
            }
            else if (dayNightTime == despawnTime && _enemiesSpawned)
            {
                _enemySpawner.DespawnMobs();
                _enemiesSpawned = false;
            }
        }

        #endregion
    }
}