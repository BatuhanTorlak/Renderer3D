using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderMachineDotNetCore
{
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
        public static position operator *(position a, size b) => new position(a.x * b.x, a.y * b.y, a.z * b.z);
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
        public static rotation operator *(rotation y, double a) => new rotation((int)(a * y.x), (int)(a * y.y), (int)(a * y.z));
        public static rotation operator *(double a, rotation y) => new rotation((int)(a * y.x), (int)(a * y.y), (int)(a * y.z));
        public static rotation operator /(rotation y, double a) => new rotation((int)(y.x / a), (int)(y.y / a), (int)(y.z / a));
        public static rotation operator /(double a, rotation y) => new rotation((int)(a / y.x), (int)(a / y.y), (int)(a / y.z));
        public static bool operator ==(rotation x, rotation y) { return (x.x == y.x && x.y == y.y && x.z == y.z); }
        public static bool operator !=(rotation x, rotation y) { return !(x == y); }

        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

        public rotation(int x, int y, int z)
        {
            this.x = (int)MathB.SetPositive(x) - 360 * (int)(MathB.SetPositive(x) / 360);
            if (x < 0)
            {
                this.x = 360 - this.x;
            }
            this.y = (int)MathB.SetPositive(y) - 360 * (int)(MathB.SetPositive(y) / 360);
            if (y < 0)
            {
                this.y = 360 - this.y;
            }
            this.z = (int)MathB.SetPositive(z) - 360 * (int)(MathB.SetPositive(z) / 360);
            if (z < 0)
            {
                this.z = 360 - this.z;
            }
        }

        public rotation(int x, int y)
        {
            this.x = (int)MathB.SetPositive(x) - 360 * (int)(MathB.SetPositive(x) / 360);
            if (x < 0)
            {
                this.x = 360 - this.x;
            }
            this.y = (int)MathB.SetPositive(y) - 360 * (int)(MathB.SetPositive(y) / 360);
            if (y < 0)
            {
                this.y = 360 - this.y;
            }
            this.z = 0;
        }

        public override string ToString()
        {
            return $"rotation({x}, {y}, {z})";
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
                this = GetPoint(new size(1, 1, 1));
            }
        }
        public double r;
        public double Zenit;
        public double Azimut;

        internal Point GetPoint(size siz)
        {
            Point nPoint = new Point(_position * siz);

            return nPoint;
        }

        public Point(position p)
        {
            _position = p;
            r = MathB.Distance(new position(0, 0, 0), p);
            rotation rot = MathB.CartesienToSphere(_position);
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
}
