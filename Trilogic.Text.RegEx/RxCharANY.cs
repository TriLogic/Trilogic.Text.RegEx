using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Match functionality to match any single char ".".
	/// </summary>
	class RxCharANY : RxAtom
	{
        #region Constructors and Destructors
        public RxCharANY()
		{
		}
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
			if(f.Offset<s.Length && f.Maxlen>0)
				if (s[f.Offset]!='\r')
				{
					f.Length = f.Count = 1;
					return true;
				}
			return false;
		}
        #endregion

        #region Support Code
        public override string ToString() 
		{
			return ".";
		}
        #endregion
    }
}
