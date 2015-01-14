﻿using System;
namespace BLETest
{
    public class FinishedEventArgs : EventArgs
    {
        private RobotState finishedState;

        public FinishedEventArgs(RobotState finishedState)
        {
            this.finishedState = finishedState;
        }

        public override string ToString()
        {
            return "Finished task " + finishedState.ToString("G");
        }
    }

    public enum RobotState
    {
        RotateTo,
        LookAt,
        LookInDirection,
        MoveTo,
        Finished,
        Stop,
        PenUp,
        PenDown,
        EraserUp,
        EraserDown,
        Idle
    }

    public interface IRobotController
    {
        event EventHandler<BLETest.FinishedEventArgs> Finished;
        void Halt();
        bool Idle { get; }
        void NextState();
        event Action<string> OnLogController;
        
        //not used anymore
        event Action<string> OnLogRobot;
        
        void ReceiveCommand(BLETest.ACommand command);
        void Speed(double linear, double angular);
        
        void UseTickLogger(Logging.ILogger logger);

        void Start();
        void Pause();
        void Stop();
    }
}
