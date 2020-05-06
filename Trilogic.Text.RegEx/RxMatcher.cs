using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Summary description for RxRegex.
	/// </summary>
	public class RxMatcher
	{
		RxFrame mFrame=new RxFrame();
		RxAtom mAtom=null;
		int mGroups=1;
		string mPattern=null;
        bool mIgnoreCase = false;

		public RxMatcher(string expr)
		{
			Pattern = expr;
		}
		public RxMatcher(string expr, bool ignoreCase)
		{
            mIgnoreCase = ignoreCase;
			Pattern = expr;
		}

		public string Pattern
		{
			get { return mPattern;	}
			set
			{
				RxCompiler c=new RxCompiler();
				mAtom = c.CC(value, mIgnoreCase);
				mGroups = c.Slots;
				mPattern = value;
			}
		}

		public bool MatchImmediate(RxCharSource Input, out RxMatch Match)
		{
			RxMatch m = new RxMatch(mGroups);
			Match = null;

			if(Input==null) 
				throw new RxException("null input string");

			mFrame.Initialize(Input);

			if (mAtom.Match(m, mFrame, Input)) 
			{
				m.SetMatch(mFrame.Offset, mFrame.Length);
				Match = m;

				return true;
			}

			return false;
		}

		public bool MatchFirst(RxCharSource Input, out RxMatch Match)
		{
			RxMatch m = new RxMatch(mGroups);
			Match = null;

			if(Input==null) 
				throw new RxException("null input string");

			mFrame.Initialize(Input);

			while (mFrame.Offset <= Input.Length)
			{
				if (mAtom.Match(m,mFrame, Input)) 
				{
					m.SetMatch(mFrame.Offset, mFrame.Length);
					Match = m;
					return true;
				}

				mFrame.Offset++;
				mFrame.Maxlen--;
			}

			return false;
		}

		public bool MatchNext(RxCharSource Input, RxMatch MatchLast, out RxMatch Match)
		{
			RxMatch m = new RxMatch(mGroups);
			Match = null;

			if (MatchLast==null)
				throw new RxException("invalid previous match");

			mFrame.Initialize(Input);
			mFrame.Offset = MatchLast.Offset;
			mFrame.Maxlen -= mFrame.Offset;

			if (MatchLast.Length < 1) 
			{
				mFrame.Offset++;
				mFrame.Maxlen--;
			}
			else
			{
				mFrame.Offset += MatchLast.Length;
				mFrame.Maxlen -= MatchLast.Length;
			}

			if (mFrame.Maxlen < 0)
			{
				Match = null;
				return false;
			}
			try 
			{
				while (mFrame.Offset <= Input.Length)
				{
					if (mAtom.Match(m, mFrame, Input)) 
					{
						m.SetMatch(mFrame.Offset, mFrame.Length);
						Match = m;
						return true;
					}

					mFrame.Offset++;
					mFrame.Maxlen--;
				}
			}
			catch
			{
                // oops
			}

			return false;
		}

		public RxMatch[] MAtchAll(string input)
		{
			return null;
		}

		public override string ToString()
		{
			if(mAtom!=null)
				return mAtom.ToString();

			return "x?x";
		}
	}
}
