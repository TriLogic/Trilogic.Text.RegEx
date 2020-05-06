using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Produces a single zero length match.
	/// </summary>
	class RxAtomZMA : RxAtom
	{
        #region Constructors and Destructors
        public RxAtomZMA()
		{
		}
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
		{
			f.Count  = 1;
            f.Length = 0;
			return true;
		}
        #endregion

        #region Support Code
        public override string ToString() 
		{
			return string.Empty;
		}
        #endregion
    }
}
