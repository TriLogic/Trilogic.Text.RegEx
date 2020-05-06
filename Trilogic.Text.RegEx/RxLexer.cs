using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Token structure return by the RxLexer object.
    /// </summary>
	public class RxLexerToken
	{
		public int TokenType;
		public string TokenValue;
	}

	/// <summary>
	/// A multi-state text lexer implemented using regular expressions for matching.
	/// </summary>
	public class RxLexer
	{
        #region Class Members
        protected List<RxLexerState> mStatesA=null;
		protected Dictionary<string, RxLexerState> mStatesH=null;
        protected List<RxLexerToken> mTokenList = new List<RxLexerToken>();

		protected Stack<RxLexerState> mStatesS=null;
		protected Stack<StreamReader> mStreamS=null;

        protected StringBuilder mBuilder;
		protected RxCharSource mBuffer;
		protected int mBuffLen;
        protected int mTokenHist;

		protected int		mCol=0;
		protected int		mRow=1;
		protected int		mNextCol=0;
		protected int		mNextRow=1;
		protected bool		mHadCR=false;
        #endregion

        #region Constructors and Destructors
        public RxLexer()
		{
            mBuilder = new StringBuilder();
            mBuffer = new RxStringBuilderSource(mBuilder);

            mStatesA = new List<RxLexerState>();
			mStatesH = new Dictionary<string, RxLexerState>();
			mStatesS = new Stack<RxLexerState>();
			StateAdd( "default" );
			StatePush( "default" );
		}
        #endregion

        #region Current Row and Column
        public int Row
		{
			get { return mRow; }
			set { mRow = value; }
		}

		public int Col
		{
			get { return mCol; }
			set { mCol = value; }
		}
        #endregion

        #region State Management
        public RxLexerState StateAdd(string Name)
		{
			RxLexerState ls = new RxLexerState(Name);
			mStatesH.Add( Name, ls );
			mStatesA.Add( ls );
			return ls;
		}

		public bool StateExists(string Name)
		{
			return mStatesH.ContainsKey(Name);
		}

		public RxLexerState DefaultState
		{
			get { return (RxLexerState)mStatesH["default"]; }
		}

		public RxLexerState State(string Name)
		{
			return mStatesH[Name];
		}
	

		public RxLexerState StatePush(string Name)
		{
			mStatesS.Push( mStatesH[Name] );
			return StatePeek();
		}

		public RxLexerState StatePop( )
		{
			return (RxLexerState)mStatesS.Pop();
		}

		public RxLexerState StatePeek()
		{
			return (RxLexerState)mStatesS.Peek();
		}
        #endregion

        #region UpdateRow & Column
        protected void UpdateRowCol(int offset, int length)
		{
			// while not cr or lf
			mCol = mNextCol;
			mRow = mNextRow;

			if (mBuffer.Length == 0)
				return;

			for(int i=offset; i< (offset+length); i++)
			{
				switch(mBuffer[i])
				{
					default:
						mNextCol++;
						mHadCR=false;
						break;
					case '\r':
						mHadCR=true;
						mNextCol=0;
						mNextRow+=1;
						break;
					case '\n':
						if(!mHadCR)
						{
							mNextCol=0;
							mNextRow+=1;
							mHadCR=true;
						}
						else
							mHadCR = false;
						break;
				}
			}
		}
        #endregion

        public bool Match(out RxMatch Match, out int MatchID, out string MatchValue)
		{
			Match = null;
			MatchID = 0;
			MatchValue = null;

			if (IsEOB)
                return false;

			while ((!IsEOB) && MatchID==0)
			{
				if (!StatePeek().MatchFirst(mBuffer, out Match, out MatchID))
					return false;

				// don't let the user return a zero length match! (infinite loop bait)
				if (Match == null || Match.Length == 0)
					return false;

				// Update row and column information
				UpdateRowCol(Match.Offset, Match.Length);

				// If the match returned a zero id then discard the matching text
                // by removing it from the buffer.
				if (MatchID==0)
				{
                    mBuilder.Remove(0, Match.Length);
				}
			}

			// final match may have been 'ignored'
			if (MatchID == 0)
				return false;

			// Update row and column information
			UpdateRowCol(Match.Offset, Match.Length);

            // Calculate the match value for this match
            MatchValue = mBuilder.ToString(Match.Offset, Match.Length);

			if (Match.Length == mBuffer.Length)
			{
                mBuilder.Remove(0, mBuffer.Length);
			}
			else
			{
                mBuilder.Remove(0, Match.Length);
			}

			return true;
		}

		public bool PeekMatch(out RxMatch Match, out int MatchID, out string MatchValue)
		{
			Match = null;
			MatchID = 0;
			MatchValue = null;

			if (IsEOB)
                return false;

			while ( (!IsEOB) && MatchID==0 )
			{
				if ( ! StatePeek().MatchFirst(mBuffer, out Match, out MatchID ) )
					return false;

				// don't let the user return a zero length match! (infinite loop bait)
				if (Match == null || Match.Length == 0 )
					return false;

				// if the match returned a zero id then discard the match
				if (MatchID==0)
                    mBuilder.Remove(0, Match.Length);
			}

			// final match may have been 'ignored'
			if (MatchID == 0)
				return false;

			MatchValue = mBuilder.ToString(Match.Offset, Match.Length);
			return true;
		}

        public void AppendText(string input)
        {
            mBuilder.Append(input);
        }
        public void AppendText(StringBuilder input)
        {
            mBuilder.Append(input.ToString());
        }
        public void AppentText(FileInfo fileInput)
        {
            AppendText(fileInput.OpenText());
        }
        public void AppendText(StreamReader reader)
        {
            mBuilder.Append(reader.ReadToEnd());
        }

		/// <summary>
		/// Returns whether the current buffer is empty or null
		/// </summary>
		public bool IsEOB
		{
			get { return mBuffer.Length == 0; }
		}
	}
}
