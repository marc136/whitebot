using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BLETest
{

    public enum RobotControllerType { PidController, MLController }

    /// <summary>
    /// Any class deriving from this should override the tick-function (and call base.Timer_Elapsed)
        /// protected void OnTick()
        /// public void InitializeController()
    /// </summary>
    public abstract class AbstractRobotController
    {
        protected string name = "Abstract";
        protected Robot robot;
        protected Timer timer;
        protected bool Paused { get; set; }

        protected RobotState _state = RobotState.Idle;
        public RobotState State { get { return _state; } protected set { _state = value; } }

        protected Vector2 targetPoint;
        protected Vector2 startPoint;

        protected int waitCount = 0;

        public AbstractRobotController(Robot robot)
        {
            State = RobotState.Idle;
            Paused = false;

            this.robot = robot;
            //robot.Log += LogRobot;
            timer = new System.Timers.Timer(50);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
        }

        #region events
        public event EventHandler<FinishedEventArgs> Finished;
        void OnFinished(FinishedEventArgs e)
        {
            if (Finished != null)
            {
                Finished(this, e);
            }
        }

        public event Action<string> OnLogController;
        protected void LogController(string message)
        {
            if (OnLogController != null)
            {
                OnLogController("RobotController"+this.name+"\t" + message);
            }
        }
        #endregion

        /// <summary>
        /// This may be overriden (the Machine Learning Controller uses this function to learn how to move
        /// </summary>
        public virtual void InitializeController() 
        {
        }

        public void NextState()
        {
            var lastState = State;
            State = RobotState.Finished;
            OnFinished(new FinishedEventArgs(lastState));
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var paused = false;

            if (!robot.KnowsPosition && State != RobotState.Idle)
            {
                if (!Paused) LogController("Paused(reason: position is unknown)");
                //State = RobotState.Stop;
                paused = true;
            }

            if (robot.Alignment == Alignment.Unknown && State != RobotState.Idle)
            {
                if (!Paused) LogController("Paused(reason: alignment is unknown)");
                //State = RobotState.Stop;
                paused = true;
            }

            if (tickLogger != null)
            {
                var humanReadableOutput = false;
                if (humanReadableOutput) 
                {
                    tickLogger.Log(String.Format("position({0})\tlookDirection({1})\tgravity({2})\torientation({3})",
                                    robot.Position.ToString(), robot.LookDirection, robot.Gravity.vec, robot.Orientation.GetDirectionVector()));
                }
                else
                {
                    var dirVec = robot.Orientation.GetDirectionVector();
                    tickLogger.Log(robot.Position.X + "\t" + robot.Position.Y + "\t" +
                        robot.LookDirection.X + "\t" + robot.LookDirection.Y + "\t" +
                        robot.Gravity.vec.X + "\t" + robot.Gravity.vec.Y + "\t" + robot.Gravity.vec.Z + "\t" + 
                        dirVec.X + "\t" + dirVec.Y + "\t" + dirVec.Z + "\t"
                        );
                }
            }

            if (!paused)
            {
                if (Paused) Paused = false;

                OnTick();
            }
        }

        /// <summary>
        /// This is the function where the derived controllers execute their code after every tick
        /// Call base.OnTick() in the deriving class to handle simple Commands like Stop, Finished, Idle, 
        /// and also Pen and Eraser Movement
        /// But the States RotateTo, LookInDirection, LookAt, 
        /// </summary>
        protected virtual void OnTick()
        {
            switch (State)
            {
                case RobotState.Stop:
                    LogController("Stop");
                    Speed(0, 0);
                    NextState();
                    break;
                case RobotState.Finished:
                    LogController("Stop(reason: path was finished)");
                    Speed(0, 0);
                    State = RobotState.Idle;
                    break;
                case RobotState.Idle:
                    //LogController("Idle");
                    break;
                case RobotState.PenUp:
                    if (waitCount == 0)
                    {
                        LogController("PenUp");
                        robot.penUp();
                    }
                    ++waitCount;
                    if (waitCount == 4)
                    {
                        waitCount = 0;
                        NextState();
                    }
                    break;
                case RobotState.PenDown:
                    if (waitCount == 0)
                    {
                        LogController("PenDown");
                        robot.penDown();
                    }
                    ++waitCount;
                    if (waitCount == 4)
                    {
                        waitCount = 0;
                        NextState();
                    }
                    break;
                case RobotState.EraserUp:
                    if (waitCount == 0)
                    {
                        LogController("EraserUp");
                        robot.eraserUp();
                    }
                    ++waitCount;
                    if (waitCount == 4)
                    {
                        waitCount = 0;
                        NextState();
                    }
                    break;
                case RobotState.EraserDown:
                    if (waitCount == 0)
                    {
                        LogController("EraserDown");
                        robot.eraserDown();
                    }
                    ++waitCount;
                    if (waitCount == 4)
                    {
                        waitCount = 0;
                        NextState();
                    }
                    break;
            }
        }

        
        Logging.ILogger tickLogger = null;
        public void UseTickLogger(Logging.ILogger logger)
        {
            tickLogger = logger;
        }
        protected void LogOnTick(string msg)
        {
            if (tickLogger != null) tickLogger.Log(msg);
        }



        public void Start()
        {
            timer.Start();
            InitializeController();
        }

        /// <summary>
        /// stop moving the robot, but do not change the command queue
        /// </summary>
        public void Pause()
        {
            Speed(0, 0);
        }

        public void Halt()
        {
            timer.Stop();
        }

        public virtual void Stop()
        {
            this.State = RobotState.Stop;
            robot.Speed(0, 0);
        }

        public void Speed(double linear, double angular)
        {
            robot.Speed(linear, angular);
        }

        public bool Idle { get { return State == RobotState.Idle; } }


        //obsolete, not used anymore
        public event Action<string> OnLogRobot;
        protected void LogRobot(string message)
        {
            if (OnLogRobot != null)
            {
                OnLogRobot(message);
            }
        }

        public void ReceiveCommand(ACommand command)
        {
            var commandType = command.CommandType;
            if (commandType == RobotState.Stop ||
                commandType == RobotState.PenDown || commandType == RobotState.PenUp ||
                commandType == RobotState.EraserDown || commandType == RobotState.EraserUp)
            {
                State = commandType;
                return;
            }

            try
            {
                DirectionalCommand cmd = command as DirectionalCommand;
                if (cmd != null)
                {
                    this.targetPoint = cmd.Vector;

                    switch (commandType)
                    {
                        case RobotState.LookAt:
                            //Console.WriteLine("angle: " + directionalDeviation());
                            this.startPoint = robot.Position;
                            break;
                        case RobotState.LookInDirection:
                            this.startPoint = Vector2.Zero;
                            break;
                        case RobotState.MoveTo:
                            this.startPoint = robot.Position;
                            break;
                        default:
                            throw new ArgumentException("Command " + command.ToString() + " is not a supported directional command");
                    }

                    State = commandType;
                }
                else
                {
                    throw new ArgumentException("Cannot handle command: " + command.ToString());
                }

            }
            catch (Exception)
            {
                State = RobotState.Idle;
                throw;
            }
        }

    }
}
