using PF;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ETModel
{
    public struct UnitState
    {
        public uint Frame;
        public Vector3 Position;
        public float Rotate;
        public float Velocity;

        public Quaternion Rotation
        {
            get
            {
                return Quaternion.Euler(0, this.Rotate, 0);
            }
        }

        public Vector3 Forward
        {
            get
            {
                return this.Rotation * Vector3.forward;
            }
        }
    }

    public static class UnitStateExtension
    {
        public static UnitStateInfo Pack(this UnitState state, long id, uint frame)
        {
            UnitStateInfo info = new UnitStateInfo();
            info.UnitId = id;
            info.Frame = frame;
            info.PosX = state.Position.x;
            info.PosY = state.Position.y;
            info.PosZ = state.Position.z;
            info.Rotate = state.Rotate;
            info.Velocity = state.Velocity;

            return info;
        }

        public static UnitState Read(this UnitStateInfo info)
        {
            UnitState newState = new UnitState();
            newState.Frame = info.Frame;
            newState.Position = new Vector3(info.PosX, info.PosY, info.PosZ);
            newState.Rotate = info.Rotate;
            newState.Velocity = info.Velocity;

            return newState;
        }

        public static UnitState Lerp(this UnitState stateA, UnitState stateB, float lerp, uint frame)
        {
            UnitState newState = new UnitState();
            newState.Frame = frame;
            newState.Position = Vector3.Lerp(stateA.Position, stateB.Position, lerp);
            newState.Rotate = LerpRotate(stateA.Rotate, stateB.Rotate, lerp);
            newState.Velocity = Mathf.Lerp(stateA.Velocity, stateB.Velocity, lerp);

            return newState;
        }

        public static UnitState Inertance(this UnitState state, uint inertialFrame)
        {
            if (inertialFrame > 10000)
            {
                Log.Trace($"模拟帧异常{inertialFrame}");
                return state;
            }

            state.Frame += inertialFrame;
            for (int i = 0; i < inertialFrame; i++)
            {
                if (state.Velocity == 0)
                {
                    continue;
                }

                Vector3 move = state.Forward * state.Velocity;
                NNInfo mapNodeInfo = PathFindHelper.GetNearest(state.Position + move);

                state.Position = mapNodeInfo.position;

                if (state.Velocity >= VelocityConst.Deceleration)
                {
                    state.Velocity -= VelocityConst.Deceleration;
                }
                else
                {
                    state.Velocity = 0;
                }
            }

            return state;
        }

        public static float LerpRotate(float rotateA, float rotateB, float lerp)
        {
            float lerpRotate;

            if (rotateB - rotateA > 180)
            {
                rotateA += 360;
            }
            else if (rotateB - rotateA < -180)
            {
                rotateB += 360;
            }

            lerpRotate = Mathf.Lerp(rotateA, rotateB, lerp);

            return lerpRotate % 360;
        }
    }
}
