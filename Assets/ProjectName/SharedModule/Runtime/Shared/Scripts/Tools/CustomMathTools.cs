using System;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class CustomMathTools
    {
        public static Vector3 Multiply(this Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(vector1.x * vector2.x, vector1.y * vector2.y, vector1.z * vector2.z);
        }

        public static Vector3 Abs(this Vector3 vector3)
        {
            return new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
        }

        public static double GetRoundedDouble(this float number)
        {
            return Math.Round(number, MidpointRounding.AwayFromZero);
        }
        
        public static float GetRoundedFloat(this float number)
        {
            return (float)GetRoundedDouble(number);
        }
        
        public static int GetRoundedInt(this float number)
        {
            return (int)GetRoundedDouble(number);
        }
        
        public static bool IsInsideRange(this float value, float minimumValue, float maximumValue) => value >= minimumValue && value <= maximumValue;
    }
}