using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;

namespace Text_Editor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Text File(*.txt)|*.txt|Rich Text File(*.rtf)|*.rtf";
            openFileDialog1.Filter = "Text File(*.txt)|*.txt|Rich Text File(*.rtf)|*.rtf|All files(*.*)|*.*";
            mainTextBox.AllowDrop = true;
            mainTextBox.DragDrop += mainTextBox_DragDrop;
        }

        private string currentFileName = String.Empty;
        private string toFind = string.Empty;
        private bool unSavedFile = false;

        public void saveFile()
        {
            saveAsToolStripMenuItem.PerformClick();
        }

        private void mainTextBox_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop);
            if(data != null)
            {
                var fileName = data as string[];
                if (fileName[0].EndsWith(".rtf")) { mainTextBox.LoadFile(fileName[0]); }
                if(fileName[0].EndsWith("*.jpg") || fileName[0].EndsWith("*.png"))
                {
                    Image image = Image.FromFile(fileName[0]);
                    Clipboard.SetImage(image);
                    mainTextBox.Paste();
                    mainTextBox.Focus();
                }
                else
                {
                    string fileText = File.ReadAllText(fileName[0]);
                    mainTextBox.Text = fileText;
                }

            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                unSavedFile = true;
                currentFileName = openFileDialog1.FileName;
                if (currentFileName.EndsWith(".rtf")) { mainTextBox.LoadFile(currentFileName); }
                else if (currentFileName.EndsWith(".png") || currentFileName.EndsWith(".jpg"))
                {
                    Image image = Image.FromFile(currentFileName);
                    Clipboard.SetImage(image);
                    mainTextBox.Paste();
                    Clipboard.Clear();
                }
                else if(currentFileName.EndsWith(".txt"))
                {
                    string fileText = File.ReadAllText(currentFileName);
                    mainTextBox.Text = fileText;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(currentFileName != String.Empty)
            {
                if (currentFileName.EndsWith(".rtf")) { File.WriteAllText(currentFileName, mainTextBox.Rtf); }
                else { File.WriteAllText(currentFileName, mainTextBox.Text); }
                unSavedFile = false;
                toolStripStatusLabel1.Text = "Saved...";
            }
            else { closeToolStripMenuItem.PerformClick(); }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                if (fileName.EndsWith(".rtf")) { File.WriteAllText(fileName, mainTextBox.Rtf); }
                else { File.WriteAllText(fileName, mainTextBox.Text); }
                unSavedFile = false;
                toolStripStatusLabel1.Text = "Saved...";
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(mainTextBox.SelectedText.Length > 0) { mainTextBox.Copy(); }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTextBox.Paste();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainTextBox.SelectedText.Length > 0) { mainTextBox.Cut(); }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTextBox.Clear();
            currentFileName = String.Empty;
        }

        private void fontToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                if (mainTextBox.SelectedText.Length > 0) mainTextBox.SelectionFont = fontDialog1.Font;
                else mainTextBox.Font = fontDialog1.Font;
            }
        }

        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                if (mainTextBox.SelectedText.Length > 0) mainTextBox.SelectionColor = colorDialog1.Color;
                else mainTextBox.ForeColor = colorDialog1.Color;
            }
        }

        private void mainTextBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) { contextMenuStrip1.Show(Cursor.Position); }
        }

        private void backFontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                if (mainTextBox.SelectedText.Length > 0) mainTextBox.SelectionBackColor = colorDialog1.Color;
                else mainTextBox.BackColor = colorDialog1.Color;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTextBox.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTextBox.Redo();
        }

        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(mainTextBox.Text.Length > 0) { mainTextBox.SelectAll(); }
        }

        private void lowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(mainTextBox.SelectedText.Length > 0) { mainTextBox.SelectedText = mainTextBox.SelectedText.ToLower(); }
            else { mainTextBox.Text = mainTextBox.Text.ToLower(); }
        }

        private void upperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(mainTextBox.SelectedText.Length > 0) { mainTextBox.SelectedText = mainTextBox.SelectedText.ToUpper(); }
            else { mainTextBox.Text = mainTextBox.Text.ToUpper(); }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (unSavedFile)
            {
                CloseMessageForm message = new CloseMessageForm();
                message.Owner = this;
                message.ShowDialog();
            }
        }

        private void mainTextBox_TextChanged(object sender, EventArgs e)
        {
            unSavedFile = true;
            toolStripStatusLabel1.Text = "Unsaved changes*";
        }

        private void leftTextAligntoolStripButton1_Click(object sender, EventArgs e)
        {
            mainTextBox.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void middleTextAligntoolStripButton2_Click(object sender, EventArgs e)
        {
            mainTextBox.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void rightTextAligntoolStripButton3_Click(object sender, EventArgs e)
        {
            mainTextBox.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void textAlingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            indentSettingForm indent = new indentSettingForm();
            indent.Owner = this;
            indent.ShowDialog();
        }

        public void setIndent(int left, int right)
        {
            if (left >= 0) { mainTextBox.SelectionIndent = left; }
            if (right >= 0) { mainTextBox.SelectionRightIndent = right; }
        }

        private void searchToolStripButton_Click(object sender, EventArgs e)
        {
            if (toFind.Length > 0)
            {
                int startIndex = 0;
                while (startIndex < mainTextBox.Text.Length)
                {
                    int wordStartIndex = mainTextBox.Find(toFind, startIndex, RichTextBoxFinds.None);
                    if (wordStartIndex != -1)
                    {
                        mainTextBox.SelectionStart = wordStartIndex;
                        mainTextBox.SelectionLength = toFind.Length;
                        mainTextBox.SelectionBackColor = Color.Yellow;
                    }
                    else { break; }
                    startIndex += toFind.Length;
                }
            }
        }

        private void searchToolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            toFind = searchToolStripTextBox1.Text;
        }

        private void clearToolStripButton1_Click(object sender, EventArgs e)
        {
            mainTextBox.SelectionStart = 0;
            mainTextBox.SelectAll();
            mainTextBox.SelectionBackColor = Color.White;
            mainTextBox.DeselectAll();
        }

        private void selectionBulletToolStripButton1_Click(object sender, EventArgs e)
        {
            mainTextBox.BulletIndent = 25;
            mainTextBox.SelectionIndent = 20;
            mainTextBox.SelectionBullet = true;
        }

        private void fontUpToolStripButton1_Click(object sender, EventArgs e)
        {
            mainTextBox.Font = new Font(mainTextBox.Font.Name, mainTextBox.Font.Size + 1.0f, mainTextBox.Font.Style);
        }

        private void fontDownToolStripButton1_Click(object sender, EventArgs e)
        {
            mainTextBox.Font = new Font(mainTextBox.Font.Name, mainTextBox.Font.Size - 1.0f, mainTextBox.Font.Style);
        }

        private void speachToolStripButton1_Click(object sender, EventArgs e)
        {
            if(mainTextBox.Text.Trim().Length > 0)
            {
                System.Speech.Synthesis.SpeechSynthesizer speaker = new System.Speech.Synthesis.SpeechSynthesizer();
                speaker.SpeakAsync(mainTextBox.Text);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
