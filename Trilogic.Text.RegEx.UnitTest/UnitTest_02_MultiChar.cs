using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Trilogic.Text.RegEx;

namespace UnitTestPatternPro
{
    [TestClass]
    public class UnitTest_02_Concatenation
    {
        static string PatternAB = "ab";
        static string PatternCC = "cc";
        static string PatternA_B_PLUS = "ab+";
        static string PatternA_B_STAR = "ab*";
        static string PatternA_OR_B = "a|b";
        static string PatternA_IIF_B = "a/b";
        static string PatternAA_OR_BB = "aa|bb";
        static string PatternAA_IIF_BB = "aa/bb";
        static string PatternAABB_IIF_CC = "aa|bb/c";
        static string Pattern_WHOLE_STRING = "ababaaaabbcc";

        static RxStringSource TestString_ABC = new RxStringSource(Pattern_WHOLE_STRING);

        [TestMethod]
        public void Assert_01_Match_AB()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternAB, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(2, match.Length);
        }

        [TestMethod]
        public void Assert_02_MatchAll_AB()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternAB, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(2, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(2, match.Offset);
            Assert.AreEqual(2, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(7, match.Offset);
            Assert.AreEqual(2, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }


        [TestMethod]
        public void Assert_03_MatchTerminal_CC()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternCC, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(10, match.Offset);
            Assert.AreEqual(2, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_04_Match_Original_Pattern()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(TestString_ABC.ToString(), out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(TestString_ABC.Length, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }
    }
}
