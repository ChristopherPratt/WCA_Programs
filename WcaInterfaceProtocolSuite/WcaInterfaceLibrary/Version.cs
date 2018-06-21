using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace WcaInterfaceLibrary
{
    public class Version
    {
        public static string GetAssemblyVersionNumber()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly().GetName().FullName;
        }

        public static string GetAssemblyCodeBase()
        {
            return Assembly.GetExecutingAssembly().GetName().CodeBase;
        }
    }
}
