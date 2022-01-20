namespace Systems
{
    using System.Collections.Generic;
    using ComponentsData;
    using Unity.Entities;
    using Unity.Mathematics;

    public class GenerateMeshSystem : SystemBase
    {
        public static EntityArchetype triangleArchetype;
        public static EntityArchetype edgeArchetype;
        public static EntityArchetype nodeArchetype;

        public static int nodeId = 0;
        public static int edgesId = 0;
        public float3[] positions;
        public int[][] edges;
        public Dictionary<int, Entity> lookupNodes;
        public Dictionary<int, Entity> lookupEdges;

        protected override void OnCreate()
        {
            float phi = ((1.0f + math.sqrt(5.0f)) / 2.0f);
            float du = (1.0f / math.sqrt(phi * phi + 1.0f));
            float dv = phi * du;

            triangleArchetype = EntityManager.CreateArchetype(typeof(TriangleData), typeof(ListEdgeData), typeof(ListNodeData));
            edgeArchetype = EntityManager.CreateArchetype(typeof(EdgeData), typeof(ListNodeData));
            nodeArchetype = EntityManager.CreateArchetype(typeof(NodeData));

            positions = new[] { new float3(0, +dv, +du), new float3(0, +dv, -du), new float3(0, -dv, +du), new float3(0, -dv, -du), new float3(+du, 0, +dv), new float3(-du, 0, +dv), new float3(+du, 0, -dv), new float3(-du, 0, -dv), new float3(+dv, +du, 0), new float3(+dv, -du, 0), new float3(-dv, +du, 0), new float3(-dv, -du, 0) };
            edges = new[] { new[] { 0, 1 }, new[] { 0, 4 }, new[] { 0, 5 }, new[] { 0, 8 }, new[] { 0, 10 }, new[] { 1, 6 }, new[] { 1, 7 }, new[] { 1, 8 }, new[] { 1, 10 }, new[] { 2, 3 }, new[] { 2, 4 }, new[] { 2, 5 }, new[] { 2, 9 }, new[] { 2, 11 }, new[] { 3, 6 }, new[] { 3, 7 }, new[] { 3, 9 }, new[] { 3, 11 }, new[] { 4, 5 }, new[] { 4, 8 }, new[] { 4, 9 }, new[] { 5, 10 }, new[] { 5, 11 }, new[] { 6, 7 }, new[] { 6, 8 }, new[] { 6, 9 }, new[] { 7, 10 }, new[] { 7, 11 }, new[] { 8, 9 }, new[] { 10, 11 }, };

            CreateNodes();
            CreateEdges();

            Enabled = false;
        }

        protected override void OnUpdate() {}

        private void CreateNodes()
        {
            foreach (float3 value in positions)
            {
                lookupNodes[nodeId] = EntityManager.CreateEntity(nodeArchetype);
                EntityManager.SetComponentData(lookupNodes[nodeId], new NodeData { id = nodeId++, position = value });
            }
        }

        private void CreateEdges()
        {
            foreach (int[] value in edges)
            {
                lookupEdges[edgesId] = EntityManager.CreateEntity(edgeArchetype);
                EntityManager.SetComponentData(lookupEdges[edgesId], new EdgeData { id = edgesId++ });
                EntityManager.SetComponentData(lookupEdges[edgesId], new ListNodeData { nodes = new [] { lookupNodes[value[0]], lookupNodes[value[1]] } });
            }
        }
    }
}
