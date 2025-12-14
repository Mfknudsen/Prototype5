using DayNightCycle;
using NPCs.Enemies;
using ScriptableVariables.Objects;
using UI;
using UnityEngine;

namespace Managers
{
    public class EndStateManager : MonoBehaviour
    {
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private EnemiesIndication enemiesIndication;
        [SerializeField] private GameEvent failStateEvent;
        [SerializeField] private GameEvent winStateEvent;
        [SerializeField] private int maxNumberOfDays = 2;
        private int _daysPassed = 1;
        private int _eveningsPassed;

        private void OnEnable()
        {
            DayNight.AddListener(CheckWinCondition);
            DayNight.AddListener(CheckEveningsPassed);
        }

        private void OnDisable()
        {
            DayNight.RemoveListener(CheckWinCondition);
            DayNight.RemoveListener(CheckEveningsPassed);
        }

        private void CheckWinCondition(DayNightTime dayNightTime)
        {
            if (dayNightTime != DayNightTime.Morning) return;
            _daysPassed++;
            if (_daysPassed <= maxNumberOfDays) return;
            
            if (enemySpawner.GetEnemyCount() == 0)
                winStateEvent?.InvokeGameEvents();
            else
                failStateEvent?.InvokeGameEvents();
        }

        private void CheckEveningsPassed(DayNightTime dayNightTime)
        {
            if (dayNightTime != DayNightTime.Evening) return;
            _eveningsPassed++;
            if (_eveningsPassed != maxNumberOfDays) return;

            enemiesIndication?.ShowEnemiesIndication();
        }
    }
}
