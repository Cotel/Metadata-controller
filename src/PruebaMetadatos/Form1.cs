using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PruebaMetadatos
{
    public partial class Form1 : Form
    {
        OpenFileDialog data = new OpenFileDialog();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data.Title = "Open First Image File";
            data.InitialDirectory = "@C:\\";
            data.Filter = "JPF files (*.jpg)|*.jpg|All files (*.*)|(*.*)";
            data.FilterIndex = 1;
            data.RestoreDirectory = true;

            if (data.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(data.FileName);
            }

        }

        public void meta()
        {
            Bitmap image = new Bitmap(data.FileName);
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
            
            //try
            //{
            //    string make = encodings.GetString(propItems[1].Value);
            //    listBox2.Items.Add("The equipment make is " + make + ".");
            //}
            //catch
            //{
            //    listBox2.Items.Add("no Meta Data Found");
            //}

            //try
            //{
            //    string model = encodings.GetString(propItems[2].Value);
            //    listBox2.Items.Add("The model is " + model + ".");
            //}
            //catch
            //{
            //    listBox2.Items.Add("no Model Found");
            //}

            //try
            //{
            //    string soft = encodings.GetString(propItems[4].Value);
            //    listBox2.Items.Add("The Software is " + soft + ".");
            //}
            //catch
            //{
            //    listBox2.Items.Add("no Software Found");
            //}

            //try
            //{
            //    string DT = encodings.GetString(propItems[15].Value);
            //    listBox2.Items.Add("The Date & Time is " + DT + ".");
            //}
            //catch
            //{
            //    listBox2.Items.Add("no Date & Time Found");
            //}

            //try
            //{
            //    string lastDT = encodings.GetString(propItems[5].Value);
            //    listBox2.Items.Add("The Last Modification Date is " + lastDT + ".");
            //} 
            //catch
            //{
            //    listBox2.Items.Add("no Last Modification Date Found");
            //}
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
    }
}
