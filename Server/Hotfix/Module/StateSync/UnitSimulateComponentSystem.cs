using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UnitSimulateComponentAwakeSystem : AwakeSystem<UnitSimulateComponent>
    {
        public override void Awake(UnitSimulateComponent self)
        {
            self.Awake();
        }
    }

    public static class UnitSimulateComponentSystem
    {
        public static void Awake(this UnitSimulateComponent self)
        {
            self.SimulateQueue = new Queue<StateCommand>();
            self.ExecuteQueue = new Queue<StateCommand>();

            self.Simulate();
        }

        public static void Input(this UnitSimulateComponent self, Command command)
        {
            self.SimulateQueue.Enqueue(command.ToState());
        }

        private static async void Simulate(this UnitSimulateComponent self)
        {
            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();

            while (!self.IsDisposed)
            {
                self.Frame++;
                self.OnSimulateBefore();
                self.OnSimulateInput();
                self.OnSimulateInertance();
                self.OnSimulateAfter();

                await timerComponent.WaitAsync(UnitStateComponent.UpdateDelay);
            }
        }

        private static void OnSimulateBefore(this UnitSimulateComponent self)
        {

        }

        private static void OnSimulateAfter(this UnitSimulateComponent self)
        {
            ActorMessageSenderComponent actorLocationSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
            UnitGateComponent unitGateComponent = self.Entity.GetComponent<UnitGateComponent>();
            ActorMessageSender actorMessageSender = actorLocationSenderComponent.Get(unitGateComponent.GateSessionActorId);

            if (unitGateComponent.IsDisconnect)
            {
                return;
            }

            if (self.ExecuteQueue.Count > 0)
            {
                Actor_ServerCommond serverCommond = new Actor_ServerCommond();
                while (self.ExecuteQueue.Count > 0)
                {
                    StateCommand stateCommand = self.ExecuteQueue.Dequeue();
                    serverCommond.Result.Add(stateCommand.ToCommand());
                }

                actorMessageSender.Send(serverCommond);
            }

            if (self.Frame % UnitStateComponent.SyncFrame == 1)
            {
                Actor_StateSync stateSync = new Actor_StateSync();

                Unit[] units = Game.Scene.GetComponent<UnitComponent>().GetAll();
                foreach (Unit unit in units)
                {
                    UnitStateComponent unitStateComponent = unit.GetComponent<UnitStateComponent>();
                    UnitState state = unitStateComponent.State;
                    UnitStateInfo info = state.Pack(unit.Id, state.Frame);
                    stateSync.States.Add(info);
                }

                actorMessageSender.Send(stateSync);
            }
        }

        private static void OnSimulateInput(this UnitSimulateComponent self)
        {
            while (self.SimulateQueue.Count > 0)
            {
                StateCommand command = self.SimulateQueue.Dequeue();
                self.ExecuteCommand(command);
                self.ExecuteQueue.Enqueue(command);
            }
        }

        private static void OnSimulateInertance(this UnitSimulateComponent self)
        {
            UnitStateComponent unitStateComponent = self.Entity.GetComponent<UnitStateComponent>();
            unitStateComponent.State = unitStateComponent.State.Inertance(1);
        }

        private static void ExecuteCommand(this UnitSimulateComponent self, StateCommand command)
        {
            UnitStateComponent unitStateComponent = self.Entity.GetComponent<UnitStateComponent>();
            UnitState newState = unitStateComponent.State;

            newState.Rotate = self.CalculaAngle(command.input.axisX, command.input.axisY);
            newState.Velocity = VelocityConst.Accelerate;

            unitStateComponent.State = newState;

            StateResult result = new StateResult();
            result.rotate = newState.Rotate;
            result.velocity = newState.Velocity;

            command.result = result;
            command.frame = newState.Frame;
        }

        private static float CalculaAngle(this UnitSimulateComponent self, float x, float y)
        {
            return Quaternion.LookRotation(new Vector3(x, 0, y), Vector3.up).eulerAngles.y;
        }
    }
}
