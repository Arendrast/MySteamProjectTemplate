using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class SharedNetworkTools
    {
        public static void TryCustomSpawn(this ServerManager serverManager, GameObject gameObject,
            NetworkConnection ownerConnection = null, bool trySyncParent = true)
        {
            TryCustomSpawn(serverManager, gameObject.GetComponent<NetworkObject>(), ownerConnection, trySyncParent);
        }

        public static void TryCustomSpawn(this ServerManager serverManager, NetworkObject networkObject,
            NetworkConnection ownerConnection = null, bool trySyncParent = true)
        {
            if (networkObject == null || networkObject.IsSpawned ||
                (!serverManager.Started && (networkObject.PredictedSpawn == null ||
                                            !networkObject.PredictedSpawn.GetAllowSpawning())))
            {
                return;
            }

            if (trySyncParent && networkObject.transform.parent != null &&
                networkObject.transform.parent.TryGetComponent(out NetworkObject parent))
            {
                networkObject.SetParent(parent);
            }

            serverManager.Spawn(networkObject, ownerConnection,
                networkObject.transform.root == null ? SceneManager.GetActiveScene() : default);
        }

        public static void SetNetworkParent(this Transform transform, Transform parent)
        {
            transform.SetParent(parent);
        }

        public static GameObject GetSpawned(this ServerManager serverManager, GameObject gameObject, NetworkConnection networkConnection = null)
        {
            serverManager.Spawn(gameObject, networkConnection);
            return gameObject;
        }

        public static async void TryDespawnOrDestroyAsync(this ServerManager serverManager, GameObject gameObject,
            bool onlyIfServer = false, float timeBeforeDespawn = 0f)
        {
            if (!gameObject)
                return;

            if (gameObject.TryGetComponent<NetworkObject>(out var networkObject) &&
                (!onlyIfServer || serverManager.Started))
            {
                if (await AsyncTools.AwaitTaskAndGetDoesThrowOperationCancelledException(
                        UniTask.WaitForSeconds(timeBeforeDespawn,
                            cancellationToken: networkObject.destroyCancellationToken)))
                {
                    return;
                }

                serverManager.Despawn(networkObject);
            }
            else
            {
                Object.Destroy(gameObject, timeBeforeDespawn);
            }
        }

        public static bool CustomEquals(this NetworkConnection networkConnection1, NetworkConnection networkConnection2)
            => ReferenceEquals(networkConnection1, networkConnection2);

        public static Dictionary<NetworkConnection, T> GetWithRemovedItAndAllInvalid<T>(
            this Dictionary<NetworkConnection, T> connections, NetworkConnection networkConnection)
        {
            return GetWithRemovedItAndAllInvalid(connections, networkConnection, out var removed);
        }

        public static Dictionary<NetworkConnection, T> GetWithRemovedItAndAllInvalid<T>(
            this Dictionary<NetworkConnection, T> connections, NetworkConnection networkConnection, out T[] removed)
        {
            var localRemoved = new List<T>();

            var dictionary = connections
                .Where(pair =>
                {
                    if (pair.Key.IsValid && !pair.Key.Equals(networkConnection)) return true;

                    localRemoved.Add(pair.Value);
                    return false;
                })
                .ToDictionary(key => key.Key, pair => pair.Value);

            removed = localRemoved.ToArray();

            return dictionary;
        }

        public static T GetOwners<T>(this IReadOnlyDictionary<NetworkConnection, T> dictionary,
            ClientManager clientManager)
            => dictionary.GetValueOrDefault(GetOwnerConnection(clientManager));

        public static NetworkObject TryGetNetworkObjectById(this ClientManager clientManager, int id) =>
            clientManager.Objects.Spawned.GetValueOrDefault(id);

        public static NetworkConnection GetNetworkConnectionByClientId(this ClientManager clientManager, int id)
        {
            var ownerConnection = GetOwnerConnection(clientManager);
            return id == ownerConnection.ClientId ? ownerConnection : clientManager.Clients.GetValueOrDefault(id);
        }

        public static bool IsOwnerOrInvalid(this NetworkConnection networkConnection, ClientManager clientManager) =>
            networkConnection.IsOwner(clientManager) || !networkConnection.IsValid();

        public static bool IsOwnerIdInvalid(this ClientManager clientManager) =>
            !GetOwnerConnection(clientManager).IsValid;

        public static bool IsOwner(this NetworkConnection networkConnection, ClientManager clientManager) =>
            networkConnection.CustomEquals(GetOwnerConnection(clientManager));

        public static bool IsOwner(this int networkConnectionId, ClientManager clientManager) =>
            clientManager.GetNetworkConnectionByClientId(networkConnectionId)
                .CustomEquals(GetOwnerConnection(clientManager));

        public static NetworkConnection GetOwnerConnection(this ClientManager clientManager) =>
            clientManager?.Connection;


        public static void ReappointTransformsAndRebindAnimator(this Transform clientsAnimatorChildrenParent,
            Animator animator)
        {
            var children = Enumerable.Range(0, clientsAnimatorChildrenParent.childCount)
                .Select(i =>
                    clientsAnimatorChildrenParent.transform.GetChild(i)).ToList();

            var boolParams = new Dictionary<string, bool>();
            var floatParams = new Dictionary<string, float>();
            var intParams = new Dictionary<string, int>();

            foreach (var param in animator.parameters)
            {
                switch (param.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        boolParams[param.name] = animator.GetBool(param.name);
                        break;
                    case AnimatorControllerParameterType.Float:
                        floatParams[param.name] = animator.GetFloat(param.name);
                        break;
                    case AnimatorControllerParameterType.Int:
                        intParams[param.name] = animator.GetInteger(param.name);
                        break;
                }
            }

            children.ForEach(child => child.SetParent(animator.transform));

            animator.Rebind();

            foreach (var kvp in boolParams)
                animator.SetBool(kvp.Key, kvp.Value);
            foreach (var kvp in floatParams)
                animator.SetFloat(kvp.Key, kvp.Value);
            foreach (var kvp in intParams)
                animator.SetInteger(kvp.Key, kvp.Value);
        }
    }
}