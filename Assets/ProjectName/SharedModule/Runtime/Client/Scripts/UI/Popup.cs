using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectName.SharedModule.Runtime.Client.Scripts.UI
{
    public class Popup : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        
        public event Action Opened, Closed;
        public event Action<bool> ChangedOpenState;

        public Button OpenButton => _openButton;
        public Button CloseButton => _closeButton;
        
        [SerializeField] private bool _isOpenOnStart;
        [SerializeField] private Button _openButton, _closeButton;


        private void Awake()
        {
            SetOpenState(_isOpenOnStart);
            _openButton?.onClick.AddListener(TryOpen);
            _closeButton?.onClick.AddListener(TryClose);
        }

        public void TrySetOpenState()
        {
            if (IsOpen)
                TryClose();
            else
                TryOpen();
        }
        
        public void TrySetOpenState(bool isOpen)
        {
            if (isOpen)
                TryOpen();
            else
                TryClose();
        }

        public void TryOpen()
        {
            if (IsOpen)
                return;
            
            SetOpenState(true);
        }

        public void TryClose()
        {
            if (!IsOpen)
                return;

            SetOpenState(false);
        }

        private void SetOpenState(bool isOpen)
        {
            IsOpen = isOpen;
            
            if (isOpen)
                Opened?.Invoke();
            else
                Closed?.Invoke();
            
            gameObject.SetActive(isOpen);
            ChangedOpenState?.Invoke(isOpen);
        }
    }
}