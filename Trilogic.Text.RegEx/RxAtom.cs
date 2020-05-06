using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Abstract base class for all Regex Atoms.
	/// </summary>
    internal abstract class RxAtom
    {
        public abstract bool Match(RxMatch m, RxFrame f, RxCharSource s);

        public virtual int ID
        {
            get { return 0; }
            internal set { /* empty */ }
        }

        public virtual string Name
        {
            get { return null; }
            internal set { /* empty */ }
        }
    }
}
