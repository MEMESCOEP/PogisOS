using Cosmos.HAL;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using System.Threading;

namespace PogisOS
{
    class Drivers
    {
        // Variables
        public Cosmos.System.FileSystem.CosmosVFS DISK;
        TimeFunctions tmf = new TimeFunctions();

        // Functions
        public void INIT()
        {
            try
            {
                Console.WriteLine("[INFO -> DRIVERS] >> Enabling ACPI...");
                Cosmos.Core.ACPI.Enable();
                Console.WriteLine("[INFO -> DRIVERS] >> Starting ACPI...");
                Cosmos.Core.ACPI.Start();
                Console.WriteLine("[INFO -> DRIVERS] >> Initializing disk...");
                DISK = new Cosmos.System.FileSystem.CosmosVFS();
                Console.WriteLine("[INFO -> DRIVERS] >> Setting up PCI...");
                Cosmos.HAL.PCI.Setup();
                Console.WriteLine("[INFO -> DRIVERS] >> PS/2 Keyboard layout: " + Cosmos.System.KeyboardManager.GetKeyLayout());
                Console.WriteLine("[INFO -> DRIVERS] >> Setting up PCI...");
                Console.WriteLine("[INFO -> DRIVERS] >> PCI Devices: ");
                foreach (var device in Cosmos.HAL.PCI.Devices)
                {
                    Console.WriteLine("[===== PCI DEVICE \"" + device.DeviceID + "\" =====]\n\tStatus: " + device.Status + "\n\tVendor ID: " + device.VendorID + "\n\tSlot: " + device.slot + "\n\tDevice bus: " + device.bus);
                }
                Console.WriteLine("[INFO -> DRIVERS] >> Block devices: ");
                foreach (var device in Cosmos.HAL.BlockDevice.BlockDevice.Devices)
                {
                    tmf.Sleep(25);
                    Console.WriteLine("[INFO -> BLOCKDEVICES] >> Block count: " + device.BlockCount + " || Block size: " + device.BlockSize);
                }
                Console.WriteLine("[INFO -> DRIVERS] >> Registering VFS...");
                Cosmos.System.FileSystem.VFS.VFSManager.RegisterVFS(DISK);
                Console.WriteLine("[INFO -> DRIVERS] >> Mounting drives...");
                foreach (var drive in Cosmos.System.FileSystem.VFS.VFSManager.GetDisks())
                {
                    Console.WriteLine("[INFO -> DRIVERS] >> Mounting new drive...");
                    try
                    {
                        if(drive.Partitions.Count < 1)
                        {
                            throw new Exception("Drive \"" + Cosmos.System.FileSystem.VFS.VFSManager.GetDisks().IndexOf(drive) + "\" doesn't have any partitions!");
                        }
                        for (int i = 0; i < drive.Partitions.Count; i++)
                        {
                            Console.WriteLine("[INFO -> DRIVERS] >> Mounting partition " + i + "...");
                            try
                            {
                                drive.MountPartition(i);
                                Cosmos.System.FileSystem.FileSystem fs = drive.Partitions[i].MountedFS;
                                Console.WriteLine("[INFO -> DRIVERS] >> Partition " + fs.Label + " mounted as " + fs.RootPath);
                            }
                            catch(Exception EX)
                            {
                                Console.WriteLine("[ERROR -> MOUNT] >> Error mounting partition " + i + ": " + EX.Message);
                            }
                        }                        
                    }
                    catch(Exception EX)
                    {
                        Console.WriteLine("[ERROR -> MOUNT] >> Error mounting drive: " + EX.Message);
                    }
                }
                Console.WriteLine("[DONE]\n[INFO -> DRIVERS] >> Initializing network...");
                Console.WriteLine("[INFO -> DRIVERS] >> Initializing DHCP Client...");
                using (var xClient = new Cosmos.System.Network.IPv4.UDP.DHCP.DHCPClient())
                {
                    try
                    {
                        Console.WriteLine("[INFO -> DRIVERS] >> Sending DHCP discover...");
                        xClient.SendDiscoverPacket();
                        if (NetworkConfig.CurrentConfig.Value.IPAddress.ToString() == "0.0.0.0")
                        {
                            throw new Exception("DHCP discover returned IP \"0.0.0.0\"");
                        }
                        else
                        {
                            Console.WriteLine("[DONE]\n[INFO -> DRIVERS] >> IPv4 Address: " + NetworkConfig.CurrentConfig.Value.IPAddress.ToString());
                        }
                    }
                    catch(Exception EX)
                    {
                        Console.WriteLine("[ERROR -> DHCP] >> DHCP Autoconfig failed.\nDetails: " + EX.Message);
                    }
                }
            }
            catch(Exception EX)
            {
                Console.WriteLine("[ERROR -> DRIVERS] >> Driver initialization failed! Details: " + EX.Message + "\n");
                Console.WriteLine("Press enter to reboot, or press any other key to continue booting.");
                ConsoleKeyInfo Character = Console.ReadKey();
                if(Character.Key == ConsoleKey.Enter)
                {
                    Cosmos.System.Power.Reboot();
                }
                else
                {
                    Console.WriteLine("\n[INFO -> DRIVERS] >> Continuing boot...");
                }
            }
            Thread.Sleep(1000);
        }
    }
}
