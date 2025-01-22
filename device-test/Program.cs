using System;
using System.Diagnostics;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Serilog;

namespace device_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                //.WriteTo.MSSqlServer("Server=.;Database=akka_logging;Integrated Security=SSPI;", "Logs")
                //.MinimumLevel.Information()
                .CreateLogger();

            var cfg = ConfigurationFactory.ParseString(File.ReadAllText("Config.properties"));

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            var _paasActorSystem = ActorSystem.Create("acirroplus", cfg);
            _paasActorSystem.ActorOf(Props.Create(()=> new DeviceActor()), "device");
            _paasActorSystem.WhenTerminated.Wait();
        }
    }
}
