using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text_Editor
{
    public partial class indentSettingForm : Form
    {
        public indentSettingForm()
        {
            InitializeComponent();
        }

        int left = -1;
        int right = -1;

        private void indentSettingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == String.Empty) { left = -1; }
            else { left = Convert.ToInt32(textBox2.Text); }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == String.Empty) { right = -1; }
            else { right = Convert.ToInt32(textBox3.Text); }
        }

        private void okBTN_Click(object sender, EventArgs e)
        {
            MainForm main = this.Owner as MainForm;
            if (main != null)
            {
                main.setIndent(left, right);
            }
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
