using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Provide functionality for the implicit infix operation "AND" which has no symbol.
	/// </summary>
	class RxOpAND : RxAtomTree 
	{
        #region Constructors and Destructors
        public RxOpAND()
        {
        }

		public RxOpAND(RxAtom a, RxAtom b) : base(a,b) 
		{
		}
        #endregion

        #region Backtracking
        private bool Backtrack(RxFrame f) 
		{
			if (frameA.Count < 1 || frameA.Length < 1)
				return false;
			frameA.Count   = 0;
			frameA.Maxlen  = frameA.Length - 1;
			frameA.Length  = 0;
			return true;
		}
        #endregion

        #region Child Atom Matching
        protected virtual bool doChildA(RxMatch m, RxFrame f, RxCharSource s) 
		{
			frameA.Count  = 0;
			frameA.Length = 0;

			return (childA != null) && childA.Match(m,frameA,s);
		}

        protected virtual bool doChildB(RxMatch m, RxFrame f, RxCharSource s) 
		{
			// copy state content from the previous match frame
			frameB.Offset = frameA.Offset + frameA.Length;
			frameB.Count  = 0;
			frameB.Length = 0;
			frameB.Maxlen = f.Maxlen;
			frameB.Maxlen = frameB.Maxlen - frameA.Length;
			frameB.Total  = f.Total;

			return (childB != null) && childB.Match(m,frameB,s);
		}
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
			frameA.CopyFrom(f);

			while (doChildA(m,f,s)) 
			{
				if (doChildB(m,f,s))
				{
					f.Length = frameA.Length + frameB.Length;
					f.Count = 1;
					return true;
				}

				if (!Backtrack(f) )
					return false;
			}

			return false;
		}
        #endregion

        #region Support Code
        public override RxAtomTree GetAtomOp()
        {
            return new RxOpAND();
        }
        #endregion
    }
}