using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RenderMachineDotNetCore
{
    public class Object3D
    {
        internal rotation _rotation;
        internal List<Point> _verticals = new List<Point>();

        public position Position;
        public size Size;
        public rotation Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                SetVerticals();
            }
        }
        internal string path { get; set; }
        public List<Point> verticals
        {
            get
            {
                return _verticals;
            }
            set
            {
                _verticals = value;
                SetVerticals();
            }
        }

        public List<Triangle> Triangles = new List<Triangle>();

        public List<Component> Components = new List<Component>();

        public Point[] Verticals;

        public void SetNewVerticals(string path)
        {
            verticals = Generators.GetVerticals(path);
        }

        private void SetVerticals()
        {
            Verticals = new Point[_verticals.Count()];
            for (int c = 0; c < _verticals.Count(); c++)
            {
                Verticals[c] = new Point(MathB.SphereToCartesien(new rotation((int)_verticals[c].Zenit + _rotation.x, (int)_verticals[c].Azimut + _rotation.y), MathB.Distance(new position(0, 0, 0), _verticals[c].position * Size), _verticals[c].XPos, _verticals[c].ZPos));
            }
        }
    }

    public static class Generators
    {
        public static Point[] DefaultPoints(int x)
        {
            return new Point[8]
            {
                new Point(x, x, x), //0.5f,0.5f,0.5f 0
                new Point(-x, x, -x), //-0.5f,0.5f,-0.5f x
                new Point(x, x, -x), //0.5f,0.5f,-0.5f 2
                new Point(-x, x, x), //-0.5f,0.5f,0.5f 3 
                new Point(x, -x, x), //0.5f,-0.5f,0.5f 4 
                new Point(-x, -x, -x), //-0.5f,-0.5f,-0.5f 5
                new Point(x, -x, -x), //0.5f,-0.5f,-0.5f 6
                new Point(-x, -x, x) //-0.5f,-0.5f,0.5f 7
            };
        }
        public static Object3D GetObject3D()
        {
            Object3D o = new Object3D();

            o.Position = new position(0, 0, 0);
            o._rotation = new rotation(0, 0, 0);
            o.Size = new size(1, 1, 1);

            o.path = "Default";
            o._verticals.AddRange(DefaultPoints(1));

            o.Verticals = new Point[o._verticals.Count()];

            for (int x = 0; x < o._verticals.Count(); x++)
            {
                o.Verticals[x] = o._verticals[x].GetPoint(o.Size);
            }

            o.Triangles.AddRange(new Triangle[12] {
                new Triangle(o.Verticals[4], o.Verticals[7], o.Verticals[3]),
                new Triangle(o.Verticals[4], o.Verticals[0], o.Verticals[3]),
                new Triangle(o.Verticals[1], o.Verticals[0], o.Verticals[3]),
                new Triangle(o.Verticals[1], o.Verticals[0], o.Verticals[2]),
                new Triangle(o.Verticals[1], o.Verticals[5], o.Verticals[2]),
                new Triangle(o.Verticals[6], o.Verticals[5], o.Verticals[2]),
                new Triangle(o.Verticals[1], o.Verticals[5], o.Verticals[3]),
                new Triangle(o.Verticals[7], o.Verticals[5], o.Verticals[3]),
                new Triangle(o.Verticals[7], o.Verticals[5], o.Verticals[4]),
                new Triangle(o.Verticals[6], o.Verticals[5], o.Verticals[4]),
                new Triangle(o.Verticals[6], o.Verticals[2], o.Verticals[4]),
                new Triangle(o.Verticals[6], o.Verticals[2], o.Verticals[2])
            });

            return o;
        }
        public static Object3D GetObject3D(string path)
        {
            Object3D o = new Object3D();

            o.Position = new position(0, 0, 0);
            o.Rotation = new rotation(0, 0, 0);
            o.Size = new size(1, 1, 1);

            o.verticals = GetVerticals(path);
            o.path = GetPath(path);

            o.Verticals = new Point[o._verticals.Count()];

            for (int x = 0; x < o._verticals.Count(); x++)
            {
                o.Verticals[x] = o._verticals[x].GetPoint(o.Size);
            }

            if (o.path == "Default")
            {
                o.Triangles.AddRange(new Triangle[12] {
                    new Triangle(o.Verticals[4], o.Verticals[7], o.Verticals[3]),
                    new Triangle(o.Verticals[4], o.Verticals[0], o.Verticals[3]),
                    new Triangle(o.Verticals[1], o.Verticals[0], o.Verticals[3]),
                    new Triangle(o.Verticals[1], o.Verticals[0], o.Verticals[2]),
                    new Triangle(o.Verticals[1], o.Verticals[5], o.Verticals[2]),
                    new Triangle(o.Verticals[6], o.Verticals[5], o.Verticals[2]),
                    new Triangle(o.Verticals[1], o.Verticals[5], o.Verticals[3]),
                    new Triangle(o.Verticals[7], o.Verticals[5], o.Verticals[3]),
                    new Triangle(o.Verticals[7], o.Verticals[5], o.Verticals[4]),
                    new Triangle(o.Verticals[6], o.Verticals[5], o.Verticals[4]),
                    new Triangle(o.Verticals[6], o.Verticals[2], o.Verticals[4]),
                    new Triangle(o.Verticals[6], o.Verticals[2], o.Verticals[2])
                });
            }

            return o;
        }
        public static string GetPath(string path)
        {
            if (path.Length > 5)
            {
                if (path[path.Length - 1] == 'j' && path[path.Length - 2] == 'b' && path[path.Length - 3] == 'o')
                {
                    string[] Data = File.ReadAllLines(path);

                    int VerticalCount = 0;

                    foreach (var oVer in Data)
                    {
                        if (oVer[0] == 'v' && oVer[1] == ' ')
                        {
                            VerticalCount++;
                        }
                    }

                    if (VerticalCount <= 0)
                    {
                        path = "Default";
                    }
                }
                else
                {
                    path = "Default";
                }
            }
            else
            {
                path = "Default";
            }
            return path;
        }

        public static List<Point> GetVerticals(string path)
        {
            List<Point> verticals = new List<Point>();

            if (path.Length > 5)
            {
                if (path[path.Length - 1] == 'j' && path[path.Length - 2] == 'b' && path[path.Length - 3] == 'o')
                {

                    string[] Data = File.ReadAllLines(path);

                    int VerticalCount = 0;

                    foreach (var oVer in Data)
                    {
                        if (oVer[0] == 'v' && oVer[1] == ' ')
                        {
                            VerticalCount++;
                        }
                    }

                    if (VerticalCount > 0)
                    {
                        int countIndex = 0;
                        foreach (var oVer in Data)
                        {
                            string Text = "";
                            Nullable<double> VerPosX = null, VerPosY = null, VerPosZ = null;
                            foreach (var oWord in oVer)
                            {
                                if (oWord != ' ')
                                {
                                    Text += oWord;
                                }
                                else
                                {
                                    try
                                    {
                                        int N1 = 0, N2 = 0; bool bf = false;
                                        foreach (var word in Text)
                                        {
                                            if (!bf)
                                            {
                                                if (word != '.')
                                                {
                                                    N1 = N1 * 10 + Convert.ToInt32(word);
                                                }
                                            }
                                            else
                                            {
                                                N2 = N2 * 10 + Convert.ToInt32(word);
                                            }
                                        }

                                        if (VerPosX == null)
                                        {
                                            VerPosX = N1 + N2 / N2.ToString().Length * 10;
                                        }
                                        else if (VerPosY == null)
                                        {
                                            VerPosY = N1 + N2 / N2.ToString().Length * 10;
                                        }
                                        else
                                        {
                                            VerPosZ = N1 + N2 / N2.ToString().Length * 10;
                                        }
                                        Text = "";
                                    }
                                    catch
                                    {
                                        Text = "";
                                    }
                                }
                            }
                            if (VerPosX != null && VerPosY != null && VerPosZ != null)
                            {
                                verticals.Add(
                                    new Point(
                                        Convert.ToSingle(VerPosX),
                                        Convert.ToSingle(VerPosY),
                                        Convert.ToSingle(VerPosZ)
                                ));

                                countIndex++;
                            }
                        }
                    }
                    else
                    {
                        path = "Default";
                        verticals.AddRange(DefaultPoints(1));
                    }
                }
                else
                {
                    path = "Default";
                    verticals.AddRange(DefaultPoints(1));
                }
            }
            else
            {
                path = "Default";
                verticals.AddRange(DefaultPoints(1));
            }
            return verticals;
        }
    }
}
