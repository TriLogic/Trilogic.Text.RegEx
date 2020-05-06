using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Performs matching functionality for the infix operator "/".
	/// </summary>
	class RxOpOPTION : RxAtomParent
	{
		public RxOpOPTION(RxAtom atom) : base(atom)
		{
		}

		public override	bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
			base.Match(m,f,s);
			return true;
		}

		public override	string ToString() 
		{
			return base.ToString() + "?";
		}
	}
}
