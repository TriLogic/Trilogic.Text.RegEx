using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Match functionality for \u (uppercase) and \U (not uppercase).
    /// </summary>
    class RxCharUPR : RxCharABS
    {
        #region Constructors and Destructors
        public RxCharUPR() : base()
        {
        }
        public RxCharUPR(bool negate) : base(negate)
        {
        }
        #endregion

        #region CharMatch Override
        protected override bool CharMatch(char c)
        {
            return c >= 'A' && c <= 'Z';
        }
        #endregion

        #region Support Code
        public override string ToString()
        {
            return "\\" + ((negated) ? ("U") : ("u"));
        }
        #endregion
    }
}
