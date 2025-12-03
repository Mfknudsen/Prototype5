using DayNightCycle;
using NPCs.Enemies;
using ScriptableVariables.Objects;
using UnityEngine;

namespace Managers
{
    public class EndStateManager : MonoBehaviour
    {
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private GameEvent failStateEvent;
        [SerializeField] private GameEvent winStateEvent;
        [SerializeField] private int maxNumberOfDays = 2;
        private int _daysPassed = 1;

        private void OnEnable()
        {
            DayNight.AddListener(CheckWinCondition);
        }

        private void OnDisable()
        {
            DayNight.RemoveListener(CheckWinCondition);
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
    }
}
