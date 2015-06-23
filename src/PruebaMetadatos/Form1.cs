using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using System.Reflection;

namespace PruebaMetadatos
{
    public partial class Form1 : Form
    {
        OpenFileDialog data = new OpenFileDialog();
        Boolean open = false;
        Boolean modeI = true;
        Boolean modeD = false;

        public Form1()
        {
            InitializeComponent();
            listBox2.Items.Add("Load an Image File to begin...");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (modeI)
            {
                data.Title = "Open Image File";
                data.InitialDirectory = "@C:\\";
                data.Filter = "Image files (*.jpg; *.gif; *png)|*.jpg;*.jpeg;*.png;*.gif|All files (*.*)|*.*";
                data.FilterIndex = 1;
                data.RestoreDirectory = true;

                if (data.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Load(data.FileName);
                        open = true;
                    }
                    catch
                    {
                        MessageBox.Show("That file is not an image!", "File format exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            else if (modeD)
            {
                data.Title = "Open PDF File";
                data.InitialDirectory = "@C:\\";
                data.Filter = "PDF files (*.pdf)|*.pdf";
                data.FilterIndex = 1;
                data.RestoreDirectory = true;

                if (data.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Image = (Properties.Resources.PDF_2);
                        pictureBox1.Refresh();
                        open = true;
                    }
                    catch
                    {
                        MessageBox.Show("That file is not a PDF!", "File format exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                
            }
        }

        private void metaI()
        {
            Bitmap image = new Bitmap(@data.FileName);
            PropertyItem[] propItems = image.PropertyItems;
            ASCIIEncoding encodings = new ASCIIEncoding();

            int count = 0;
            foreach (PropertyItem item in propItems)
            {
                listBox1.Items.Add("Property Item " + count.ToString());
                listBox1.Items.Add("iD: 0x" + item.Id.ToString("x"));
                listBox1.Items.Add("Type: " + item.Type.ToString());
                listBox1.Items.Add("");
                count++;
            }

            foreach (PropertyItem item in propItems)
            {
                string s = "0x"+item.Id.ToString("x");
                switch (s)
                {
                    case "0x10f":
                        string make = encodings.GetString(item.Value);
                        listBox2.Items.Add("The equipment make is " + make + ".");
                        break;

                    case "0x110":
                        string model = encodings.GetString(item.Value);
                        listBox2.Items.Add("The model is " + model + ".");
                        break;

                    case "0x131":
                        string soft = encodings.GetString(item.Value);
                        listBox2.Items.Add("The software is " + soft + ".");
                        break;

                    case "0x132":
                        string lastDT = encodings.GetString(item.Value);
                        listBox2.Items.Add("The last modification date is " + lastDT + ".");
                        break;

                    case "0x13b":
                        string artist = encodings.GetString(item.Value);
                        listBox2.Items.Add("The artist is " + artist + ".");
                        break;

                    case "0x13c":
                        string host = encodings.GetString(item.Value);
                        listBox2.Items.Add("The host is " + host + ".");
                        break;

                    case "0x8298":
                        string copyright = encodings.GetString(item.Value);
                        listBox2.Items.Add("The copyright is " + copyright + ".");
                        break;

                    case "0x9003":
                        string creation = encodings.GetString(item.Value);
                        listBox2.Items.Add("The original date is " + creation + ".");
                        break;

                    case "0x9004":
                        string creationO = encodings.GetString(item.Value);
                        listBox2.Items.Add("The creation date is " + creationO + ".");
                        break;

                    case "0x9286":
                        string comment = encodings.GetString(item.Value);
                        listBox2.Items.Add("User comment: " + comment + ".");
                        break;

                    case "0xa420":
                        string uniqueId = encodings.GetString(item.Value);
                        listBox2.Items.Add("The unique ID is " + uniqueId + ".");
                        break;

                    case "0xa430":
                        string owner = encodings.GetString(item.Value);
                        listBox2.Items.Add("The owner name is " + owner + ".");
                        break;

                    case "0xa431":
                        string serial = encodings.GetString(item.Value);
                        listBox2.Items.Add("The serial number is " + serial + ".");
                        break;
                }
            }
            image.Dispose();
        }

        private void metaD()
        {
            listBox1.Items.Add(data.SafeFileName);
            PdfReader pdfFile = new PdfReader(data.FileName);
            IDictionary<String, String> metadic = pdfFile.Info;
            var dic = from m in metadic select m;
            foreach (var d in dic)
            {
                listBox2.Items.Add(d.Key + ": " + d.Value);
            }
            pdfFile.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (open)
            {
                listBox2.Items.Clear();
                listBox1.Items.Clear();
                if (modeI)
                {
                    metaI();
                }
                else
                {
                    metaD();
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            if (open)
            {
                if (modeI)
                {
                    
                    saveFileDialog1.Title = "Save an Image File";
                    saveFileDialog1.Filter = "Image files (*.jpg; *.gif; *png)|*.jpg;*.jpeg;*.png;*.gif|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.FileName = data.FileName;

                    Bitmap image = new Bitmap(data.FileName);
                    PropertyItem[] propItems = image.PropertyItems;

                    foreach (PropertyItem item in propItems)
                    {
                        if (item.Type == 2)
                        {
                            image.RemovePropertyItem(item.Id);
                        }
                    }

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName != "")
                    {
                        if (saveFileDialog1.FileName == data.FileName)
                        {
                            MessageBox.Show("For security reasons I can't overwrite!", "Overwrite not allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            image.Save(saveFileDialog1.FileName);
                        }
                    }
                    image.Dispose();
                }
                else if (modeD)
                {
                    saveFileDialog1.Title = "Save a PDF File";
                    saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.FileName = data.FileName;

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName != "")
                    {
                        if (saveFileDialog1.FileName == data.FileName)
                        {
                            MessageBox.Show("For security reasons I can't overwrite!", "Overwrite not allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            PdfReader reader = new PdfReader(data.FileName);
                            PdfStamper stamper = new PdfStamper(reader,
                                new FileStream(saveFileDialog1.FileName, FileMode.Create));
                            IDictionary<String, String> inf = reader.Info;
                            SortedDictionary<String, String> res = new SortedDictionary<String,String>();
                            var dic = from m in inf select m;
                            foreach (var d in dic)
                            {
                                res.Add(d.Key, "");
                            }


                            //inf.Add("Title", "");
                            //inf.Add("Subject", "");
                            //inf.Add("Author", "");
                            //inf.Add("Creator", "");
                            //inf.Add("Producer", "");
                            //inf.Add("ModData", "");
                            //inf.Add("Keywords", "");

                            stamper.MoreInfo = res;
                            stamper.Close();
                            reader.Close();
                        }
                    }
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            modeI = true;
            modeD = false;
            open = false;
            pictureBox1.Image = null;
            pictureBox1.Refresh();
            listBox2.Items.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Add("Load an Image File to begin...");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            modeI = false;
            modeD = true;
            open = false;
            pictureBox1.Refresh();
            listBox2.Items.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Add("Load a PDF File to begin...");
        }
    }
}
