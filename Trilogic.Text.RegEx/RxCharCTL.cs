using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Match functionality for \c (control char) and \C (not control char).
    /// </summary>
    class RxCharCTL : RxCharABS
	{
        #region Constructors and Destructors
        public RxCharCTL() : base()
		{
		}

		public RxCharCTL(bool negate) : base(negate)
		{
		}
        #endregion

        #region CharMatch Override
        protected override bool CharMatch(char c)
        {
            return char.IsControl(c);
        }
        #endregion

        #region Support Code
        public override	string ToString() 
		{
			return (negated) ? ("\\C") : ("\\c");
		}
        #endregion
    }
}
