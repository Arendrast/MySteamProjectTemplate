using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.SharedModule.Runtime.Shared.Scripts.Pulsation;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Modules.HudModule.Runtime.Scripts.LowHealPoints
{
    public class LowHealPointsPopupController : IDisposable
    {
        private readonly PulsationController _pulsationController;
        private readonly LowHealPointsPopupSerializableComponents _serializableComponents;
        private readonly Vignette _vignette;
        private readonly int _startHealthPointsForShow;

        private TweenerCore<float, float, FloatOptions> _setVingetteValueTweenerCore;
        private TweenerCore<float, float, FloatOptions> _setClearRadiusValueTweenerCore;

        public LowHealPointsPopupController(
            LowHealPointsPopupSerializableComponents serializableComponents,
            HealthModel healthModel, Vignette vignette)
        {
            _serializableComponents = serializableComponents;
            _vignette = vignette;
            
            _pulsationController = new PulsationController(serializableComponents.VignettePulsationConfig,
                serializableComponents.destroyCancellationToken);
            _pulsationController.Pulsated += value => _vignette.intensity.value += value;

            _startHealthPointsForShow = (healthModel.MaxHealthPoints / 100f *
                                         serializableComponents.StartHealthPointsPercentageForShow).GetRoundedInt();


            healthModel.ChangedHealthPoints += UpdateView;

            UpdateView(healthModel.HealthPoints);
        }

        public void Dispose()
        {
            _vignette.intensity.value = 0;
        }

        private void UpdateView(int healPoints)
        {
            var shouldEnable = healPoints <= _startHealthPointsForShow;

            _vignette.active = shouldEnable;

            if (!shouldEnable)
            {
                _vignette.intensity.value = _serializableComponents.VignetteStartValue;
            }

            if (!shouldEnable)
            {
                _pulsationController.Stop();
                return;
            }
            
            _pulsationController.TryStartPulsate(true, false);

            var t = 0;

            SetVignetteValue(t);
        }

        private void SetVignetteValue(float t)
        {
            var value = Mathf.Lerp(_serializableComponents.VignetteEndValue,
                _serializableComponents.VignetteStartValue,
                _serializableComponents.VignetteValueAnimationCurve.Evaluate(t));

            _setVingetteValueTweenerCore?.Kill();
            _setVingetteValueTweenerCore = DOTween.To(() => _vignette.intensity.value,
                value => _vignette.intensity.value = value,
                value,
                _serializableComponents.SetVignetteValueTimeOnSetHealthPoints);
        }
    }
}