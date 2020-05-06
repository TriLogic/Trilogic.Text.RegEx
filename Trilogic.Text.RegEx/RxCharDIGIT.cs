using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Match functionality for \d (digit) and \D (not digit).
    /// </summary>
    class RxCharDIGIT : RxCharABS
	{
        #region Constructors and Destructors
        public RxCharDIGIT() : base()
		{
		}

		public RxCharDIGIT(bool negate) : base(negate)
		{
		}
        #endregion

        #region CharMatch Override
        protected override bool CharMatch(char c)
        {
            return char.IsDigit(c);
        }
        #endregion

        #region Support Code
        public override	string ToString() 
		{
			return (negated) ? ("\\D") : ("\\d");
		}
        #endregion
    }
}
