using System;
using System.Collections;
using System.Text;

namespace Trilogic.Text.RegEx
{
	public class StatePattern
	{
		public int id;
		internal RxAtom atom;
		public RxMatch match;
	}

	/// <summary>
	/// Summary description for RxLexerState.
	/// </summary>
	public class RxLexerState
	{
		string		mName;
		ArrayList	mAtoms=null;

		public RxLexerState(string Name)
		{
			mName = Name;
			mAtoms = new ArrayList();
		}

		public string Name
		{
			get { return mName; }
		}

        public RxLexerState AddPattern(int PatternID, string RegexPattern)
        {
            return AddPattern(PatternID, RegexPattern, false);
        }

		public RxLexerState AddPattern(int PatternID, string RegexPattern, bool ignoreCase )
		{
			StatePattern sp = new StatePattern();
			RxCompiler c = new RxCompiler();
			sp.atom = c.CC( RegexPattern, ignoreCase );
			sp.match = new RxMatch(c.Slots);
			sp.id = PatternID;
			mAtoms.Add(sp);
            return this;
		}

		public bool HasPattern(int PatternID)
		{
			return (GetPattern(PatternID) == null);
		}

		internal StatePattern GetPattern(int PatternID)
		{
			for(int i=0;i<mAtoms.Count;i++)
				if( this[i].id == PatternID )
					return ((StatePattern)mAtoms[i]);
			return (StatePattern)null;
		}

		public int Count 
		{
			get { return mAtoms.Count; }
		}

		public StatePattern this[int index]
		{
			get { return (StatePattern)mAtoms[index]; }
		}

		public bool MatchLongest(RxCharSource Input, out RxMatch Match, out int MatchID )
		{
			Match = null;
			MatchID = 0;
			RxFrame f = new RxFrame();
			int maxIndex=-1;

			for( int i=0; i< this.Count; i++)
			{
				RxMatch temp = this[i].match;
				RxAtom atom = this[i].atom;
				f.Initialize(Input);
				temp.Reset();
				if ( atom.Match( temp, f, Input ) )
					maxIndex = i;
			}

			if ( maxIndex >= 0 )
			{
				Match = this[maxIndex].match;
				MatchID = this[maxIndex].id;
				return true;
			}

			return false;
		}

		public bool MatchFirst(RxCharSource Input, out RxMatch Match, out int MatchID )
		{
			Match = null; 
			MatchID = 0;
			RxFrame f = new RxFrame();
			for( int i=0; i< this.Count; i++)
			{
				RxMatch temp = this[i].match;
				RxAtom atom = this[i].atom;
				f.Initialize(Input);
				temp.Reset();
				if ( atom.Match( temp, f, Input ) )
				{
					temp.SetMatch( f.Offset, f.Length );
					Match = temp;
					MatchID = this[i].id;
					return true;
				}
			}
			return false;
		}

	}
}
