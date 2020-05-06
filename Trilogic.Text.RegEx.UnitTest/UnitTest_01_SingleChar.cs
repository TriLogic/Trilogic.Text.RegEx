using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Trilogic.Text.RegEx;

namespace UnitTestPatternPro
{
    [TestClass]
    public class UnitTest_01_SingleChar
    {
        static string PatternA = "a";
        static string PatternC = "c";
        static string PatternANY = ".";

        static string TestString_BUF = "ababaaaabbcc";
        static RxCharSource TestString_ABC = new RxStringSource(TestString_BUF);

        [TestMethod]
        public void Assert_01_MatchFirst_Char_A()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(1, match.Length);
        }

        [TestMethod]
        public void Assert_02_MatchNext_Char_A()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternA, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);

            Assert.IsTrue(regex.MatchNext(TestString_ABC, match, out match));
            Assert.IsNotNull(match);

            Assert.AreEqual(2, match.Offset);
            Assert.AreEqual(1, match.Length);
        }

        [TestMethod]
        public void Assert_03_MatchFirst_Char_C()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternC, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(10, match.Offset);
            Assert.AreEqual(1, match.Length);
        }

        [TestMethod]
        public void Assert_04_MatchFirst_Char_ANY()
        {
            RxMatcher regex;
            RxMatch match;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternANY, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);
            Assert.AreEqual(0, match.Offset);
            Assert.AreEqual(1, match.Length);
        }

        [TestMethod]
        public void Assert_05_MatchEvery_Char_ANY()
        {
            RxMatcher regex;
            RxMatch match;

            int offset = 0;
            int expect = TestString_ABC.Length;
            bool more = false;

            Assert.IsTrue(UnitTestTools.BuildAndCompare(PatternANY, out regex));

            Assert.IsTrue(regex.MatchFirst(TestString_ABC, out match));
            Assert.IsNotNull(match);

            more = true;
            while (more)
            {
                // first match
                Assert.AreEqual(offset, match.Offset);
                Assert.AreEqual(1, match.Length);

                more = regex.MatchNext(TestString_ABC, match, out match);

                if (more)
                    offset++;
            }

            Assert.AreEqual(TestString_ABC.Length - 1, offset);
        }

    }
}
