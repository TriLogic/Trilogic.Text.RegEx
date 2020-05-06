using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
    /// <summary>
    /// Provide functionality to re-match a prevsious match referenced match by ID or named key.
    /// </summary>
    class RxAtomBREF : RxAtom
    {
        #region Member Data
        int mRefID;
        string mRefName;
        string mRepr;
        #endregion

        #region Constructors andDestructors
        public RxAtomBREF()
        {
        }
        public RxAtomBREF(string repr, int refID)
        {
            mRefID = refID;
            mRepr = repr;
        }
        public RxAtomBREF(string repr, string refName)
        {
            mRefName = refName;
            mRepr = repr;
        }
        #endregion

        #region Matching Code
        public override bool Match(RxMatch m, RxFrame f, RxCharSource s)
        {
            RxSubMatch subMatch = null;

            // matching name or id?
            if (!string.IsNullOrEmpty(mRefName) && m.MatchNames.ContainsKey(mRefName))
                subMatch = m.MatchNames[mRefName];
            else if (mRefID >= 1 && mRefID < m.MatchArray.Length)
                subMatch = m.MatchArray[mRefID];
            else
                return false;

            // if not enough rooom then exit now
            if (subMatch.Length > f.Maxlen)
                return false;

            // attmpt to match char for char (case sensitive)
            for (int span = 0; span < subMatch.Length; span++)
            {
                if (s[subMatch.Offset + span] != s[f.Offset + span])
                    return false;
            }

            // register one match at same length
            f.Count = 1;
            f.Length = subMatch.Length;

            return true;
        }
        #endregion

        #region Support Code
        public override string ToString()
        {
            return mRepr;
        }

        public override int ID { get => mRefID; internal set => mRefID = value; }
        public override string Name { get => mRefName; internal set => mRefName = value; }
        #endregion
    }
}
