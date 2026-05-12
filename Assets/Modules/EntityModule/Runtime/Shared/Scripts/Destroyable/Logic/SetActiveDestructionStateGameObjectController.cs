using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Destroyable.Logic
{
    public class SetActiveDestructionStateGameObjectController
    {
        private readonly DestructionStatesConfig _destructionStatesConfig;

        public SetActiveDestructionStateGameObjectController(DestructionStatesConfig destructionStatesConfig,
            DestructionStatesConfig.DestructionStateConfig currentDestructionConfig)
        {
            _destructionStatesConfig = destructionStatesConfig;

            SetActiveTrueOnlyActiveGameObject(currentDestructionConfig);
        }

        public void SetActiveTrueOnlyActiveGameObject(
            DestructionStatesConfig.DestructionStateConfig destructionStateConfig)
        {
            _destructionStatesConfig.DestructionStateConfigs.ForEach(config =>
            {
                if (config == destructionStateConfig)
                {
                    foreach (var gameObject in config.GameObjectsToUnlinkFromParentOnEnable)
                    {
                        UnlinkGameObjectAndDestroyAfterTimeAsync(gameObject,
                            config.TimeBeforeDestroyUnlinkedGameObjects);
                    }
                }

                config.ActiveGameObject.SetActive(
                    config == destructionStateConfig);
            });
        }

        private void UnlinkGameObjectAndDestroyAfterTimeAsync(GameObject gameObject, float time)
        {
            gameObject.transform.parent = null;
            Object.Destroy(gameObject, time);
        }
    }
}