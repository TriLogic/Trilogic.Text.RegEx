using System;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Performs matching functionality for the postfix operator "*".
    /// </summary>
    class RxOpSTR : RxAtomN2M
	{
		public RxOpSTR(RxAtom a) : base(a,0)
		{
		}

		public override string ToString() 
		{
			return child.ToString() + "*";
		}
	}
}
