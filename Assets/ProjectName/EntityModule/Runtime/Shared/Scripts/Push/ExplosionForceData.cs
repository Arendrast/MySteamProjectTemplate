namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push
{
    public readonly struct ExplosionForceData
    {
        public readonly float ForceAtCenter;
        public readonly float ForceAtEdge;
        public readonly float Radius;

        public ExplosionForceData(float forceAtCenter, float forceAtEdge, float radius)
        {
            ForceAtCenter = forceAtCenter;
            ForceAtEdge = forceAtEdge;
            Radius = radius;
        }
    }
}