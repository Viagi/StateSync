using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public class UnitStateComponent : Component
    {
        public const int UpdateDelay = 16;
        public const int SyncFrame = 6;
        public const float DeltaTime = (float)UpdateDelay / 1000;

        public UnitState State { get; set; }
    }
}
