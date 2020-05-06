using System;
using System.Collections;

namespace Trilogic.Text.RegEx
{
	/// <summary>
	/// Optimized way of matching against multiple characters with a single lookup (DEPRECATED).
	/// </summary>
	public class RxBitset 
	{
		public     bool 	negated;
		protected  BitArray bits;
	
		public RxBitset() 
		{
			bits = new BitArray(16);
			reset();
		}
	
		public bool Negated	
		{
			get { return  negated; }
			set { negated = value; }
		}
		public bool this[int index]	
		{
			get	
			{ 
				if ( (bits.Length <= index) || (index <0)) return negated;
				return bits[index] && !negated; 
			}
			set	
			{ 
				if (bits.Length<=index) bits.Length = index+1;
				bits[index]=value;
			}
		}		
		public  bool Get(int index) 
		{
			return this[index];
		}
		public RxBitset Get(int start, int end) 
		{
			RxBitset that = new RxBitset();
			for(int index=start;index<end;index++)
				that[index]=this[index];
			that.Negated = this.Negated;
			return that;
		}
		public void Set(int index,bool value) 
		{
			this[index]=value;
		}
		public void Set(int start, int end, bool value) 
		{
			for(int index=start;index<=end;index++)
				this[index]=value;
		}
	
		public void Unset(int index) 
		{
			this[index]=false;
		}
		public void Unset(int start, int end) 
		{
			for(int index=start;index<=end;index++)
				this[index]=false;
		}
		public string toString() 
		{
			string tmp=""; int val;
			for(int index=0;index<bits.Count;index+=4) 
			{
				val=0;
				for(int jndex=0;jndex<4;jndex++) 
				{
					val |= ((bits[index+jndex]?1:0) << jndex);
				}
				tmp = "0123456789abcdef"[val] + tmp;
			}
			return "0x" + tmp;
		}
		public void reset() 
		{
			bits = new BitArray(16);
		}
	}
}
