using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class Actor_ClientCommondHandler : AMActorLocationHandler<Unit, Actor_ClientCommond>
    {
        protected override void Run(Unit entity, Actor_ClientCommond message)
        {
            UnitSimulateComponent unitSimulateComponent = entity.GetComponent<UnitSimulateComponent>();
            foreach (Command command in message.Input)
            {
                unitSimulateComponent.Input(command);
            }
        }
    }
}
