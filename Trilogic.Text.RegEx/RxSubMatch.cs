using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Summary description for RxSubMatch.
	/// </summary>
	public class RxSubMatch 
	{
        private string mName = null;
        private int  mID = 0;
		private int  mOffset;
		private int  mLength;
		private bool mMatched;

		internal RxSubMatch()
		{
		}
        internal RxSubMatch(string name)
        {
            mName = name;
        }
        internal RxSubMatch(int id)
        {
            mID = id;
        }

        public string Name
        {
            get { return mName; }
            internal set { mName = string.IsNullOrEmpty(value) ? string.Empty : value; }
        }

        public int ID
        {
            get { return mID; }
            internal set { mID = value; }
        }

		public bool Matched
		{
			get	{ return mMatched; }
            internal set { mMatched = value; }
		}
		public int Length
		{
			get	{ return mLength; }
            internal set { mLength = value; }
		}
		public int Offset
		{
			get	{ return mOffset; }
            internal set { mOffset = value; }
		}

        internal void SetMatch(int offset, int length)
        {
            mOffset = offset;
            mLength = length;
            mMatched = true;
        }
        internal void SetMatch(int offset, int length, string name)
        {
            mOffset = offset;
            mLength = length;
            mMatched = true;
            mName = name;
        }

        internal void Unset()
        {
            mOffset = mLength = 0;
            mMatched = false;
            mName = null;
        }
    }
}
