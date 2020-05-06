using System;
using System.Collections.Generic;
using System.Text;

namespace Trilogic.Text.RegEx
{
    abstract class RxAnchorB : RxAtom
    {
        public RxAnchorB() : base()
        {
        }

        protected bool hasCharAt(RxFrame f, RxCharSource s, int offset, char c)
        {
            if (f.Offset + offset < 0 || f.Offset + offset >= s.Length)
                return false;

            return s[f.Offset + offset] == c;
        }

        protected bool hasCR(RxFrame f, RxCharSource s, int offset)
        {
            return hasCharAt(f, s, offset, '\r');
        }

        protected bool hasLF(RxFrame f, RxCharSource s, int offset)
        {
            return hasCharAt(f, s, offset, '\n');
        }
    }
}
