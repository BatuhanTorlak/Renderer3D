using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RenderMachineDotNetCore
{
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
}
