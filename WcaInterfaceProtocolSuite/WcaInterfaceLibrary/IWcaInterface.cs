using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WcaInterfaceLibrary
{
    [ComVisible(true)]
    public enum WcaInterfaceType
    {
        UART,
        USB
    };

    [ComVisible(true)]
    public enum WcaInterfaceAddress
    {
        WCA_WCT100x,
        WCA_BOLERO,
        PROGRAMMER
    };

    [ComVisible(true)]
    public enum WcaInterfaceCommandResult
    {
        WaitForExecution,
        InProgress,
        PosAck,
        NegAck,
        ExecutionTimeout
    }



    [Guid("4ac2d68a-c93a-4bbc-8f6b-2efc69e59345")]
    [ComVisible(true)]
    public interface IWcaInterface
    {
        [DispId(1)]
        bool Open(WcaInterfaceType type, string name, int baudrate);

        [DispId(2)]
        void Close();

        [DispId(3)]
        WcaInterfaceCommandResult SendCommandSync(WcaInterfaceAddress destination, 
            byte cmd,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1)]out byte[] receivedData);

        [DispId(4)]
        WcaInterfaceCommandResult SendCommandSync(WcaInterfaceAddress destination, 
            byte cmd,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1)]byte[] data,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1)]out byte[] receivedData);

        [DispId(5)]
        bool SendCommandASync(WcaInterfaceAddress destination, 
            byte cmd);

        [DispId(6)]
        bool SendCommandASync(WcaInterfaceAddress destination, 
            byte cmd,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1)]byte[] data);

        [DispId(7)]
        string GetVersion();
    }


    [Guid("c113430b-5779-4b40-b71d-8fddd546fd80"),
       InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [ComVisible(true)]
    public interface IWcaInterfaceEvents
    {
        [DispId(1)]
        void DataReceivedEvent(byte[] data);
    }
}
