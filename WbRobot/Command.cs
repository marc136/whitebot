using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLETest
{
    public interface ICommand
    {
        void apply(RobotController controller);
    }

    class MoveToCommand : ICommand
    {
        Vector2 target;

        public MoveToCommand(Vector2 target)
        {
            this.target = target;
        }

        public void apply(RobotController controller)
        {
            controller.moveTo(target);
        }
    }

    class LookAtCommand : ICommand
    {
        Vector2 target;

        public LookAtCommand(Vector2 target)
        {
            this.target = target;
        }

        public void apply(RobotController controller)
        {
            controller.lookAt(target);
        }
    }


    class LookInDirectionCommand : ICommand
    {
        Vector2 direction;

        public LookInDirectionCommand(Vector2 direction)
        {
            this.direction = direction;
        }

        public void apply(RobotController controller)
        {
            controller.lookInDirection(direction);
        }
    }

    class PenUpCommand : ICommand
    {
        public PenUpCommand()
        {
        }

        public void apply(RobotController controller)
        {
            controller.penUp();
        }
    }

    class PenDownCommand : ICommand
    {
        public PenDownCommand()
        {
        }

        public void apply(RobotController controller)
        {
            controller.penDown();
        }
    }
    class EraserUpCommand : ICommand
    {
        public EraserUpCommand()
        {
        }

        public void apply(RobotController controller)
        {
            controller.eraserUp();
        }
    }
    class EraserDownCommand : ICommand
    {
        public EraserDownCommand()
        {
        }

        public void apply(RobotController controller)
        {
            controller.eraserDown();
        }
    }
    class StopCommand : ICommand
    {
        public StopCommand()
        {
        }

        public void apply(RobotController controller)
        {
            controller.stop();
        }
    }
}
