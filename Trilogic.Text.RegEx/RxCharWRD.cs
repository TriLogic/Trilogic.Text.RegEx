using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Match functionality for \w and \W (word char and not word char).
	/// </summary>
	class RxCharWRD : RxCharABS
	{
        #region Constructors and Destructuctors
        public RxCharWRD() : base()
		{
		}

		public RxCharWRD(bool negate) : base(negate)
		{
		}
        #endregion

        #region CharMatch Override
        protected override bool CharMatch(char c)
        {
            return char.IsLetterOrDigit(c);
        }
        #endregion

        #region Support Code
        public override	string ToString() 
		{
			return (negated) ? ("\\W") : ("\\w");
		}
        #endregion
    }
}
