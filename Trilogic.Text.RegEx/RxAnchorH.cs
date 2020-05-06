using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Provide functionality for matching anchor head of line "^".
    /// </summary>
    class RxAnchorH : RxAnchorB
	{
		public RxAnchorH() : base()
		{
		}

		public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
		{
			bool ret;
			if(f.Offset>=s.Length) 
				return false;

			if (f.Offset==0)
			{
				f.Count=1;
				return true;
			}

			switch(s[f.Offset])
			{
				case '\n':
					if(hasCR(f,s,-1)) return false;
					ret = hasLF(f,s,-1);
					break;
				case '\r':
					ret = hasCR(f,s,-1);
					break;
				default:
					ret = hasLF(f,s,-1) || hasLF(f,s,-1);
					break;
			}

			if(ret)
			{
				f.Count=1;
				return true;
			}

			return false;
		}

        public override string ToString()
        {
            return "^";
        }

        /*
        private bool hasCharAt(RxFrame f, StringBuilder s, int offset, char c) 
		{
			if(f.Offset+offset < 0 || f.Offset+offset >= s.Length)
                return false;

			return s[f.Offset+offset] == c;
		}
        private bool hasCharAt(RxFrame f, string s, int offset, char c)
        {
            if (f.Offset + offset < 0 || f.Offset + offset >= s.Length)
                return false;

            return s[f.Offset + offset] == c;
        }

        private bool hasCR(RxFrame f, StringBuilder s, int offset)
		{
			return hasCharAt(f,s,offset,'\r');
		}
        private bool hasCR(RxFrame f, string s, int offset)
        {
            return hasCharAt(f, s, offset, '\r');
        }

        private bool hasLF(RxFrame f, StringBuilder s, int offset)
		{
			return hasCharAt(f,s,offset,'\n');
		}
        private bool hasLF(RxFrame f, string s, int offset)
        {
            return hasCharAt(f, s, offset, '\n');
        }
        */

    }
}
