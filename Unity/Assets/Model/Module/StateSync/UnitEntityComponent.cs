using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class UnitEntityComponentAwakeSystem : AwakeSystem<UnitEntityComponent, bool>
    {
        public override void Awake(UnitEntityComponent self, bool isLocal)
        {
            self.Awake(isLocal);
        }
    }

    [ObjectSystem]
    public class UnitEntityComponentUpdateSystem : UpdateSystem<UnitEntityComponent>
    {
        public override void Update(UnitEntityComponent self)
        {
            self.Update();
        }
    }

    public class UnitEntityComponent : Component
    {
        public UnitState State { get; set; }
        public GameObject Instance { get; set; }

        private Animator anim;
        private bool isLocal;

        public void Awake(bool isLocal)
        {
            this.isLocal = isLocal;
        }

        public void Update()
        {
            ClientPredictionComponent clientPredictionComponent = Game.Scene.GetComponent<ClientPredictionComponent>();
            UnitStateComponent unitStateComponent = this.Entity.GetComponent<UnitStateComponent>();
            UnitState serverState;

            if (this.isLocal)
            {
                serverState = clientPredictionComponent.VerifyState;
            }
            else
            {
                //serverState = unitStateComponent.GetServerState();
                serverState = unitStateComponent.State;
            }

            if (this.Instance == null && serverState.Frame > 0)
            {
                ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset("Unit.unity3d", "Unit");
                GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");
                GameObject instance = UnityEngine.Object.Instantiate(prefab);
                GameObject parent = GameObject.Find($"/Global/Unit");

                instance.transform.SetParent(parent.transform, false);

                this.Instance = instance;
                this.anim = instance.GetComponent<Animator>();

                if (this.isLocal)
                {
                    ETModel.Game.Scene.AddComponent<CameraComponent>().Unit = this.Instance;
                }
            }
            else if (this.Instance != null)
            {
                if (this.isLocal)
                {
                    this.State = clientPredictionComponent.PredictedState;
                }
                else
                {
                    this.State = unitStateComponent.State;
                }

                UpdateInstance();
            }
        }

        private void UpdateInstance()
        {
            this.Instance.transform.position = this.State.Position;
            this.Instance.transform.rotation = this.State.Rotation;

            if (this.State.Velocity > 0)
            {
                this.anim.SetFloat("Speed", 1);
            }
            else
            {
                this.anim.SetFloat("Speed", 0);
            }
        }
    }
}
