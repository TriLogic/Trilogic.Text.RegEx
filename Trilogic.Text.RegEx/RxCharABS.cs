using System;
using System.Collections.Generic;
using System.Text;

namespace Trilogic.Text.RegEx
{
    abstract class RxCharABS : RxAtom
    {
        #region Class Members
        protected bool negated = false;
        #endregion

        #region Constructors and Destructors
        public RxCharABS()
        {
        }
        public RxCharABS(bool negate)
        {
            negated = negate;
        }
        #endregion

        #region Abstract Members
        protected abstract bool CharMatch(char c);
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
        {
            if (f.Maxlen < 1)
                return false;

            if ( CharMatch(s[f.Offset]) )
            {
                if (negated)
                    return false;
            }
            else
            {
                if (!negated)
                    return false;
            }

            f.Count = f.Length = 1;
            return true;
        }
        #endregion
    }
}
