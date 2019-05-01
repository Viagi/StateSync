namespace ETModel
{
    public static class VelocityConst
    {
        public const float Accelerate = 5.0f * UnitStateComponent.DeltaTime;
        public const float Deceleration = Accelerate / UnitStateComponent.SyncFrame;
    }
}
