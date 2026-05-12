using System;
using Modules.SharedModule.Runtime.Shared.Scripts.UI;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Entity
{
    public class HealthBarController : IDisposable
    {
        public BarController Controller { get; }
        private readonly HealthModel _healthModel;

        public HealthBarController(BarController controller, HealthModel healthModel)
        {
            Controller = controller;
            _healthModel = healthModel;

            Controller.UpdatePointsMaxValue(_healthModel.MaxHealthPoints);
            Controller.UpdatePoints(_healthModel.HealthPoints);

            _healthModel.ChangedMaxHealthPoints += Controller.UpdatePointsMaxValue;
            _healthModel.ChangedHealthPoints += UpdatePoints;
        }

        public void Dispose()
        {
            _healthModel.ChangedMaxHealthPoints -= Controller.UpdatePointsMaxValue;
            _healthModel.ChangedHealthPoints -= UpdatePoints;
        }

        private void UpdatePoints(int points)
        {
            Controller.UpdatePoints(points);
        }
    }
}