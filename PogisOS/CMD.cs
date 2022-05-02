using Cosmos.System.Graphics;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using IL2CPU.API.Attribs;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;

namespace PogisOS
{
    class CMD
    {
        // Variables
        public string CWD = "0:\\";

        [ManifestResourceStream(ResourceName = "PogisOS.Resources.MouseCursor.bmp")]
        public static byte[] mousedatalol;

        [ManifestResourceStream(ResourceName = "PogisOS.Resources.ButtonTemplate.bmp")]
        public static byte[] ButtonDataLol;

        GlobalVars gbv = new GlobalVars();

        // Functions
        public static bool PingHost(string hostUri, int portNumber)
        {
            try
            {
                using (var client = new Cosmos.System.Network.IPv4.TCP.TcpClient(Cosmos.System.Network.IPv4.Address.Parse(hostUri), portNumber))
                {
                    Console.WriteLine("Connecting...");
                    client.Connect(Cosmos.System.Network.IPv4.Address.Parse(hostUri), portNumber);
                    byte[] payload = Encoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwabcdefghi");
                    Console.WriteLine("Sending data...");
                    client.Send(payload);
                    Console.WriteLine("Done.");
                    return true;
                }                    
            }
            catch
            {
                return false;
            }
        }
        
        static Int32 CountInString(string orig, string find)
        {
            var s2 = orig.Replace(find, "");
            return (orig.Length - s2.Length) / find.Length;
        }

        public static string GetTopmostFolder(string path)
        {
            if (path.StartsWith(Path.DirectorySeparatorChar.ToString()))
                path = path.Substring(1);

            Uri inputPath;

            if (Uri.TryCreate(path, UriKind.Absolute, out inputPath))
                return Path.GetPathRoot(path);

            if (Uri.TryCreate(path, UriKind.Relative, out inputPath))
                return path.Split(Path.DirectorySeparatorChar)[0];

            return path;
        }

        public void Parse(string input)
        {
            string command = input;
            string[] args = {input};
            if(input.Contains(" "))
            {
                command = input.Split(' ')[0];
                args = input.Split(' ');
            }
            if (command.ToLower() == "help")
            {
                Console.WriteLine("PogisOS Help:");
                Console.WriteLine("1. help");
                Console.WriteLine("2. sysinfo");
                Console.WriteLine("3. cls");
                Console.WriteLine("4. reboot");
                Console.WriteLine("5. shutdown");
                Console.WriteLine("6. mkf <file name>");
                Console.WriteLine("7. mkd <dir name>");
                Console.WriteLine("8. rm <file name");
                Console.WriteLine("9. rmd <dir name>");
                Console.WriteLine("10. cd <dir name>");
                Console.WriteLine("11. ls");
                Console.WriteLine("12. ipaddr");
                Console.WriteLine("13. ping <ip address>");
                Console.WriteLine("14. edit <file name>");
                Console.WriteLine("15. cat <file name>");
                Console.WriteLine("16. gui (primitive; testing only)");
                Console.WriteLine("17. acpi <disable, enable>");
                Console.WriteLine("18. about");
                Console.WriteLine("19. ldrv");
                Console.WriteLine("20. lvol");
                Console.WriteLine("21. lpart");
            }
            else if(command == "ldrv")
            {
                try
                {
                    int DriveCount = 1;
                    foreach (var drive in Cosmos.System.FileSystem.VFS.VFSManager.GetDisks())
                    {
                        Console.WriteLine("[===== DRIVE " + DriveCount + " =====]");
                        drive.DisplayInformation();
                        DriveCount++;
                        Console.WriteLine();
                    }
                }
                catch
                {

                }
            }
            else if(command == "lvol")
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
            else if (command == "lpart")
            {
                try
                {
                    int DriveCount = 0;
                    foreach (var drive in Cosmos.System.FileSystem.VFS.VFSManager.GetDisks())
                    {
                        Console.WriteLine("[===== DRIVE " + DriveCount + " =====]");
                        Console.WriteLine("\tDrive \"" + DriveCount + "\" contains " + drive.Partitions.Count + " partition(s).");
                        for (int i = 0; i < drive.Partitions.Count; i++)
                        {
                            Console.WriteLine("\tPartition " + i);
                        }
                        Console.WriteLine();
                        DriveCount++;
                    }
                }
                catch
                {

                }
            }
            else if (command.ToLower() == "ls")
            {
                try
                {
                    string[] fileEntries = Directory.GetFiles(Directory.GetCurrentDirectory());
                    string[] dirEntries = Directory.GetDirectories(Directory.GetCurrentDirectory());
                    foreach (string File in fileEntries)
                    {
                        Console.WriteLine(File + "\t[FILE]");
                    }
                    foreach (string Dir in dirEntries)
                    {
                        Console.WriteLine(Dir + "\t[DIR]");
                    }
                }
                catch
                {

                }
            }
            else if (command == "gui")
            {
                Canvas canvas;
                GlobalVars gbv = new GlobalVars();
                canvas = FullScreenCanvas.GetFullScreenCanvas();
                try
                {
                    Pen pen = new Pen(Color.Red);
                    canvas.Mode = new Mode(gbv.ScreenWidth, gbv.ScreenHeight, ColorDepth.ColorDepth32);
                    pen.Color = Color.LimeGreen;
                    Cosmos.System.MouseManager.MouseSensitivity = 1;
                    Cosmos.System.MouseManager.ScreenHeight = (uint)gbv.ScreenHeight;
                    Cosmos.System.MouseManager.ScreenWidth = (uint)gbv.ScreenWidth;
                    Bitmap cursorBMP = new Bitmap(mousedatalol);
                    Bitmap button = new Bitmap(ButtonDataLol);
                    int X = Convert.ToInt32(Cosmos.System.MouseManager.X);
                    int Y = Convert.ToInt32(Cosmos.System.MouseManager.Y);
                    while (true)
                    {
                        Thread.Sleep(1);
                        X = Convert.ToInt32(Cosmos.System.MouseManager.X);
                        Y = Convert.ToInt32(Cosmos.System.MouseManager.Y);
                        canvas.Clear(Color.Blue);
                        canvas.DrawImageAlpha(button, 100, 300);
                        canvas.DrawImageAlpha(cursorBMP, X, Y);
                        canvas.Display();
                    }
                }
                catch (Exception e)
                {
                    canvas.Disable();
                    Console.WriteLine("GUI ERROR: " + e.Message);
                }
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
                    Console.WriteLine("[===== SYSTEM INFORMATION =====]\n[== SYSTEM ==]\nOS Version: " + gbv.Version + "\n");
                    Console.WriteLine("[== CPU ==]");
                    Console.WriteLine("CPU Brand: " + cpu_brand + "\nCPU Vendor: " + cpu_vendor + "\nCPU speed: " + cpu_speed + " MHz\nCPU EBP value: " + cpu_ebp);
                    Console.WriteLine("\n[== MEMORY ==]");
                    Console.WriteLine("Total MEM: " + mem_total + " MB\nMEM Available: " + mem_avail + " MB\nMEM Used: " + used_mem + " MB\n");
                }
                catch(Exception EX)
                {
                    Console.WriteLine("ERROR: " + EX.Message);
                }
            }
            else if (command == ("mkd"))
            {
                if (!Directory.Exists(args[1]))
                {
                    Directory.CreateDirectory(args[1]);
                }
                else
                {
                    Console.WriteLine("Directory already exists!");
                }
            }
            else if(command == "about")
            {
                Console.WriteLine("Pogis OS (" + gbv.Version + ")");
                Console.WriteLine(gbv.Copyright);
                Console.WriteLine(gbv.Disclaimer);
                Console.WriteLine(gbv.DetailsUrl);
            }
            else if (command == ("rmd"))
            {
                try
                {
                    if (args[1].Length > 0 && input[3] == ' ' && input.Length > 4)
                    {
                        if (Directory.Exists(Directory.GetCurrentDirectory() + args[1]))
                        {
                            Directory.Delete(Directory.GetCurrentDirectory() + args[1], true);
                        }
                        else if (Directory.Exists(args[1]))
                        {
                            Directory.Delete(args[1], true);
                        }
                        else
                        {
                            Console.WriteLine("Directory \"" + args[1] + "\" doesn't exist!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You need to specify a directory!");
                    }
                }
                catch
                {

                }
            }
            else if (command == ("rm"))
            {
                try
                {
                    if (args[1].Length > 0 && input[2] == ' ' && input.Length > 3)
                    {
                        if (File.Exists(Directory.GetCurrentDirectory() + args[1]))
                        {
                            File.Delete(Directory.GetCurrentDirectory() + args[1]);
                        }
                        else if (File.Exists(args[1]))
                        {
                            File.Delete(args[1]);
                        }
                        else
                        {
                            Console.WriteLine("File \"" + args[1] + "\" doesn't exist!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You need to specify a file!");
                    }
                }
                catch
                {

                }
            }
            else if (command == ("edit"))
            {
                try
                {
                    if (args[1].Length > 0 && input[4] == ' ' && input.Length > 3)
                    {
                        if (File.Exists(Directory.GetCurrentDirectory() + args[1]))
                        {
                            Applications.Edit editor = new Applications.Edit();
                            editor.EditFile(Directory.GetCurrentDirectory() + args[1]);
                        }
                        else if (File.Exists(args[1]))
                        {
                            Applications.Edit editor = new Applications.Edit();
                            editor.EditFile(args[1]);
                        }
                        else
                        {
                            Console.WriteLine("File \"" + args[1] + "\" doesn't exist!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You need to specify a file!");
                    }
                }
                catch
                {

                }
            }
            else if (command == ("mkf"))
            {
                try
                {
                    if (args[1].Length > 0 && input[3] == ' ' && input.Length > 4)
                    {
                        if (!File.Exists(Directory.GetCurrentDirectory() + args[1]))
                        {
                            File.CreateText(Directory.GetCurrentDirectory() + args[1]);
                        }
                        else
                        {
                            Console.WriteLine("File \"" + args[1] + "\" already exists!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You need to specify a file name!");
                    }
                }
                catch
                {

                }
            }
            else if (command == ("cd"))
            {
                try
                {
                    if (args[1].Length > 0 && input[2] == ' ' && input.Length > 3)
                    {
                        if (args[1].Contains("..\\"))
                        {
                            int count = CountInString(args[1], "..\\");
                            for (int i = 0; i < count; i++)
                            {                                                               
                                try
                                {
                                    if (Path.GetPathRoot(Directory.GetCurrentDirectory()) != Directory.GetCurrentDirectory())
                                    {
                                        if (CountInString(Path.GetFullPath(Directory.GetParent(Directory.GetCurrentDirectory()).FullName), @"\") < 1)
                                        {

                                        }
                                        else if (Directory.Exists(Path.GetFullPath(Directory.GetParent(Directory.GetCurrentDirectory()).FullName)))
                                        {
                                            string dirname = Path.GetFullPath(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
                                            Directory.SetCurrentDirectory(dirname);
                                            CWD = Directory.GetCurrentDirectory();
                                        }
                                    }                                  
                                }
                                catch
                                {

                                }
                            }
                        }
                        else if (Directory.Exists(Directory.GetCurrentDirectory() + args[1]))
                        {
                            if (args[1].EndsWith("\\"))
                            {
                                args[1] = args[1].Remove(args[1].Length - 1, 1);
                            }
                            if (args[1].StartsWith("\\"))
                            {
                                args[1] = args[1].Remove(0, 1);
                            }
                            string dir = args[1];
                            dir = Path.GetFullPath(dir);
                            Directory.SetCurrentDirectory(dir + "\\");
                            CWD = Directory.GetCurrentDirectory();
                        }
                        else if (Directory.Exists(args[1]))
                        {
                            string dir = args[1];
                            dir = Path.GetFullPath(dir);
                            Directory.SetCurrentDirectory(dir + "\\");
                            CWD = Directory.GetCurrentDirectory();
                        }
                        else
                        {
                            Console.WriteLine("Directory \"" + args[1] + "\" doesn't exist!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You need to specify a directory!");
                    }
                }
                catch
                {

                }
            }
            else if(command == "cat")
            {
                try
                {
                    if (args.Length > 1 && args[1].Length > 0 && input[3] == ' ' && input.Length > 4)
                    {
                        if (File.Exists(args[1]))
                        {
                            Console.WriteLine(File.ReadAllText(args[1]));
                        }
                        else
                        {
                            Console.WriteLine("File \"" + args[1] + "\" doesn't exist!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You need to specify a file name!");
                    }
                }
                catch
                {

                }
            }
            else if (command.ToLower() == "acpi")
            {
                try
                {
                    if (args[1].Length > 0 && input[4] == ' ' && input.Length > 5)
                    {
                        if (args[1] == "disable")
                        {
                            Console.WriteLine("WARNING! Disabling ACPI can limit OS functionality!");
                            Console.Write("Are you ABSOLUTELY SURE you want to continue? (Yes/No) >> ");
                            if (Console.ReadLine().ToLower() == "yes")
                            {
                                Console.WriteLine("[ACPI] >> Disabling ACPI...");
                                try
                                {
                                    Cosmos.Core.ACPI.Disable();
                                    Console.WriteLine("[ACPI] >> Done.");
                                }
                                catch (Exception EX)
                                {
                                    Console.WriteLine("[ACPI -> Disable] >> ERROR: " + EX.Message);
                                }
                            }
                        }
                        else if (args[1] == "enable")
                        {
                            Console.WriteLine("[ACPI] >> Enabling ACPI...");
                            try
                            {
                                Cosmos.Core.ACPI.Enable();
                                Console.WriteLine("[ACPI] >> Done.");
                            }
                            catch (Exception EX)
                            {
                                Console.WriteLine("[ACPI -> Enable] >> ERROR: " + EX.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("[ACPI] >> Invalid option. Valid options are 'enable' and 'disable'");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[ACPI] >> You need to specify an option!");
                    }
                }
                catch
                {

                }
            }
            else if (command.ToLower() == "shutdown")
            {
                try
                {
                    Cosmos.Core.ACPI.Shutdown();
                }
                catch
                {
                    try
                    {
                        Cosmos.System.Power.Shutdown();
                    }
                    catch (Exception EX)
                    {
                        Console.WriteLine("[ERROR -> SHUTDOWN] Shutdown failed!\nDetails: " + EX.Message);
                    }
                }
                Cosmos.Core.CPU.Halt();
            }
            else if (command.ToLower() == "reboot")
            {
                try
                {
                    Cosmos.Core.ACPI.Reboot();
                }
                catch
                {
                    try
                    {
                        Cosmos.System.Power.Reboot();
                    }
                    catch(Exception EX)
                    {
                        Console.WriteLine("[ERROR -> REBOOT] Reboot failed!\nDetails: " + EX.Message);
                    }                    
                }
                Cosmos.Core.CPU.Reboot();
            }
            else if (command.ToLower() == "cls")
            {
                Console.Clear();
            }
            else if (command == "ipaddr")
            {
                Console.WriteLine("IPv4 address for interface \"ETH0\": " + NetworkConfig.CurrentConfig.Value.IPAddress.ToString() + "\nDefault gateway: " + NetworkConfig.CurrentConfig.Value.DefaultGateway.ToString() + "\nSubnet mask: " + NetworkConfig.CurrentConfig.Value.SubnetMask.ToString());
            }
            else if (command == "format")
            {
                if(args.Length > 0)
                {
                    if (args[1].Length > 0 && input[7] == ' ' && input.Length > 8)
                    {
                        Console.WriteLine("Formatting drive '" + args[1] + "'...");
                        try
                        {
                            Cosmos.System.FileSystem.CosmosVFS DISK = new Cosmos.System.FileSystem.CosmosVFS();
                            foreach (var Disk in DISK.GetDisks())
                            {
                                if (Disk.Partitions[0].RootPath == args[1])
                                {
                                    Disk.Partitions[0].MountedFS.Format("FAT32", true);
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            else if (command == "ping")
            {
                if (args[1].Length > 0 && input[4] == ' ' && input.Length > 5)
                {
                    Console.WriteLine("Pinging " + args[1] + "...");
                    try
                    {
                        using (var xClient = new Cosmos.System.Network.IPv4.ICMPClient())
                        {
                            int PacketSent = 0;
                            int PacketReceived = 0;
                            int PacketLost = 0;
                            int PercentLoss;
                            Cosmos.System.Network.IPv4.Address Destination = Cosmos.System.Network.IPv4.Address.Parse(args[1]);
                            xClient.Connect(Destination);
                            for (int i = 0; i < 4; i++)
                            {
                                xClient.SendEcho();

                                PacketSent++;

                                var endpoint = new EndPoint(Address.Zero, 0);

                                int second = xClient.Receive(ref endpoint, 4000);

                                if (second == -1)
                                {
                                    Console.WriteLine("Destination host unreachable.");
                                    PacketLost++;
                                }
                                else
                                {
                                    if (second < 1)
                                    {
                                        Console.WriteLine("Reply received from " + endpoint.Address.ToString() + " time < 1s");
                                        Thread.Sleep(1000);
                                    }
                                    else if (second >= 1)
                                    {
                                        Console.WriteLine("Reply received from " + endpoint.Address.ToString() + " time " + second + "s");
                                        Thread.Sleep(1000);
                                    }

                                    PacketReceived++;
                                }
                            }

                            xClient.Close();
                            PercentLoss = 25 * PacketLost;

                            Console.WriteLine();
                            Console.WriteLine("Ping statistics for " + Destination.ToString() + ":");
                            Console.WriteLine("    Packets: Sent = " + PacketSent + ", Received = " + PacketReceived + ", Lost = " + PacketLost + " (" + PercentLoss + "% loss)");
                        }
                    }
                    catch (Exception EX)
                    {
                        Console.WriteLine("Ping failed: " + EX.Message);
                    }
                }
            }
            else if (command.ToLower() == "")
            {

            }
            else
            {
                Console.WriteLine("Invalid command: \"" + command + "\"");
            }
        }
    }
}
