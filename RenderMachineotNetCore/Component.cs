using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderMachineDotNetCore
{
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
}
