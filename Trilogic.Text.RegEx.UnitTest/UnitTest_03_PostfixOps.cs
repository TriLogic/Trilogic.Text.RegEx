using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Trilogic.Text.RegEx;

namespace UnitTestPatternPro
{
    [TestClass]
    public class UnitTest_03_PostfixOps
    {
        static string PatternA_PLUS = "a+";
        static string PatternB_PLUS = "b+";
        static string PatternA_B_PLUS = "ab+";
        static string PatternA_PLUS_B = "a+b";
        static string PatternA_PLUS_B_PLUS = "a+b+";
        static string PatternA_B_STAR = "ab*";
        static string PatternA_STAR_B = "a*b";
        static string PatternA_B_PLUS_OPT = "ab+?";
        static string PatternA_2_B = "a{2}b";
        static string PatternA_2_TO_B = "a{2,}b";
        static string PatternA_2_TO_3_B = "a{2,3}b";
        static string PatternA_0_TO_2_B = "a{,2}b";
        static string PatternA_2_TO_2_B = "a{1,2}b";

        static string PatternA_OR_B = "a|b";
        static string PatternA_IIF_B = "a/b";
        static string PatternAA_OR_BB = "aa|bb";
        static string PatternAA_IIF_BB = "aa/bb";
        static string PatternAABB_IIF_CC = "aa|bb/c";
        static string Pattern_WHOLE_STRING = "aaabbbcc";

        static string TestString = Pattern_WHOLE_STRING;
        static RxStringSource TestString_ABC = new RxStringSource(TestString);

        [TestMethod]
        public void Assert_01_Match_A_PLUS()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_PLUS, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(match.Offset, 0);
            Assert.AreEqual(match.Length, 3);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_02_Match_B_PLUS()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternB_PLUS, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(3, match.Offset);
            Assert.AreEqual(3, match.Length);

            /*
            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(match.Offset, 2);
            Assert.AreEqual(match.Length, 2);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(match.Offset, 7);
            Assert.AreEqual(match.Length, 2);
            */
            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_03_Match_AB_PLUS()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_B_PLUS, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(2, match.Offset);
            Assert.AreEqual(4, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_04_Match_A_PLUS_B()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_PLUS_B, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(4, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_05_Match_A_PLUS_B_PLUS()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_PLUS_B_PLUS, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(6, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_06_Match_AB_STAR()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_B_STAR, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(1, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(1, match.Offset);
            Assert.AreEqual(1, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(2, match.Offset);
            Assert.AreEqual(4, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_07_Match_A_STAR_B()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_STAR_B, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(4, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(4, match.Offset);
            Assert.AreEqual(1, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(5, match.Offset);
            Assert.AreEqual(1, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_08_Match_A_B_PLUS_OPT()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_B_PLUS_OPT, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(1, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(1, match.Offset);
            Assert.AreEqual(1, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(2, match.Offset);
            Assert.AreEqual(4, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_09_Match_A_2_B()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_2_B, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(1, match.Offset);
            Assert.AreEqual(3, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_10_Match_A_2_B()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_2_TO_B, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(4, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_11_Match_A_2_3_B()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_2_TO_3_B, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(4, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_12_Match_A_0_2_B()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_0_TO_2_B, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(1, match.Offset);
            Assert.AreEqual(3, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(4, match.Offset);
            Assert.AreEqual(1, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(5, match.Offset);
            Assert.AreEqual(1, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_13_Match_A_2_TO_2_B()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_2_TO_2_B, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(1, match.Offset);
            Assert.AreEqual(3, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }
    }
}
