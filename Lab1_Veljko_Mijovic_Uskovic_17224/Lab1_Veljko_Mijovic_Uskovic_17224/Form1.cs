using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Lab1_Veljko_Mijovic_Uskovic_17224
{
    public partial class RC4Cryptography : Form
    {
        string sourcePath = null;
        string destPath= null;
        string watchPath = null;
        string title = null;
        FileSystemWatcher fW = new FileSystemWatcher();
        public RC4Cryptography()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void RC4Cryptography_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var fDiag = new OpenFileDialog();
            //fDiag.Filter = "(*.txt)|(*.rc4)";

            if(fDiag.ShowDialog()== DialogResult.OK)
            {
                sourcePath = fDiag.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var fbDiag = new FolderBrowserDialog();

            if(fbDiag.ShowDialog() == DialogResult.OK)
            {
                destPath = fbDiag.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sourcePath == null || !sourcePath.Contains(".txt") || destPath==null)
                return;

            string plainText = System.IO.File.ReadAllText(sourcePath);

            var rc4Encrypt = RC4CryptographyLab.GetInstance();
            byte[] cyText = rc4Encrypt.Encrypt(plainText);

            var bWrite = new BinaryWriter(File.Open(destPath + "\\enc1.rc4", FileMode.OpenOrCreate));
            bWrite.Write(cyText, 0, cyText.Length);
            bWrite.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sourcePath == null || !sourcePath.Contains(".rc4") || destPath == null)
                return;

            byte[] cypherText = System.IO.File.ReadAllBytes(sourcePath);

            var rc4Encrypt = RC4CryptographyLab.GetInstance();
            string plainText = rc4Encrypt.Dencrypt(cypherText);

            var sWrite = new StreamWriter(File.Open(destPath + "\\denc1.txt", FileMode.OpenOrCreate));
            sWrite.Write(plainText);
            sWrite.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (watchPath == null || destPath==null)
                return;

            if (checkBox1.Checked)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                textBox1.Enabled = false;
                int count = 0;


                fW.Path = watchPath;
                fW.EnableRaisingEvents = true;

                fW.Created += (object sender, FileSystemEventArgs e) =>
                {
                    if (!e.FullPath.Contains(".txt") || destPath == null)
                        return;

                    var rc4Encrypt = RC4CryptographyLab.GetInstance();
                    string plainText = System.IO.File.ReadAllText(e.FullPath);

                    byte[] cyText = rc4Encrypt.Encrypt(plainText);

                    var bWrite = new BinaryWriter(File.Open(destPath + "\\enc" + count.ToString() + ".rc4", FileMode.OpenOrCreate));
                    bWrite.Write(cyText, 0, cyText.Length);
                    bWrite.Close();
                    count++;
                };
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                textBox1.Enabled = true;

                fW.EnableRaisingEvents = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var fbDiag = new FolderBrowserDialog();

            if (fbDiag.ShowDialog() == DialogResult.OK)
            {
                watchPath = fbDiag.SelectedPath;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var rc4Encrypt = RC4CryptographyLab.GetInstance();
            rc4Encrypt.Key = textBox1.Text;
        }
    }
}
