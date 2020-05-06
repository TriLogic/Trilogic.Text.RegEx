using System;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Summary description for RegexException.
	/// </summary>
	public class RxException : Exception 
	{
		public readonly int Column;
		public RxException(string msg) : base(msg) 
		{
			Column = 0;
		}
		public RxException(string msg, int column)
		{
			Column = column;
		}
	}
}
