using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace RenderMachineDotNetCore
{
    #region DefaultFunctions
    public static class DefaultFuncs
    {
        private static DateTime BackTime;
        public static double DeltaTime;
        public static void Setup(Double SizeX, Double SizeY)
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
    #endregion

    public static class Camera
    {
        public static position Position;
        public static rotation Rotation;
        public static size Size;
        public static double FoV { get; set; }
        public static DList Objects;

        public static void Render()
        {
            Console.Clear();
            double DegreePerPx = 0.1;
            int[,] colorCodes = new int[(int)Size.x, (int)Size.y]; //1 px = 0.04 degree

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    colorCodes[x, y] = 0;
                }
            }

            double RotMinX = Rotation.x - (int)Size.x * DegreePerPx;
            double RotMinY = Rotation.y - (int)Size.y * DegreePerPx;
            rotation_ rotMax = new rotation_(Rotation.x + (int)Size.x * DegreePerPx, Rotation.y + (int)Size.y * DegreePerPx);

            Console.WriteLine($"{RotMinX} ! {RotMinY} | {rotMax.x} ! {rotMax.y}");

            foreach (var o in Objects.ts)
            {
                foreach (var T in o.Triangles)
                {
                    foreach (var v in T.verticals)
                    {
                        position ps = v.position + o.Position;

                        rotation_ rot = MathB.CartesienToSphere(ps - Position);

                        Console.WriteLine(MathB.Distance(Position, ps));

                        Console.WriteLine(rot.ToString());

                        int XPos = 0;
                        int YPos = 0;

                        bool XPassed = false;
                        bool YPassed = false;

                        if (rot.x > rotMax.x)
                        {
                            if (rot.x > RotMinX && rotMax.x - RotMinX < 0)
                            {
                                XPassed = true;
                                XPos = (int)(RotMinX - rot.x);
                            }
                        }
                        else
                        {
                            if (rotMax.x - RotMinX > 0)
                            {
                                if (rot.x > RotMinX)
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
                                if (rot.y > RotMinY && rotMax.y - RotMinY < 0)
                                {
                                    YPassed = true;
                                    YPos = (int)(rot.y - RotMinY);
                                }
                            }
                            else
                            {
                                if (rotMax.y - RotMinY > 0)
                                {
                                    if (rot.y > RotMinY)
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
                            if (colorCodes[(int)(Size.x - XPos / DegreePerPx - 1), (int)(Size.y - YPos / DegreePerPx - 1)] == 0)
                            {
                                colorCodes[(int)(Size.x - XPos / DegreePerPx - 1), (int)(Size.y - YPos / DegreePerPx - 1)] = 1;
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
    }

    #region DistanceList
    public class DList : IEnumerable<Object3D>
    {
        internal List<Object3D> ts;
        private Object3D Target;
        private bool SetCamera;
        private bool setLower;

        public DList(Object3D T)
        {
            ts = new List<Object3D>();
            Target = T;
            SetCamera = false;
            setLower = false;
        }

        public DList()
        {
            ts = new List<Object3D>();
            Target = null;
            SetCamera = true;
            setLower = false;
        }

        public int Lenght
        {
            get
            {
                return ts.Count();
            }
        }

        public int IndexCount
        {
            get
            {
                return ts.Count() - 1;
            }
        }

        public void Add(Object3D o)
        {
            ts.Add(o);
            Set();
        }

        public void AddRange(Object3D[] o)
        {
            ts.AddRange(o);
            Set();
        }

        public void AddRange(List<Object3D> o)
        {
            ts.AddRange(o.ToArray());
            Set();
        }

        public void RemoveAt(int i)
        {
            ts.RemoveAt(i);
        }

        public void RemoveAll(Object3D o)
        {
            ts.Remove(o);
        }

        public void Clear()
        {
            DList ds = new DList();

            ts = ds.ts;
            Target = ds.Target;
            SetCamera = ds.SetCamera;
            setLower = ds.setLower;
        }

        public void Set()
        {
            if (setLower)
            {
                SetLower();
            }
            else
            {
                SetBigger();
            }
        }

        public void SetLower()
        {
            if (Lenght > 0)
            {
                List<Object3D> lw = new List<Object3D>();
                List<Object3D> list = ts;

                for (int c = 0; c < Lenght; c++)
                {
                    Object3D o = null;
                    foreach (var obj in list)
                    {
                        if (o != null)
                        {
                            if (SetCamera)
                            {
                                if (MathB.Distance(Camera.Position, obj.Position) < MathB.Distance(Camera.Position, o.Position))
                                {
                                    o = obj;
                                }
                            }
                            else
                            {
                                if (MathB.Distance(Target.Position, obj.Position) < MathB.Distance(Target.Position, o.Position))
                                {
                                    o = obj;
                                }
                            }
                        }
                        else
                        {
                            o = obj;
                        }
                    }

                    lw.Add(o);
                    list.Remove(o);
                }

                ts = lw;
            }
        }

        public Object3D get(int i)
        {
            return ts[i];
        }

        public Object3D[] get()
        {
            return ts.ToArray();
        }

        public void SetBigger()
        {
            if (Lenght > 0)
            {
                List<Object3D> bg = new List<Object3D>();
                List<Object3D> list = ts;

                for (int c = 0; c < Lenght; c++)
                {
                    Object3D o = null;
                    foreach (var obj in list)
                    {
                        if (o != null)
                        {
                            if (SetCamera)
                            {
                                if (MathB.Distance(Camera.Position, obj.Position) > MathB.Distance(Camera.Position, o.Position))
                                {
                                    o = obj;
                                }
                            }
                            else
                            {
                                if (MathB.Distance(Target.Position, obj.Position) > MathB.Distance(Target.Position, o.Position))
                                {
                                    o = obj;
                                }
                            }
                        }
                        else
                        {
                            o = obj;
                        }
                    }

                    bg.Add(o);
                    list.Remove(o);
                }

                ts = bg;
            }
        }

        public void SetForPosDis(Object3D o)
        {
            Target = o;
            SetCamera = false;

            Set();
        }

        public void SetForCameraDis()
        {
            SetCamera = true;

            Set();
        }

        public IEnumerator<Object3D> GetEnumerator()
        {
            return ((IEnumerable<Object3D>)ts).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ts.GetEnumerator();
        }
    }
    #endregion

    public abstract class Component
    {
        internal Object3D body;
        public string name
        {
            get;
            internal set;
        }
        public bool enabled;

        internal abstract void ComponentWorks();
    }

    public class Collider : Component
    {
        internal override void ComponentWorks()
        {

        }
    }

    #region MathBatuhan
    public static class MathB
    {
        public static double SetPositive(double x)
        {
            double y = 0;

            if (x < 0)
            {
                y = -x;
            }
            else if (x == 0)
            {
                y = 0;
            }
            else
            {
                y = x;
            }

            return y;
        }
        public static double GetBigger(params double[] x)
        {
            double y = double.MinValue;

            foreach (var z in x)
            {
                if (z > y)
                {
                    y = z;
                }
            }

            return y;
        }
        public static double SubstractLittleFromBigger(params double[] x)
        {
            double y = GetBigger(x);

            double Cy = y;

            foreach (var z in x)
            {
                if (z != Cy)
                {
                    y -= z;
                }
            }

            return y;
        }
        public static double Limit(double x, double y)
        {
            return x - (y * (int)(x / y));
        }
        public static double Distance(position x, position y)
        {
            double a1 = SubstractLittleFromBigger(x.x, y.x);
            a1 *= a1;

            double a2 = SubstractLittleFromBigger(x.y, y.y);
            a2 *= a2;

            double a3 = SubstractLittleFromBigger(x.z, y.z);
            a3 *= a3;

            return (a1 + a2 + a3 != 0) ? Math.Sqrt(a1 + a2 + a3) : 0;
        }

        public static rotation_ CartesienToSphere(position Pos)
        {
            // double O = Math.Atan(ps.x / ps.z);
            // double oT = Math.Acos(ps.y / MathB.Distance(new position(0, 0, 0), ps));
            rotation_ rot = new rotation_(Math.Acos(Pos.y / Distance(new position(0, 0, 0), Pos)) * 180, Math.Atan(Pos.x / Pos.z) * 180);

            return rot;
        }
    }
    #endregion

    #region Vectors
    public struct position
    {
        public static position operator +(position y, position a) => new position(a.x + y.x, a.y + y.y, a.z + y.z);
        public static position operator -(position a, position y) => new position(a.x - y.x, a.y - y.y, a.z - y.z);
        public static position operator *(position y, position a) => new position(a.x * y.x, a.y * y.y, a.z * y.z);
        public static position operator *(position y, double a) => new position(a * y.x, a * y.y, a * y.z);
        public static position operator *(double a, position y) => new position(a * y.x, a * y.y, a * y.z);
        public static position operator /(position y, double a) => new position(y.x / a, y.y / a, y.z / a);
        public static position operator /(double a, position y) => new position(a / y.x, a / y.y, a / y.z);
        public static position operator -(position x) => new position(-x.x, -x.y, -x.z);
        public static bool operator ==(position x, position y) { return (x.x == y.x && x.y == y.y && x.z == y.z); }
        public static bool operator !=(position x, position y) { return !(x == y); }

        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public position(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"position({x}, {y}, {z})";
        }
    }

    public struct size
    {
        public static size operator +(size y, size a) => new size(a.x + y.x, a.y + y.y, a.z + y.z);
        public static size operator -(size a, size y) => new size(a.x - y.x, a.y - y.y, a.z - y.z);
        public static size operator *(size y, size a) => new size(a.x * y.x, a.y * y.y, a.z * y.z);
        public static size operator *(size y, double a) => new size(a * y.x, a * y.y, a * y.z);
        public static size operator *(double a, size y) => new size(a * y.x, a * y.y, a * y.z);
        public static size operator /(size y, double a) => new size(y.x / a, y.y / a, y.z / a);
        public static size operator /(double a, size y) => new size(a / y.x, a / y.y, a / y.z);
        public static bool operator ==(size x, size y) { return (x.x == y.x && x.y == y.y && x.z == y.z); }
        public static bool operator !=(size x, size y) { return !(x == y); }

        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public size(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"size({x}, {y}, {z})";
        }
    }

    public struct rotation
    {
        public static rotation operator +(rotation y, rotation a) => new rotation(a.x + y.x, a.y + y.y, a.z + y.z);
        public static rotation operator -(rotation a, rotation y) => new rotation(a.x - y.x, a.y - y.y, a.z - y.z);
        public static rotation operator *(rotation y, rotation a) => new rotation(a.x * y.x, a.y * y.y, a.z * y.z);
        public static rotation operator *(rotation y, double a) => new rotation(a * y.x, a * y.y, a * y.z);
        public static rotation operator *(double a, rotation y) => new rotation(a * y.x, a * y.y, a * y.z);
        public static rotation operator /(rotation y, double a) => new rotation(y.x / a, y.y / a, y.z / a);
        public static rotation operator /(double a, rotation y) => new rotation(a / y.x, a / y.y, a / y.z);
        public static bool operator ==(rotation x, rotation y) { return (x.x == y.x && x.y == y.y && x.z == y.z); }
        public static bool operator !=(rotation x, rotation y) { return !(x == y); }

        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public rotation(double x, double y, double z)
        {
            this.x = MathB.SetPositive(x) - 360 * (int)(MathB.SetPositive(x) / 360);
            this.y = MathB.SetPositive(y) - 360 * (int)(MathB.SetPositive(y) / 360);
            this.z = MathB.SetPositive(z) - 360 * (int)(MathB.SetPositive(z) / 360);
        }

        public override string ToString()
        {
            return $"rotation({x}, {y}, {z})";
        }
    }

    public struct rotation_
    {
        public static bool operator ==(rotation_ x, rotation_ y) { return (x.x == y.x && x.y == y.y); }
        public static bool operator !=(rotation_ x, rotation_ y) { return !(x == y); }

        internal double x { get; set; }
        internal double y { get; set; }
        internal rotation_(double x, double y, double Xmax, double Xmin, double Ymax, double Ymin, double Yplus)
        {
            if (x > Xmax)
            {
                y += Yplus;
                this.x = MathB.Limit(x, Xmax);
            }
            else if (x < Xmin)
            {
                y += Yplus;
                this.x = Xmin - MathB.Limit(x, Xmin);
            }
            else
            {
                this.x = x;
            }

            if (y > Ymax)
            {
                this.y = MathB.Limit(y, Ymax);
            }
            else if (y < Ymin)
            {
                this.y = Ymax - MathB.Limit(y, Ymax);
            }
            else
            {
                this.y = y;
            }
        }

        internal rotation_(double x, double y)
        {
            if (x > 180)
            {
                y += 180;
                this.x = MathB.Limit(x, 180);
            }
            else if (x < 0)
            {
                y += 180;
                this.x = MathB.Limit(-x, 180);
            }
            else
            {
                this.x = x;
            }

            if (y > 360)
            {
                this.y = MathB.Limit(y, 360);
            }
            else if (y < 0)
            {
                this.y = 360 - MathB.Limit(-y, 360);
            }
            else
            {
                this.y = y;
            }
        }

        public override string ToString()
        {
            return $"rotation_({x}, {y})";
        }

        public rotation ToRotation()
        {
            return new rotation(x, y, 0);
        }
    }

    public struct Point
    {
        public static Point operator +(Point y, Point a) => new Point(a.position.x + y.position.x, a.position.y + y.position.y, a.position.z + y.position.z);
        public static Point operator -(Point a, Point y) => new Point(a.position.x - y.position.x, a.position.y - y.position.y, a.position.z - y.position.z);
        public static Point operator *(Point y, Point a) => new Point(a.position.x * y.position.x, a.position.y * y.position.y, a.position.z * y.position.z);
        public static Point operator *(Point y, double a) => new Point(a * y.position.x, a * y.position.y, a * y.position.z);
        public static Point operator *(double a, Point y) => new Point(a * y.position.x, a * y.position.y, a * y.position.z);
        public static Point operator /(Point y, double a) => new Point(y.position.x / a, y.position.y / a, y.position.z / a);
        public static Point operator /(double a, Point y) => new Point(a / y.position.x, a / y.position.y, a / y.position.z);
        public static bool operator ==(Point x, Point y) { return (x.position.x == y.position.x && x.position.y == y.position.y && x.position.z == y.position.z); }
        public static bool operator !=(Point x, Point y) { return !(x == y); }
        internal position _position;
        public position position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                this = GetPoint();
            }
        }
        internal double r;
        internal double Zenit;
        internal double Azimut;

        internal Point GetPoint()
        {
            Point nPoint = new Point(_position);
            nPoint.r = MathB.Distance(new position(0, 0, 0), _position);
            rotation_ rot = MathB.CartesienToSphere(_position);
            Zenit = rot.x;
            Azimut = rot.y;

            return nPoint;
        }

        public Point(position p)
        {
            _position = p;
            r = MathB.Distance(new position(0, 0, 0), p);
            rotation_ rot = MathB.CartesienToSphere(_position);
            Zenit = rot.x;
            Azimut = rot.y;
        }
        public Point(double x, double y, double z)
        {
            this = new Point(new position(x, y, z));
        }
        public override string ToString()
        {
            return $"point({position.x}, {position.y}, {position.z})";
        }
    }

    public class Triangle
    {
        public Point[] verticals = new Point[3];

        public Triangle(Point px, Point py, Point pz)
        {
            this.verticals = new Point[3] { px, py, pz };
        }

        public void SetVerticals(Point px, Point py, Point pz)
        {
            this.verticals = new Point[3] { px, py, pz };
        }
    }
    #endregion

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
                double r = MathB.Distance(_verticals[c].position, new position(0, 0, 0));

                //if (_verticals[c].Azimut < 0)
                Verticals[c] = new Point(
                    r * Math.Sin(new rotation_(0, _verticals[c].Azimut + Rotation.y).y) * Math.Sin(new rotation_(_verticals[c].Zenit + Rotation.x, 0).x),
                    r * Math.Cos(new rotation_(_verticals[c].Zenit + Rotation.x, 0).x),
                    r * Math.Cos(new rotation_(0, _verticals[c].Azimut + Rotation.y).y) * Math.Sin(new rotation_(_verticals[c].Zenit + Rotation.x, 0).x)
                    );
            }
        }
    }

    public static class Generators
    {
        public static Object3D GetObject3D()
        {
            Object3D o = new Object3D();

            o.Position = new position(0, 0, 0);
            o._rotation = new rotation(0, 0, 0);
            o.Size = new size(1, 1, 1);

            o.path = "Default";
            o._verticals.AddRange(new Point[8] {
                new Point(0.5f,0.5f,0.5f), //0.5f,0.5f,0.5f 0
                new Point(-0.5f,0.5f,-0.5f), //-0.5f,0.5f,-0.5f 1
                new Point(0.5f,0.5f,-0.5f), //0.5f,0.5f,-0.5f 2
                new Point(-0.5f,0.5f,0.5f), //-0.5f,0.5f,0.5f 3 
                new Point(0.5f,-0.5f,0.5f), //0.5f,-0.5f,0.5f 4 
                new Point(-0.5f,-0.5f,-0.5f), //-0.5f,-0.5f,-0.5f 5
                new Point(0.5f,-0.5f,-0.5f), //0.5f,-0.5f,-0.5f 6
                new Point(-0.5f,-0.5f,0.5f) //-0.5f,-0.5f,0.5f 7
            });

            o.Verticals = new Point[o._verticals.Count()];

            for (int x = 0; x < o._verticals.Count(); x++)
            {
                o.Verticals[x] = o._verticals[x].GetPoint();
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
                o.Verticals[x] = o._verticals[x].GetPoint();
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
            Point[] verticals = new Point[0];

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
                        verticals = new Point[VerticalCount];

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
                                verticals[countIndex] =
                                    new Point(
                                        Convert.ToSingle(VerPosX),
                                        Convert.ToSingle(VerPosY),
                                        Convert.ToSingle(VerPosZ)
                                );

                                countIndex++;
                            }
                        }
                    }
                    else
                    {
                        path = "Default";
                        verticals = new Point[8] {
                        new Point(0.5f,0.5f,0.5f),
                        new Point(-0.5f,0.5f,-0.5f),
                        new Point(0.5f,0.5f,-0.5f),
                        new Point(-0.5f,0.5f,0.5f),
                        new Point(0.5f,-0.5f,0.5f),
                        new Point(-0.5f,-0.5f,-0.5f),
                        new Point(0.5f,-0.5f,-0.5f),
                        new Point(-0.5f,-0.5f,0.5f)
                        };
                    }
                }
                else
                {
                    path = "Default";
                    verticals = new Point[8] {
                        new Point(0.5f,0.5f,0.5f),
                        new Point(-0.5f,0.5f,-0.5f),
                        new Point(0.5f,0.5f,-0.5f),
                        new Point(-0.5f,0.5f,0.5f),
                        new Point(0.5f,-0.5f,0.5f),
                        new Point(-0.5f,-0.5f,-0.5f),
                        new Point(0.5f,-0.5f,-0.5f),
                        new Point(-0.5f,-0.5f,0.5f)
                    };
                }
            }
            else
            {
                path = "Default";
                verticals = new Point[8] {
                        new Point(0.5f,0.5f,0.5f),
                        new Point(-0.5f,0.5f,-0.5f),
                        new Point(0.5f,0.5f,-0.5f),
                        new Point(-0.5f,0.5f,0.5f),
                        new Point(0.5f,-0.5f,0.5f),
                        new Point(-0.5f,-0.5f,-0.5f),
                        new Point(0.5f,-0.5f,-0.5f),
                        new Point(-0.5f,-0.5f,0.5f)
                    };
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
                        verticals.AddRange(new Point[8] {
                        new Point(0.5f,0.5f,0.5f),
                        new Point(-0.5f,0.5f,-0.5f),
                        new Point(0.5f,0.5f,-0.5f),
                        new Point(-0.5f,0.5f,0.5f),
                        new Point(0.5f,-0.5f,0.5f),
                        new Point(-0.5f,-0.5f,-0.5f),
                        new Point(0.5f,-0.5f,-0.5f),
                        new Point(-0.5f,-0.5f,0.5f)
                        });
                    }
                }
                else
                {
                    path = "Default";
                    verticals.AddRange(new Point[8] {
                        new Point(0.5f,0.5f,0.5f),
                        new Point(-0.5f,0.5f,-0.5f),
                        new Point(0.5f,0.5f,-0.5f),
                        new Point(-0.5f,0.5f,0.5f),
                        new Point(0.5f,-0.5f,0.5f),
                        new Point(-0.5f,-0.5f,-0.5f),
                        new Point(0.5f,-0.5f,-0.5f),
                        new Point(-0.5f,-0.5f,0.5f)
                    });
                }
            }
            else
            {
                path = "Default";
                verticals.AddRange(new Point[8] {
                    new Point(0.5f,0.5f,0.5f),
                    new Point(-0.5f,0.5f,-0.5f),
                    new Point(0.5f,0.5f,-0.5f),
                    new Point(-0.5f,0.5f,0.5f),
                    new Point(0.5f,-0.5f,0.5f),
                    new Point(-0.5f,-0.5f,-0.5f),
                    new Point(0.5f,-0.5f,-0.5f),
                    new Point(-0.5f,-0.5f,0.5f)
                });
            }
            return verticals;
        }
    }
}
