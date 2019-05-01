using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UnitStateComponentAwakeSystem : AwakeSystem<UnitStateComponent>
    {
        public override void Awake(UnitStateComponent self)
        {
            self.Awake();
        }
    }

    public static class UnitStateComponentSystem
    {
        public static void Awake(this UnitStateComponent self)
        {
            UnitState defaultState = new UnitState();
            defaultState.Position = new Vector3(24, 0, -13);
            defaultState.Rotate = 0;
            defaultState.Velocity = 0;
            self.State = defaultState;
        }
    }
}
