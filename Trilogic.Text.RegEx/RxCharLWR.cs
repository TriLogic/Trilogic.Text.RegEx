using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Match functionality for \l (lowercase) and \L (not lowercase).
    /// </summary>
    class RxCharLWR : RxCharABS
    {
        #region Constructors and Destructors
        public RxCharLWR() : base()
        {
        }
        public RxCharLWR(bool negate) : base(negate)
        {
        }
        #endregion

        #region CharMatch Override
        protected override bool CharMatch(char c)
        {
            return c >= 'a' && c <= 'z';
        }
        #endregion

        #region Support Code
        public override string ToString()
        {
            return "\\" + ((negated) ? ("L") : ("l"));
        }
        #endregion
    }
}
