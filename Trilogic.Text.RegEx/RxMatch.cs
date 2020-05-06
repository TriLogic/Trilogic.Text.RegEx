using System;
using System.Collections.Generic;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Summary description for RegexMatch.
	/// </summary>
	public class RxMatch 
	{
        #region Class Members
        protected RxSubMatch[] mGroups;
        protected Dictionary<string, RxSubMatch> mNamed = new Dictionary<string, RxSubMatch>();
        #endregion

        #region Constructors and Destructors
        internal RxMatch(int groups)
		{
			mGroups = new RxSubMatch[groups];
            for (int i = 0; i < groups; i++)
                mGroups[i] = new RxSubMatch(i);
			Reset();
		}
        #endregion

        #region Setting Length and Offset
        internal void SetLength(int length)
		{
            mGroups[0].Length = length;
		}

		internal void SetOffset(int offset)
		{
            mGroups[0].Offset = offset;
		}
        #endregion

        #region SetMatch
        internal void SetMatch(int offset, int length)
		{
			mGroups[0].SetMatch(offset, length);
		}

        internal void SetMatch(int offset, int length, int index)
        {
            mGroups[index].SetMatch(offset, length);
        }

        internal void SetMatch(int offset, int length, string key)
        {
            if (!mNamed.ContainsKey(key))
                mNamed.Add(key, new RxSubMatch());
            mNamed[key].SetMatch(offset, length, key);
        }

        internal void UnsetMatch(int index)
        {
            mGroups[index].Unset();
        }
        internal void UnsetMatch(string key)
        {
            if (mNamed.ContainsKey(key))
                mNamed[key].Unset();
        }
        #endregion

        #region Reset
        internal void Reset() 
		{
            for (int i=0; i< mGroups.Length; i++)
                mGroups[i].Unset();
		}
        #endregion

        #region Main Properties
        public bool Matched
		{
			get	{ return mGroups[0].Matched; }
            internal set { mGroups[0].Matched = value; }
		}

		public int Offset
        {
			get	{ return mGroups[0].Offset; }
		}

		public int Length
        {
            get	{ return mGroups[0].Length; }
		}

		public int Count 
		{
            get { return mGroups.Length; }
		}

		public RxSubMatch[] MatchArray
		{
			get { return mGroups; }
		}

        public Dictionary<string, RxSubMatch> MatchNames
        {
            get { return mNamed; }
        }
        #endregion

        #region Indexers
        public RxSubMatch this [int index] 
		{
			get { return index >= mGroups.Length ? null : mGroups[index];  }
        }
        public RxSubMatch this[string index]
        {
            get { return mNamed.ContainsKey(index) ? mNamed[index] : null; }
            internal set
            {
                if (! mNamed.ContainsKey(index))
                    mNamed.Add(index, new RxSubMatch(index));
            }
        }
        #endregion

        #region ToString
        public override string ToString() 
		{
			StringBuilder sb = new StringBuilder();
			for (int i=0; i< this.Count; i++)
				sb.Append(this[i].ToString());
			return sb.ToString();
		}

		public string ToStringX() 
		{
			StringBuilder sb = new StringBuilder();
            for (int i = 1; i < mGroups.Length; i++)
            {
                if (mGroups[i] != null)
                    sb.Append("{" + mGroups[i].ToString() + "}");
                else
                    sb.Append("{}");
            }
			return sb.ToString();
		}
        #endregion
    }
}
