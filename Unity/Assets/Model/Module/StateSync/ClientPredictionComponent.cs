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
    public class ClientPredictionComponentAwakeSystem : AwakeSystem<ClientPredictionComponent>
    {
        public override void Awake(ClientPredictionComponent self)
        {
            self.Awake();
        }
    }

    public class ClientPredictionComponent : Component
    {
        public uint Sequence;
        public LinkedList<StateCommand> SimulateQueue;
        public UnitState VerifyState;
        public UnitState PredictedState;

        private uint lastSimulateFrame;
        private bool isSimulate;

        public void Awake()
        {
            this.SimulateQueue = new LinkedList<StateCommand>();
        }

        public void SetVerifyState(UnitState state)
        {
            this.VerifyState = state;

            if (!isSimulate)
            {
                this.isSimulate = true;
                Simulate();
            }
        }

        public void Verify(Command command)
        {
            StateCommand simulate = this.SimulateQueue.First.Value;

            simulate.result.rotate = command.Result.Rotate;
            simulate.result.velocity = command.Result.Velocity;

            this.lastSimulateFrame = simulate.frame;
            this.VerifyState = this.VerifyState.Inertance(command.Frame - this.VerifyState.Frame);
            this.VerifyState = ExecuteResult(this.VerifyState, simulate);
            this.SimulateQueue.RemoveFirst();
        }

        private async void Simulate()
        {
            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();

            while (!this.IsDisposed)
            {
                OnSimulateBefore();
                OnSimulateInput();
                OnSimulateAfter();
                OnSimulateInertance();
                await timerComponent.WaitAsync(UnitStateComponent.UpdateDelay);
            }
        }

        private void OnSimulateInput()
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                StateCommand inputCommand = CollectCommandInput();
                Actor_ClientCommond clientCommond = new Actor_ClientCommond();
                clientCommond.Input.Add(inputCommand.ToCommand());
                SessionComponent.Instance.Session.Send(clientCommond);
                this.SimulateQueue.AddLast(inputCommand);
            }
        }

        private void OnSimulateInertance()
        {
            this.PredictedState = this.PredictedState.Inertance(1);
        }

        private void OnSimulateBefore()
        {
            this.PredictedState = this.VerifyState;

            if (this.SimulateQueue.Count > 0 && this.SimulateQueue.First.Value.frame - this.lastSimulateFrame > 10000)
            {
                Log.Debug($"回滚预测惯性错误");
            }

            foreach (StateCommand command in this.SimulateQueue)
            {
                this.PredictedState = this.PredictedState.Inertance(command.frame - this.lastSimulateFrame);
                this.lastSimulateFrame = command.frame;

                command.frame = this.PredictedState.Frame;
                command.result = ExecuteInput(this.PredictedState, command);
                this.PredictedState = ExecuteResult(this.PredictedState, command);
                command.flags = StateCommandFlags.HAS_EXECUTED;
            }
        }

        private void OnSimulateAfter()
        {
            foreach (StateCommand command in this.SimulateQueue)
            {
                if (command.flags == StateCommandFlags.NOT_EXECUTED)
                {
                    command.result = ExecuteInput(this.PredictedState, command);
                    this.PredictedState = ExecuteResult(this.PredictedState, command);
                    command.flags = StateCommandFlags.HAS_EXECUTED;
                }
            }
        }

        private StateCommand CollectCommandInput()
        {
            StateCommand stateCommand = new StateCommand();
            stateCommand.frame = this.PredictedState.Frame;
            stateCommand.sequence = ++this.Sequence;
            stateCommand.input = new StateInput();
            stateCommand.input.axisX = Input.GetAxis("Horizontal");
            stateCommand.input.axisY = Input.GetAxis("Vertical");

            return stateCommand;
        }

        private StateResult ExecuteInput(UnitState state, StateCommand command)
        {
            state.Rotate = CalculaAngle(command.input.axisX, command.input.axisY);
            state.Velocity = VelocityConst.Accelerate;

            StateResult result = new StateResult();
            result.rotate = state.Rotate;
            result.velocity = state.Velocity;

            return result;
        }

        private UnitState ExecuteResult(UnitState state, StateCommand command)
        {
            if (this.lastSimulateFrame == 0)
            {
                this.lastSimulateFrame = command.frame;
            }

            state.Rotate = command.result.rotate;
            state.Velocity = command.result.velocity;

            return state;
        }

        private float CalculaAngle(float x, float y)
        {
            return Quaternion.LookRotation(new Vector3(x, 0, y), Vector3.up).eulerAngles.y;
        }
    }
}
