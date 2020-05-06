using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Performs case insensitive matching functionality for a string of characters.
    /// </summary>
    class RxAtomSTRI : RxAtom
    {
        #region Member Data
        string mString;
        string mUpperC;
        #endregion

        #region Constructors and Destructors
        public RxAtomSTRI(string chars)
        {
            mString = chars;
            mUpperC = mString.ToUpperInvariant();
        }
        public RxAtomSTRI(StringBuilder chars)
        {
            mString = chars.ToString();
            mUpperC = mString.ToUpperInvariant();
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
                if (mUpperC[span] != char.ToUpperInvariant(s[f.Offset + span]))
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
