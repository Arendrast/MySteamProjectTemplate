using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.UI
{
    public class BarController
    {
        private readonly BarSerializableComponents _serializableComponents;

        public BarController(BarSerializableComponents serializableComponents)
        {
            _serializableComponents = serializableComponents;
        }

        public void SetActiveBar(bool isActive)
        {
            _serializableComponents.Slider.gameObject.SetActive(isActive);
        }

        public void UpdatePoints(float value, string format = "f1", float? sliderValue = null)
        {
            _serializableComponents.Slider.value = Mathf.Min(_serializableComponents.Slider.maxValue, sliderValue ?? value);
            _serializableComponents.Slider.value = Mathf.Min(_serializableComponents.Slider.maxValue, sliderValue ?? value);
            _serializableComponents.Points.text = value.ToString(format);
        }

        public void UpdatePoints(int value, int? sliderValue = null)
        {
            UpdatePoints(value, "f0", sliderValue);
        }

        public void UpdatePointsMaxValue(int value)
        {
            UpdatePointsMaxValue(value, "f0");
        }

        public void UpdatePointsMaxValue(float value, string format = "f1")
        {
            _serializableComponents.Slider.maxValue = value;
            _serializableComponents.MaxPoints.text = value.ToString(format);
        }
    }
}