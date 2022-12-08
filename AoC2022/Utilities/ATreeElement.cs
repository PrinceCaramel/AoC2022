using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.Utilities
{
    public abstract class ATreeElement
    {
        #region Fields

        private List<ATreeElement> mChildren = new List<ATreeElement>();

        #endregion Fields

        #region Properties

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

        protected virtual Func<string> DisplaybleInformation
        {
            get
            {
                return () => "";
            }
        }

        #endregion Properties

        #region Constructors

        protected ATreeElement(ATreeElement pParent, string pId)
        {
            this.Parent = pParent;
            this.Id = pId;
        }

        #endregion Constructors

        #region Methods

        public virtual void AddChild(ATreeElement pChild)
        {
            pChild.Parent = this;
            this.Children.Add(pChild);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", DisplaybleInformation(), this.Id);
        }

        public string GetTreeAsString()
        {
            return this.GetTreeAsString(0);
        }

        protected string GetTreeAsString(int pIndentationLevel)
        {
            StringBuilder lStringBuilder = new StringBuilder();
            lStringBuilder.AppendLine(string.Format("{0}-{1}", new string(' ', 2 * pIndentationLevel), this.ToString()));
            foreach (ATreeElement lChild in this.Children)
            {
                lStringBuilder.AppendLine(lChild.GetTreeAsString(pIndentationLevel + 1));
            }
            return lStringBuilder.ToString().Replace("\r\n\r\n", "\r\n");
        }

        #endregion Methods
    }
}
