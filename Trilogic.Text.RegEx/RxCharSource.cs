using System;
using System.Collections.Generic;
using System.Text;

namespace Trilogic.Text.RegEx
{
    #region Interface RxCharSource
    public interface RxCharSource
    {
        int Length { get; }
        char this[int index] { get; }
    }
    #endregion

    #region RxStringSource
    public class RxStringSource : RxCharSource
    {
        protected string _value;

        public RxStringSource(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public int Length
        {
            get { return _value.Length; }
        }

        public char this[int index]
        {
            get { return _value[index]; }
        }

        public override string ToString()
        {
            return _value.ToString();    
        }
    }
    #endregion

    #region RxStringBuilderSource
    public class RxStringBuilderSource : RxCharSource
    {
        protected StringBuilder _value;

        public RxStringBuilderSource(string value)
        {
            _value = new StringBuilder(value);
        }
        public RxStringBuilderSource(StringBuilder value)
        {
            _value = value;
        }

        public StringBuilder Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public int Length
        {
            get { return _value.Length; }
        }

        public char this[int index]
        {
            get { return _value[index]; }
        }

        public override string ToString()
        {
            return _value.ToString();    
        }
    }
    #endregion

    #region RxMatchCharArraySource
    public class RxCharArraySource : RxCharSource
    {
        protected char[] _value;

        public RxCharArraySource(string value)
        {
            _value =value.ToCharArray();
        }
        public RxCharArraySource(StringBuilder value)
        {
            _value = value.ToString().ToCharArray();
        }
        public RxCharArraySource(char[] value)
        {
            _value = value.ToString().ToCharArray();
        }

        public char[] Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public int Length
        {
            get { return _value.Length; }
        }

        public char this[int index]
        {
            get { return _value[index]; }
        }

        public override string ToString()
        {
            return _value.ToString();      
        }
    }
    #endregion
}
