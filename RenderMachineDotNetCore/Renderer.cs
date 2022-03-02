using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RenderMachineDotNetCore
{

    public static class Camera
    {
        public static position Position;
        public static rotation Rotation;
        public static size Size;
        public static double FoV { get; set; }
        public static DList Objects;

        //[LoaderOptimization(LoaderOptimization.MultiDomain)]
        public static void RenderConsole()
        {
            //Console.Clear();
            double DegreePerPx = 0.1;
            int[,] colorCodes = new int[(int)Size.x, (int)Size.y]; //1 px = 0.04 degree

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    colorCodes[x, y] = 0;
                }
            }

            rotation rotMin = new rotation((int)(Rotation.x - Size.x * DegreePerPx), (int)(Rotation.y - Size.y * DegreePerPx));
            rotation rotMax = new rotation((int)(Rotation.x + Size.x * DegreePerPx), (int)(Rotation.y + Size.y * DegreePerPx));

            Console.WriteLine($"{rotMin.x} ! {rotMin.y} | {rotMax.x} ! {rotMax.y}");
            
            foreach (var o in Objects.ts)
            {
                foreach (var T in o.Triangles)
                {
                    foreach (var v in T.verticals)
                    {
                        position ps = v.position + o.Position;

                        rotation rot = MathB.CartesienToSphere(ps - Position);

                        Console.WriteLine(MathB.Distance(Position, ps));

                        Console.WriteLine(rot.ToString());

                        int XPos = 0;
                        int YPos = 0;

                        bool XPassed = false;
                        bool YPassed = false;

                        if (rot.x > rotMax.x)
                        {
                            if (rot.x > rotMin.x && rotMax.x - rotMin.x < 0)
                            {
                                XPassed = true;
                                XPos = (int)(rotMin.x - rot.x);
                            }
                        }
                        else
                        {
                            if (rotMax.x - rotMin.x > 0)
                            {
                                if (rot.x > rotMin.x)
                                {
                                    XPassed = true;
                                    XPos = (int)(rotMax.x - rot.x);
                                }
                            }
                            else
                            {
                                XPassed = true;
                                XPos = (int)(rotMax.x - rot.x);
                            }
                        }

                        Console.WriteLine(XPassed.ToString());

                        if (XPassed)
                        {
                            if (rot.y > rotMax.y)
                            {
                                if (rot.y > rotMin.y && rotMax.y - rotMin.y < 0)
                                {
                                    YPassed = true;
                                    YPos = (int)(rot.y - rotMin.y);
                                }
                            }
                            else
                            {
                                if (rotMax.y - rotMin.y > 0)
                                {
                                    if (rot.y > rotMin.y)
                                    {
                                        YPassed = true;
                                        YPos = (int)(rotMax.y - rot.y);
                                    }
                                }
                                else
                                {
                                    YPassed = true;
                                    YPos = (int)(rotMax.y - rot.y);
                                }
                            }
                        }

                        Console.WriteLine(YPassed.ToString() + "\n");

                        if (YPassed)
                        {
                            if (colorCodes[(int)(Size.x - XPos / DegreePerPx), (int)(Size.y - YPos / DegreePerPx)] == 0)
                            {
                                colorCodes[(int)(Size.x - XPos / DegreePerPx), (int)(Size.y - YPos / DegreePerPx)] = 1;
                            }
                        }
                    }
                }
            }

            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    Console.Write(colorCodes[x, y]);
                }

                Console.Write("\n");
            }
        }

        public static Color[,] Render()
        {
            //Console.Clear();
            double DegreePerPx = 0.04;
            Color[,] colorCodes = new Color[(int)Size.x, (int)Size.y]; //1 px = 0.04 degree

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    colorCodes[x, y] = new Color(0, 0, 0, 0);
                }
            }

            rotation rotMin = new rotation((int)(Rotation.x - Size.x * DegreePerPx), (int)(Rotation.y - Size.y * DegreePerPx));
            rotation rotMax = new rotation((int)(Rotation.x + Size.x * DegreePerPx), (int)(Rotation.y + Size.y * DegreePerPx));

            Console.WriteLine($"{rotMin.x} ! {rotMin.y} | {rotMax.x} ! {rotMax.y}");

            foreach (var o in Objects.ts)
            {
                foreach (var T in o.Triangles)
                {
                    foreach (var v in T.verticals)
                    {
                        position ps = v.position + o.Position;

                        rotation rot = MathB.CartesienToSphere(ps - Position);

                        Console.WriteLine(MathB.Distance(Position, ps));

                        Console.WriteLine(rot.ToString());

                        int XPos = 0;
                        int YPos = 0;

                        bool XPassed = false;
                        bool YPassed = false;

                        if (rot.x > rotMax.x)
                        {
                            if (rot.x > rotMin.x && rotMax.x - rotMin.x < 0)
                            {
                                XPassed = true;
                                XPos = (int)(rotMin.x - rot.x);
                            }
                        }
                        else
                        {
                            if (rotMax.x - rotMin.x > 0)
                            {
                                if (rot.x > rotMin.x)
                                {
                                    XPassed = true;
                                    XPos = (int)(rotMax.x - rot.x);
                                }
                            }
                            else
                            {
                                XPassed = true;
                                XPos = (int)(rotMax.x - rot.x);
                            }
                        }

                        Console.WriteLine(XPassed.ToString());

                        if (XPassed)
                        {
                            if (rot.y > rotMax.y)
                            {
                                if (rot.y > rotMin.y && rotMax.y - rotMin.y < 0)
                                {
                                    YPassed = true;
                                    YPos = (int)(rot.y - rotMin.y);
                                }
                            }
                            else
                            {
                                if (rotMax.y - rotMin.y > 0)
                                {
                                    if (rot.y > rotMin.y)
                                    {
                                        YPassed = true;
                                        YPos = (int)(rotMax.y - rot.y);
                                    }
                                }
                                else
                                {
                                    YPassed = true;
                                    YPos = (int)(rotMax.y - rot.y);
                                }
                            }
                        }

                        Console.WriteLine(YPassed.ToString() + "\n");

                        if (YPassed)
                        {
                            if (colorCodes[(int)(Size.x - XPos / DegreePerPx), (int)(Size.y - YPos / DegreePerPx)] == new Color(0, 0, 0, 0))
                            {
                                colorCodes[(int)(Size.x - XPos / DegreePerPx), (int)(Size.y - YPos / DegreePerPx)] = T.color;
                            }
                        }
                    }
                }
            }

            return colorCodes;
        }
    }
}
