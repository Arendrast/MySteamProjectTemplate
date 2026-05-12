#if WWISE
using CrazySWAT.SharedModule.Runtime.Shared.Scripts.Tools;
using FishNet.Object;
using UnityEngine;

namespace CrazySWAT.SharedModule.Runtime.Shared.Scripts.Sounds
{
    public class PostEventPlayerOnStartNetwork : NetworkBehaviour
    {
        [SerializeField] private PostEventPlayer _postEventPlayer;
        [SerializeField] private string _eventName;
        
        public override void OnStartNetwork()
        {
            _postEventPlayer.PostEvent(_eventName.IsNullOrEmptyOrWhiteSpace() ? "null" : _eventName);
        }
    }
}
#endif