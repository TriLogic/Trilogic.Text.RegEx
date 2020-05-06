using System;
using System.IO;
using System.Text;
using System.Collections;

using Trilogic.Text.RegEx;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Stame management structure used by the Lexer Generator.
    /// </summary>
	public struct StateToken
	{
		string pattern_name;
		string token_name;
		
		public StateToken( string pattern, string token )
		{
			pattern_name = pattern;
			token_name = token;
		}
	}

	public enum LexerMsgClass
	{
		Info = 0,
		Warning,
		Error
	}

	public struct LexerMessage
	{
		LexerMsgClass Class;
		int Row, Col;
		string Message;

		internal LexerMessage( LexerMsgClass cls, int row, int col, string message )
		{
			Class = cls;
			Row = row;
			Col = col;
			Message = message;
		}
	}

	public enum TokenID:int
	{
		tk_ignore = 0,
		tk_comment = 0,
		tk_base = 2048,
		tk_default,
		tk_linefeed,
		tk_end_block,
		tk_scanner,
		tk_macro,
		tk_pattern,
		tk_token,
		tk_state,
		tk_block,
		tk_id_block,
		tk_id_state,
		tk_name,
		tk_regex,
		tk_else,
		tk_consume
	}

	/// <summary>
	/// A Lexer Generator that reads input files and outputs source code for a multi-state lexer.
	/// </summary>
	public class RxLexerGen
	{
		private const string rgx_white		= "\\s+";
		private const string rgx_linefeed	= "(\\r\\n)|(\\n)|(\\r)";
		private const string rgx_comment	= "#.*";
		private const string rgx_end_block	= "^\\.";
		private const string rgx_scanner	= "^\\.scanner";
		private const string rgx_macro		= "^\\.macro";
		private const string rgx_pattern	= "^\\.pattern";
		private const string rgx_token		= "^\\.token";
		private const string rgx_state		= "^\\.state";
		private const string rgx_block		= "^\\.[a-zA-Z][a-zA-Z0-9_]*";
		private const string rgx_id_block	= "^\\.<([a-zA-Z][a-zA-Z0-9_]*)?>\\s*[a-zA-Z][a-zA-Z0-9_]*";
		private const string rgx_id_state	= "<([a-zA-Z][a-zA-Z0-9_]*)?>";
		private const string rgx_name		= "[a-zA-Z][a-zA-Z0-9_]*";
		private const string rgx_regex		= "\\/([^/]|(\\/\\/))+\\/";
		private const string rgx_else		= "^.+";
		private const string rgx_consume	= ".*(\\r\\n)|(\\n)|(\\r)";

		ArrayList	mMessages=null;

		Hashtable	mMacros;
		string		mCurrMacroName=null;
		ArrayList	mCurrMacroArray=null;

		Hashtable	mCodeBlocks;
		string		mCurrBlockName=null;
		ArrayList	mCurrBlockArray=null;

		Hashtable	mTokensD;
		ArrayList	mTokensA;

		Hashtable	mPatterns;
		Hashtable	mStates;
		RxLexer		mLex;
		RxMatcher		mRegexSubs=null;
		string		mScannerName=null;

		RxMatch mMatch = null;
		int		mMatchID = 0;
		string	mMatchValue = null;

		public RxLexerGen()
		{
			mMessages = new ArrayList();

			mMacros = new Hashtable();
			mCodeBlocks = new Hashtable();
			
			mTokensD = new Hashtable();
			mTokensA = new ArrayList();

			mPatterns = new Hashtable();
			mStates = new Hashtable();
			mLex = null;
			mRegexSubs = new RxMatcher("\\{$[a-zA-Z][a-zA-Z0-9_]*\\}");
		}

		private void Reset()
		{
			//
			// Default State
			//
			mLex = new RxLexer();

			mLex.State("default")
			    .AddPattern((int)TokenID.tk_ignore, rgx_white)
			    .AddPattern((int)TokenID.tk_linefeed, rgx_linefeed)
			    .AddPattern((int)TokenID.tk_comment, rgx_comment)
			    .AddPattern((int)TokenID.tk_scanner, rgx_scanner);

            //
            // Scanner State
            //
            mLex.StateAdd("scanner")
                .AddPattern((int)TokenID.tk_pattern, rgx_pattern)
                .AddPattern((int)TokenID.tk_linefeed, rgx_linefeed)
                .AddPattern((int)TokenID.tk_macro, rgx_macro)
                .AddPattern((int)TokenID.tk_token, rgx_token)
                .AddPattern((int)TokenID.tk_state, rgx_state)
                .AddPattern((int)TokenID.tk_block, rgx_block)
                .AddPattern((int)TokenID.tk_id_block, rgx_id_block)
                .AddPattern((int)TokenID.tk_end_block, rgx_end_block)
                .AddPattern((int)TokenID.tk_else, rgx_else);


			//
			// Expect a valid name identifier
			//
			mLex.StateAdd("expect_name")
			    .AddPattern((int)TokenID.tk_ignore, rgx_white)
			    .AddPattern((int)TokenID.tk_name, rgx_name);

            //
            // Pattern State
            //
            mLex.StateAdd("pattern")
               .AddPattern((int)TokenID.tk_regex, rgx_regex)
               .AddPattern((int)TokenID.tk_end_block, rgx_end_block)
               .AddPattern((int)TokenID.tk_ignore, rgx_white)
               .AddPattern((int)TokenID.tk_linefeed, rgx_linefeed)
               .AddPattern((int)TokenID.tk_comment, rgx_comment)
               .AddPattern((int)TokenID.tk_name, rgx_name);

            //
            // Pattern State
            //
            mLex.StateAdd("token")
                .AddPattern((int)TokenID.tk_end_block, rgx_end_block)
                .AddPattern((int)TokenID.tk_ignore, rgx_white)
                .AddPattern((int)TokenID.tk_linefeed, rgx_linefeed)
                .AddPattern((int)TokenID.tk_comment, rgx_comment)
                .AddPattern((int)TokenID.tk_name, rgx_name);

			//
			// 'State' State
			//
			mLex.StateAdd("state")
			    .AddPattern((int)TokenID.tk_end_block, rgx_end_block)
			    .AddPattern((int)TokenID.tk_ignore, rgx_white)
			    .AddPattern((int)TokenID.tk_linefeed, rgx_linefeed)
			    .AddPattern((int)TokenID.tk_comment, rgx_comment)
			    .AddPattern((int)TokenID.tk_id_state, rgx_id_state)
                .AddPattern((int)TokenID.tk_name, rgx_name);


			//
			// Block State
			//
			mLex.StateAdd("block")
			    .AddPattern((int)TokenID.tk_linefeed, rgx_linefeed)
			    .AddPattern((int)TokenID.tk_end_block, rgx_end_block)
			    .AddPattern((int)TokenID.tk_else, rgx_else);

			//
			// Consume everything up to the end of line
			//
			mLex.StateAdd("consume")
			    .AddPattern((int)TokenID.tk_consume, rgx_consume);

		}

        public void Scan(FileInfo input)
        {
            StreamReader reader = input.OpenText();
            Scan(reader);
            reader.Close();
        }

        public void Scan(string input)
        {
            mLex.AppendText(input);
            Scan();
        }

        public void Scan(StringBuilder input)
        {
            mLex.AppendText(input);
            Scan();
        }

        public void Scan(StreamReader input)
		{
            mLex.AppendText(input);
            Scan();
		}

        public void Scan()
        {
            Reset();

            while (mLex.Match(out mMatch, out mMatchID, out mMatchValue))
            {
                PrintMatch();
                if (mMatchID == (int)TokenID.tk_scanner)
                    ScanScanner();
            }

            if (mLex.StatePeek().Name.CompareTo("default") != 0)
                AddError("unexpected end of file");
        }

        public void ScanScanner()
		{
			// Reguire a valid name for the scanner
			mLex.StatePush("scanner");
			mLex.StatePush("expect_name");
			if ( !mLex.Match( out mMatch, out mMatchID, out mMatchValue ) ) 
			{
				mLex.StatePop();
				AddError( "missing scanner name" );
			}
			mLex.StatePop();
			mScannerName = mMatchValue;

			while ( mLex.Match( out mMatch, out mMatchID, out mMatchValue ) )
			{
				PrintMatch();
				switch ( mMatchID )
				{
					case (int)TokenID.tk_macro:
						ScanMacro();
						break;

					case (int)TokenID.tk_pattern:
						ScanPatterns();
						break;

					case (int)TokenID.tk_token:
						ScanTokens();
						break;

					case (int)TokenID.tk_state:
						ScanStates();
						break;

					case (int)TokenID.tk_block:
						ScanBlock();
						break;

					case (int)TokenID.tk_id_block:
						ScanStateBlock();
						break;

					case (int)TokenID.tk_end_block:
						mLex.StatePop();
						return;
				}
			}
			mLex.StatePop();
		}

		public void ScanStates()
		{
			bool more = true;
			string name=null, state=null, token=null;
			mLex.StatePush( "state" );
			while ( mLex.Match( out mMatch, out mMatchID, out mMatchValue ) && more )
			{
				PrintMatch();
				switch ( mMatchID )
				{
					case (int)TokenID.tk_id_state:
						if(state!=null)
							ExpectedIdentifier();
						state = mMatchValue;
						name = token = null;
						break;

					case (int)TokenID.tk_name:
						if ( name == null )
						{
							name = mMatchValue;
						}
						else
						{
							if (token == null)
							{
								token = mMatchValue;
							}
							else
							{
								if ( token != null )
									ExpectedIdentifier();
							}
						}
						break;

					case (int)TokenID.tk_linefeed:
						if ( state != null && name == null )
							ExpectedIdentifier();
						if ( name != null )
							AddStateToken( state, name, token );
						state = name = token = null;
						break;

					case (int)TokenID.tk_comment:
						if ( state != null && name == null )
							ExpectedIdentifier();
						break;

					case (int)TokenID.tk_end_block:
						if ( state != null )
							ExpectedIdentifier();
						more = false;
						break;
				}
			}
			mLex.StatePop();
			Console.WriteLine( "ALL STATES READ" );
		}

		public void ScanPatterns()
		{
			bool more = true;
			string name=null, regex=null;
			mLex.StatePush( "pattern" );
			while ( mLex.Match( out mMatch, out mMatchID, out mMatchValue ) && more )
			{
				PrintMatch();
				switch ( mMatchID )
				{
					case (int)TokenID.tk_name:
						if ( name != null )
							ExpectedIdentifier();
						name = mMatchValue;
						break;

					case (int)TokenID.tk_regex:
						if ( name == null )
							ExpectedIdentifier();
						regex = mMatchValue;
						AddPattern( name, regex );
						name = regex =null;
						break;

					case (int)TokenID.tk_linefeed:
						break;

					case (int)TokenID.tk_comment:
						break;

					case (int)TokenID.tk_end_block:
						if (name!=null)
							Expected("pattern");
						more = false;
						break;
				}
			}
			mLex.StatePop();
			Console.WriteLine( "ALL PATTERNS READ" );
		}
		void CompileRegexList()
		{
			IEnumerator enm = mPatterns.Keys.GetEnumerator();
			while (enm.MoveNext())
			{
				RxMatcher rx = new RxMatcher( (string)mPatterns[ enm.Current ] );
			}
		}

		public void ScanTokens()
		{
			bool more = true;
			mLex.StatePush( "token" );
			while ( mLex.Match( out mMatch, out mMatchID, out mMatchValue ) && more )
			{
				PrintMatch();
				switch ( mMatchID )
				{
					case (int)TokenID.tk_name:
						AddToken( mMatchValue );
						break;

					case (int)TokenID.tk_linefeed:
						break;

					case (int)TokenID.tk_comment:
						break;

					case (int)TokenID.tk_end_block:
						if ( mTokensA.Count == 0 )
							AddError("no tokes defined");
						more = false;
						break;
				}
			}
			mLex.StatePop();
			Console.WriteLine( "ALL TOKENS READ" );
		}

		public void ScanMacro()
		{
			// expect a macro name
			mLex.StatePush("expect_name");
			if ( !mLex.Match( out mMatch, out mMatchID, out mMatchValue ) ) 
			{
				mLex.StatePop();
				throw new Exception( "missing identifer" );
			}
			mLex.StatePop();

			// begin a new macro block
			mCurrMacroName = mMatchValue;
			mCurrMacroArray = new ArrayList();

			// consume remaining junk from the definition line
			Consume();
			mLex.StatePush( "block" );
			while ( mLex.Match( out mMatch, out mMatchID, out mMatchValue ) )
			{
				PrintMatch();
				if ( mMatchID == (int)TokenID.tk_end_block )
					break;
			}
			mLex.StatePop();

			// add the macro code block to the content
			AddMacro();

			mCurrMacroName = null;
			mCurrMacroArray = null;
		}


		public void ScanStateBlock()
		{
			// begin a new code block
			mCurrBlockName = mMatchValue.Substring( 1, mMatchValue.Length - 1);
			mCurrBlockArray = new ArrayList();

			// consume remaining junk from the definition line
			Consume();
			mLex.StatePush( "block" );
			while ( mLex.Match( out mMatch, out mMatchID, out mMatchValue ) )
			{
				PrintMatch();
				if ( mMatchID == (int)TokenID.tk_end_block )
					break;
			}
			mLex.StatePop();

			// add the <state>token code block to the content
			mCodeBlocks.Add( mCurrBlockName, mCurrBlockArray );
			mCurrBlockName = null;
			mCurrBlockArray = null;
		}

		public void ScanBlock()
		{
			// begin a new code block
			mCurrBlockName = mMatchValue.Substring( 1, mMatchValue.Length - 1);
			mCurrBlockArray = new ArrayList();
			
			// consume remaining junk from the definition line
			Consume();

			// read the block content
			mLex.StatePush( "block" );
			while ( mLex.Match( out mMatch, out mMatchID, out mMatchValue ) )
			{
				PrintMatch();
				mCurrBlockArray.Add( mMatchValue );
				if ( mMatchID == (int)TokenID.tk_end_block )
					break;
			}
			mLex.StatePop();

			// add the code block to the content
			mCodeBlocks.Add( mCurrBlockName, mCurrBlockArray );
			mCurrBlockName = null;
			mCurrBlockArray = null;
		}

		private void AddToken( string token )
		{
			if ( mTokensD.ContainsKey( token ) )
				throw new Exception("token redefinition" );
			mTokensD.Add( token, token );
			mTokensA.Add( token );
		}

		private void AddStateToken( string state_name, string pattern_name, string token_name )
		{
			string tmp_state_name, tmp_token_name;
			tmp_state_name = (state_name==null)?("default"):state_name.Substring(1,state_name.Length-2);
			tmp_token_name = (token_name==null)?(pattern_name):(token_name);

			if ( !mStates.ContainsKey( tmp_state_name ) )
				mStates.Add( tmp_state_name, new ArrayList() );
			ArrayList state = (ArrayList)mStates[tmp_state_name];
			state.Add( new StateToken(pattern_name, tmp_token_name) );
		}

		private void AddPattern(string name, string regex)
		{
			string temp;
			if ( !mPatterns.ContainsKey( name ) )
			{
				temp = regex.Substring( 1, regex.Length - 2 );
				temp = temp.Replace("//", "/");
				mPatterns.Add( name, temp );
			}
			else
				AddError( "pattern redefinition" );
		}

		private void AddMacro()
		{
			if (mMacros.ContainsKey( mCurrMacroName ) )
				mMacros.Remove( mCurrMacroName );
			mMacros.Add( mCurrMacroName, mCurrMacroArray );
		}

		private void Consume()
		{
			mLex.StatePush( "consume" );
			mLex.Match( out mMatch, out mMatchID, out mMatchValue );
			mLex.StatePop();
		}

		private void PrintMatch()
		{
			if ( mMatchID != 0 && mMatchID != (int)TokenID.tk_linefeed )
				Console.WriteLine( "{0}:{1} {2}={3}", fmt(mLex.Row), fmt(mLex.Col), mMatchID, mMatchValue );
		}
		private string fmt( int value )
		{
			return value.ToString().PadLeft(5,'0');
		}

		internal void AddError( string Message )
		{
			mMessages.Add( new LexerMessage( LexerMsgClass.Error, mLex.Row, mLex.Col, Message ) );
		}
		internal void AddWarning( string Message )
		{
			mMessages.Add( new LexerMessage( LexerMsgClass.Warning, mLex.Row, mLex.Col, Message ) );
		}
		internal void AddInfo( string Message )
		{
			mMessages.Add( new LexerMessage( LexerMsgClass.Info, mLex.Row, mLex.Col, Message ) );
		}
		internal void Expected( string expected )
		{
			AddError("expected " + expected );
			throw new Exception("expected pattern name");
		}
		internal void ExpectedIdentifier()
		{
			Expected( "identifier" );
		}

	}
}
