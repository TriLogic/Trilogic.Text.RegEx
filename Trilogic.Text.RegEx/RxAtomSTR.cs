using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Performs matching functioan lit for a string of characters.
    /// </summary>
    class RxAtomSTR : RxAtom
    {
        #region Member Data
        string mString;
        #endregion

        #region Constructors and Destructors
        public RxAtomSTR(string chars)
        {
            mString = chars;
        }
        public RxAtomSTR(StringBuilder chars)
        {
            mString = chars.ToString();
        }
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
        {
            // if not enough rooom then exit now
            if (mString.Length > f.Maxlen)
                return false;

            // attmpt to match char for char (case sensitive)
            for (int span = 0; span < mString.Length; span++)
            {
                
                if (mString[span] != s[f.Offset + span])
                    return false;
            }

            // register one match at same length
            f.Count = 1;
            f.Length = mString.Length;

            return true;
        }
        #endregion

        #region Support Code
        public override string ToString()
        {
            return mString;
        }
        #endregion
    }
}
