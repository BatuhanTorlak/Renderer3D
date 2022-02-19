using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderMachineDotNetCore
{
    public static class DefaultFuncs
    {
        private static DateTime BackTime;
        public static double DeltaTime;
        public static void Setup(double SizeX, double SizeY)
        {
            Camera.Objects = new DList();
            Camera.Size = new size(SizeX, SizeY, 0);
            Camera.Position = new position(0, 0, 0);
            Camera.Rotation = new rotation(0, 0, 0);
            Camera.FoV = 10;
        }

        public static void DeltaTimer()
        {
            DeltaTime = (DateTime.Now - BackTime).TotalMilliseconds / 500;
            BackTime = DateTime.Now;
        }

        public static void Log(object msg)
        {
            Console.WriteLine($"{DateTime.Now} : {msg.ToString()}");
        }
    }
}
