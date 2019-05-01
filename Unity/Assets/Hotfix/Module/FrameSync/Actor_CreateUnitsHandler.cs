using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_CreateUnitsHandler : AMHandler<Actor_CreateUnits>
    {
        protected override void Run(ETModel.Session session, Actor_CreateUnits message)
        {
            // 加载Unit资源
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"Unit.unity3d");

            UnitComponent unitComponent = ETModel.Game.Scene.GetComponent<UnitComponent>();

            foreach (UnitInfo unitInfo in message.Units)
            {
                if (unitComponent.Get(unitInfo.UnitId) != null)
                {
                    continue;
                }
                Unit unit = ETModel.ComponentFactory.CreateWithId<Unit>(unitInfo.UnitId);
                unitComponent.Add(unit);
                //Unit unit = UnitFactory.Create(unitInfo.UnitId);
                //unit.Position = new Vector3(unitInfo.X / 1000f, 0, unitInfo.Z / 1000f);
                //unit.IntPos = new VInt3(unitInfo.X, 0, unitInfo.Z);

                if (PlayerComponent.Instance.MyPlayer.Id == unit.Id)
                {
                    unit.AddComponent<UnitEntityComponent, bool>(true);
                    ETModel.Game.Scene.AddComponent<MapGridComponent>();
                }
                else
                {
                    unit.AddComponent<UnitStateComponent>();
                    unit.AddComponent<UnitEntityComponent, bool>(false);
                }
            }

            //Game.Scene.AddComponent<OperaComponent>();
        }
    }
}
