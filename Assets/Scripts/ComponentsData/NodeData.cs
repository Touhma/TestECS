namespace ComponentsData
{
    using Unity.Entities;
    using Unity.Mathematics;

    public struct NodeData : IComponentData
    {
        public int id;
        public float3 position;
    }
}
