using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NationalInstruments.DAQmx;

namespace WcaDVConsole
{
    public class Daq
    {
        public Task task1;
        public Task task2;
        public Task task3;
        public AnalogMultiChannelReader myAnalogReader1;
        public AnalogMultiChannelReader myAnalogReader2;
        public AnalogMultiChannelReader myAnalogReader3;

        public Daq(string[] ai)
        {
            try
            {
                // string[] myChannels = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External);
                //foreach (string a in myChannels) { Console.Write(a + " "); }

                task1 = new Task();
                task2 = new Task();
                task3 = new Task();



                task1.AIChannels.CreateVoltageChannel("Dev1/ai" + ai[0], "0",
                         (AITerminalConfiguration.Rse), 0,
                         10, AIVoltageUnits.Volts);

                task1.Control(TaskAction.Verify);

                task2.AIChannels.CreateVoltageChannel("Dev1/ai" + ai[1], "1",
                        (AITerminalConfiguration.Rse), 0,
                        10, AIVoltageUnits.Volts);

                task2.Control(TaskAction.Verify);

                task3.AIChannels.CreateVoltageChannel("Dev1/ai" + ai[2], "2",
                        (AITerminalConfiguration.Rse), 0,
                        10, AIVoltageUnits.Volts);

                task3.Control(TaskAction.Verify);

                myAnalogReader1 = new AnalogMultiChannelReader(task1.Stream);
                myAnalogReader2 = new AnalogMultiChannelReader(task2.Stream);
                myAnalogReader3 = new AnalogMultiChannelReader(task3.Stream);
            } catch (Exception e) { Console.WriteLine("DAQ not connected."); }
        }

        public double[] getVolts()
        {
            try { return new double[] { myAnalogReader1.ReadSingleSample()[0], myAnalogReader2.ReadSingleSample()[0], myAnalogReader3.ReadSingleSample()[0] }; }
            catch (Exception e) { return new double[] { 0.0, 0.0, 0.0 }; }
        }

    }
}
