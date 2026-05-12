using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class CurvedSpeedMovementTools
    {
        public static async UniTask MoveObjectAlongStraightPathAsync(
            Transform targetObject,
            Vector3 startPoint,
            Vector3 targetEndPoint,
            AnimationCurve speedCurve, float accelerationTime,
            float maxSpeed, CancellationToken additionalToken = default)
        {
            await InvokeUpdateAlongStraightPathAsync(targetObject, startPoint, targetEndPoint, speedCurve,
                accelerationTime,
                maxSpeed,
                MoveObject, additionalToken);

            return;

            void MoveObject(float pathProgress, float distance)
            {
                targetObject.position = Vector3.Lerp(startPoint, targetEndPoint, pathProgress);
            }
        }


        public static async UniTask InvokeUpdateAlongStraightPathAsync(
            Transform targetObject,
            Vector3 startPoint,
            Vector3 targetEndPoint,
            AnimationCurve speedCurve, float accelerationTime,
            float maxSpeed, Action<float, float> updated = null, CancellationToken additionalToken = default)
        {
            targetObject.position = startPoint;

            float speedProgress = 0;
            float pathProgress = 0;

            var totalDistance = Vector3.Distance(startPoint, targetEndPoint);

            targetObject.position = startPoint;

            var token = CancellationTokenSource
                .CreateLinkedTokenSource(targetObject.GetCancellationTokenOnDestroy(), additionalToken).Token;

            while (pathProgress < 1 && !token.IsCancellationRequested)
            {
                speedProgress += Time.deltaTime / Mathf.Max(Time.deltaTime, accelerationTime);
                speedProgress = Mathf.Clamp01(speedProgress);
                var curveValue = speedCurve.Evaluate(speedProgress);
                var currentSpeed = Mathf.Lerp(0, maxSpeed, curveValue);
                var distanceThisFrame = currentSpeed * Time.deltaTime;
                pathProgress += distanceThisFrame / totalDistance;
                pathProgress = Mathf.Clamp01(pathProgress);
                updated?.Invoke(pathProgress, distanceThisFrame);
                await UniTask.DelayFrame(1, cancellationToken: token);
            }
        }
    }
}