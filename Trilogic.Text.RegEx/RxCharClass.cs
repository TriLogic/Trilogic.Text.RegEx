using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    class RxAtomOrCS : RxAtomTree
    {
        #region Constructructors and Destructors
        public RxAtomOrCS() : base()
        {
        }
        public RxAtomOrCS(RxAtom a, RxAtom b) : base(a, b)
        {
        }
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
        {
            return childA.Match(m, f, s) || (childB != null && childB.Match(m, f, s));
        }
        #endregion

        #region Suport Code
        public override RxAtomTree GetAtomOp()
        {
            return new RxAtomOrCS();
        }
        #endregion
    }

    class RxAtomCharCS : RxCharC
    {
        #region Constructors and Destructors
        public RxAtomCharCS(char c) : base(c)
        {
        }
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
        {
            // See if we are off the end or...
            if (chr == s[f.Offset])
            {
                f.Count = f.Length = 1;
                return true;
            }
            return false;
        }
        #endregion
    }

    class RxAtomChar2CS : RxAtom
    {
        #region Member Data
        protected char cMin, cMax;
        #endregion

        #region Constructors and Destructors
        public RxAtomChar2CS(char a, char b) : base()
        {
            cMin = a;
            cMax = b;
        }
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
        {
            if (cMin <= s[f.Offset] && cMax >= s[f.Offset])
            {
                f.Count = f.Length = 1;
                return true;
            }
            return false;
        }
        #endregion

    }


    /// <summary>
    /// Provide match functionality for character classes "[...]".
    /// </summary>
    class RxCharClass : RxAtomOrCS
    {
        #region Member Data
        private RxOpOR atom = new RxOpOR();

		private RxBitset bits = new RxBitset();
		private bool negated = false;
		private StringBuilder repr = new StringBuilder();
        #endregion

        #region Constructors and Destructors
        public RxCharClass()
		{
		}

        public RxCharClass(string s)
		{
			AddChar(s);
		}

		public RxCharClass(char start, char end)
		{
			AddChar(start,end);
		}
        #endregion

        #region Adding String Representation
        private void AddRepr(string s)
		{
			repr.Append(s);
		}
        #endregion

        #region Adding Chars & Char Ranges
        public void Append(char c)
        {
            Append(new RxCharC(c));
        }
        public void Append(char cMin, char cMax)
        {
            Append(new RxAtomChar2CS(cMin, cMax));
        }
        #endregion

        #region Negate Property
        public bool Negate
		{
			get	{ return negated; }
			set	{ negated = value; }
		}
        #endregion

        #region Adding Chars
        public void AddChar(char cMin, char cMax)
		{
            repr.Append(cMin);
            repr.Append('-');
            repr.Append(cMax);
            Append(new RxAtomChar2CS(cMin, cMax));

            /*
            if (start<=end)
				for(int i=(int)end;i>=(int)start;i--)
					bits[i]=true;
			else
				for(int i=(int)start;i>=(int)end;i--)
					bits[i]=true;
             */
		}

		public void AddChar(string s)
		{
			repr.Append(s);
            for (int i = 0; i < s.Length; i++)
                Append(new RxAtomCharCS(s[i]));
            /*
			for(int i=0;i<s.Length;i++)
				bits[(int)s[i]]=true;
            */
        }

        public void AddChar(char c)
		{
            repr.Append(c);
            Append(new RxAtomCharCS(c));
            // bits[(int)c]=true;
        }
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
            // Optimized to check the length only one time
            if (f.Maxlen < 1)
                return false;

            if (base.Match(m, f, s))
                return !negated;

            return negated;

            /*
			// see if we are off the end or...
			if(f.Maxlen<1)
                return false;

            if (bits[(int)s[f.Offset]] )
			{
				if (negated) return false;
			}
			else
			{
				if (!negated) return false;
			}

            f.Count = f.Length = 1;
			f.Id = s[f.Offset];

			return true;
            */
		}
        #endregion

        #region Support Code
        public override string ToString() 
		{
            return negated ? "[^" + repr + "]" : "[" + repr + "]";
		}
        #endregion
    }


    class RxAtomCharLTE : RxCharC
    {
        #region Constructors and Destructors
        public RxAtomCharLTE(char c) : base(c)
        {
        }
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
        {
            // See if we are off the end or...
            if (f.Maxlen >= 1 && (chr <= s[f.Offset]))
            {
                f.Count = f.Length = 1;
                return true;
            }
            return false;
        }
        #endregion

    }

    class RxAtomCharGTE : RxCharC
    {
        #region Constructors and Destructors
        public RxAtomCharGTE(char c) : base(c)
        {
        }
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
        {
            // See if we are off the end or...
            if (f.Maxlen >= 1 && (chr >= s[f.Offset]))
            {
                f.Count = f.Length = 1;
                return true;
            }
            return false;
        }
        #endregion
    }

}
