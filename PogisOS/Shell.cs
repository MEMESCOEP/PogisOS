using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PogisOS
{
    class Shell
    {
        // Functions
        public void shell()
        {
            Console.WriteLine("[INFO] >> Shell loaded.\n[INFO] >> Loading drivers...");
            Drivers drv = new Drivers();
            Console.WriteLine("[INFO] >> Loading command interpreter...");
            CMD cmd = new CMD();
            Console.WriteLine("[INFO] >> Initializing disk...");
            drv.INIT();
            Console.WriteLine("[INFO] >> Setting current directory...");
            Directory.SetCurrentDirectory(cmd.CWD);
            GlobalVars gbv = new GlobalVars();
            float cpu_speed = Cosmos.Core.CPU.GetCPUCycleSpeed();
            string mem_avail = Cosmos.Core.GCImplementation.GetAvailableRAM().ToString();
            string mem_total = Cosmos.Core.CPU.GetAmountOfRAM().ToString();
            string cpu_vendor = Cosmos.Core.CPU.GetCPUVendorName();
            string cpu_brand = Cosmos.Core.CPU.GetCPUBrandString();
            string cpu_ebp = Cosmos.Core.CPU.GetEBPValue().ToString();
            double used_mem = (Convert.ToInt32(mem_total) - Convert.ToInt32(mem_avail));
            cpu_speed = (cpu_speed / 1000000);
            Console.WriteLine("[===== SYSTEM INFORMATION =====]\n[== SYSTEM ==]\nOS Version: " + gbv.Version + "\n");
            Console.WriteLine("[== CPU ==]");
            Console.WriteLine("CPU Brand: " + cpu_brand + "\nCPU Vendor: " + cpu_vendor + "\nCPU speed: " + cpu_speed + " MHz\nCPU EBP value: " + cpu_ebp);
            Console.WriteLine("\n[== MEMORY ==]");
            Console.WriteLine("Total MEM: " + mem_total + " MB\nMEM Available: " + mem_avail + " MB\nMEM Used: " + used_mem + " MB\n");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("Welcome to PogisOS! (" + gbv.Version + ")");
            while (true)
            {
                Console.Write(cmd.CWD + ">> ");
                var input = Console.ReadLine();
                cmd.Parse(input);
            }
        }
    }
}
