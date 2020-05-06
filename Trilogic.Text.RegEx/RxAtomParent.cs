using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Summary description for RxParentAtom.
	/// </summary>
	abstract class RxAtomParent : RxAtom
	{
		protected RxAtom child;
		
		public RxAtomParent()
		{
			child=null;
		}

		public RxAtomParent(RxAtom atom)
		{
			child = atom;
		}

		public RxAtom Child
		{
			get { return child; }
			set	{ child = value; }
		}

		public override	bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
			return child.Match(m,f,s);
		}

        public override	string ToString() 
		{
			return child.ToString();
		}
	}
}