using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Matches a group of atoms and produces a match value with an ID and optionally by named key.
	/// </summary>
	class RxAtomGroup : RxAtomParent
	{
        #region Member Data
        protected int mGroupID;
        protected string mGroupName = string.Empty;
        #endregion

        #region Constructors and Destructors
        public RxAtomGroup(int GroupID, RxAtom a) : base(a)
		{
			mGroupID = GroupID;
		}
        #endregion

        #region Matching Code
        public override	bool Match(RxMatch m, RxFrame f, RxCharSource s) 
		{
			if (child.Match(m,f,s))
			{
                if (mGroupID >= 0)
                {
                    m.SetMatch(f.Offset, f.Length, mGroupID);
                    m[mGroupID].SetMatch(f.Offset, f.Length);
                }
                else
                {
                    m.SetMatch(f.Offset, f.Length);
                }

                if (!string.IsNullOrEmpty(mGroupName))
                {
                    m.SetMatch(f.Offset, f.Length, mGroupName);
                    //m[mGroupID].SetMatch(f.Offset, f.Length, mGroupName);
                }

                return true;
			}

            if (mGroupID >= 0)
                m.UnsetMatch(mGroupID);

            if (!string.IsNullOrEmpty(mGroupName))
                m.UnsetMatch(mGroupName);

			return false;
		}
        #endregion

        #region Support Code
        public override	string ToString()
		{
            if (!string.IsNullOrEmpty(mGroupName))
                return $"(?<{mGroupName}>{child.ToString()})";

            if (mGroupID >= 0)
                return $"({child.ToString()})";

			return $"(?:{child.ToString()})";
		}

        public override int ID { get => mGroupID; internal set => mGroupID = value; }
        public override string Name { get => mGroupName; internal set => mGroupName = value; }
        #endregion
    }
}
