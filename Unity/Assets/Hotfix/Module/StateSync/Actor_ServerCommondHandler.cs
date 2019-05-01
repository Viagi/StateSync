using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_ServerCommondHandler : AMHandler<Actor_ServerCommond>
    {
        protected override void Run(ETModel.Session session, Actor_ServerCommond message)
        {
            ClientPredictionComponent clientPredictionComponent = ETModel.Game.Scene.GetComponent<ClientPredictionComponent>();

            foreach (Command command in message.Result)
            {
                clientPredictionComponent.Verify(command);
            }
        }
    }
}
