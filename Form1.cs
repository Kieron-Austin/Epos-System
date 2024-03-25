using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using MySql.Data.MySqlClient;
using Mysqlx.Prepare;
namespace Motapart_Core
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Purple100, Primary.Purple100,
                Primary.Purple100, Accent.Purple100,
                TextShade.WHITE
            );

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
                        item.image = (string)reader["Image"];
                    else
                        item.image = "";

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
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        void listsearch()
        {
            materialListView1.Items.Clear();

            List<StockData> users = RetreieveStock();
         
            for (int i = 0; i < users.Count; i++)
            {
                StockData user = users[i];
                ListViewItem lvi = new ListViewItem();

                if (materialSingleLineTextField1.Text == user.barcode.ToString())
                {
                    lvi.Text = user.barcode.ToString();
                    lvi.SubItems.Add(user.name.ToString());
                    pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromFile(@"Images\\" + user.image.ToString());
                    lvi.SubItems.Add(user.stocklevel.ToString());
                    lvi.SubItems.Add(user.supplier.ToString());
                    lvi.SubItems.Add("£" + user.price.ToString());
                    lvi.SubItems.Add("£" + user.costprice.ToString());
                    lvi.SubItems.Add(user.warninglevel.ToString());
                    materialListView1.Items.Add(lvi);
                    Text_To_Speech.SpeechToMe("Item Found");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListStockItems();
        }

        private void materialSingleLineTextField2_Click(object sender, EventArgs e)
        {

        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            new Customer().Show();
            Hide();
        }

        private void materialFlatButton2_Click(object sender, EventArgs e)
        {

        }

        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void materialLabel3_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void materialFlatButton2_Click_1(object sender, EventArgs e)
        {
            new Customer().Show();
        }

        private void materialFlatButton1_Click_1(object sender, EventArgs e)
        {
            new Add_Item().Show();
           // Hide();
        }
        private void ListStockItems()
        {
            List<StockData> users = RetreieveStock();
            for (int i = 0; i < users.Count; i++)
            {
                StockData user = users[i];
                ListViewItem lvi = new ListViewItem();

                lvi.Text = user.barcode.ToString();
                lvi.SubItems.Add(user.name.ToString());
                lvi.SubItems.Add(user.stocklevel.ToString());
                lvi.SubItems.Add(user.supplier.ToString());
                lvi.SubItems.Add("£" + user.price.ToString());
                lvi.SubItems.Add("£" + user.costprice.ToString());
                lvi.SubItems.Add(user.warninglevel.ToString());
                materialListView1.Items.Add(lvi);
            }
        }

        private void materialListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void materialSingleLineTextField1_Click(object sender, EventArgs e)
        {
        }
 
        private void materialListView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void materialFlatButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(materialSingleLineTextField1.Text))
            {
                ListStockItems();
                Text_To_Speech.SpeechToMe("Item Not Found");
            }
            else 
            {
                listsearch();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
