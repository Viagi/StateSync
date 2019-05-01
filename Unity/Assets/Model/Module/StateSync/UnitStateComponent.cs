using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class UnitStateComponentAwakeSystem : AwakeSystem<UnitStateComponent>
    {
        public override void Awake(UnitStateComponent self)
        {
            self.Awake();
        }
    }

    public class UnitStateComponent : Component
    {
        public const int UpdateDelay = 16;
        public const int SyncFrame = 6;
        public const int DelayFrame = 6;
        public const int MaxDiffFrame = SyncFrame * 10;
        public const float DeltaTime = (float)UpdateDelay / 1000;

        public List<UnitState> ServerStates { get; set; }
        public UnitState State { get; set; }

        public uint ServerFrame
        {
            get
            {
                return this.frame - DelayFrame;
            }
        }

        private uint frame;
        private uint updateSpeed;

        public void Awake()
        {
            this.ServerStates = new List<UnitState>();
            this.updateSpeed = 1;
        }

        public void SetState(UnitState state)
        {
            int diffFrame = (int)state.Frame - (int)this.frame;

            this.ServerStates.Add(state);

            if (this.frame == 0)
            {
                this.frame = state.Frame;
                Update();
            }
            else if (Math.Abs(diffFrame) >= MaxDiffFrame)
            {
                this.frame = state.Frame;
            }
        }

        public UnitState GetServerState()
        {
            if (this.ServerStates.Count > 0)
            {
                return this.ServerStates[this.ServerStates.Count - 1];
            }
            else
            {
                return new UnitState();
            }
        }

        private async void Update()
        {
            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();

            while (!this.IsDisposed)
            {
                for (int i = this.ServerStates.Count - 1; i >= 0; i--)
                {
                    if (this.ServerStates[i].Frame <= this.ServerFrame)
                    {
                        UnitState stateA = this.ServerStates[i];
                        if (i == this.ServerStates.Count - 1)
                        {
                            this.State = stateA;
                        }
                        else
                        {
                            UnitState stateB = this.ServerStates[i + 1];
                            float lerp = (float)(this.ServerFrame - stateA.Frame) / (stateB.Frame - stateA.Frame);
                            this.State = stateA.Lerp(stateB, lerp, this.ServerFrame);
                        }
                        break;
                    }
                }

                AdjustUpdateSpeed();
                this.frame += updateSpeed;
                await timerComponent.WaitAsync(UpdateDelay);
            }
        }

        private void AdjustUpdateSpeed()
        {
            int diffFrame = (int)GetServerState().Frame - (int)this.frame;

            if (diffFrame < 0)
            {
                this.updateSpeed = 0;
            }
            else
            {
                this.updateSpeed = (uint)(1 + (diffFrame / SyncFrame));
            }
        }
    }
}
