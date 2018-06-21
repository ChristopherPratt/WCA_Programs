using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcaFlashApplicationManaged
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Laird Dabendorf GmbH, Moray Ruby 1.2 Flasher (managed), Version 2.0.0.1");
            Console.WriteLine();

            if (args.Length == 2)
            {
                string comport = args[0];
                string file = args[1];
                Ruby12Flasher flasher = new Ruby12Flasher();
                flasher.Run(comport, file);
            }
            else
            {
                Console.WriteLine("Run application as follow: ");
                Console.WriteLine("WcaFlashApplicationManaged.exe <COMPORT> <S19-FILE>");
                Console.WriteLine("e.g. WcaFlashApplicationManaged.exe COM1 MO_WC_11_1_8_6.S19");
            }
        }
    }
}
