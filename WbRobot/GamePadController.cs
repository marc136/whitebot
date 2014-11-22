using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BLETest
{
    public class ButtonEventArgs : EventArgs
    {
        public Buttons Button { get; private set; }

        public ButtonEventArgs(Buttons button)
        {
            Button = button;
        }
    }

    class GamePadController
    {
        Timer timer;

        Robot[] robots;
        const int interval = 50;

        public event EventHandler<ButtonEventArgs> ButtonPressed;

        private void OnButtonPressed(ButtonEventArgs e)
        {
            if (ButtonPressed != null)
            {
                ButtonPressed(this, e);
            }
        }

        public event EventHandler<ButtonEventArgs> ButtonReleased;

        private void OnButtonReleased(ButtonEventArgs e)
        {
            if (ButtonReleased != null)
            {
                ButtonReleased(this, e);
            }
        }

        public GamePadController(params Robot[] robots)
        {
            this.robots = robots;
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = true;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        bool connected = true;

        double nonLinear(double value)
        {
            if (value > 0)
            {
                return 1 - Math.Cos(value * Math.PI / 2);
            }
            else
            {
                return Math.Cos(value * Math.PI / 2) - 1;
            }

        }

        GamePadState previousState;

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var state = GamePad.GetState(PlayerIndex.One);
            if (state.IsConnected)
            {
                timer.Interval = interval;
                handleButtons(robots[0],
                    state.IsButtonDown(Buttons.LeftShoulder),
                    state.IsButtonDown(Buttons.RightShoulder),
                    state.IsButtonDown(Buttons.X),
                    state.IsButtonDown(Buttons.A),
                    state.IsButtonDown(Buttons.Start));
                if(controlsMovement)
                {
                    handleMovement(robots[0], state.ThumbSticks.Right);
                    handleMovement(robots[1], state.ThumbSticks.Left);
                }

                connected = true;
            }
            else
            {
                if (connected)
                {
                    timer.Interval = 2000;
                    if (robots[0] != null)
                    {
                        robots[0].speed(0, 0);
                    }
                    if (robots[1] != null)
                    {
                        robots[1].speed(0, 0);
                    }

                    connected = false;
                }

            }
            //Console.WriteLine(state.IsConnected);
            for (int i = 0; i < 25; ++i )
            {
                int id = 1 << i;
                if(previousState.IsButtonUp((Buttons)id) && state.IsButtonDown((Buttons)id))
                {
                    OnButtonPressed(new ButtonEventArgs((Buttons)id));
                }
                else if(previousState.IsButtonDown((Buttons)id) && state.IsButtonUp((Buttons)id))
                {
                    OnButtonReleased(new ButtonEventArgs((Buttons)id));
                }
            }
            previousState = state;
        }

        bool oldPen = false;
        bool oldEraser = false;
        bool oldX = false;
        bool oldA = false;
        bool oldStart = false;
        bool controlsMovement = false;

        private void handleButtons(Robot robot, bool pen, bool eraser, bool x, bool a, bool start)
        {
            if (oldPen != pen)
            {
                oldPen = pen;
                if (pen)
                {
                    robot.penDown();
                }
                else
                {
                    robot.penUp();
                }
            }
            if (oldEraser != eraser)
            {
                oldEraser = eraser;
                if (eraser)
                {
                    robot.eraserDown();
                }
                else
                {
                    robot.eraserUp();
                }
            }
            if (oldX != x)
            {
                oldX = x;
                if (x)
                {
                    robot.penRelease();
                    robot.eraserRelease();
                }
            }
            if (oldStart != start)
            {
                oldStart = start;
                if (start)
                {
                    controlsMovement = controlsMovement ? false : true;
                    if(!controlsMovement)
                    {
                        foreach(var r in robots)
                        {
                            if(r != null)
                            {
                                r.speed(0, 0);
                            }
                        }
                    }
                }
            }
        }

        void handleMovement(Robot robot, Vector2 vector)
        {
           /* if (robot == null)
            {
                return;
            }*/
           /* if(robot.Id == 0)
            {
                Console.Write("X: " + vector.X + "\tY: " + vector.Y + "\tlength: " + vector.Length());
            }*/
            
            if (vector.Length() < 0.0001)
            {
               /* if (robot.Id == 0)
                {
                    Console.WriteLine("foooo x:" + vector.X + "\ty:" + vector.Y + "\tvector length:" + vector.Length());
                }*/
                robot.speed(0, 0);
            }
            else
            {
                //Console.WriteLine("X: " + vector.X + "\tY: " + vector.Y);
                robot.speed(nonLinear(vector.Y) * 200, nonLinear(vector.X) * -1000);
            }
        }
    }
}
