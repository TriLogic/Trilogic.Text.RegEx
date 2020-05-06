using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Performs the matching logic for the postfix operator {n, m}.
	/// </summary>
	class RxAtomN2M : RxAtomParent
	{
        #region Mamber Data
        private RxFrame frame=new RxFrame();
		private int minMatch=0, maxMatch=0;
        #endregion

        #region Constructors and Destructors
        public RxAtomN2M(RxAtom atom, int min) : base(atom)
		{
			minMatch = min;
		}

		public RxAtomN2M(RxAtom atom, int min, int max) : base(atom)
		{
			minMatch = min;
			maxMatch = max;
		}
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
			return (maxMatch > 0) ? MatchWithMax(m,f,s) : MatchWithMin(m,f,s);
		}

        private bool MatchWithMax(RxMatch m, RxFrame f, RxCharSource s) 
		{
			int length=0,count=0;

			frame.CopyFrom(f);

			while(child.Match(m,frame,s) && (count < maxMatch))
			{
				count++;
				length+=frame.Length;
				if(frame.Length<1)
					break;
				frame.CopyFrom(f);
				frame.Maxlen -= length;
				frame.Offset += length;
			}

			return success(f,count,length);
		}

        private bool MatchWithMin(RxMatch m, RxFrame f, RxCharSource s)
		{
			int length=0,count=0;

			frame.CopyFrom(f);

			while(child.Match(m,frame,s) && frame.Length<=f.Maxlen)
			{
				count++;
				length+=frame.Length;
				if(frame.Length<1)
					break;
				frame.CopyFrom(f);
				frame.Maxlen -= length;
				frame.Offset += length;

				if(frame.Maxlen<1)
					break;
			}

			return success(f,count,length);
		}
        #endregion

        #region Support Code
        private bool success(RxFrame f, int count, int length)
		{
			if(count>=minMatch) {
				f.Length = length;
				f.Count  = count;
				return true;
			}

			return false;
		}

		public override string ToString() 
		{
			return base.ToString() + "{" + minMatch.ToString() + "," + maxMatch.ToString() + "}";
		}
        #endregion
    }
}
