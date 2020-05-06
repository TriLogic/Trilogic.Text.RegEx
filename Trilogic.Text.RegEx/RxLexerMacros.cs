using System;

using Trilogic.Text.RegEx;

namespace Trilogic.Text.RegEx
{

	/// <summary>
	/// Summary description for RxLexerMacros.
	/// </summary>
	class RxLexerMacros
	{
		internal enum TokenID
		{
			tk_ignore = 0,
			tk_base = 2048,
			tk_not_macro,
			tk_not_rparen,
			tk_ident,
			tk_macro,
			tk_lparen,
			tk_rparen,
			tk_white,
			tk_dqstring,
			tk_sqchar,
			tk_comma,
			tk_not_comma,
			tk_anpersand
		}

		private const string rgx_not_macro		= "([^@]|(@@))+";
		private const string rgx_not_rparen		= "([^)@]]|(@@)))+";
		private const string rgx_ident			= "[a-zA-Z]([a-zA-Z0-9_]*[a-zA-Z])*";
		private const string rgx_macro			= "@[a-zA-Z]([a-zA-Z0-9_]*[a-zA-Z])";
		private const string rgx_lparen			= "\\(";
		private const string rgx_rparen			= "\\)";
		private const string rgx_white			= "[ \t]+";
		private const string rgx_dqstring		= "\\\"([^\r\n\"\\]+|(\\[rnlt\\\"\\\\]))*\\\"";
		private const string rgx_sqcar			= "'([^\\r\\n'\\\\]|(\\\\[rnltf'\\\\]))'";
		private const string rgx_comma			= ",";
		private const string rgx_not_comma		= "[^, \\t\\r\\n\\f]+";
		private const string rgx_ampersand		= "@";

		RxLexer mLEx;
		internal RxLexerMacros()
		{
			mLEx = new RxLexer();
		}
	}
}
