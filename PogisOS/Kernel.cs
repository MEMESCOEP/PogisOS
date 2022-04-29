using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace PogisOS
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            Console.Clear();
            Console.WriteLine("[INFO] >> Kernel loaded successfully.");
        }

        protected override void Run()
        {
            Console.WriteLine("[INFO] >> Loading Shell...");
            try
            {
                Shell shell = new Shell();
                shell.shell();
            }
            catch(Exception EX)
            {
                Console.WriteLine("[CRITICAL_ERR#KERNEL] >> Shell initialization failed! Halting...");
                while (true)
                {

                }
            }
        }
    }
}
