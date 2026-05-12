using TMPro;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.UI
{
    public class FPSView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private float _interval = 0.5f;
        private float _time;

        private void Update()
        {
            _time += Time.deltaTime;

            if (_time > _interval)
            {
                var fps = 1 / Time.deltaTime;
                _text.text = $"{fps:F0}";
                _time = 0;
            }
        }
    }
}