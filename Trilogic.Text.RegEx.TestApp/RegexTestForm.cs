using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Trilogic.Text.RegEx;

namespace PatternProTest
{
	/// <summary>
	/// Summary description for RegexTestForm.
	/// </summary>
	public class RegexTestForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnFirst;
		private System.Windows.Forms.TextBox txtExpr;
		private System.Windows.Forms.Label labelRegex;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private RxMatcher r;
		private RxMatch m;
        private StringBuilder pattern = new StringBuilder();
        private Panel panelTop;
        private Panel panelExpression;
        private SplitContainer splitPanel;
        private TextBox txtSource;
        private FlowLayoutPanel panelButtons;
        private Panel panelLower;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusCompiled;
        private ToolStripStatusLabel lblCompiled;
        private ListBox listBoxMatches;
        private bool recompile = true;

		public RegexTestForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnNext = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.txtExpr = new System.Windows.Forms.TextBox();
            this.labelRegex = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.panelExpression = new System.Windows.Forms.Panel();
            this.splitPanel = new System.Windows.Forms.SplitContainer();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.panelLower = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusCompiled = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCompiled = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBoxMatches = new System.Windows.Forms.ListBox();
            this.panelTop.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelExpression.SuspendLayout();
            this.splitPanel.Panel1.SuspendLayout();
            this.splitPanel.Panel2.SuspendLayout();
            this.splitPanel.SuspendLayout();
            this.panelLower.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(128, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(119, 29);
            this.btnNext.TabIndex = 6;
            this.btnNext.Text = "Find &Next";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirst.Location = new System.Drawing.Point(3, 3);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(119, 29);
            this.btnFirst.TabIndex = 5;
            this.btnFirst.Text = "&First Match";
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // txtExpr
            // 
            this.txtExpr.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtExpr.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExpr.Location = new System.Drawing.Point(5, 23);
            this.txtExpr.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtExpr.Name = "txtExpr";
            this.txtExpr.Size = new System.Drawing.Size(776, 26);
            this.txtExpr.TabIndex = 2;
            this.txtExpr.Text = "[-+]?([0-9]+|([0-9]*\\.[0-9]+)([eE][-+]?[0-9]+)?)";
            this.txtExpr.WordWrap = false;
            this.txtExpr.TextChanged += new System.EventHandler(this.txtExpr_TextChanged);
            // 
            // labelRegex
            // 
            this.labelRegex.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelRegex.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRegex.Location = new System.Drawing.Point(5, 5);
            this.labelRegex.Name = "labelRegex";
            this.labelRegex.Size = new System.Drawing.Size(776, 18);
            this.labelRegex.TabIndex = 1;
            this.labelRegex.Text = "Regular Expression:";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panelButtons);
            this.panelTop.Controls.Add(this.panelExpression);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(5);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(5);
            this.panelTop.Size = new System.Drawing.Size(796, 99);
            this.panelTop.TabIndex = 0;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnFirst);
            this.panelButtons.Controls.Add(this.btnNext);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(5, 59);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(786, 35);
            this.panelButtons.TabIndex = 3;
            // 
            // panelExpression
            // 
            this.panelExpression.Controls.Add(this.txtExpr);
            this.panelExpression.Controls.Add(this.labelRegex);
            this.panelExpression.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExpression.Location = new System.Drawing.Point(5, 5);
            this.panelExpression.Name = "panelExpression";
            this.panelExpression.Padding = new System.Windows.Forms.Padding(5);
            this.panelExpression.Size = new System.Drawing.Size(786, 54);
            this.panelExpression.TabIndex = 12;
            // 
            // splitPanel
            // 
            this.splitPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPanel.Location = new System.Drawing.Point(0, 0);
            this.splitPanel.Name = "splitPanel";
            // 
            // splitPanel.Panel1
            // 
            this.splitPanel.Panel1.Controls.Add(this.listBoxMatches);
            // 
            // splitPanel.Panel2
            // 
            this.splitPanel.Panel2.Controls.Add(this.txtSource);
            this.splitPanel.Size = new System.Drawing.Size(796, 340);
            this.splitPanel.SplitterDistance = 178;
            this.splitPanel.TabIndex = 0;
            // 
            // txtSource
            // 
            this.txtSource.AcceptsReturn = true;
            this.txtSource.AcceptsTab = true;
            this.txtSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSource.Location = new System.Drawing.Point(0, 0);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(614, 340);
            this.txtSource.TabIndex = 0;
            this.txtSource.Text = "Integer numbers: +100, -25, 5000\r\nReal Numbers: 3.1417, -200.25, +1.6625e-2, -3.3" +
    "75009e+7, .0025\r\n\r\nThere is a blank line preceeding this text.\r\n\r\nend.";
            this.txtSource.WordWrap = false;
            // 
            // panelLower
            // 
            this.panelLower.Controls.Add(this.splitPanel);
            this.panelLower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLower.Location = new System.Drawing.Point(0, 99);
            this.panelLower.Name = "panelLower";
            this.panelLower.Size = new System.Drawing.Size(796, 340);
            this.panelLower.TabIndex = 7;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusCompiled,
            this.lblCompiled});
            this.statusStrip.Location = new System.Drawing.Point(0, 439);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(796, 22);
            this.statusStrip.TabIndex = 1200;
            // 
            // toolStripStatusCompiled
            // 
            this.toolStripStatusCompiled.Name = "toolStripStatusCompiled";
            this.toolStripStatusCompiled.Size = new System.Drawing.Size(0, 17);
            // 
            // lblCompiled
            // 
            this.lblCompiled.Name = "lblCompiled";
            this.lblCompiled.Size = new System.Drawing.Size(0, 17);
            // 
            // listBoxMatches
            // 
            this.listBoxMatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMatches.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxMatches.FormattingEnabled = true;
            this.listBoxMatches.ItemHeight = 20;
            this.listBoxMatches.Location = new System.Drawing.Point(0, 0);
            this.listBoxMatches.Name = "listBoxMatches";
            this.listBoxMatches.Size = new System.Drawing.Size(178, 340);
            this.listBoxMatches.TabIndex = 0;
            // 
            // RegexTestForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(796, 461);
            this.Controls.Add(this.panelLower);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panelTop);
            this.Name = "RegexTestForm";
            this.Text = "PatterPro Regular Expression Tester";
            this.Load += new System.EventHandler(this.RegexTestForm_Load);
            this.panelTop.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelExpression.ResumeLayout(false);
            this.panelExpression.PerformLayout();
            this.splitPanel.Panel1.ResumeLayout(false);
            this.splitPanel.Panel2.ResumeLayout(false);
            this.splitPanel.Panel2.PerformLayout();
            this.splitPanel.ResumeLayout(false);
            this.panelLower.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void btnCompile_Click(object sender, System.EventArgs e)
		{
			r = new RxMatcher(txtExpr.Text);
			lblCompiled.Text = r.ToString();
            recompile = false;
		}

		private void btnFirst_Click(object sender, System.EventArgs e)
		{
            ClearMatchView();

            r = new RxMatcher(txtExpr.Text, true);
            lblCompiled.Text = r.ToString();
			
			if( r.MatchFirst(new RxStringSource(txtSource.Text), out m) )
			{
				txtSource.Select(m.Offset,m.Length);
				txtSource.Select();

				DisplayMatchView(m ,txtSource.Text);

                btnNext.Enabled = true;
			}
			else
			{
                btnNext.Enabled = false;
				MessageBox.Show(this,"No match found!","Match result",System.Windows.Forms.MessageBoxButtons.OK);
			}
		}

		private void btnNext_Click(object sender, System.EventArgs e)
		{
            if (m==null)
			{
                btnNext.Enabled = false;
				MessageBox.Show(this,"No previous match!","Match result",System.Windows.Forms.MessageBoxButtons.OK);
				return;
			}

            ClearMatchView();

			if (r.MatchNext(new RxStringSource(txtSource.Text),m, out m))
			{
				txtSource.Select(m.Offset,m.Length);
				txtSource.Select();

                DisplayMatchView(m ,txtSource.Text);

                btnNext.Enabled = true;
			}
			else
			{

                btnNext.Enabled = false;
                MessageBox.Show(this,"No match was found!", "Match result", MessageBoxButtons.OK,MessageBoxIcon.Stop);
			}
		}

		private void RegexTestForm_Load(object sender, System.EventArgs e)
		{
            // empty
		}

        private void ClearMatchView()
        {

        }

        private void DisplayMatchView(RxMatch m, string s)
        {
            if (listBoxMatches.Items.Count > 0)
                listBoxMatches.Items.Clear();

            if (m.Length < 1)
                return;

            for (int i = 0; i < m.MatchArray.Length; i++)
            {
                RxSubMatch sm = m.MatchArray[i];

                StringBuilder sb = new StringBuilder();
                sb.Append(i.ToString().PadLeft(2, '0'));

                if (sm != null)
                {
                    sb.Append(":" + sm.Length.ToString().PadLeft(3, '0'));

                    if (sm.Length > 0)
                    {
                        sb.Append(",");
                        sb.Append(s, sm.Offset, sm.Length);
                    }
                }
                else
                {
                    sb.Append(":000");
                    listBoxMatches.Items.Add(sb.ToString());
                }
                listBoxMatches.Items.Add(sb.ToString());
            }
        }

        private void txtExpr_TextChanged(object sender, EventArgs e)
        {
            btnFirst.Enabled = true;
            btnNext.Enabled = false;
            m = null;
        }
    }
}
