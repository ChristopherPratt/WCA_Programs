using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace WcaProgrammerConsole
{
    public class WcaTextWriterTraceListener : TextWriterTraceListener
    {
        public WcaTextWriterTraceListener(Stream stream)
            : base(stream)
        {

        }

        public WcaTextWriterTraceListener(TextWriter writer)
            : base(writer)
        {

        }

        public override void WriteLine(string message)
        {
            base.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + message);
        }
    }
}
