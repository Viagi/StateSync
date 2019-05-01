using PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class UnitSimulateComponent : Component
    {
        public uint Frame;
        public Queue<StateCommand> SimulateQueue;
        public Queue<StateCommand> ExecuteQueue;
    }
}
