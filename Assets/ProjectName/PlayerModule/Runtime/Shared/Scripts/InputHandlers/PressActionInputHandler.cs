namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class PressActionInputHandler
    {
        private bool _wasPressedLastFrame;
        
        private readonly RequestSetActiveStateModel _requestSetActiveStateModel;

        public PressActionInputHandler(RequestSetActiveStateModel requestSetActiveStateModel)
        {
            _requestSetActiveStateModel = requestSetActiveStateModel;
        }

        public void TryEnter()
        {
            if (_wasPressedLastFrame) return;
            _requestSetActiveStateModel.SetRequestedSetActiveState(true);
            _wasPressedLastFrame = true;
        }

        public void TryExit()
        {
            if (!_wasPressedLastFrame) return;
            _requestSetActiveStateModel.SetRequestedSetActiveState(false);
            _wasPressedLastFrame = false;
        }
    }
}