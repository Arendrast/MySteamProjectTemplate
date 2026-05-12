using ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer.View.Rig;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer.View
{
    public class PlayerViewComponents
    {
        public readonly PlayerViewSerializableComponents SerializableComponents;
        public readonly PlayerViewRigSerializableComponents ViewRigSerializableComponents;
        public readonly CharacterType CharacterType;

        public PlayerViewComponents(
            PlayerViewSerializableComponents serializableComponents,
            PlayerViewRigSerializableComponents viewRigSerializableComponents, 
            CharacterType characterType)
        {
            SerializableComponents = serializableComponents;
            ViewRigSerializableComponents = viewRigSerializableComponents;
            CharacterType = characterType;
        }
    }
}