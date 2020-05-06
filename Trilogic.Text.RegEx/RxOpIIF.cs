using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Performs matching functionality of the postfix operator "?".
	/// </summary>
	class RxOpIIF : RxOpAND
	{
        #region Constructors and Destructors
        public RxOpIIF(RxAtom atomA, RxAtom atomB) : base(atomA, atomB)
		{
		}
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
			frameA.CopyFrom(f);

			if (!doChildA(m,f,s))
                return false;

			if (!doChildB(m,f,s))
                return false;

			f.Length = frameA.Length;
			f.Count  = frameA.Count;

			return true;
		}
        #endregion

        #region support Code
        public override	string ToString()
		{
			return childA.ToString() + "/" + childB.ToString();
		}
        #endregion
    }
}
