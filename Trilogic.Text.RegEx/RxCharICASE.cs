using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Match functionality for a single character (case insensitive).
    /// </summary>
    class RxCharICASE : RxAtom
    {
        #region Member Data
        protected char chr;
        protected char chrU;
        #endregion

        #region Constructors and Destructors
        public RxCharICASE(char c)
        {
            chr = c;
            chrU = char.ToUpperInvariant(c);
        }
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
        {
            // See if we are off the end or...
            if (f.Maxlen >= 1 && chrU == char.ToUpperInvariant(s[f.Offset]))
            {
                f.Count = f.Length = 1;
                return true;
            }
            return false;
        }
        #endregion

        #region Support Code
        public override string ToString()
        {
            return Char.ToString(chr);
        }
        #endregion
    }
}
