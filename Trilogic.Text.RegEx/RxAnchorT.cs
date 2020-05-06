using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Provide functionality for matching anchor end of line "$".
    /// </summary>
    class RxAnchorT : RxAnchorB
	{
		public RxAnchorT() : base()
		{
		}

		private bool success(RxFrame f) 
		{
			f.Count = 1;
			return true;
		}

		public override	bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
			bool ret=false;

			if (f.Offset>=s.Length)
                return success(f);

			switch (s[f.Offset])
			{
				default:
					ret = false;
					// ret = (hasCR(f,s,1) || hasLF(f,s,1));
					break;
				case '\r':
					ret = true;
					// ret = (hasCR(f,s,1)  || hasLF(f,s,1));
					break;
				case '\n':
					ret = !hasCR(f,s,-1);
					//if(!hasCR(f,s,-1)
					//ret = hasLF(f,s,1);
					break;
			}

			f.Count = (ret) ? (1) : (0);

			return ret;
		}

        public override string ToString()
        {
            return "$";
        }
        /*
                private bool hasCharAt(RxFrame f, StringBuilder s, int offset, char c) 
                {
                    // FIXME: this is not supposed to look beyond the maxlen parameter
                    if(f.Offset+offset < 0 || f.Offset+offset >= s.Length) return false;
                    return s[f.Offset+offset]==c;
                }

                private bool hasCR(RxFrame f, StringBuilder s, int offset)
                {
                    return hasCharAt(f,s,offset,'\r');
                }

                private bool hasLF(RxFrame f, StringBuilder s, int offset)
                {
                    return hasCharAt(f,s,offset,'\n');
                }
         */
    }
}
