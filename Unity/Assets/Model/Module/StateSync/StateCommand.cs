namespace ETModel
{
    /// <summary>
    /// 状态命令Flags
    /// </summary>
    public enum StateCommandFlags
    {
        NOT_EXECUTED = 0,       //尚未执行
        HAS_EXECUTED = 1,       //已经执行
        VERIFIED = 2            //已经验证
    }

    public class StateCommand
    {
        public StateCommandFlags flags;
        public uint frame;                  //模拟帧
        public uint sequence;               //指令序号
        public StateInput input;            //操作指令的输入
        public StateResult result;          //操作指令执行后得到的结果
    }

    public struct StateInput
    {
        public float axisX;
        public float axisY;
    }

    public struct StateResult
    {
        public float rotate;
        public float velocity;
    }

    public static class StateCommandUtility
    {
        public static Command ToCommand(this StateCommand stateCommand)
        {
            Command command = new Command();
            command.Frame = stateCommand.frame;
            command.Sequence = stateCommand.sequence;
            command.Input = new CommandInput
            {
                AxisX = stateCommand.input.axisX,
                AxisY = stateCommand.input.axisY
            };
            command.Result = new CommandResult
            {
                Rotate = stateCommand.result.rotate,
                Velocity = stateCommand.result.velocity
            };

            return command;
        }

        public static StateCommand ToState(this Command command)
        {
            StateCommand stateCommand = new StateCommand();
            stateCommand.frame = command.Frame;
            stateCommand.sequence = command.Sequence;
            stateCommand.input = new StateInput
            {
                axisX = command.Input.AxisX,
                axisY = command.Input.AxisY
            };
            stateCommand.result = new StateResult
            {
                rotate = command.Result.Rotate,
                velocity = command.Result.Velocity
            };

            return stateCommand;
        }
    }
}
