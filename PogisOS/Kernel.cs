using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using Sys = Cosmos.System;

namespace PogisOS
{
    public class Kernel : Sys.Kernel
    {
        [ManifestResourceStream(ResourceName = "PogisOS.Resources.PogisLogo.bmp")]
        public static byte[] PogisLogo;

        TimeFunctions tmf = new TimeFunctions();
        string Commands = "Commands:\nshutdown, reboot, help, lvol, sysinfo";

        protected override void BeforeRun()
        {
            Console.Clear();
            Console.WriteLine("[INFO -> KERNEL] >> Kernel loaded successfully.");
        }

        protected override void Run()
        {
            try
            {
                GlobalVars gbv = new GlobalVars();
                Console.WriteLine("Pogis OS (" + gbv.Version + ")");
                Console.WriteLine(gbv.Copyright);
                Console.WriteLine(gbv.Disclaimer);
                Console.WriteLine(gbv.DetailsUrl);
                tmf.Sleep(1000);
            }
            catch
            {

            }
            Console.WriteLine("[INFO -> KERNEL] >> Loading Shell...");
            try
            {
                Shell shell = new Shell();
                shell.shell();
            }
            catch(Exception EX)
            {
                Console.WriteLine("[CRITICAL_ERROR -> KERNEL] >> Shell initialization failed!\nDetails:\n" + EX.Message);
                Console.WriteLine("Starting backup shell...");
                while (true)
                {
                    Console.Write(">> ");
                    string command = Console.ReadLine();
                    if(command == "help")
                    {
                        Console.WriteLine(Commands);
                    }
                    else if (command == "sysinfo")
                    {
                        try
                        {
                            float cpu_speed = Cosmos.Core.CPU.GetCPUCycleSpeed();
                            string mem_avail = Cosmos.Core.GCImplementation.GetAvailableRAM().ToString();
                            string mem_total = Cosmos.Core.CPU.GetAmountOfRAM().ToString();
                            string cpu_vendor = Cosmos.Core.CPU.GetCPUVendorName();
                            string cpu_brand = Cosmos.Core.CPU.GetCPUBrandString();
                            string cpu_ebp = Cosmos.Core.CPU.GetEBPValue().ToString();
                            double used_mem = (Convert.ToInt32(mem_total) - Convert.ToInt32(mem_avail));
                            cpu_speed = (cpu_speed / 1000000);
                            Console.WriteLine("[===== SYSTEM INFORMATION =====]");
                            Console.WriteLine("[== CPU ==]");
                            Console.WriteLine("CPU Brand: " + cpu_brand + "\nCPU Vendor: " + cpu_vendor + "\nCPU speed: " + cpu_speed + " MHz\nCPU EBP value: " + cpu_ebp);
                            Console.WriteLine("\n[== MEMORY ==]");
                            Console.WriteLine("Total MEM: " + mem_total + " MB\nMEM Available: " + mem_avail + " MB\nMEM Used: " + used_mem + " MB\n");
                        }
                        catch (Exception EX2)
                        {
                            Console.WriteLine("ERROR: " + EX2.Message);
                        }
                    }
                    else if(command == "reboot")
                    {
                        Cosmos.System.Power.Reboot();
                    }
                    else if (command == "shutdown")
                    {
                        Cosmos.System.Power.Shutdown();
                    }
                    else if (command == "lvol")
                    {
                        try
                        {
                            foreach (var drive in Cosmos.System.FileSystem.VFS.VFSManager.GetVolumes())
                            {
                                Console.WriteLine(drive.mName);
                            }
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid command: \"" + command + "\"");
                        Console.WriteLine(Commands);
                    }
                }
            }
        }
    }
}
