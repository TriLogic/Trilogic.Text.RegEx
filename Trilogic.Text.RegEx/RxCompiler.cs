using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Token structure used by the RxCompiler.
    /// </summary>
    class RxRegexToken
	{
		public char   id;
		public int    offset;
		public RxAtom atom;
		public int    ccmax;
		public int    ccmin;
        public string name;
	}

	/// <summary>
	/// Regular expression compiler.
	/// </summary>
	class RxCompiler
	{
		private string mPattern="";
		private int    mOffset=0;

		private Stack<RxRegexToken>       mLeft;
		private Stack<RxRegexToken>       mRight;
        private Stack<bool>          mInfix;
        private Stack<StringBuilder> mGroupStk;

		private char   mChar;
		private int    mSlots=0;
        private bool   mHasInfix = false;
        private bool   mIgnoreCase = false;


        public RxCompiler()
		{
			Reset();
		}

		private void Reset()
		{
			mRight    = new Stack<RxRegexToken>();
			mLeft     = new Stack<RxRegexToken>();
            mInfix    = new Stack<bool>();

            mGroupStk   = new Stack<StringBuilder>();
            mGroupStk.Push(new StringBuilder("0"));

			mPattern  = "";
			mOffset   = 0;
			mChar     = '\0';
			mSlots    = 0;
            mHasInfix = false;
            mIgnoreCase = false;
		}

		public int Slots
		{
			get { return mSlots + 1; }
		}

        public bool IgnoreCase
        {
            get { return mIgnoreCase; }
            set { mIgnoreCase = value; }
        }

		// pushes a token onto the LEFT STACK
		private void PushLeft(char c, int offset, RxAtom atom)
		{
			RxRegexToken t = new RxRegexToken();
			t.id     = c;
			t.atom   = atom;
			t.offset = offset;
			mLeft.Push(t);
		}

		private void PushLeft(int offset, RxAtom atom)
		{
			PushLeft((char)0,offset,atom);
		}

		private void PushLeft(char c)
		{
			PushLeft(c,mOffset,null);
		}

		private void PushLeft(RxAtom atom)
		{
			PushLeft(mOffset,atom);
		}

		private void PushLeft(char c, int imin, int imax, int offset)
		{
			RxRegexToken t = new RxRegexToken();
			t.id     = c;
			t.atom   = null;
			t.offset = offset;
			t.ccmin  = imin;
			t.ccmax  = imax;
			mLeft.Push(t);
		}

		private void PushLeft(RxRegexToken t)
		{
			mLeft.Push(t);
		}

		private RxRegexToken PopLeft()
		{
			return (RxRegexToken)mLeft.Pop();
		}

		private RxRegexToken PeekLeft()
		{
			return (RxRegexToken)mLeft.Peek();
		}

		private void PushRight(RxRegexToken t)
		{
			mRight.Push(t);
		}

		private void PushRight(RxAtom a)
		{
			RxRegexToken t = new RxRegexToken();
			t.atom = a;
			PushRight(t);
		}

		private RxRegexToken PopRight()
		{
			return (RxRegexToken)mRight.Pop();
		}

		private RxRegexToken PeekRight()
		{
			return (RxRegexToken)mRight.Peek();
		}


		// The actual compiler functionality
		public RxAtom CC(string pattern, bool ignoreCase)
		{
			Reset();

			mPattern = pattern;
            mIgnoreCase = ignoreCase;

			while (!EOF) 
			{

				mChar = GetC();
				switch(mChar)
				{
					default:
                        if (mIgnoreCase)
                            PushLeft(new RxCharICASE(mChar));
                        else
						    PushLeft(new RxCharC(mChar));
						break;

					case '[':
						CompileCharClass();
						break;

					case '"':
						CompileQuoted();
						break;

					case '{':
						CompileMulti();
						ResolvePostfix();
						break;

					case '^':
						PushLeft(new RxAnchorH());
						break;

					case '$':
						PushLeft(new RxAnchorT());
						break;

					case '(':
                        CompileGroupBegin();
						break;

					case ')':
                        CompileGroupEnd();
						break;

					case '*':
                    case '+':
                    case '?':
                        PushLeft(mChar);
						ResolvePostfix();
						break;

					case '.':
						PushLeft(new RxCharANY());
						break;

					case '/':
                    case '|':
                        ResolveSubAnd();
						PushLeft(mChar);
                        mHasInfix = true;
						break;

					case '\\':

                        if (char.IsDigit(PeekC()))
                            PushLeft(CompileBackRef());
                        else if (PeekMatch('k'))
                            PushLeft(CompileBackRef());
                        else
                            PushLeft(CompileEscapedAtom());
						break;
				}
			}

            // resolbe any left handed and operation
            ResolveSubAnd();

            if (mHasInfix)
                ResolveInfix();

            ResolveAnd(false, 0, null);

			// Return the atomic operation!
			return ((RxRegexToken)mLeft.Pop()).atom;
		}

        private void CompileGroupBegin()
        {
            int offset = mOffset;

            PushLeft('(');
            mInfix.Push(mHasInfix);

            mHasInfix = false;

            StringBuilder name = new StringBuilder();

            if (PeekMatch('?'))
            {
                GetC();
                if (PeekMatch(':'))
                {
                    // formal group non-capturing
                    GetC();
                    mGroupStk.Push(new StringBuilder("?:"));
                    return;
                }

                if (!PeekMatch('<'))
                    throw new RxException("Invalid group syntax", offset);

                name.Append("?<");

                // formal group with name
                GetC();

                // consume the name chars
                while (!EOF && PeekC() != '>' && char.IsLetter(PeekC()))
                    name.Append(GetC());

                // name must be terminated properly
                if (EOF || PeekC() != '>')
                    throw new RxException("Invalid group syntax", offset);

                // consume the terminal '>'
                GetC();

                // append the seperator
                name.Append('>');
            }

            // include the groupID number
            name.Append($"{++mSlots}");

            // add the name to the group
            mGroupStk.Push(name);
        }

        private void CompileGroupEnd()
        {
            if (mHasInfix)
            {
                ResolveSubAnd();
                ResolveInfix();
            }

            mHasInfix = mInfix.Pop();

            StringBuilder s = mGroupStk.Pop();
            int groupID = 0;
            string groupName = null;

            if (s[0] == '?')
            {
                if (s[1] == ':')
                {
                    // formal group no-capturing
                    ResolveAnd(true, -1, null);
                    return;
                }
                else
                {
                    // capturing named group
                    s.Remove(0, 2);

                    StringBuilder name = new StringBuilder();

                    while (s.Length > 0 && s[0] != '>')
                    {
                        name.Append(s[0]);
                        s.Remove(0, 1);
                    }
                    groupName = name.ToString();

                    // remove the '>' char
                    s.Remove(0, 1);
                }
            }

            groupID = int.Parse(s.ToString());
            ResolveAnd(true, groupID, groupName);
        }

        // compiles a backreference atom
        private RxAtom CompileBackRef()
        {
            int intID = 0;
            char c = GetC();

            if (char.IsDigit(c))
            {
                StringBuilder digits = new StringBuilder();
                digits.Append(c);

                while (!EOF && char.IsDigit(PeekC()))
                {
                    digits.Append(GetC());
                }

                intID = 0;

                if (!int.TryParse(digits.ToString(),out intID))
                    throw new RxException("invalid back reference \\digits", mOffset);

                return new RxAtomBREF($"\\{digits}", intID);
            }

            if (!PeekMatch('{'))
                throw new RxException("invalid back reference syntax", mOffset);

            // consume the '{' chars
            GetC();

            StringBuilder id = new StringBuilder();
            StringBuilder repr = new StringBuilder("\\k{");

            while (!EOF)
            {
                c = PeekC();
                if (!(char.IsLetterOrDigit(c) || c == '_'))
                    break;

                id.Append(c);
                repr.Append(GetC());
            }

            if (PeekC() != '}')
                throw new RxException("invalid back reference syntax", mOffset);

            // consume the final '}'
            repr.Append(GetC());

            if (id.Length < 1)
                throw new RxException("invalid back reference syntax", mOffset);

            intID = 0;
            if (int.TryParse(id.ToString(), out intID))
            {
                if (intID==0)
                    throw new RxException("invalid back reference {0}", mOffset);

                return new RxAtomBREF(repr.ToString(), intID);
            }

            // check for correctness
            if (char.IsDigit(id[0]))
                throw new RxException($"invalid back reference {{{id}}}", mOffset);

            return new RxAtomBREF(repr.ToString(), id.ToString());
        }

        // compiles an escaped character
        private char CompileEscaped()
		{
			char c = GetC();
			switch(c)
			{
				case (char) 98: return (char) 8;
				case (char)102: return (char)12;
				case (char)110: return (char)10;
				case (char)114: return (char)13;
				case (char)116: return (char) 9;
				case (char)117:
				case (char)120:
					return CompileNumeric(16,"0123456789abcdef");
				case (char)111:
					return CompileNumeric( 8,"012345678");
				case (char)105:
					return CompileNumeric(10, "0123456789");
			}
			return c;
		}

		// compiles an escaped character atom
		private RxAtom CompileEscapedAtom()
		{
			char tmp=GetC();
			switch(tmp)
			{
				case 'b': 
				case 'B':
					return new RxAnchorW(tmp=='B');
				case 'c': 
				case 'C':
					return new RxCharCTL(tmp=='C');
				case 'd':
				case 'D':
					return new RxCharDIGIT(tmp=='D');
                case 'l':
                case 'L':
                    return new RxCharLWR(tmp == 'L');
                case 'p':
				case 'P':
					return new RxCharPUN(tmp=='P');
				case 's':
				case 'S':
					return new RxCharWHT(tmp=='S');
                case 'u':
                case 'U':
                    return new RxCharUPR(tmp == 'L');
                case 'w':
				case 'W':
					return new RxCharWRD(tmp=='W');
				case (char)102:
					return new RxCharC((char)12);
				case (char)114:
					return new RxCharC((char)13);
				case (char)110:
					return new RxCharC((char)10);
				case (char)116:
					return new RxCharC((char)9);
				case 'x':
				case 'X':
					return new RxCharC(CompileNumeric(16,"0123456789abcdef"));
				case (char)111:
					return new RxCharC(CompileNumeric( 8,"01234567"));
				case (char)105:
					return new RxCharC(CompileNumeric(10,"0123456789"));
			}
			return new RxCharC(tmp);
		}

		// compiles a numeric escape sequence into a char
		private char CompileNumeric(int nbase, string control)
		{
			int idx, count, result;
			char tmp;

			count=result=0;
			while(!EOF) 
			{
				tmp = GetC();
				idx = control.IndexOf(char.ToLower(tmp));
				if (idx>=0) 
				{
					count++;
					result *= nbase;
					result +=  idx;
				} 
				else 
				{
					if(count==0)
						throw new RxException("invalid numeric escape",mOffset);
					UnGetC();
					break;
				}
			}
			return (char)result;
		}

		// compile a quoted string into a sequence of and statements
		public void CompileQuoted()
		{
			bool  more, escaped=false;
			char  tmp;
			int   offset = mOffset;
			RxAtom atom;

			Stack<RxAtom> stk = new Stack<RxAtom>();
			more = true;
			while (more && !EOF) 
			{
				tmp = GetC();
				if (escaped) 
				{
					escaped = false;
					switch(tmp)
					{
						case '"':
							stk.Push(new RxCharC(tmp));
							break;
						case '\\':
							stk.Push(new RxCharC(tmp));
							break;
						case 'r':
							stk.Push(new RxCharC('\r'));
							break;
						case 'n':
							stk.Push(new RxCharC('\n'));
							break;
						case 't':
							stk.Push(new RxCharC('\t'));
							break;
						case 'x':
						case 'X':
							stk.Push(new RxCharC(CompileNumeric(16,"0123456789abcdef")));
							break;
						case (char)111:
							stk.Push(new RxCharC(CompileNumeric( 8,"01234567")));
							break;
						case (char)105:
							stk.Push(new RxCharC(CompileNumeric(10,"0123456789")));
							break;
						default:
							stk.Push(new RxCharC(tmp));
							break;
					}
				}
				else 
				{
					switch(tmp)
					{
						case '\\':
							escaped = true;
							break;
						case '"':
							more = false;
							break;
						default:
							stk.Push(new RxCharC(tmp));
							break;
					}
				}
			}

			// if we reached the end of the non terminated space...
			if (more && EOF) 
				throw new RxException("unterminated quote starting at" + offset.ToString() );

			// if there were no atoms in the string...
			if (stk.Count==0)
				throw new RxException("empty string starting at " + offset.ToString() );

			// reduce the stack until it contains only one item.
			while(stk.Count > 1)
			{
				atom = (RxAtom)stk.Pop();
				stk.Push(new RxOpAND((RxAtom)stk.Pop(), atom));
			}

			// move the final result to the global token stack
			PushLeft(offset,(RxAtom)stk.Pop());

			// all done!
			return;
		}

		// compile the {n,m} postfix operator and
		// place the result on the left stack
		private void CompileMulti() 
		{
			char tmp;
			bool more;
			int  offset;
			int  ccmin, ccmax;

			offset = mOffset;

			// while there is more to read
			ccmin = 0;
			more  = true;
			tmp   =(char)0;

			// parse the first numeric
			while (more && !EOF) 
			{
				tmp = GetC();
				switch (tmp)
				{
					default:
						if(!char.IsDigit(tmp))
							throw new RxException("syntax error in postfix operator", mOffset);
						ccmin  *= 10;
						ccmin  += ((int)tmp - (int)'0');
						break;
					case ',':
						more = false;
						break;
					case '}':
						ccmax = ccmin;
						if (ccmax<1)
							throw new RxException("invalid postfix operator", mOffset);
						PushLeft('{',ccmin,ccmax,offset);
						return;
				}
			}

			// insure there is more to read
			if (tmp == ',' && EOF ) 
				throw new RxException("invalid postfix operator", offset);

			ccmax = 0;
			more = true;
			while (more && !EOF) 
			{
				tmp = GetC();
				if(tmp=='}')
				{
					if(ccmax != 0 && ccmax < ccmin)
						throw new RxException("invalid postifx operator", mOffset);
					PushLeft('{',ccmin,ccmax,offset);
					return;
				}	
				if(char.IsDigit(tmp))
				{
					ccmax *= 10;
					ccmax += ((int)tmp - (int)'0');
				}
				else
					throw new RxException("syntax error", mOffset);
			}

			// if we get here it is an exception...
			throw new RxException("unterminated postfix operator", offset);
		}

		private void CompileCharClass() 
		{
			bool  negate, more, store;
			int   range, count, offset;
			char  tmp, last, low;
			RxCharClass result = new RxCharClass();

			offset = mOffset;
			count  = 0;
			more   = true;
			negate = false;
			range  = 0;
			last   = low = (char)0;

			more = true;
			while( more && !EOF ) 
			{
				tmp = GetC();
				store=true;

				switch(tmp)
				{
					case '-':
						if (count!=0)
						{
							range = 1;
							tmp   = last;
							store = false;
						}
						break;
					case '^':
						if(count==0)
						{
							negate = true;
							store  = false;
						}
						break;
					case '\\':
						tmp = this.CompileEscaped();
						break;
					case ']':
						if(range!=0)
							throw new RxException("syntax error in character class", mOffset);
						store = more = (count==0);
						break;
				}

				switch(range)
				{
					case 1:
						range++;
						low = last;
						break;

					case 2:
						range=0;
						if(low<tmp)
							result.AddChar(low,tmp);
						else
							result.AddChar(tmp,low);
						low=(char)0;
						break;

					default:
						if (!EOF)
							if ( PeekC()!='-' && store)
								result.AddChar(tmp);
						last=tmp;
						break;
				}
				count++;
			}

			if(more)
				throw new RxException("unterminated character class", offset);

			if(negate)
				result.Negate = true;

			// push the result onto the stack!
			PushLeft(mOffset,result);
		}

		private void ResolvePostfix() 
		{
			RxRegexToken op;
			RxAtom  atom;

			// pop the operator from the stack
			op = PopLeft();

			// error and syntax control
			if (mLeft.Count<1) 
				throw new RxException("postfix operator has no left operand", op.offset);
			if (PeekLeft().id != (char)0)
				throw new RxException("postfix operator has no left operand", op.offset);

			// pull the child atom from the stack
			atom = PopLeft().atom;

			// create the parent atom
			switch(op.id)
			{
				case '*':
					atom = new RxOpSTR(atom);
					break;
				case '+':
					atom = new RxOpPLUS(atom);
					break;
				case '{':
					atom = new RxAtomN2M(atom, op.ccmin,op.ccmax);
					break;
				case '?':
					atom = new RxOpOPTION(atom);
					break;
			}
			PushLeft(atom);
		}

        private bool ResolveSubAnd()
        {
            // make sure there is something to do
            if (mLeft.Count < 1)
                return false;

            Stack<RxRegexToken> tmp = new Stack<RxRegexToken>();

            // while there are suitable atoms on the left
            while (mLeft.Count > 0)
            {
                char t = PeekLeft().id;
                if ((t == '(') || (t == '|') || (t == '/'))
                    break;

                tmp.Push(PopLeft());
            }

            if (tmp.Count == 0)
                return false;
        
            if (tmp.Count == 1)
            {
                PushLeft(tmp.Pop());
                return true;
            }

            // combine the atoms into an RxAtomAnd
            RxOpAND atom = new RxOpAND();
            while (tmp.Count > 0)
            {
                atom.Append(tmp.Pop().atom);
            }

            PushLeft(atom);
            return true;
        }

        private void ResolveAnd(bool isFormal, int groupID, string groupName)
		{
            if (mLeft.Count < 1 || !ResolveSubAnd())
                throw new RxException("empty sub-expression", PeekLeft().offset);

            // if it's a formal grouping
            if (isFormal)
            {
                RxRegexToken inner = PopLeft();

                if (mLeft.Peek().id != '(')
                    throw new RxException("fatal compiler error on '('");

                mLeft.Pop();

                RxAtomGroup group = new RxAtomGroup(groupID, inner.atom);
                group.Name = groupName;

                PushLeft(group);
            }
		}

		private void ResolveInfix() 
		{
			bool    more;
			RxRegexToken tkn;

			more = true;
			while(more && mLeft.Count>0)
			{
				tkn = PopLeft();
				switch(tkn.id)
				{
					default:
						PushRight(tkn);
						break;

					case '(':
						// put it back
						PushLeft(tkn);
						more = false;
						break;

					case '|':
						if (mRight.Count<1)
							throw new RxException("infix operator has no right operand");
						if ( ((RxRegexToken)mRight.Peek()).id!=0)
							throw new RxException("infix operator has invalid right operand");
						if (mLeft.Count<1)
							throw new RxException("infix operator has no left operand");
						if ( ((RxRegexToken)mRight.Peek()).id!=0)
							throw new RxException("infix operator has invalid left operand");
						
						// push the result onto the left hand stack... we're done with it
						PushRight(new RxOpOR(PopLeft().atom,PopRight().atom));
						break;

					case '/':
						if (mRight.Count<1)
							throw new RxException("infix operator has no right operand");
						if ( ((RxRegexToken)mRight.Peek()).id!=0)
							throw new RxException("infix operator has invalid right operand");
						if (mLeft.Count<1)
							throw new RxException("infix operator has no left operand");
						if ( ((RxRegexToken)mRight.Peek()).id!=0)
							throw new RxException("infix operator has invalid left operand");

						// push the result onto the left hand stack... we're done with it
						PushRight(new RxOpIIF(PopLeft().atom,PopRight().atom));
						break;
				}
			}

			// return everything back to the left hand stack
			while(mRight.Count>0)
				PushLeft(PopRight());

            mHasInfix = false;
		}

        #region IO Methods
        private bool EOF
		{
			get { return (mOffset > mPattern.Length - 1); }
		}

		private char GetC()
		{
			try 
			{
				return mPattern[mOffset++];
			} 
			catch (Exception) 
			{
				throw new RxException("unexpected end of pattern", mOffset);
			}
		}

		private char PeekC()
		{
			try
			{
				return mPattern[mOffset];
			}
			catch (Exception)
			{
				throw new RxException("unexpected end of pattern", mOffset);
			}
		}

        private bool PeekMatch(char c)
        {
            if (mOffset >= mPattern.Length || mPattern[mOffset] != c)
                return false;
            return true;
        }

        private bool PeekMatch(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (mOffset + i >= mPattern.Length || mPattern[mOffset + i] != text[i])
                    return false;
            }

            return true;
        }

		private void UnGetC()
		{
			if (mOffset<1)
				throw new RxException("seek beyond start of stream", mOffset);
			mOffset-=1;
		}

		private void UnGetC(int count)
		{
			if ((mOffset-count)<0)
				throw new RxException("seek beyond start of stream", mOffset);
			mOffset-=count;
		}
        #endregion
    }
}
