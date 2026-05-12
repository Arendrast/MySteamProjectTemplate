using System;
using FishNet.Object;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.EntityModule.Runtime.Shared.Scripts.Repositories;

namespace Modules.EntityModule.Runtime.Shared.Scripts.AliveStateTrigger
{
    public class AliveStateActionTriggerPredicate : IActionTriggerPredicate
    {
        public event Action ChangedResult;
        private bool _isDisposed;

        private HealthModel _targetHealthModel;
        private readonly NetworkObject _networkObject;
        private readonly bool _shouldBeLive;
        private readonly HealthModelsRepository _healthModelsesRepository;

        public AliveStateActionTriggerPredicate(NetworkObject networkObject,
            bool shouldBeLive,
            HealthModelsRepository healthModelsesRepository)
        {
            _networkObject = networkObject;
            _shouldBeLive = shouldBeLive;
            _healthModelsesRepository = healthModelsesRepository;

            if (healthModelsesRepository.ValueByKey.TryGetValue(networkObject.ObjectId, out var healthModel))
            {
                TrySubscribeToHealthModel(networkObject.ObjectId, healthModel);
            }
            else
            {
                healthModelsesRepository.Added += TrySubscribeToHealthModel;
            }
        }

        private void TrySubscribeToHealthModel(int id, HealthModel healthModel)
        {
            if (_networkObject.ObjectId != id)
            {
                return;
            }

            _targetHealthModel = healthModel;

            if (GetResult())
            {
                InvokeChangedResultAndDispose();
            }
            else
            {
                healthModel.DiedWithoutArgs += InvokeChangedResultAndDispose;
            }
        }

        public bool GetResult()
        {
            return _isDisposed || (_targetHealthModel != null && _targetHealthModel.IsDied != _shouldBeLive);
        }

        public void Dispose()
        {
            _isDisposed = true;
            _healthModelsesRepository.Added -= TrySubscribeToHealthModel;

            if (_targetHealthModel != null)
                _targetHealthModel.DiedWithoutArgs -= InvokeChangedResultAndDispose;
            
            _targetHealthModel = null;
            ChangedResult = null;
        }

        private void InvokeChangedResultAndDispose()
        {
            ChangedResult?.Invoke();
            Dispose();
        }
    }
}