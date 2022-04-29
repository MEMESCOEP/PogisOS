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

        // Functions
        public void INIT()
        {
            try
            {
                Console.WriteLine("[INFO#DRIVERS] >> Initializing disk...");
                DISK = new Cosmos.System.FileSystem.CosmosVFS();
                Cosmos.System.FileSystem.VFS.VFSManager.RegisterVFS(DISK);
                Console.WriteLine("[DONE]\n[INFO#DRIVERS] >> Initializing network...");
                Console.WriteLine("[INFO#DRIVERS] >> Initializing DHCP Client...");
                using (var xClient = new Cosmos.System.Network.IPv4.UDP.DHCP.DHCPClient())
                {
                    /** Send a DHCP Discover packet **/
                    //This will automatically set the IP config after DHCP response
                    try
                    {
                        xClient.SendDiscoverPacket();
                        Console.WriteLine("[DONE]\n[INFO#DRIVERS] >> IPv4 Address: " + NetworkConfig.CurrentConfig.Value.IPAddress.ToString());
                    }
                    catch(Exception EX)
                    {
                        Console.WriteLine("[ERROR#DHCP] >> DHCP Autoconfig failed.");
                    }
                }
            }
            catch(Exception EX)
            {
                Console.WriteLine("[ERROR] >> Driver initialization failed! Details:\n" + EX.Message + "\n");
                Console.WriteLine("Press \"ENTER\" to reboot, or press any other key to continue booting.");
                ConsoleKeyInfo Character = Console.ReadKey();
                if(Character.Key == ConsoleKey.Enter)
                {
                    Cosmos.System.Power.Reboot();
                }
            }
            Thread.Sleep(1000);
        }

    }
}
