using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public class CurvedSpeedByTimeMovementCalculator
    {
        public float TotalDistance { get; private set; }
        private float _pathProgress, _elapsedTime, _pathDuration;
        private float _effectiveSpeedScalingFactor;
        private AnimationCurve _speedProfileCurve;

        public void Configure(
            Vector3 startPoint,
            Vector3 targetEndPoint,
            AnimationCurve speedProfileCurve,
            float duration, out bool isConfigured)
        {
            var totalDistance = Vector3.Distance(startPoint, targetEndPoint);
            isConfigured = false;

            // Обработка случаев с нулевой дистанцией или нулевой/отрицательной продолжительностью
            if (totalDistance < 0.001f || duration <= 0f)
            {
                return;
            }

            // --- Шаг 1: Вычисление "интеграла" кривой скорости ---
            // Чтобы пройти заданную дистанцию за заданное время, следуя форме кривой,
            // нам нужно знать, "сколько" движения кривая предлагает при масштабировании на единицу.
            // Это можно найти, численно интегрируя кривую от 0 до 1.
            var curveIntegral = GetNumericalIntegralOfCurve(speedProfileCurve, 100); // 100 сэмплов для точности

            // Если интеграл очень мал или равен нулю, это означает, что кривая
            // почти не дает скорости (например, всегда возвращает 0),
            // и мы не сможем пройти дистанцию.
            if (Mathf.Abs(curveIntegral) < 0.0001f)
            {
                Debug.LogWarning(
                    "Speed profile curve integral is too small. Object might not move or move unpredictably.");
                return;
            }

            // --- Шаг 2: Расчет эффективного коэффициента масштабирования скорости ---
            // Этот коэффициент (по сути, "максимальная скорость") гарантирует,
            // что общая дистанция будет пройдена за заданное время.
            // totalDistance = (средняя скорость * duration)
            // средняя скорость = (эффективный коэффициент * интеграл кривой / 1)
            // (так как интеграл кривой нормирован от 0 до 1)
            // Поэтому: totalDistance = effectiveSpeedScalingFactor * curveIntegral * duration

            isConfigured = true;
            _speedProfileCurve = speedProfileCurve;
            TotalDistance = totalDistance;
            _effectiveSpeedScalingFactor = totalDistance / (curveIntegral * duration);
            _elapsedTime = 0f; // Прошедшее время
            _pathProgress = 0f; // Прогресс вдоль пути (от 0 до 1)
            _pathDuration = duration;
        }

        public void TryUpdateMovement(out bool didEndMove, out float distanceToMoveThisFrame, out float pathProgress)
        {
            didEndMove = _pathProgress >= 1;
            pathProgress = 0;
            distanceToMoveThisFrame = 0;

            var deltaTime = Time.deltaTime;

            _elapsedTime += deltaTime;

            // Нормализованное время (от 0 до 1) относительно общей продолжительности
            var normalizedTime = Mathf.Clamp01(_elapsedTime / _pathDuration);

            // Вычисляем относительную скорость из профиля кривой
            var relativeSpeedFactor = _speedProfileCurve.Evaluate(normalizedTime);

            // Убеждаемся, что относительная скорость не отрицательна
            relativeSpeedFactor = Mathf.Max(0f, relativeSpeedFactor);

            // Вычисляем текущую абсолютную скорость
            var currentSpeed = relativeSpeedFactor * _effectiveSpeedScalingFactor;

            // Расстояние, которое нужно пройти за этот кадр
            distanceToMoveThisFrame = currentSpeed * deltaTime;

            // Обновляем общий прогресс по пути (долю от totalDistance)
            _pathProgress += distanceToMoveThisFrame / TotalDistance;
            _pathProgress = Mathf.Clamp01(_pathProgress); // Гарантируем, что прогресс не превышает 1
            
            pathProgress = _pathProgress;
            didEndMove = _pathProgress >= 1;
        }

        private float GetNumericalIntegralOfCurve(AnimationCurve curve, int samples)
        {
            if (curve == null || curve.length == 0)
            {
                return 0f;
            }

            var integral = 0f;
            var step = 1f / samples; // Шаг для сэмплирования от 0 до 1

            // Используем метод трапеций для лучшей точности
            for (var i = 0; i < samples; i++)
            {
                var t0 = i * step;
                var t1 = (i + 1) * step;

                var y0 = curve.Evaluate(t0);
                var y1 = curve.Evaluate(t1);

                // Площадь трапеции = (средняя высота) * ширина
                integral += (y0 + y1) / 2f * step;
            }

            return integral;
        }
    }
}