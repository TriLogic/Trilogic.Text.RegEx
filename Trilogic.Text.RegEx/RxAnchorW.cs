using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Provide functionality for matching \w (word boundary) and \W (not word boundary).
	/// </summary>
	class RxAnchorW : RxAtom
	{
		private bool negated=false;
		private RxBitset bits=new RxBitset();

		public RxAnchorW(bool negate)
		{
			bits.Set((int)'0',(int)'9',true);
			bits.Set((int)'A',(int)'Z',true);
			bits.Set((int)'a',(int)'z',true);
            bits.Set((int)'_', true);
			negated = negate;
		}

		public override bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
            if (s.Length == 0)
                return false;

            f.Length = 0;
            f.Count = 1;

            // bos test
            if (f.Offset == 0)
            {
                if (bits[(int)s[f.Offset]])
                    return !negated;

                if (s.Length > 1 && bits[(int)s[f.Offset + 1]])
                    return !negated;
 
                return negated;
            }

            // eos test
            if (f.Offset == s.Length)
                return bits[(int)s[f.Offset-1]] && !negated;

            // current char is a word char?
            if (bits[(int)s[f.Offset]])
            {
                // neither prev or next char is a word char?
                if (!bits[(int)s[f.Offset - 1]] || !bits[(int)s[f.Offset + 1]])
                    return !negated;

                return negated;
            }

            // current character is NOT a word character
            if (bits[(int)s[f.Offset - 1]] || bits[(int)s[f.Offset + 1]])
                return !negated;

            return negated;
		}

        private bool MatchStart(int offset, RxCharSource s)
		{
			return (offset>0) ? (bits[(int)s[offset-1]]) : (offset==0);
		}

        private bool MatchEnd(int offset, RxCharSource s)
		{
			return (offset<s.Length-1) ? (bits[(int)s[offset+1]]) : (offset==s.Length-1);
		}
 
        public override	string ToString() 
		{
			return (negated) ? ("\\B") : ("\\b");
		}
	}
}
