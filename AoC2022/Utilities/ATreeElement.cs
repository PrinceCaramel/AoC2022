using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Utilities
{
    public abstract class ATreeElement
    {
        private List<ATreeElement> mChildren = new List<ATreeElement>();
        public string Id { get; protected set; }
        public ATreeElement Parent
        {
            get; protected set;
        }
        public ATreeElement Root
        {
            get
            {
                ATreeElement lElement = this;
                while (lElement.Parent != null)
                {
                    lElement = lElement.Parent;
                }
                return lElement;
            }
        }
        public List<ATreeElement> Children
        {
            get
            {
                return this.mChildren;
            }
        }

        public virtual void AddChild(ATreeElement pChild)
        {
            pChild.Parent = this;
            this.Children.Add(pChild);
        }
    }
}
