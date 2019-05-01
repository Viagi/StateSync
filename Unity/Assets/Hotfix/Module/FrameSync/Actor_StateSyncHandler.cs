using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_StateSyncHandler : AMHandler<Actor_StateSync>
    {
        protected override void Run(ETModel.Session session, Actor_StateSync message)
        {
            UnitComponent unitComponent = ETModel.Game.Scene.GetComponent<UnitComponent>();
            foreach (UnitStateInfo stateInfo in message.States)
            {
                Unit unit = unitComponent.Get(stateInfo.UnitId);
                if(unit == null)
                {
                    continue;
                }

                UnitState state = stateInfo.Read();

                if (stateInfo.UnitId == PlayerComponent.Instance.MyPlayer.UnitId)
                {
                    ClientPredictionComponent clientPredictionComponent = ETModel.Game.Scene.GetComponent<ClientPredictionComponent>();
                    clientPredictionComponent.SetVerifyState(state);
                }
                else
                {
                    UnitStateComponent unitStateComponent = unit.GetComponent<UnitStateComponent>();
                    unitStateComponent.SetState(state);
                }
            }
        }
    }
}
