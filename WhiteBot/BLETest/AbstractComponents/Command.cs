using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhiteBot
{
    public abstract class ACommand
    {
        public ACommand(RobotState commandType)
        {
            this.CommandType = commandType;
        }

        public RobotState CommandType { get; private set; }

        public override string ToString()
        {
            return CommandType.ToString("G");
        }
    }

    public abstract class DirectionalCommand : ACommand
    {
        public DirectionalCommand(Vector2 vector, RobotState commandType) : base(commandType)
        {
            this.Vector = vector;
        }

        public Vector2 Vector { get; private set; }

        public override string ToString()
        {
            return String.Format("{2}(x:{0}|y:{1})", Vector.X, Vector.Y, base.ToString());
        }
    }

    public class MoveToCommand : DirectionalCommand
    {
        public MoveToCommand(Vector2 target) : base(target, RobotState.MoveTo) {}
    }

    public class LookAtCommand : DirectionalCommand
    {
        public LookAtCommand(Vector2 target) : base(target, RobotState.LookAt) {}
    }


    public class LookInDirectionCommand : DirectionalCommand
    {
        public LookInDirectionCommand(Vector2 direction) : base(direction, RobotState.LookInDirection) {}
    }

    public class PenUpCommand : ACommand
    {
        public PenUpCommand() : base(RobotState.PenUp) { }

        public override string ToString()
        {
            return "Pen up";
        }
    }

    public class PenDownCommand : ACommand
    {
        public PenDownCommand() : base(RobotState.PenDown) { }

        public override string ToString()
        {
            return "Pen down";
        }
    }

    public class EraserUpCommand : ACommand
    {
        public EraserUpCommand() : base(RobotState.EraserUp) { }

        public override string ToString()
        {
            return "Eraser up";
        }
    }

    public class EraserDownCommand : ACommand
    {
        public EraserDownCommand() : base(RobotState.EraserDown) { }

        public override string ToString()
        {
            return "Eraser Down";
        }
    }

    public class StopCommand : ACommand
    {
        public StopCommand() : base(RobotState.Stop) { }
    }
}
