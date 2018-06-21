using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WcaInterfaceLibrary
{
  /*  public enum CommandResult
    {
        WaitForExecution,
        InProgress,
        PosAck,
        NegAck,
        ExecutionTimeout
    }*/


    public interface ICommand : ICommandBroker
    {
        void Execute(resend myResend);
        string ToString();
        byte Command { get; }
        WcaInterfaceCommandResult Result { get; }
        string Name{ get; }
        string comport { get; }
        byte[] getData { get; }
        bool isReceive { get; }
    }
}
