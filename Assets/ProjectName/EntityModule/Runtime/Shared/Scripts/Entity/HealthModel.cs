using System;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Index;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Entity
{
    public class HealthModel
    {
        public string Name { get; }
        public int MaxHealthPoints { get; private set; }
        public int HealthPoints { get; private set; }
        public bool IsDied => HealthPoints == 0;
        public event Action<int> ChangedHealthPoints, ChangedMaxHealthPoints;
        public event Action<int> Died;
        public event Action DiedWithoutArgs;

        public HealthModel(int maxHealthPoints, int? healthPoints = null, string name = null)
        {
            Name = name ?? "NoName";
            TrySetMaxHealth(maxHealthPoints, checkDeath: false, changeHealthPoints: false);
            TrySetHealthPoints(healthPoints ?? maxHealthPoints, IndexableTools.MissingOrInvalidId, checkDeath: false, int.MaxValue);
        }

        public bool IsHealthPointsFull()
        {
            return HealthPoints >= MaxHealthPoints;
        }

        public void TrySetMaxHealth(int value, bool checkDeath = true, bool changeHealthPoints = true)
        {
            if (IsDied && checkDeath || value <= 0 || MaxHealthPoints == value)
            {
                return;
            }

            var oldMaxHealthPoints = HealthPoints;
            MaxHealthPoints = value;

            if (changeHealthPoints && oldMaxHealthPoints != 0)
            {
                HealthPoints *= value / oldMaxHealthPoints;
            }

            ChangedMaxHealthPoints?.Invoke(value);
        }

        public void TrySetHealthPoints(int value, int setterId,
            bool checkDeath = true, int? overridedMaxHealthPoints = null)
        {
            TrySetHealthPoints(value, setterId, out var pointsDifference, checkDeath, overridedMaxHealthPoints);
        }

        public void TrySetHealthPoints(int value, int setterId, out int pointsDifference,
            bool checkDeath = true, int? overridedMaxHealthPoints = null)
        {
            pointsDifference = 0;
            
            if (IsDied && checkDeath || value < 0)
            {
                return;
            }

            var oldHealthPoints = HealthPoints;
            
            HealthPoints = Mathf.Clamp(value, 0,
                overridedMaxHealthPoints ?? Mathf.Max(HealthPoints, MaxHealthPoints));

            if (oldHealthPoints == HealthPoints)
            {
                return;
            }
            
            pointsDifference = HealthPoints - oldHealthPoints;

            ChangedHealthPoints?.Invoke(HealthPoints);

            if (IsDied)
            {
                Died?.Invoke(setterId);
                DiedWithoutArgs?.Invoke();
            }
        }

        public override string ToString()
        {
            return $"{nameof(HealthModel)}_{Name}";
        }
    }
}