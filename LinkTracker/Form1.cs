using Microsoft.VisualBasic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace LinkTracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadCSV();
        }

        private void LoadCSV()
        {
            var lines = File.ReadAllLines("links.csv");
            foreach (var line in lines)
            {                                         // name,url1,2,3,4,5,6
                string[] lineData = line.Split(',');
                listBox1.Items.Add(lineData[0]);
                string lineData2 = "";
                var itemPos = 0;
                foreach (var item in lineData.Skip(1))
                    if (itemPos == 0)
                    {
                        lineData2 += item;
                    }
                    else
                    {
                        lineData2 += "," + item;
                    }
                    itemPos = itemPos++;
                listBox2.Items.Add(lineData2);
            }
        }
        private void WriteCSV()
        {
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < listBox1.Items.Count; x++)
            {
                sb.Append(listBox1.Items[x].ToString() + "," + listBox2.Items[x].ToString() + "\n");
            }
            using StreamWriter file = new("links.csv");
            file.Write(sb.ToString());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            listBox2.Items.RemoveAt(listBox1.SelectedIndex); listBox2.ClearSelected();
            listBox1.Items.RemoveAt(listBox1.SelectedIndex); listBox1.ClearSelected();
            WriteCSV();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = nameInput.Text;
            if (name.Contains(",")) 
            {
                name = name.Replace(",", "");
            }
            string url = urlInput.Text;
            if (url == "")
            {
                MessageBox.Show("URL Cannot be Empty!", "LinkTracker Alert");
                return;
            }
            listBox1.Items.Add(name);
            listBox2.Items.Add(url);
            WriteCSV();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                return;
            }
            string selection = listBox2.Items[listBox1.SelectedIndex].ToString();
            while (selection == null)
            {
                try
                {
                    selection = listBox2.Items[listBox1.SelectedIndex].ToString();
                }
                catch
                {
                    selection = null;
                    continue;
                }
            }
            Clipboard.SetText(selection);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                listBox3.Visible = false;
                return;
            }

            var filtered = new List<string>();
            foreach (string word in listBox1.Items)
            {
                if (word.ToLower().Contains(textBox1.Text.ToLower()))
                {
                    filtered.Add(word);
                }
            }
            listBox3.Items.Clear();
            foreach (string word in filtered)
            {
                listBox3.Items.Add(word);
            }
            listBox3.Visible = true;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }
    }
}