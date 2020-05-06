using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Provide functionality to match a child atom and record the match by ID and optionally named key.
	/// </summary>
	class RxAtomCapture : RxAtomParent
	{
        #region Member Data
        char mID;
        string mName;
        #endregion

        #region Constructors and Destructors
        public RxAtomCapture(RxAtom child) : base(child)
		{
			mID = '\0';
            mName = null;
		}

		public RxAtomCapture(RxAtom child, char id) : base(child)
		{
		    mID = id;
		}
        #endregion

        #region Matching Code
        public override	bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
			if (base.Match(m,f,s))
			{
                m.SetMatch(f.Offset, f.Length, ID);
				return true;
			}
			return false;
		}
        #endregion

        #region Override Code
        public override string ToString() 
		{
			return base.ToString() + "?";
		}
        #endregion
    }
}
