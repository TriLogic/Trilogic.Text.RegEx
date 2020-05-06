using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Trilogic.Text.RegEx;

namespace UnitTestPatternPro
{
    [TestClass]
    public class UnitTest_04_InfixOps
    {
        static string PatternA_OR_B = "a|b";
        static string PatternAAA_OR_BBB = "aaa|bbb";
        static string PatternA_OR_AA_OR_BBB = "a|aa|bbb";
        static string PatternA_PLUS_OR_B_PLUS = "a+|b+";
        static string PatternAAA_IIF_BBB = "aaa/bbb";
        static string PatternA_2_IIF_B_PLUS = "a{2}/b+";
        static string Pattern_WHOLE_STRING = "aaabbbcc";

        static RxCharSource TestString_ABC = new RxStringSource(Pattern_WHOLE_STRING);

        [TestMethod]
        public void Assert_01_Match_A_OR_B()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_OR_B, out regex));

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
            Assert.AreEqual(1, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(3, match.Offset);
            Assert.AreEqual(1, match.Length);

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
        public void Assert_02_Match_AAA_OR_BBB()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternAAA_OR_BBB, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(3, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(3, match.Offset);
            Assert.AreEqual(3, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_03_Match_A_OR_AA_OR_BBB()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_OR_AA_OR_BBB, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(2, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(2, match.Offset);
            Assert.AreEqual(1, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(3, match.Offset);
            Assert.AreEqual(3, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_04_Match_A_PLUS_OR_B_PLUS()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_PLUS_OR_B_PLUS, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(3, match.Length);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(3, match.Offset);
            Assert.AreEqual(3, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_05_Match_AAA_IIF_BBB()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternAAA_IIF_BBB, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(3, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

        [TestMethod]
        public void Assert_06_Match_A_2_IIF_BBB()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA_2_IIF_B_PLUS, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(1, match.Offset);
            Assert.AreEqual(2, match.Length);

            Assert.IsFalse(regex.MatchNext(TestString_ABC, match, out match));
        }

    }
}
