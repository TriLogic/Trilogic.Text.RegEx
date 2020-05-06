using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Trilogic.Text.RegEx;

namespace UnitTestPatternPro
{
    class UnitTestTools
    {
        public static bool BuildAndCompare(string pattern, out RxMatcher r)
        {
            r = new RxMatcher(pattern);
            return string.Compare(pattern, r.Pattern, false) == 0;
        }
    }
}
