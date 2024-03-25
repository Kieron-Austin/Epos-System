using K4os.Compression.LZ4.Internal;
using MaterialSkin;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using NAudio.Wave;
using System.Net.Http;

namespace Motapart_Core
{
    public partial class Add_Item : MaterialForm
    {
        private byte[] ImageForDB;
        private Image SelectedImage;
        public Add_Item()
        {
            InitializeComponent();
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Purple100, Primary.Purple100,
                Primary.Purple100, Accent.Purple100,
                TextShade.WHITE
            );

        }

        private void Add_Item_Load(object sender, EventArgs e)
        {
            ListStockItems();
        }

        public byte[] imageToByte(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

       

       
        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.Filter = "Image files (*.jpg, *.png) | *.jpg; *.png";

                if (f.ShowDialog() == DialogResult.OK)
                {
                    SelectedImage = Image.FromFile(f.FileName);
                    pictureBox1.Image = SelectedImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<StockData> RetreieveStock()
        {
            materialListView1.Items.Clear();

            List<StockData> list = new List<StockData>();
            MySqlConnection conn = new MySqlConnection("datasource=192.168.1.132;port=3306;username=root;password=;database=new_motapart;");
            conn.Open();
            MySqlDataReader reader = new MySqlCommand("SELECT * FROM `stock`;", conn).ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    StockData item = new StockData();
                    if (!reader.IsDBNull(reader.GetOrdinal("Barcode")))
                        item.barcode = (int)reader["Barcode"];
                    else
                        item.barcode = 0;

                    if (!reader.IsDBNull(reader.GetOrdinal("Name")))
                        item.name = (string)reader["Name"];
                    else
                        item.name = "";

                    if (!reader.IsDBNull(reader.GetOrdinal("StockLevel")))
                        item.stocklevel = (int)reader["StockLevel"];
                    else
                        item.stocklevel = 0;

                    if (!reader.IsDBNull(reader.GetOrdinal("Supplier")))
                        item.supplier = (string)reader["Supplier"];
                    else
                        item.supplier = "";

                    if (!reader.IsDBNull(reader.GetOrdinal("Price")))
                        item.price = (string)reader["Price"];
                    else
                        item.price = "";

                    if (!reader.IsDBNull(reader.GetOrdinal("WarningLevel")))
                        item.warninglevel = (int)reader["WarningLevel"];
                    else
                        item.warninglevel = 0;

                    if (!reader.IsDBNull(reader.GetOrdinal("CostPrice")))
                        item.costprice = (string)reader["CostPrice"];
                    else
                        item.costprice = "";

                    if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                        item.image = (Byte[])reader["Image"];
                    else
                        item.image = item.image;

                    list.Add(item);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                reader.Close();
            }
            return list;
        }


        private void ListStockItemsDataGrid()
        {
            List<StockData> users = RetreieveStock();
            for (int i = 0; i < users.Count; i++)
            {
                StockData user = users[i];
                byte[] pData = user.image;
                MemoryStream mem = new MemoryStream(user.image);

                DataTable dt = new DataTable();

                dt.Columns.Add("image", typeof(byte[]));
                dt.Columns.Add("text", typeof(string));

                //var images = SmallImages();

                //dt.Rows.Add(images[1], "Some text");

                //DataGrid lvi = new DataGrid();

                //lvi.Columns.Insert(5, iconColumn);

                //lvi.Text = user.barcode.ToString();
                //lvi.SubItems.Add(user.name.ToString());
                //lvi.SubItems.Add(user.stocklevel.ToString());
                //lvi.SubItems.Add(user.supplier.ToString());
                //materialListView1.Items.Add(lvi);
            }
        }

        private void ListStockItems()
        {
            List<StockData> users = RetreieveStock();
            for (int i = 0; i < users.Count; i++)
            {
                StockData user = users[i];
                byte[] pData = user.image;
                MemoryStream mem = new MemoryStream(user.image);
                ListViewItem lvi = new ListViewItem();
              
                lvi.Text = user.barcode.ToString();
                lvi.SubItems.Add(user.name.ToString());

                if (user.stocklevel <= user.warninglevel)
                {
                    string Message = user.name.ToString() + " Is Critically Low In Stock Only " + user.stocklevel.ToString() + " Remaining";
                    Text_To_Speech.SpeechToMe(Message);
                    MessageBox.Show(Message, "Stock Low", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lvi.SubItems.Add(user.stocklevel.ToString());
                   
                }
                else
                {
                    lvi.SubItems.Add(user.stocklevel.ToString());
                }

                lvi.SubItems.Add(user.supplier.ToString());
                lvi.SubItems.Add("£" + user.price.ToString());
                lvi.SubItems.Add("£" + user.costprice.ToString());
                lvi.SubItems.Add(user.warninglevel.ToString());
                materialListView1.Items.Add(lvi);
            }
        }
        public static int ToInt(string pStr)
        {
            return int.Parse(pStr);
        }
        private void AddStock()
        {
            Random CustomerIDGenerator = new Random();
            int RandomCustomerID = CustomerIDGenerator.Next(0, 999999999);
            MySqlConnection conn = new MySqlConnection("datasource=192.168.1.132;port=3306;username=root;password=;database=new_motapart;");
            conn.Open();
            MySqlCommand comm = conn.CreateCommand();
            comm.CommandText = "INSERT INTO stock(Barcode, Name, StockLevel, Supplier, Image, Price, WarningLevel, CostPrice) VALUES('" + ToInt(materialSingleLineTextField1.Text) + "','" + materialSingleLineTextField2.Text.ToString() + "','" + ToInt(materialSingleLineTextField3.Text) + "','" + materialSingleLineTextField4.Text.ToString() + "','" + imageToByte(SelectedImage) + "','" + materialSingleLineTextField6.Text.ToString() + "','" + ToInt(materialSingleLineTextField5.Text) + "','" + materialSingleLineTextField7.Text.ToString() + "')";
            comm.ExecuteNonQuery();
            conn.Close();
            ListStockItems();
            pictureBox1.Image.Dispose();
        }
        private void materialListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void materialSingleLineTextField1_Click(object sender, EventArgs e)
        {

        }

        private void materialSingleLineTextField2_Click(object sender, EventArgs e)
        {

        }

        private void materialSingleLineTextField3_Click(object sender, EventArgs e)
        {

        }

        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            AddStock();
        }

        private void materialSingleLineTextField4_Click(object sender, EventArgs e)
        {

        }

        private void lvwBooks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void materialSingleLineTextField5_Click(object sender, EventArgs e)
        {

        }

        private void materialSingleLineTextField5_Click_1(object sender, EventArgs e)
        {

        }

        private void materialSingleLineTextField7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void materialSingleLineTextField8_Click(object sender, EventArgs e)
        {

        }

       
    }
}