using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Provide matching fro a single char (case sensitive).
	/// </summary>
	class RxCharC : RxAtom 
	{
        #region Member Data
        protected char chr;
        #endregion

        #region Constructors and Destructors
        public RxCharC(char c) 
		{
			chr = c;
		}
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
			// See if we are off the end or...
			if (f.Maxlen >= 1 && (chr == s[f.Offset])) 
			{
				f.Count=f.Length = 1;
				return true;
			}
			return false;
		}
        #endregion

        #region Support Code
        public override string ToString() 
		{
			return Char.ToString(chr);
		}
        #endregion
    }
}
