using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Performs matching functionality for the infix operator "|".
    /// </summary>
    class RxOpOR : RxAtomTree
	{
        #region Constructors and Destructors
        public RxOpOR() : base()
        {
        }

		public RxOpOR(RxAtom atomA, RxAtom atomB) : base(atomA, atomB)
		{
		}
        #endregion

        #region Matchign code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
		{
			RxFrame ret=null;
            bool matchA, matchB;

			frameA.CopyFrom(f);
			frameB.CopyFrom(f);

            matchA = (childA != null) && childA.Match(m, frameA, s);
            matchB = (childB != null) && childB.Match(m, frameB, s);

            if (matchA && matchB)
                ret = (frameA.Length > frameB.Length) ? frameA : frameB;
            else
                ret = matchA ? frameA : (matchB ? frameB : null);

            if (ret == null)
                return false;

			f.Length = ret.Length;
			f.Count  = ret.Count;

			return true;
		}
        #endregion

        public override string ToString()
		{
			return childA.ToString() + "|" + childB.ToString();
		}

        public override RxAtomTree GetAtomOp()
        {
            return new RxOpOR();
        }
    }
}
