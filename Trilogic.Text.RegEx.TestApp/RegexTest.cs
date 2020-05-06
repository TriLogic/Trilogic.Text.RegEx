using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using TriLogic.PatternPro.Regex;

namespace TriLogic.PatternPro.Regex.Test
{

	/// <summary>
	/// Summary description for RegexTest.
	/// </summary>
	public class RegexTest : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.TextBox txtSource;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnFirst;
		private System.Windows.Forms.Button btnNext;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.TextBox txtExpr;
		private System.Windows.Forms.Button btnCompile;
		private System.Windows.Forms.Label lblCompiled;

		private Regex      r;
		private RegexMatch m;

		public RegexTest()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			r = null;
			m = null;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RegexTest));
			this.panel2 = new System.Windows.Forms.Panel();
			this.txtSource = new System.Windows.Forms.TextBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.txtExpr = new System.Windows.Forms.TextBox();
			this.btnFirst = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnCompile = new System.Windows.Forms.Button();
			this.lblCompiled = new System.Windows.Forms.Label();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel2.Controls.Add(this.txtSource);
			this.panel2.Location = new System.Drawing.Point(192, 112);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(424, 280);
			this.panel2.TabIndex = 0;
			// 
			// txtSource
			// 
			this.txtSource.AcceptsReturn = true;
			this.txtSource.AcceptsTab = true;
			this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSource.Location = new System.Drawing.Point(0, 0);
			this.txtSource.Multiline = true;
			this.txtSource.Name = "txtSource";
			this.txtSource.Size = new System.Drawing.Size(408, 264);
			this.txtSource.TabIndex = 0;
			this.txtSource.Text = "Integer numbers: +100, -25, 5000\r\nReal Numbers: 3.1417, -200.25, +1.6625e-2, -3.3" +
				"75009e+7, .0025\r\n\r\nThere is a blank line preceeding this text.\r\n\r\nend.";
			// 
			// panel3
			// 
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel3.Location = new System.Drawing.Point(8, 112);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(168, 280);
			this.panel3.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Regular Expression:";
			// 
			// txtExpr
			// 
			this.txtExpr.Location = new System.Drawing.Point(8, 24);
			this.txtExpr.Name = "txtExpr";
			this.txtExpr.Size = new System.Drawing.Size(608, 20);
			this.txtExpr.TabIndex = 3;
			this.txtExpr.Text = "[-+]?(([0-9]+)|([0-9]*\\.[0-9]+)([eE][-+]?[0-9]+)?)";
			// 
			// btnFirst
			// 
			this.btnFirst.Location = new System.Drawing.Point(192, 80);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.Size = new System.Drawing.Size(104, 24);
			this.btnFirst.TabIndex = 4;
			this.btnFirst.Text = "&First Match";
			this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(304, 80);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(96, 24);
			this.btnNext.TabIndex = 5;
			this.btnNext.Text = "Find &Next";
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnCompile
			// 
			this.btnCompile.Location = new System.Drawing.Point(8, 80);
			this.btnCompile.Name = "btnCompile";
			this.btnCompile.Size = new System.Drawing.Size(128, 24);
			this.btnCompile.TabIndex = 6;
			this.btnCompile.Text = "&Compile";
			this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
			// 
			// lblCompiled
			// 
			this.lblCompiled.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblCompiled.Location = new System.Drawing.Point(8, 56);
			this.lblCompiled.Name = "lblCompiled";
			this.lblCompiled.Size = new System.Drawing.Size(600, 16);
			this.lblCompiled.TabIndex = 7;
			// 
			// RegexTest
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(624, 405);
			this.Controls.Add(this.lblCompiled);
			this.Controls.Add(this.btnCompile);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.btnFirst);
			this.Controls.Add(this.txtExpr);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RegexTest";
			this.Text = "PatternPro Regular Expression Demo";
			this.Load += new System.EventHandler(this.RegexTest_Load);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private void RegexTest_Load(object sender, System.EventArgs e)
		{
		
		}

		private void btnFirst_Click(object sender, System.EventArgs e)
		{
			r = new Regex(txtExpr.Text);
			m = r.FirstMatch(txtSource.Text);
			if(m!=null)
			{
				txtSource.Select(m.Offset,m.Length);
				txtSource.Select();
			}
			else
			{
				System.Windows.Forms.MessageBox.Show(this,"No match found!","Match result",System.Windows.Forms.MessageBoxButtons.OK);
			}
		}

		private void btnNext_Click(object sender, System.EventArgs e)
		{
			if(m==null)
			{
				System.Windows.Forms.MessageBox.Show(this,"No previous match!","Match result",System.Windows.Forms.MessageBoxButtons.OK);
				return;
			}
			m = r.NextMatch(m,txtSource.Text);
			if(m!=null)
			{
				txtSource.Select(m.Offset,m.Length);
				txtSource.Select();
			}
			else
			{
				System.Windows.Forms.MessageBox.Show(this,"No match found!","Match result",System.Windows.Forms.MessageBoxButtons.OK);
			}
		}

		private void btnCompile_Click(object sender, System.EventArgs e)
		{
			r = new Regex(txtExpr.Text);
			lblCompiled.Text = r.ToString();
		}
	}
}
