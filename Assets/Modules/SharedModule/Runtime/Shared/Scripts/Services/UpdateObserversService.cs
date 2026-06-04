using System.Collections.Generic;
using System.Linq;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Repository;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Services
{
    public class UpdateObserversService : IPersistentService
    {
        private readonly Dictionary<UpdateType, List<UpdateObserver>> _updateObserversByUpdateType;

        private readonly Dictionary<GameObject, List<UpdateObserver>> _updateObserverByGameObject =
            new Dictionary<GameObject, List<UpdateObserver>>();

        private readonly TimeScaleRepository _timeScaleRepository;

        public UpdateObserversService(MonoBehaviourObserver globalMonoBehaviourObserver, TimeScaleRepository timeScaleRepository)
        {
            _timeScaleRepository = timeScaleRepository;
            _updateObserversByUpdateType = CollectionTools.ParseEnumToList<UpdateType>()
                .ToDictionary(updateType => updateType, updateType => new List<UpdateObserver>());
            
            globalMonoBehaviourObserver.Updated += UpdateUpdateObservers;
            globalMonoBehaviourObserver.FixedUpdated += UpdateFixedUpdateObservers;
            globalMonoBehaviourObserver.LateUpdated += UpdateLateUpdateObservers;
            globalMonoBehaviourObserver.DrawGizmos += UpdateDrawGizmosObservers;
        }

        public bool TryAddOrGetUpdateObserver(GameObject gameObject, UpdateType updateType, out UpdateObserver updateObserver)
        {
            updateObserver = null;
            
            if (_updateObserverByGameObject.TryGetValue(gameObject, out var observers))
            {
                updateObserver = observers.FirstOrDefault(observer => observer.UpdateType == updateType);
                var shouldAddObserver = updateObserver == null;
                
                if (shouldAddObserver)
                {
                    updateObserver = GetInitializedUpdater();
                    observers.Add(updateObserver);
                }

                return shouldAddObserver;
            }

            updateObserver = GetInitializedUpdater();
            _updateObserverByGameObject[gameObject] = new List<UpdateObserver> {updateObserver};

            return true;

            void RemoveUpdateObserversByGameObject()
            {
                if (_updateObserverByGameObject.Remove(gameObject, out var gameObjectObservers))
                {
                    foreach (var gameObjectObserver in gameObjectObservers)
                    {
                        _updateObserversByUpdateType[gameObjectObserver.UpdateType].Remove(gameObjectObserver);
                    }
                }
            }

            UpdateObserver GetInitializedUpdater()
            {
                var observer = new UpdateObserver(updateType, gameObject);
                _updateObserversByUpdateType[observer.UpdateType].Insert(0, observer);
                gameObject.GetOrAddComponent<DestroyObserver>().Destroyed += RemoveUpdateObserversByGameObject;

                return observer;
            }
        }

        private void UpdateUpdateObservers()
        {
            UpdateObservers(UpdateType.Update, Time.deltaTime * _timeScaleRepository.TimeScale);
        }
        
        private void UpdateDrawGizmosObservers()
        {
            UpdateObservers(UpdateType.DrawGizmos, Time.deltaTime * _timeScaleRepository.TimeScale);
        }

        private void UpdateFixedUpdateObservers()
        {
            UpdateObservers(UpdateType.FixedUpdate, Time.fixedDeltaTime * _timeScaleRepository.TimeScale);
        }

        private void UpdateLateUpdateObservers()
        {
            UpdateObservers(UpdateType.LateUpdate, Time.deltaTime * _timeScaleRepository.TimeScale);
        }

        private void UpdateObservers(UpdateType updateType, float time)
        {
            for (var index = _updateObserversByUpdateType[updateType].Count - 1; index >= 0; index--)
            {
                var observer = _updateObserversByUpdateType[updateType][index];
                
                if (observer.EnableStateProvider.IsEnable)
                {
                    observer.Update(time);
                }
            }
        }
    }
}