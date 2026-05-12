using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class Vector3Tools
    {
        public static float SqrDistance(this Vector3 vector, Vector3 vector2)
        {
            return (vector - vector2).sqrMagnitude;
        }
        
        public static Vector3 WithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        public static Vector3 WithXY(this Vector3 vector, float x, float y)
        {
            return new Vector3(x, y, vector.z);
        }

        public static Vector3 WithXZ(this Vector3 vector, float x, float z)
        {
            return new Vector3(x, vector.y, z);
        }

        public static Vector3 WithYZ(this Vector3 vector, float y, float z)
        {
            return new Vector3(vector.x, y, z);
        }

        public static bool EqualsPositions(this Vector3 p1, Vector3 p2, float sqrE = 0.0001f)
        {
            return (p1 - p2).sqrMagnitude < sqrE;
        }

        public static Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            return Vector3.Lerp(Vector3.Lerp(a, b, t), Vector3.Lerp(b, c, t), t);
        }

        public static Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            var ab = Vector3.Lerp(a, b, t);
            var bc = Vector3.Lerp(b, c, t);
            var cd = Vector3.Lerp(c, d, t);

            var ab_bc = Vector3.Lerp(ab, bc, t);
            var bc_cd = Vector3.Lerp(bc, cd, t);

            return Vector3.Lerp(ab_bc, bc_cd, t);
        }

        public static Vector3 ArcPointXZ(
            Vector3 start,
            Vector3 target,
            float t,
            float phi
        )
        {
            var basePoint = Vector3.Lerp(start, target, t);

            var dir = target - start;
            dir.y = 0f;

            var length = dir.magnitude;
            if (length < 0.0001f)
                return start;

            dir /= length;

            var normal = new Vector3(-dir.z, 0f, dir.x);

            var maxOffset = length * Mathf.Tan(phi) * 0.5f;

            var height = Mathf.Sin(Mathf.PI * t) * maxOffset;

            return basePoint + normal * height;
        }
        
        public static Vector3 Quantize(this Vector3 v, float q = 0.001f)
        {
            return new Vector3(
                Mathf.Round(v.x / q) * q,
                Mathf.Round(v.y / q) * q,
                Mathf.Round(v.z / q) * q
            );
        }

        public static Vector3 Abs(Vector3 vector)
        {
            return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }
    }
}