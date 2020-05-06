using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// StringBuilder as a Stream Device.
	/// </summary>
	public class RxBuilderStream
	{
        #region Class Members
        protected StringBuilder mBuffer;
        #endregion

        #region Constructors and Destructors
        public RxBuilderStream(string text)
		{
            mBuffer = new StringBuilder(text);
		}

        public RxBuilderStream(StringBuilder builder)
        {
            mBuffer = new StringBuilder(builder.ToString());
        }

        public RxBuilderStream(StreamReader reader)
        {
            mBuffer = new StringBuilder(reader.ReadToEnd());
        }
        #endregion

        #region Class Properties
        public StringBuilder Buffer
        {
            get { return mBuffer; }
        }

        public bool IsEOF
        {
            get { return mBuffer.Length > 0; }
        }
        #endregion

        public char GetC()
        {
            if (mBuffer.Length < 1)
                return '\0';

            char c = mBuffer[0];
            mBuffer.Remove(0, 1);
            return c;
        }

        public string GetS(int count)
        {
            if (count > mBuffer.Length)
                throw new Exception("Buffer underflow");

            string s = mBuffer.ToString(0, count);
            mBuffer.Remove(0, count);
            return s;
        }

        #region Static Creators
        public static RxBuilderStream FromString(string input)
        {
            return new RxBuilderStream(input);
        }

        public static RxBuilderStream FromBuilder(StringBuilder input)
        {
            return new RxBuilderStream(input);
        }

        public static RxBuilderStream FromFile(string path)
        {
            return new RxBuilderStream(new StreamReader(path));
        }

        public static RxBuilderStream FromStream(StreamReader reader)
        {
            return new RxBuilderStream(reader);
        }
        #endregion
    }
}
