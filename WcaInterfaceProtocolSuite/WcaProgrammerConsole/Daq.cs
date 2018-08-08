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
        public Task digitalTask1;
        public Task digitalTask2;
        public Task digitalTask3;
        public DigitalSingleChannelWriter myDigitalWriter1;
        public DigitalSingleChannelWriter myDigitalWriter2;
        public DigitalSingleChannelWriter myDigitalWriter3;

        public Daq(bool digital)
        {
            digitalTask1 = new Task();
            digitalTask2 = new Task();
            digitalTask3 = new Task();

            digitalTask1.DOChannels.CreateChannel("Dev1/port0/line1:1", "PFI14", ChannelLineGrouping.OneChannelForEachLine);
            digitalTask1.DOChannels.CreateChannel("Dev1/port1/line1:1", "PFI12", ChannelLineGrouping.OneChannelForEachLine);
            digitalTask1.DOChannels.CreateChannel("Dev1/port2/line1:2", "PFI9", ChannelLineGrouping.OneChannelForEachLine);

            //digitalTask1.DOChannels.CreateChannel("Dev1/PFI14", "PFI14", ChannelLineGrouping.OneChannelForEachLine);
            //digitalTask1.DOChannels.CreateChannel("Dev1/PFI12", "PFI12", ChannelLineGrouping.OneChannelForEachLine);
            //digitalTask1.DOChannels.CreateChannel("Dev1/PFI9", "PFI9", ChannelLineGrouping.OneChannelForEachLine);

            digitalTask1.Control(TaskAction.Verify);

            myDigitalWriter1 = new DigitalSingleChannelWriter(digitalTask1.Stream);
            myDigitalWriter2 = new DigitalSingleChannelWriter(digitalTask2.Stream);
            myDigitalWriter3 = new DigitalSingleChannelWriter(digitalTask3.Stream);
        }
        public void setHi(bool[] device)
        {
            if (device[0])
            {
                myDigitalWriter1.WriteSingleSampleSingleLine(true, device[0]);
            }
            if (device[1])
            {
                myDigitalWriter2.WriteSingleSampleSingleLine(true, device[1]);
            }
            if (device[2])
            {
                myDigitalWriter3.WriteSingleSampleSingleLine(true, device[2]);
            }
        }
        public void setLo(bool[] device)
        {
            if (device[0])
            {
                myDigitalWriter1.WriteSingleSampleSingleLine(true, device[0]);
            }
            if (device[1])
            {
                myDigitalWriter2.WriteSingleSampleSingleLine(true, device[1]);
            }
            if (device[2])
            {
                myDigitalWriter3.WriteSingleSampleSingleLine(true, device[2]);
            }
        }
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
