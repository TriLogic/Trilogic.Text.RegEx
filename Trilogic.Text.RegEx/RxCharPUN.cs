using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Match functionality for \p (punctuation) and \P (not punctuation).
    /// </summary>
    class RxCharPUN : RxCharABS
	{
        #region Constructors and Destructors
        public RxCharPUN() : base()
		{
		}

		public RxCharPUN(bool negate) : base(negate)
		{
		}
        #endregion

        #region CharMatch Override
        protected override bool CharMatch(char c)
        {
            return char.IsPunctuation(c);
        }
        #endregion

        #region Support Code
        public override	string ToString() 
		{
			return "\\" + ((negated) ? ("P") : ("p"));
		}
        #endregion
    }
}
