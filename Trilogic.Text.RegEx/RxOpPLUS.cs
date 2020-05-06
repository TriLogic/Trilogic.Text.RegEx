using System;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Performs matching functionality for the postfix operator "+".
    /// </summary>
    class RxOpPLUS : RxAtomN2M
	{
		public RxOpPLUS(RxAtom a) : base(a,1)
		{
		}

		public override string ToString() 
		{
			return child.ToString() + "+";
		}
	}
}
