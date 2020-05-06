using System;
using System.IO;

using Trilogic.Text.RegEx;

namespace LexerTest
{

	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class SimpleLexerTest
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Console.WriteLine("Strike enter key!");
			Console.ReadLine();
			//Simple();
			Complex();
			Console.WriteLine("DONE!");
			Console.ReadLine();
		}

		static void Simple()
		{
			string test = "/a/// .<>default 123 Now is the time for all -234.45e-09 good men to come to the aide of their country!";
			
			RxLexer lex = new RxLexer();
			RxLexerState ste = lex.State("default");

			ste.AddPattern( 1, "\\/([^/]|\"\\/\\/\")+\\/" );
			ste.AddPattern( 1, "\\.<([a-zA-Z][a-zA-Z0-9_]*)?>[a-zA-Z][a-zA-Z0-9_]*" );

			//ste.AddPattern( 1, "(\\w+)|\\s+","" );

			
			ste.AddPattern( 1, "\\w+" );
			ste.AddPattern( 0, "\\s+" );
			ste.AddPattern( 2, ".$" );

			ste = lex.StateAdd( "number" );
			ste.AddPattern( 3, "[-+]?(([0-9]+)|([0-9]*\\.[0-9]+)([eE][-+]?[0-9]+)?)" );

            lex.AppendText(test);

			RxMatch rm;
			int id;
			string match;

			while ( lex.Match( out rm, out id, out match ) )
				Console.WriteLine( string.Format( "Match: {0}:'{1}'", id, match ) );

			lex.StatePush( "number" );

			while ( lex.Match( out rm, out id, out match ) )
				Console.WriteLine( string.Format( "Match: {0}:'{1}'", id, match ) );

			lex.StatePop();
			while ( lex.Match( out rm, out id, out match ) )

				Console.WriteLine( string.Format( "Match: {0}:'{1}'", id, match ) );
		}

		static void Complex()
		{
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string datPath = new FileInfo(exePath).Directory.Parent.Parent.FullName;
            datPath = Path.Combine(datPath, "Data");

            string inpFile = Path.Combine(datPath, "caboose.cs.txt");
            string outFile = Path.Combine(datPath, "caboose.cs");

            RxLexerGen mLex = new RxLexerGen();
			int th1, th2, thr;
			StreamReader sr = new StreamReader(new FileStream( inpFile, System.IO.FileMode.Open ) );
			mLex.GetType();
			th1 = DateTime.Now.Millisecond;
			mLex.Scan( sr );
			th2 = DateTime.Now.Millisecond;
			thr = th1 < th2 ? th2 - th1 : (th2 + 1000) - th1;
			Console.WriteLine( "{0}ms", thr );
		}
	}
}
