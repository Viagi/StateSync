using PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class MapGridComponentAwakeSystem : AwakeSystem<MapGridComponent>
    {
        public override void Awake(MapGridComponent self)
        {
            self.Awake();
        }
    }

    public class MapGridComponent : Component
    {
        public PathReturnQueue PathReturnQueue { get; private set; }
        public PathProcessor PathProcessor { get; private set; }
        public AStarConfig AStarConfig { get; private set; }

        public void Awake()
        {
            this.PathReturnQueue = new PathReturnQueue(this);
            this.PathProcessor = new PathProcessor(this.PathReturnQueue, 1, false);
            this.AStarConfig = new AStarConfig();
            this.AStarConfig.pathProcessor = this.PathProcessor;

            LoadGraph();
        }

        private void LoadGraph()
        {
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle("graph.unity3d");
            GameObject graphRef = resourcesComponent.GetAsset("graph.unity3d", "Graph") as GameObject;
            TextAsset graph = graphRef.Get<TextAsset>("Graph");
            this.AStarConfig.graphs = DeserializeHelper.Load(graph.bytes);
        }
    }
}
