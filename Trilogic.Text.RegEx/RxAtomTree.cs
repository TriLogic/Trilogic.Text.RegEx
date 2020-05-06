using System;
using System.Text;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Performs the base mathcing functionality for two atomic operands.
	/// </summary>
	abstract class RxAtomTree : RxAtom 
	{
        #region Member Data
        protected RxAtom	childA, childB;
		protected RxFrame	frameA, frameB;
        #endregion

        #region Constructors and Destructors
        public RxAtomTree()
        {
            frameA = new RxFrame();
            frameB = new RxFrame();
            AtomA = null;
            AtomB = null;
        }

        public RxAtomTree(RxAtom a, RxAtom b)
        {
            frameA = new RxFrame();
            frameB = new RxFrame();
            AtomA = a;
            AtomB = b;
        }
        #endregion

        #region Abstract Methods
        public abstract RxAtomTree GetAtomOp();
        #endregion

        #region Atom and Frame Properties
        public RxAtom AtomA
        {
            get { return childA; }
            set { childA = value; }
        }
        public RxAtom AtomB
        {
            get { return childB; }
            set { childB = value; }
        }
        #endregion

        #region Appending Atoms
        public void Append(RxAtom atom)
        {
            if (childA == null)
            {
                childA = atom;
            }
            else if (childB == null)
            {
                childB = atom;
            }
            else //if (childB.GetType() == op.GetType()) 
            {
                RxAtomTree op = GetAtomOp();
                if (childB.GetType() == op.GetType())
                {
                    ((RxAtomTree)childB).Append(atom);
                }
                else
                {
                    op.Append(childB);
                    op.Append(atom);
                    childB = op;
                }
            }
        }
        #endregion

        #region Support Code
        public override string ToString() 
		{
			StringBuilder sb = new StringBuilder();
            if (childA != null)
			    sb.Append(childA.ToString());
            if (childB != null)
                sb.Append(childB.ToString());
			return sb.ToString();
		}
        #endregion
    }
}
