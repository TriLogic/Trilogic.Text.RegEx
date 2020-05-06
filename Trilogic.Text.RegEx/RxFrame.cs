using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Frame structure used during pattern matching.
	/// </summary>
	internal class RxFrame 
	{
        #region Member Data
        int mCount, mLength, mOffset, mMaxlen, mTotal;
        #endregion

        #region Constructors and Destructors
        public RxFrame()
        {
            Reset();
        }
        public RxFrame(RxCharSource s)
        {
            Initialize(s);
        }

        public RxFrame(RxFrame f)
        {
            CopyFrom(f);
        }
        #endregion

        public void Initialize(RxCharSource s)
        {
            Reset();
            mTotal = mMaxlen = s.Length;
        }

        public int Count 
		{
			get { return mCount; }
			set { mCount = value; }
		}

		public int Length 
		{
			get { return mLength; }
			set { mLength = value; }
		}

		public int Offset 
		{
			get { return mOffset; }
			set { mOffset = value; }
		}

		public int Maxlen 
		{
			get { return mMaxlen; }
			set { mMaxlen = value; }
		}

		public int Total 
		{
			get { return mTotal; }
			set { mTotal = value; }
		}
	
		public void Reset() 
		{
			mCount = mLength = mOffset = mMaxlen = mTotal = 0;
		}

		public void CopyTo(RxFrame etc) 
		{
			etc.mCount 	= mCount;
			etc.mLength = mLength;
			etc.mOffset = mOffset;
			etc.mMaxlen = mMaxlen;
			etc.mTotal  = mTotal;
		}

		public void CopyFrom(RxFrame etc) 
		{
			etc.CopyTo(this);
		}

		public override string ToString() 
		{
			return "This is an RxFrame";
		}
	}
}
