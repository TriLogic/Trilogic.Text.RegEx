/*
** Date     : 02.10.02
** Revise   : 02.23.02 Correct RXCHRS_TOSTRING, negate bug in charset.
**          : 02.25.02 Correct compiler bug in CHARSET, add submatch tracking.
**
** Copyright: 2002, All Rights Reserved
**          : This source code is the intellectual property of Andrew Friedl
*/

using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

using Trilogic.Text.RegEx;

namespace PatternProTest
{
	public class rxLibTest 
	{
		public static int Main(string []arg) 
		{
			RegexTestForm rs = new RegexTestForm();
			Application.Run(rs);
			return 0;
			/*
			string s = "01234 r 567890 Hello World ! 777";

			Regex      r = new Regex("\\w+");
			RegexMatch m;
			
			m = r.FirstMatch(s);
			while(m!=null)
			{
				Console.WriteLine("match");
				m = r.NextMatch(m,s);
			}

			//Console.WriteLine(bits.toString());
			return 0;
			*/
		}
	}
} // End com.protosync.regex;

/*

public class rxLibTest {

	public static int Main(string []arg) {
		rxBitset bits = new rxBitset();
		bits.Set(1,true);
		bits.Set(3,true);
		bits.Set(5,true);
		Console.WriteLine(bits.toString());
		return 0;
	}
}


} // namespace terminator

/*


// ----------------------------------------------------------------------
// Conjuction: AND
// ----------------------------------------------------------------------
function rxAnd() {
rxAnd.prototype.match = function() {
	// store a reference to the frame
	this.frames[0] = arguments[0];
	arguments[0].mCount     = 0;
	arguments[0].mLength    = 0;

	// if there are no atoms then return false
	if (this.atoms.length < 2)
		return false;

	// start the matching pointer at 1!
	this.ptr = 1;

	// initialize the context frames
	this.copyPrevFrame(arguments[0]);

	// while there are atoms left to match
	while (this.ptr < this.frames.length) {

		// test current atom with current frame for a match
		if ( this.atoms[this.ptr].match(this.frames[this.ptr],arguments[1],arguments[2]) ) {

			// the last atom matched therefore we are done
			if (this.ptr==this.frames.length-1)
				return this.success(arguments[0],arguments[1],arguments[2]);

			// beginning and end of string conditions
			if (this.atoms[this.ptr].mId == 36 || this.atoms[this.ptr].mId == 94 )
				if (this.ptr==this.atoms.length-1)
					return this.success(frm,arguments[1],arguments[2]);

			// increment to the next atom
			this.ptr++;

			// copy infromation from the last frame
			this.copyPrevFrame(arguments[0]);
		} else {
			if ( ! this.decrMatches() )
				break;
		}
	}
	return this.failure(arguments[2]);
}
rxAnd.prototype.failure = function() {
	this.frames[0]=null;
	if (this.subId >=0 )
		arguments[0].values[this.subId]=null;
	return false;
}
rxAnd.prototype.success = function() {
	var index;
    	arguments[0].mCount = 1;
    	for (index=1;index<this.frames.length;index++)
    		arguments[0].mLength += this.frames[index].mLength;
    	arguments[0].mId = this.id;
    	if (this.subId >= 0) {
    		arguments[2].values[this.subId] = new Match(false);
    		arguments[2].values[this.subId].mOffset = arguments[0].mOffset;
    		arguments[2].values[this.subId].mLength = arguments[0].mLength;
    		arguments[2].values[this.subId].value   = arguments[1].substr(arguments[0].mOffset,arguments[0].mLength);
    	}
    	this.frames[0]=null;
    	return true;
}
rxAnd.prototype.decrMatches = function () {
	while (--this.ptr > 0)
		if (this.decrMatch(this.ptr)) break;
	this.ptr = (this.ptr < 1) ? 0: this.ptr;
	return (this.ptr > 0);
}
rxAnd.prototype.decrMatch = function () {
	if (this.frames[this.ptr].mLength > 1) {
		this.frames[this.ptr].mCount = 0;
		this.frames[this.ptr].mMaxlen = this.frames[this.ptr].mLength - 1;
		this.frames[this.ptr].mLength = 0;
		return true;
	}
	return false;
}
rxAnd.prototype.copyPrevFrame = function () {
	this.frames[this.ptr].mOffset = this.frames[this.ptr-1].mOffset + this.frames[this.ptr-1].mLength;
	this.frames[this.ptr].mCount  = 0;
	this.frames[this.ptr].mLength = 0;
	this.frames[this.ptr].mMaxlen = (arguments[0].mMaxlen + arguments[0].mOffset) - 1;
	this.frames[this.ptr].mMaxlen = (this.frames[this.ptr].mMaxlen - this.frames[this.ptr].mOffset) + 1;
	this.frames[this.ptr].mId     = 0;
	this.frames[this.ptr].mTotal  = arguments[0].mTotal;
}
rxAnd.prototype.toString = function() {
	var idx,ret="";
	for(idx=1;idx<this.atoms.length;idx++)
		ret = ret.concat(this.atoms[idx].toString());
	if (this.subId>=0)
		return "(" + ret + ")";
	return ret;
}
rxAnd.prototype.addAtom   = RX_CONTAINER_ADD;
rxAnd.prototype.icase = RX_CONTAINER_ICASE;

// ----------------------------------------------------------------------
// Conjuction: OR, "|"
// ----------------------------------------------------------------------
function rxOr() {
	this.frames    = new Array();
	this.atoms     = new Array();
	this.id        = 0;
	this.frames[0] = null;
	this.atoms[0]  = null;
}
rxOr.prototype.match = function () {
	var ptr, lenMax, frmMax;

	arguments[0].mLength = 0;
	arguments[0].mCount  = 0;

	frmMax = null;
	lenMax = -1;

	for(ptr=1;ptr<this.atoms.length;ptr++) {
		this.frames[ptr].mCount  = 0;
		this.frames[ptr].mId     = 0;
		this.frames[ptr].mLength = 0;
		this.frames[ptr].mMaxlen = arguments[0].mMaxlen;
		this.frames[ptr].mOffset = arguments[0].mOffset;
		this.frames[ptr].mTotal  = arguments[0].mTotal;

		if (this.atoms[ptr].match(this.frames[ptr], arguments[1], arguments[2]) ) {
			if ( this.frames[ptr].mLength > lenMax ) {
				frmMax = this.frames[ptr];
				lenMax = frmMax.mLength;
				if (lenMax == arguments[0].mMaxlen)
					break;
			}
		}
	}

	if (frmMax != null) {
		arguments[0].mId     = frmMax.mId;
		arguments[0].mLength = frmMax.mLength;
		arguments[0].mCount  = frmMax.mCount;
		return true;
	}
	return false;
}
rxOr.prototype.toString = function() {
	var idx, ret="";
	ret = this.atoms[1].toString();
	for(idx=2;idx<this.atoms.length;idx++)
		ret = ret.concat("|", this.atoms[idx].toString());
	return ret;
}
rxOr.prototype.addAtom   = RX_CONTAINER_ADD;
rxOr.prototype.icase = RX_CONTAINER_ICASE;


// ----------------------------------------------------------------------
// Operator ".", matches anything bu the "\r" character
// ----------------------------------------------------------------------
function rxAny() {
	this.id     = 46; // '.'
}
rxAny.prototype.match = function() {
	if (arguments[0].mMaxlen>0)
		if (arguments[1].charCodeAt(arguments[0].mOffset) != 13 ) {
			arguments[0].mCount = arguments[0].mLength = 1;
			return true;
		}
	return false;
}
rxAny.prototype.toString = function() {
	return ".";
}
rxAny.prototype.addAtom   = RX_NONCONTAINER_ADD;
rxAny.prototype.icase     = RX_NONCONTAINER_ICASE;

// ----------------------------------------------------------------------
// Operator "^" matches the start of string, or the first position after
//	a carriage return.  (Zero length match).
// ----------------------------------------------------------------------
function rxHead() {
	this.id            = 94; // '^'
}
rxHead.prototype.match = function() {
	if (arguments[0].mOffset==0) return this.success(arguments[0]);
	if (arguments[1].charCodeAt(arguments[0].mOffset-1)==13) return this.success(arguments[0]);
	return false;
}
rxHead.prototype.success = function() {
	arguments[0].mCount = 1;
	return true;
}
rxHead.prototype.toString = function() {
	return "^";
}
rxHead.prototype.addAtom   = RX_NONCONTAINER_ADD;
rxHead.prototype.icase     = RX_NONCONTAINER_ICASE;

// ----------------------------------------------------------------------
// Operator "$" matches the end of string, or the first position prior
//	ot a carriage return.  (Zero length match).
// ----------------------------------------------------------------------
function rxTail() {
	this.id     = 36; // '$';
}
rxTail.prototype.match = function() {
	if (arguments[0].mOffset>arguments[1].length-1) return this.success(arguments[0]);
	if (arguments[0].mOffset==arguments[1].length-1) return (arguments[0].mLength==arguments[1].length);
	if (arguments[1].charCodeAt(arguments[0].mOffset+1)==13) return this.success(arguments[0]);
	return false;
}
rxTail.prototype.toString = function() {
	return "$";
}
rxTail.prototype.success = function() {
	arguments[0].mCount = 1;
	return true;
}
rxTail.prototype.addAtom   = RX_NONCONTAINER_ADD;
rxTail.prototype.icase     = RX_NONCONTAINER_ICASE;

// ----------------------------------------------------------------------
// Operator "*" matches zero or more occurrances of the atomic operation
//	to which it is immediate parent.
// ----------------------------------------------------------------------
function rxStar() {
	this.id     = 42;
	this.atom   = null;
}
rxStar.prototype.match = function() {
	var f = new rxFrame();

	// setup the calling frame
	arguments[0].mCount=0;
	arguments[0].mLength=0;

	f.mMaxlen = arguments[0].mMaxlen;
	f.mTotal  = arguments[0].mTotal;

	while(f.mMaxlen > 0) {

		// setup local Frame for the child atom
		f.mMaxlen = arguments[0].mMaxlen - arguments[0].mLength;
		f.mOffset = arguments[0].mOffset + arguments[0].mLength
		f.mLength = 0;
		f.mCount = 0;

		if (this.atom.match(f,arguments[1],arguments[2])) {

			//include the match on the current length
			arguments[0].mCount  += 1;
			arguments[0].mLength += f.mLength;

			// if matched everything then exit
			if (arguments[0].mLength == arguments[0].mMaxlen)
				break;
			// avoid infinite loop
			if (f.mLength == 0)
				break;
		} else {
			break;
		}
	}
	// this atom always matches
	arguments[0].mId = this.mId;
	return true;
}
rxStar.prototype.addAtom      = RX_SINGLE_ADD;
rxStar.prototype.icase        = RX_SINGLE_ICASE;
rxStar.prototype.toString     = RX_POSTFIX_TOSTRING;

// ----------------------------------------------------------------------
// Operator "+" matches one or more occurrances of the atomic operation
//	to which it is immediate parent.
// ----------------------------------------------------------------------
function rxPlus() {
	this.id     = 43;
	this.atom   = null;
}
rxPlus.prototype.match = function() {
	var f = new rxFrame();

	// setup the calling frame
	arguments[0].mCount=0;
	arguments[0].mLength=0;

	f.mMaxlen = arguments[0].mMaxlen;
	f.mTotal  = arguments[0].mTotal;

	while(f.mMaxlen > 0) {

		// setup local Frame for the child atom
		f.mMaxlen = arguments[0].mMaxlen - arguments[0].mLength;
		f.mOffset = arguments[0].mOffset + arguments[0].mLength
		f.mLength = 0;
		f.mCount = 0;

		if (this.atom.match(f,arguments[1])) {

			//include the match on the current length
			arguments[0].mCount  += 1;
			arguments[0].mLength += f.mLength;

			// if matched everything then exit
			if (arguments[0].mLength == arguments[0].mMaxlen)
				break;
			// avoid infinit loop
			if (f.mLength == 0)
				break;
		} else {
			break;
		}
	}

	// return true only if we matched something
	if (arguments[0].mCount > 0) {
		// this atom always matches
		arguments[0].mId = this.mId;
		return true;
	}
	return false;
}
rxPlus.prototype.icase	  = RX_SINGLE_ICASE;
rxPlus.prototype.toString = RX_POSTFIX_TOSTRING;
rxPlus.prototype.addAtom  = RX_SINGLE_ADD;

// ----------------------------------------------------------------------
// Operator "?" matches zero or one occurrances of the atomic operation
//	to which it is immediate parent.
// ----------------------------------------------------------------------
function rxOption() {
	this.id     = 63;
	this.atom   = null;
}
rxOption.prototype.match = function() {
	this.atom.match(arguments[0], arguments[1], arguments[2]);
	frm.mCount = 1;
	return true;
}
rxOption.prototype.addAtom  = RX_SINGLE_ADD;
rxOption.prototype.icase    = RX_SINGLE_ICASE;
rxOption.prototype.toString = RX_POSTFIX_TOSTRING;

// ----------------------------------------------------------------------
// Operator "{n,m}" matches n to m occurrances of the atomic operation
//	to which it is immediate parent.
// ----------------------------------------------------------------------
function rxMulti() {
	this.id     = 0;
	this.atom   = null;
	this.frame  = new rxFrame();
	this.mMin   = 0;
	this.mMax   = 0;
}
rxMulti.prototype.match = function() {
	var mCount, mLength, mOffset;

	arguments[0].mCount  = 0;
	arguments[0].mLength = 0;

	mOffset = arguments[0].mOffset;
	mCount  = 0;
	mLength = 0;

	this.frame.mMaxlen = arguments[0].mMaxlen;
	this.frame.mOffset = arguments[0].mOffset;
	this.frame.mTotal  = arguments[0].mTotal;
	this.frame.mCount  = 0;
	this.frame.mLength = 0;

	while (mLength < arguments[0].mMaxlen) {

		// update the match frame
		this.frame.mMaxlen -= this.frame.mLength;
		this.frame.mOffset += this.frame.mLength;
		this.frame.mCount  = 0;
		this.frame.mLength = 0;

		// test for a match
		if (!this.atom.match(this.frame,arguments[1],arguments[2]))
			break;

		// include the current match in the total matches
		mLength += this.frame.mLength;
		mCount++;

		// avoid infinit loops
		if ( this.frame.mLength < 1 ) break;
		// don't match more than is allowed
		if ( this.mMax > 0 && (mCount==this.mMax) ) break;
	}

	if (mCount >= this.mMin) {
		arguments[0].mId     = this.mId;
		arguments[0].mCount  = mCount;
		arguments[0].mLength = mLength;
		return true;
	}
	return false;
}
rxMulti.prototype.toString = function() {
	return this.atom.toString() + "{" + this.mMin + "," + this.mMax + "}";
}
rxMulti.prototype.addAtom = RX_SINGLE_ADD;
rxMulti.prototype.icase   = RX_SINGLE_ICASE;

// ----------------------------------------------------------------------
// Literal "..." matches literal values found between quotes.
// ----------------------------------------------------------------------
function rxLiteral(pattern) {
	this.id     = 0;
	this.value  = pattern.toString();
}
function RXLITERAL_MATCH(frm, str, mat) {
	var tmp;
	if (frm.mMaxlen<1) return false;
	if (str.length > frm.mMaxlen)
		tmp = (str.substring(frm.mOffset,frm.mMaxlen) == this.value );
	else
		tmp = (str.substring(frm.mOffset,this.value.length) == this.value );

	if (tmp) {
		frm.mCount  = 1;
		frm.mLength = this.value.length;
		frm.mId     = 0;
		return true;
	}
	return false;
}
function RXLITERAL_MATCHI(frm, str, mat) {
	var tmp;
	if (frm.mMaxlen<1) return false;
	if (str.length > frm.mMaxlen)
		tmp = (str.substring(frm.mOffset,frm.mMaxlen).toLowerCase() == this.value );
	else
		tmp = (str.substring(frm.mOffset,this.value.length).toLowerCase() == this.value );

	if (tmp) {
		frm.mCount  = 1;
		frm.mLength = this.value.length;
		frm.mId     = 0;
		return true;
	}
	return false;
}
rxLiteral.prototype.icase = function () {
	this.value = this.value.toLowerCase();
	this.match = (arguments[0]) ? (RXLITERAL_MATCHI) : (RXLITERAL_MATCH);
}
rxLiteral.prototype.toString = function () {
	return this.value.toString();
}
rxLiteral.prototype.match   = RXLITERAL_MATCH;
rxLiteral.prototype.addAtom = RX_NONCONTAINER_ADD;

// ----------------------------------------------------------------------
// Character Class Atom [...]
// ----------------------------------------------------------------------
function rxCharset() {
	this.id     = 0;
	this.bits   = new rxBitset();
	this.icase  = false;
	this.tos    = "";
}
function RXCHRS_MATCHI(frm, str) {
	var ret=true;
	ret = ret && this.bits.get(str.charAt(frm.mOffset).toUpperCase().charCodeAt(0));
	ret = ret && this.bits.get(str.charAt(frm.mOffset).toLowerCase().charCodeAt(0));
	if (!ret) return false;
	frm.mLength = 1;
	frm.mCount  = 1;
	frm.mId     = str.charCodeAt(frm.mOffset);
	return true;
}
function RXCHRS_MATCH(frm, str, mat) {
	if (!this.bits.get(str.charCodeAt(frm.mOffset)) ) return false;
	frm.mLength = 1;
	frm.mCount  = 1;
	frm.mId     = str.charCodeAt(frm.mOffset);
	return true;
}
rxCharset.prototype.hasChar = function() {
	return this.bits.get(arguments[0]);
}
rxCharset.prototype.icase = function() {
	this.match = (arguments[0])?(RXCHRS_MATCHI):(RXCHRS_MATCH);
}
rxCharset.prototype.addString = function () {
	var index;
	for(index=0;index<arguments[0].length;index++)
		this.bits.set(arguments[0].charCodeAt(index));
	this.tos = this.tos.concat(arguments[0]);
}
rxCharset.prototype.addRange = function() {
	var index;
	if (arguments[1]<arguments[0])
		this.addRange(arguments[1],arguments[0]);
	else {
		for(index=arguments[0];index<=arguments[1];index++) {
			this.bits.set(index);
		}
	this.tos = this.tos.concat(EscapeOf(arguments[0])+"-"+EscapeOf(arguments[1]));
	}
}
rxCharset.prototype.addChar = function() {
	this.bits.set(arguments[0]);
	this.tos = this.tos.concat(EscapeOf(arguments[0]))
}
rxCharset.prototype.toString = function() {
	if( this.bits.negate) return "[^"+this.tos+"]";
	else return "["+this.tos+"]";
}
rxCharset.prototype.setNegate = function() {
	this.bits.negate = arguments[0];
	return this.bits.negate;
}
rxCharset.prototype.getNegate = function() {
	return this.bits.negate;
}
rxCharset.prototype.match   = RXCHRS_MATCH;
rxCharset.prototype.addAtom = RX_NONCONTAINER_ADD;

// ----------------------------------------------------------------------
// Atomic Call Operator (for utility purposes)
// ----------------------------------------------------------------------
function rxCall() {
	this.atom = null;
	this.id   = 0;
}
rxCall.prototype.match = function() {
	if(this.atom.match(arguments[0],arguments[1],arguments[2])){
		frm.mId = this.id;
		return true;
	}
	return false;
}
rxCall.prototype.toString = function() {
	return this.atom.toString();
}
rxCall.prototype.addAtom = RX_SINGLE_ADD;
rxCall.prototype.icase   = RX_SINGLE_ICASE;

// ----------------------------------------------------------------------
// / operator
// ----------------------------------------------------------------------
function rxIIF() {
	this.id = 0;
	this.atom1 = this.atom2 = null;
	this.frame1 = new rxFrame();
	this.frame2 = new rxFrame();
}
rxIIF.prototype.match = function() {
	var more;

	this.frame1.mCount  = 0;
	this.frame1.mLength = 0;
	this.frame1.mOffset = arguments[0].mOffset;
	this.frame1.mMaxlen = arguments[0].mMaxlen;
	this.frame1.mTotal  = arguments[0].mTotal;

	// slight optimization
	this.frame2.mTotal  = arguments[0].mTotal;

	// prime the pump, first atom must match
	more = this.atom1.match(this.frame1,str,mat);
	while (more) {

		// setup the second frame for matching
		this.frame2.mCount  = 0;
		this.frame2.mLength = 0;
		this.frame2.mOffset = this.frame1.mOffset + this.frame1.mLength;
		this.frame2.mMaxLen = arguments[0].mMaxlen - this.frame1.mLength;

		if (this.atom2.match(this.frame2,arguments[1],arguments[2]) ) {
			// return only the first atoms match
			arguments[0].mId     = this.frame1.mId;
			arguments[0].mCount  = this.frame1.mCount;
			arguments[0].mLength = this.frame1.mLength;
			return true;
		}
		if ( !this.decrMatch() ) break;
	}
	return false;
}
rxIIF.prototype.decrMatch = function() {
	// backtracking only applies to tokens that return a nonzero length match
	if (this.frame1.mLength<1 || this.frame1.mCount<1) return false;
	this.frame1.mCount  = 0;
	this.frame1.mMaxlen = this.frame1.mLength - 1;
	this.frame1.mLength = 0
	return true;
}
rxIIF.prototype.toString = function() {
	return this.atom1.toString() + "/" + this.atom2.toString();
}
rxIIF.prototype.addAtom = RX_DOUBLE_ADD;
rxIIF.prototype.icase   = RX_DOUBLE_ICASE;

// ----------------------------------------------------------------------
// \d \D atom
// ----------------------------------------------------------------------
function rxDigit(negate) {
	this.id      = 0;
	this.atom    = new rxCharset();
	this.atom.setNegate(negate);
	this.atom.addString("0123456789");
}
rxDigit.prototype.match = function() {
	return this.atom.match(arguments[0],arguments[1],arguments[2]);
}
rxDigit.prototype.toString = function() {
	if (this.atom.getNegate()) return "\\D";
	else return "\\d";
}
rxDigit.prototype.icase   = RX_NONCONTAINER_ICASE;
rxDigit.prototype.addAtom = RX_NONCONTAINER_ADD;


// ----------------------------------------------------------------------
// \w \W atom
// ----------------------------------------------------------------------
function rxWordChar(negate) {
	this.id      = 0;
	this.atom    = new rxCharset();
	this.atom.setNegate(negate);
	this.atom.addRange(48, 57); // 0-9
	this.atom.addRange(97,122); // a-z
	this.atom.addRange(65, 90); // A-Z
}
rxWordChar.prototype.hasChar = function() {
	return this.atom.hasChar(arguments[0]);
}
rxWordChar.prototype.match = function() {
	return this.atom.match(arguments[0],arguments[1],arguments[2]);
}
rxWordChar.prototype.toString = function() {
	if (this.atom.getNegate()) return "\\W";
	else return "\\w";
}
rxWordChar.prototype.addAtom = RX_NONCONTAINER_ADD;
rxWordChar.prototype.icase   = RX_NONCONTAINER_ICASE;


// ----------------------------------------------------------------------
// \b \B atom
// ----------------------------------------------------------------------
function rxWordBound(negate) {
	this.id      = 0;
	this.negate  = negate;
	this.atom    = new rxWordChar(false);

}
rxWordBound.prototype.matchStart = function() {
	// the current character must be a word character
	if ( this.atom.hasChar(arguments[1].charCodeAt(arguments[0].mOffset)) )
		if ( arguments[0].mOffset>0 )
			return !this.atom.hasChar(arguments[1].charCodeAt(arguments[0].mOffset-1));
		else return true;
	return false;
}
rxWordBound.prototype.matchEnd = function() {
	// at the end of string
	if (arguments[0].mOffset>=arguments[1].length-1)
		if (this.atom.hasChar(arguments[1].charCodeAt(arguments[1].length-1)) ) {
			arguments[0].mCount = 1;
			return true;
		}

	// the current character must be a word character
	if ( !this.atom.hasChar(arguments[1].charCodeAt(arguments[0].mOffset)) )
		if (this.atom.hasChar(arguments[1].charCodeAt(arguments[0].mOffset-1))) {
			arguments[0].mCount = 1;
			return true;
		}
	return false;
}
rxWordBound.prototype.match = function() {
	if (str.length<1) return false;
	if (this.negate) {
		if ( this.matchStart(arguments[0],arguments[1]) || this.matchEnd(arguments[0],arguments[1]) )
			return false;
		else
			return true;
	}
	if ( this.matchStart(arguments[0],arguments[1]) || this.matchEnd(arguments[0],arguments[1]) )
		return true;
	return false;
}
rxWordBound.prototype.toString = function() {
	if (this.negate) return "\\B";
	else return "\\b";
}
rxWordBound.prototype.addAtom = RX_NONCONTAINER_ADD;
rxWordBound.prototype.icase   = RX_NONCONTAINER_ICASE;

// ----------------------------------------------------------------------
// \s \S atom
// ----------------------------------------------------------------------
function rxWhite(negate) {
	this.id      = 0;
	this.atom    = new rxCharset();
	this.atom.setNegate(negate);
	this.atom.addString(" \f\n\r\t");
}
rxWhite.prototype.match = function() {
	return this.atom.match(arguments[0],arguments[1],arguments[2]);
}
rxWhite.prototype.toString = function() {
	if (this.atom.getNegate()) return "\\S";
	else return "\\s";
}
rxWhite.prototype.addAtom = RX_NONCONTAINER_ADD;
rxWhite.prototype.icase   = RX_NONCONTAINER_ICASE;

//
// Regular Expression Compiler
//
function rxCompiler() {
	this.mPattern = "";
	this.mOffset  = 0;

	this.mLeft    = new Array();
	this.mRight   = new Array();
	this.mMode    = new Array();
	this.mMode[0] = "default";

	this.mEscaped = false;
	this.mMax     = 0;
	this.mMin     = 0;
	this.mChar    = 0;
	this.mSubId   = 0;
	this.mError   = "";
}
rxCompiler.prototype.cc = function() {

	var tok, hasIIF=false;
	this.reset();
	this.mPattern = arguments[0].toString();

	while (!this.eof() & this.mError.length==0) {

		this.mChar = this.getc()
		if (this.mChar<=47) {
			if (this.mChar==34) {
				this.compileQuoted();
			} else if (this.mChar==36) {
				this.pushToken(0,new rxTail());
			} else if (this.mChar==40) {
				this.pushToken(40, new rxAnd() );
			} else if (this.mChar==41) {
				this.resolveAll(true);
			} else if (this.mChar==42) {
				this.pushToken(42, new rxStar());
				this.resolvePostfix();
			} else if (this.mChar==43) {
				this.pushToken(43, new rxPlus());
				this.resolvePostfix();
			} else if (this.mChar==46) {
				this.pushToken(0, new rxAny());
			} else if (this.mChar==47) {
				this.pushToken(47, new rxIIF());
			} else {
				this.pushToken(0, new rxChar(this.mChar));
			}
		} else {
			if (this.mChar==63) {
				this.pushToken(63, new rxOption());
				this.resolvePostfix();
			} else if(this.mChar==91) {
				this.compileCharset();
				//this.pushToken(0, new rxAny());
			} else if (this.mChar==94) {
				this.pushToken(0,new rxHead());
			} else if (this.mChar==92) {
				this.pushToken(0, this.compileEscapedAtom() );
			} else if(this.mChar==123) {
				this.compileMulti();
				this.resolvePostfix();
			} else if (this.mChar==124) {
				this.pushToken(124, new rxOr());
			} else {
				this.pushToken(0, new rxChar(this.mChar));
			}
		}
	}
	this.resolveAll(false);

	// return the atomic operation!
	return this.mLeft.pop().value;
}
rxCompiler.prototype.compileEscaped = function() {
	var tmp;
	tmp = this.getc();
	if (tmp==98)  return  8;
	if (tmp==102) return 12;
	if (tmp==114) return 13;
	if (tmp==110) return 10;
	if (tmp==116) return  9;
	if (tmp==117 || tmp==120)
		return this.compileNumeric(16,"0123456789abcdef");
	if (tmp==111)
		return this.compileNumeric( 8,"01234567");
	if (tmp==105)
		return this.compileNumeric(10,"0123456789");
	return tmp;
}
rxCompiler.prototype.compileEscapedAtom = function() {
	var tmp;
	if (this.eof()) {
		this.mError = "unterminated escape sequence";
		return null;
	}
	tmp = this.getc();

	if (tmp==66||tmp== 98) return new rxWordBound(tmp==66);
	if (tmp==68||tmp==100) return new rxDigit(tmp==68);
	if (tmp==83||tmp==115) return new rxWhite(tmp==83);
	if (tmp==87||tmp==119) return new rxWordChar(tmp==87);
	if (tmp==102) return new rxChar(12);
	if (tmp==114) return new rxChar(13);
	if (tmp==110) return new rxChar(10);
	if (tmp==116) return new rxChar(9);
	if (tmp==117)
		return new rxChar(this.compileNumeric(16,"0123456789abcdef"));
	if (tmp==120)
		return new rxChar(this.compileNumeric(16,"0123456789abcdef"));
	if (tmp==111)
		return new rxChar(this.compileNumeric( 8,"01234567"));
	if (tmp==105)
		return new rxChar(this.compileNumeric(10,"0123456789"));

	return new rxChar(tmp);
}
rxCompiler.prototype.compileNumeric = function() {
	var tmp,result,count,idx;
	count = result = 0;
	while(!this.eof) {
		tmp = this.getc();
		idx = arguments[1].indexOf(String.fromCharCode(tmp).toLowerCase());
		if (idx>=0) {
			count++;
			result *= arguments[0];
			result +=  idx;
		} else {
			this.ungetc(tmp);
			this.mError = "invalid numeric character after escape";
			return 0;
		}
	}
	return result;
}
rxCompiler.compileQuoted = function() {
	var result="", tmp, escaped=false, more=false, ended=false;

	more = !this.eof();
	while (more && !this.eof()) {

		tmp = this.getc();
		if (tmp==34) {
			if (escaped) result = result.concat("\"");
			else more=false;
			escaped=false;
		} else if (tmp==92) {
			if (escaped) result = result.concat("\\");
			else escaped = true;
		} else if (tmp==114) {
			result = result.concat("\r");
			escaped = false;
		} else if (tmp==110) {
			result = result.concat("\n");
			escaped=false;
		} else if (tmp==9) {
			result = result.concat("\t");
			escaped=false;
		} else {
			result = result.concat(String.fromCharCode(tmp));
			escaped=false;
		}
	}
	if (more && this.eof) {
		this.mError = "unterminated quote";
		return;
	}
	// push the new literal value onto the left stack
	this.pushToken(0, new rxLiteral(result));
}
rxCompiler.prototype.compileMulti = function() {
	var tmp, result, more, offset;

	// the result is a new multiple atom
	result = new rxMulti();
	offset = this.mOffset;

	// while there is more to read
	more = !this.eof();

	// parse the first numeric
	result.mMin=0;
	while (more && !this.eof()) {
		tmp = this.getc();
		if (tmp==44) {
			more = false;
		} else if (tmp==125) {
			if (result.mMin<1) {
				this.mError = "unexpected termination of multiple";
				return;
			}
			result.mMax = result.mMin;
			this.pushToken(0,result);
			return;
		} else {
			if (tmp>=48 && tmp<=57) {
				result.mMin *= 10;
				result.mMin += (tmp-48);
			} else {
				this.mError = "unterminated multiple";
				return;
			}
		}
	}

	// insure there is more to read
	if ( tmp!=44 || this.eof() ) {
		this.mError = "unterminated multiple" + offset;
		return;
	}

	more = !this.eof();
	while (more && !this.eof()) {
		tmp = this.getc();
		if (tmp>=48 && tmp<=57) {
			result.mMax *= 10;
			result.mMax += (tmp-48);
		} else if (tmp==125) {
			more = false;
		} else {
			this.mError = "invalid character, offset=" + this.mOffset;
			return;
		}
	}

	if (more) {
		this.mError = "unterminated multiple, offset=" + offset;
		return;
	}

	if (result.mMax==0) {
		if (result.mMin==0) {
			this.mError = "invalid multiple, offset=" + offset;
			return;
		}
	} else {
		if (result.mMax < result.mMin) {
			this.mError = "invalid multiple, offset=" + offset;
			return;
		}
	}

	// push the new multiple onto the stack
	this.pushToken(0,result);
}
rxCompiler.prototype.compileCharset = function() {
	var negate, count, more, tmp, result=new rxCharset();
	var store, range, last, low, idx;

	count  = 0;
	more   = true;
	negate = false;
	range  = 0;
	last   = 0;

	while( more && !this.eof() ) {
		tmp = this.getc();
		store=true;
		if (tmp==45) {
			if (count!=0) {
				range=1;
				tmp=last;
				store=false;
			}
		} else if (tmp==94) {
			if (count==0) {
				negate = true;
				store  = false;
			}
		} else if (tmp==92) {
			tmp = this.compileEscaped();
		} else if (tmp==93) {
			if (range) {
				this.mError = "unterminated end range at or near position " + this.mOffset;
				return;
			}
			if (count!=0)
				store=more=false;
		}

		if (range==1) {
			range++;
			low = last;
		} else if (range==2) {
			range=0;
			if (low<=tmp)
				result.addRange(low,tmp);
			else
				result.addRange(tmp,low);
			low=0;
		} else {
			if ((this.peekc()!=45) && store) result.addChar(tmp);
			last=tmp;
		}
		count++;
	}
	if (negate) result.setNegate(true);
	// push the new character set onto the stack
	this.pushToken( 0, result );
}
rxCompiler.prototype.reset = function() {
	this.mPattern = "";

	this.mLeft    = new Array();
	this.mRight   = new Array();
	this.mMode    = new Array();
	this.mMode[0] = "default";
	this.mSubId   = 0;;

	this.mEscaped = false;
	this.mLiteral = "";
	this.mMax     = 0;
	this.mMin     = 0;
	this.mChar    = 0;
}
rxCompiler.prototype.eof = function() {
	return (this.mPattern.length <1);
}
rxCompiler.prototype.getc = function() {
	if (this.eof()) return -1;
	tmp = this.mPattern.charCodeAt();
	if (this.mPattern.length==1)
		this.mPattern = "";
	else
		this.mPattern = this.mPattern.substr(1);
	return tmp;
}
rxCompiler.prototype.peekc = function() {
	if (this.eof()) return -1;
	return this.mPattern.charCodeAt();
}
rxCompiler.prototype.ungetc = function() {
	this.mPattern = String.fromCharCode(arguments[0]).concat(this.mPattern);
}
rxCompiler.prototype.pushToken = function() {
	var tmp = new rxToken();
	tmp.id=arguments[0];
	tmp.value=arguments[1];
	tmp.offset=this.mOffset;
	this.mLeft.push(tmp);
}
rxCompiler.prototype.resolveAll = function() {
	this.resolveRIO();
	this.resolveAnd(arguments[0]);
}
rxCompiler.prototype.resolvePostfix = function() {
	var parent, child;
	if (this.mError.length>0) return;
	if (this.mLeft.length <2) {
		this.mError = "postfix operator applies to null";
		return;
	}
	parent = this.mLeft.pop();
	child  = this.mLeft.pop();
	parent.value.addAtom(child.value);
	parent.id = 0;
	this.mLeft.push(parent);
}
rxCompiler.prototype.resolveAnd = function() {
	var tkn, atom, back, offset;
	// until we hit the end of the stack or a left paren,
	// move everything to the left hand stack
	if (this.mError.length>0) return;
	while(this.mLeft.length>0) {
		tkn = this.mLeft.pop();
		if (tkn.id==40) {
			// put it back
			offset = tkn.offset;
			break;
		}
		// move it to the right hand side
		this.mRight.push(tkn);
	}
	back=0;

	// we are reducing an and operation
	if (this.mRight.length==0) {
		this.mError = "empty subexpression at " + offset;
		return;
	}

	if (this.mRight.length==1 && arguments[0]==false) {
		this.mLeft.push(this.mRight.pop());
		return;
	}

	atom = new rxAnd();
	if (arguments[0])
		atom.subId = this.mSubId++;
	while(this.mRight.length>0) {
		atom.addAtom(this.mRight.pop().value);
	}
	tkn = new rxToken(0,atom);
	tkn.offset = offset;
	this.mLeft.push(tkn);
}
rxCompiler.prototype.resolveRIO = function() {
	var tkn, etc, more, atom;
	if (this.mError.length>0) return;
	more=true;
	etc=null;
	while(this.mLeft.length>0 && more) {
		tkn = this.mLeft.pop();

		if (tkn.id==40) {
			this.mLeft.push(tkn);
			more=false;
		} else if (tkn.id==124) {
			if (this.mRight.length<1) {
				this.mError = "null right operand position" + tkn.offset;
				return;
			}
			if (this.mLeft.length<1) {
				this.mError = "null left operand postion" + tkn.offset;
				return;
			}
			etc=this.mLeft.pop();
			if (etc.id!=0) {
				this.mError = "null left operand postion" + tkn.offset;
				return;
			}
			atom = new rxOr();
			atom.addAtom(etc.value);
			atom.addAtom(this.mRight.pop().value);
			tkn.value = atom;
			tkn.id    = 0;
			this.mRight.push(tkn);
		} else if (tkn.id==47) {
			if (this.mRight.length<1) {
				this.mError = "null right operand position" + tkn.offset;
				return;
			}
			if (this.mLeft.length<1) {
				this.mError = "null left operand postion" + tkn.offset;
				return;
			}
			etc=this.mLeft.pop();
			if (etc.id!=0) {
				this.mError = "null left operand postion" + tkn.offset;
			}
			atom = new rxIIF();
			atom.addAtom(etc.value);
			atom.addAtom(this.mRight.pop().value);
			tkn.value = atom;
			tkn.id    = 0;
			this.mRight.push(tkn);
		} else {
			this.mRight.push(tkn);
		}
	}

	// return everything back to the left hand stack
	while(this.mRight.length>0)
		this.mLeft.push(this.mRight.pop());
}

//
// Match Result
//
function Match(subsets) {
	this.reset(subsets);
	this.mOffset = 0;
	this.value  = "";
	this.mLength = 0;
	this.values = null;
}
Match.prototype.toString = function() {
	return this.value;
}
Match.prototype.toStringX = function() {
	var idx,ret = "";
	for(idx=1;idx<this.values.length;idx++)
		ret=ret.concat("{" + this.values[idx] + "}");
	return ret;
}
Match.prototype.reset = function() {
	this.mOffset   = 0;
	this.mLength   = 0;
	this.value     = "";
	if (arguments[0]) this.values = new Array();
}

function Regex(pattern) {
	this.atom    = null;
	this.pattern = null;
	this.mError  = "";
	var c = new rxCompiler();
	this.atom = c.cc(pattern);
	this.mError = c.mError;
	c=null;
}
Regex.prototype.first = function() {
	if (this.atom==null) {
		this.mError = "missing pattern";
		return null;
	}
	return this.next(arguments[0], new Match(true));
}
Regex.prototype.next = function() {
	var f,m=null,idx;

	idx = arguments[1].mOffset + arguments[1].mLength;
	if (idx>=arguments[0].length) return null;

	f = new rxFrame();
	f.stringInit(arguments[0]);

	m = new Match(true);
	for(;idx<arguments[0].length;idx++) {
		f.mOffset = idx;
		f.mMaxlen = arguments[0].length-idx;
		m.reset(true);
		if (this.atom.match(f,arguments[0],m)) {
			m.mOffset = f.mOffset;
			m.mLength = f.mLength;
			m.value   = arguments[0].substr(f.mOffset,f.mLength);
			return m;
		}
	}
	return null;
}
Regex.prototype.all = function() {
	var a=new Array();;
	a[0] = this.first(arguments[0]);
	while ( a[a.length-1]!=null)  {
		a[a.length]=this.next(arguments[0],a[a.length-1]);
	}
	a.pop();
	if (a.length<1) return null;
	return a;
}
Regex.prototype.toString = function() {
	if (this.mError.length>0) return "Regex:" + this.mError;
	if (this.atom==null) return "Regex:null";
	else return "Regex:" + this.atom.toString();
}

function EscapeOf(c) {
	if (c==0)    return "\\0";
	if (c=='\f') return "\\f";
	if (c=='\r') return "\\r";
	if (c=='\n') return "\\n";
	if (c=='\t') return "\\t";
	if (c=='[')  return "\\[";
	if (c==']')  return "\\]";
	if (c=='-')  return "\\-";
	if (c=='\\') return "\\\\";
	return String.fromCharCode(c);
}

// ----------------------------------------------------------------------
// Generic: Assigned member functions
// ----------------------------------------------------------------------
function RX_NONCONTAINER_ADD(atom) {
}
function RX_NONCONTAINER_ICASE(value) {
}
function RX_CONTAINER_ADD(item) {
	this.frames[this.frames.length] = new rxFrame();
	this.atoms[this.atoms.length]   = item;
	return item;
}
function RX_CONTAINER_ICASE(value) {
	var index;
	if (this.atoms.length > 0)
		for (index=1;index<this.atoms.length;index++)
			this.atoms[index].icase(value);
}
function RX_SINGLE_ADD(item) {
	this.atom = item;
	return item;
}
function RX_SINGLE_ICASE(value) {
	this.atom.icase(value);
}
function RX_POSTFIX_TOSTRING() {
	return this.atom.toString().concat(String.fromCharCode(this.id));
}
function RX_DOUBLE_ADD(item) {
	if (this.atom1==null) this.atom1=item;
	else this.atom2=item;
	return item;
}
function RX_DOUBLE_ICASE(value) {
	this.atom1.icase(value);
	this.atom2.icase(value);
}
function rxToken(id,value) {
	this.id     = id;
	this.value  = value;
	this.offset = 0;
}

*/
