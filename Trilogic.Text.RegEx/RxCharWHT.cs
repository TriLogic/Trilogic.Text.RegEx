using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Match functionality for \s (white space) and \S (not white space).
    /// </summary>
    class RxCharWHT : RxCharABS
	{
        #region Constructors and Destructors
        public RxCharWHT() : base()
        {
        }

		public RxCharWHT(bool negate) : base(negate)
		{
		}
        #endregion

        #region CharMatch Override
        protected override bool CharMatch(char c)
        {
            return char.IsWhiteSpace(c);
        }
        #endregion

        #region Support Code
        public override string ToString() 
		{
			return (negated) ? ("\\S") : ("\\s");
		}
        #endregion
    }
}
