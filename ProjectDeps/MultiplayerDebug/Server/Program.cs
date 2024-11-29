#if BUILDINGFROMCSPROJ
// this macro will appear only if building from the .csproj, so in-game compilation doesn't compile this file.     
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

[assembly: global::System.Runtime.Versioning.TargetFrameworkAttribute(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
[assembly: System.Reflection.AssemblyCompanyAttribute("Lolqeuide")]
[assembly: System.Reflection.AssemblyConfigurationAttribute("Debug")]
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.0.0")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.0+4fc68fca31ba7448a98e9a578ac61e0d434355e2")]
[assembly: System.Reflection.AssemblyProductAttribute("ServerStarter")]
[assembly: System.Reflection.AssemblyTitleAttribute("ServerStarter")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]

namespace AndreContentMod.Common.Debug
{
    // To allow to do useful stuff for debugging.
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Missing tml path, pass it as a start argument");
                return;
            }
            Console.WriteLine(args[0]);
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Provided tml dll path does not exist or is not accessible");
                if (!Directory.Exists(args[0]))
                {
                    Console.WriteLine("Provided tml path was not a valid folder");
                    return;
                }
                Console.WriteLine("Provided tml path was not the dll path but its a valid folder");
            }
            else
            {
                args[0] = Path.GetDirectoryName(args[0]);
            }
            Environment.CurrentDirectory = args[0];

            //Console.WriteLine(string.Join(" ", args));
            if (args.AsSpan().Contains("-ANDRESELECTFIRSTWORLD"))
            {
                selectsFirstWorld = true;
            }
            args = args.Append("-server").ToArray();
            Console.WriteLine("Starting tml");
            DoRun(args);
        }

        static bool selectsFirstWorld = false;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void DoRun(string[] args)
        {
            On_Main.Initialize += On_Main_Initialize;
            typeof(ModLoader).Assembly.EntryPoint.Invoke(null, new object[] { args });
        }

        private static void On_Main_Initialize(On_Main.orig_Initialize orig, Main self)
        {
            orig(self);
            System.Diagnostics.Debug.Assert(Terraria.Main.dedServ, "Using ander server starter on a client?");
            bool specifiedWorld = Terraria.Program.LaunchParameters.TryGetValue("-andreserverworld", out string selectedWorld);
            //Console.WriteLine("World name: " + Terraria.Main.worldName);
            if (selectsFirstWorld || specifiedWorld)
            {
                Terraria.Main.LoadWorlds();
                if (Terraria.Main.WorldList.FirstOrDefault(t => selectsFirstWorld || t.Name == selectedWorld) is { } worldFile)
                {
                    Terraria.Main.ActiveWorldFileData = worldFile;
                    Console.WriteLine("ANDRE SERVER STARTER: Selected world: " + worldFile.Name);
                }
                else
                {
                    Console.WriteLine("ANDRE SERVER STARTER: There was no worlds in the world list or specified world was not found.");
                }
            }
        }
    }
}

#endif // BUILDINGFROMCSPROJ