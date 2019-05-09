using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace OCRSDKTest
{
    public partial class Form1 : Form
    {
        private List<Image> LoadedImages = new List<Image>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitControls();
        }

        private void InitControls()
        {
            var types = System.Enum.GetNames(typeof(TiffCreator.TiffCompressType));
            this.comboBox1.Items.AddRange(types);
            this.comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] filePaths = openFileDialog1.FileNames;
                foreach (string file in filePaths)
                {

                    ShowProperty(file);
                    Image[] imgs = GetImage(file);
                    string filename = System.IO.Path.GetFileName(file);
                    int pageNo = 0;
                    foreach (Image img in imgs)
                    {
                        this.imageList1.Images.Add(img);
                        LoadedImages.Add(img);
                        int index = this.imageList1.Images.Count - 1;
                        var item = this.listView1.Items.Add(filename + "_" + pageNo);
                        item.ImageIndex = index;
                        pageNo++;
                    }
                }
            }
        }

        private void ShowProperty(string file)
        {
            this.lblCompressType.Text = TiffCreator.GetTiffFormatName(file);
        }

        private string GetString(byte[] datas)
        {
            return System.Text.Encoding.ASCII.GetString(datas);
        }

        private Image[] GetImage(string filepath)
        {
            using (System.IO.FileStream stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                Image tiffImg = Image.FromStream(stream);
                return TiffCreator.GetImages(tiffImg);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string saveFile = saveFileDialog1.FileName;
            List<Image> images = new List<Image>();
            foreach (ListViewItem item in listView1.Items)
            {
                Image img = LoadedImages[item.ImageIndex];
                images.Add(img);
            }
            string typename = (string)comboBox1.SelectedItem;
            TiffCreator.TiffCompressType comType = (TiffCreator.TiffCompressType)System.Enum.Parse(typeof(TiffCreator.TiffCompressType), typename);
            TiffCreator.CreateTiffImage(images.ToArray(), saveFile, comType);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    listView1.Items.Remove(item);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView1.SelectedItems[0];
                Image img = LoadedImages[item.ImageIndex];
                this.pictureBox1.Image = img;
            }
            else
            {
                this.pictureBox1.Image = null;
            }
        }

        /// <summary>
        /// 上へ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedIndices.Count > 0)
            {
                int index1 = this.listView1.SelectedIndices[0];
                if (index1 == 0) return;
                int index2 = index1 - 1;
                SwapNode(this.listView1.Items, index1, index2);
            }
        }
        /// <summary>
        /// 下へ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedIndices.Count > 0)
            {
                int index1 = this.listView1.SelectedIndices[0];
                if (index1 == this.listView1.Items.Count) return;
                int index2 = index1 + 1;
                SwapNode(this.listView1.Items, index1, index2);
            }
        }



        /// <summary>
        /// 項目移動処理
        /// </summary>
        /// <param name="objArray"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        private void SwapNode(IList objArray, int index1, int index2)
        {
            if (objArray == null) return;
            if (index1 < 0 || index1 > objArray.Count - 1) return;
            if (index2 < 0 || index2 > objArray.Count - 1) return;
            if (index1 == index2) return;
            object obj1 = objArray[index1];
            object obj2 = objArray[index2];
            objArray.Remove(obj2);
            objArray.Remove(obj1);
            if (index1 < index2)
            {
                objArray.Insert(index1, obj2);
                objArray.Insert(index2, obj1);
            }
            else
            {
                objArray.Insert(index2, obj1);
                objArray.Insert(index1, obj2);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                string ext = Path.GetExtension(file);
                switch (ext.ToLower())
                {
                    case ".tif":
                        TiffCreator.SlipTiffFile(file);

                        break;
                    case ".pdf":
                        string tiffile = TiffCreator.SlipPfdfFile(file);
                        TiffCreator.SlipTiffFile(tiffile);
                        break;
                }

            }
        }
    }
}
