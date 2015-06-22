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

namespace PruebaMetadatos
{
    public partial class Form1 : Form
    {
        OpenFileDialog data = new OpenFileDialog();
        Boolean open = false;

        public Form1()
        {
            InitializeComponent();
            listBox2.Items.Add("Load an Image File to begin...");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data.Title = "Open First Image File";
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

        public void meta()
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox1.Items.Clear();
            meta();
        }

        //public Stream PatchAwayExif(Stream inStream, Stream outStream)
        //{
        //    byte[] jpegHeader = new byte[2];
        //    jpegHeader[0] = (byte)inStream.ReadByte();
        //    jpegHeader[1] = (byte)inStream.ReadByte();
        //    if (jpegHeader[0] == 0xff && jpegHeader[1] == 0xd8)
        //    {
        //        SkipAppHeaderSection(inStream);
        //    }
        //    outStream.WriteByte(0xff);
        //    outStream.WriteByte(0xd8);

        //    int readCount;
        //    byte[] readBuffer = new byte[4096];
        //    while ((readCount = inStream.Read(readBuffer, 0, readBuffer.Length)) > 0){
        //        outStream.Write(readBuffer, 0, readCount);
        //    }
        //    return outStream;
        //}

        //private void SkipAppHeaderSection(Stream inStream)
        //{
        //    byte[] header = new byte[2];
        //    header[0] = (byte)inStream.ReadByte();
        //    header[1] = (byte)inStream.ReadByte();

        //    while (header[0] == 0xff && (header[1] >= 0xe0 && header[1] <= 0xef))
        //    {
        //        int exifLength = inStream.ReadByte();
        //        exifLength = exifLength << 8;
        //        exifLength |= inStream.ReadByte();

        //        for (int i = 0; i < exifLength - 2; i++)
        //        {
        //            inStream.ReadByte();
        //        }
        //        header[0] = (byte)inStream.ReadByte();
        //        header[1] = (byte)inStream.ReadByte();
        //    }
        //    inStream.Position -= 2;

        //}

        private void button3_Click(object sender, EventArgs e)
        {
            if (open)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
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
        }
    }
}
