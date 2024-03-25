using Google.Protobuf.WellKnownTypes;
using MaterialSkin;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
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

namespace Motapart_Core
{
    public partial class Discount : MaterialForm
    {
        public Discount()
        {
            InitializeComponent();
        }

        private void Discount_Load(object sender, EventArgs e)
        {
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Purple100, Primary.Purple100,
                Primary.Purple100, Accent.Purple100,
                TextShade.WHITE
            );
        }
        public static int New_Discounted_Price;
        public static string Discounted_Price;
        public static string Discount_Customer_ID;
        private List<CustomerData> RetreieveUsers()
        {
            // materialListView1.Items.Clear();

            List<CustomerData> list = new List<CustomerData>();
            MySqlConnection conn = new MySqlConnection("datasource=192.168.1.132;port=3306;username=root;password=;database=new_motapart;");
            conn.Open();
            MySqlDataReader reader = new MySqlCommand("SELECT * FROM `customers`;", conn).ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    CustomerData item = new CustomerData();
                    if (!reader.IsDBNull(reader.GetOrdinal("Customer_ID")))
                        item.id = (int)reader["Customer_ID"];
                    else
                        item.id = 0;

                    if (!reader.IsDBNull(reader.GetOrdinal("Name")))
                        item.name = (string)reader["Name"];
                    else
                        item.name = "";

                    if (!reader.IsDBNull(reader.GetOrdinal("Address")))
                        item.address = (string)reader["Address"];
                    else
                        item.address = "";

                    if (!reader.IsDBNull(reader.GetOrdinal("Number")))
                        item.number = (string)reader["Number"];
                    else
                        item.number = "";

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
        void CustomerSearch()
        {
            List<CustomerData> users = RetreieveUsers();
            for (int i = 0; i < users.Count; i++)
            {
                CustomerData user = users[i];
                ListViewItem lvi = new ListViewItem();

                if (tname.Text == user.id.ToString())
                {
                    label1.Text = "Customer Name  -  " + user.name.ToString();
                    Discount_Customer_ID = user.name.ToString();
                    pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    pictureBox2.Image = Image.FromFile(@"Images\\" + user.image.ToString());

                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tname.Text))
            {
            }
            else
            {
                CustomerSearch();
            }
        }

        private List<StockData> RetreieveStock()
        {

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
        void ItemSearch()
        {
            List<StockData> users = RetreieveStock();

            for (int i = 0; i < users.Count; i++)
            {
                StockData user = users[i];
                ListViewItem lvi = new ListViewItem();

                if (tname.Text == user.barcode.ToString())
                {
                    pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromFile(@"Images\\" + user.image.ToString());

                    label5.Text = "Item Cost Price  -   £" + user.costprice;
                    label6.Text = "Item Usual Sale Price  -   £" + user.price;

                    New_Discounted_Price = Convert.ToInt32(user.costprice);
                    Text_To_Speech.SpeechToMe("Item Found");
                }
            }
        }

        private void tname_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tname.Text))
            {

                Text_To_Speech.SpeechToMe("Item Not Found");
            }
            else
            {
                ItemSearch();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        public static double Percent(double number, int percent)
        {
         
            return ((double)number * percent) / 100;
        }
        private void materialSingleLineTextField1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

           
            if (string.IsNullOrEmpty(textBox2.Text))
            {

                
            }
            else
            {

                double currentPrice = 0;
                double deduct = 0;


                currentPrice = Convert.ToDouble(New_Discounted_Price);
                deduct = Convert.ToDouble(textBox2.Text.ToString());


                currentPrice = currentPrice - deduct;

                label8.Text = "Item Discounted Price   = £" + currentPrice.ToString();

                Discounted_Price = currentPrice.ToString();
            }

        }
        public static int ToInt(string pStr)
        {
            return int.Parse(pStr);
        }
        private void AddDiscount()
        {
            MySqlConnection conn = new MySqlConnection("datasource=192.168.1.132;port=3306;username=root;password=;database=new_motapart;");
            conn.Open();
            MySqlCommand comm = conn.CreateCommand();
            comm.CommandText = "INSERT INTO discount(Barcode, Customer_ID, Ammount) VALUES('" + ToInt(tname.Text) + "','" + Discount_Customer_ID.ToString() + "','" + Discounted_Price.ToString() +  "')";
            comm.ExecuteNonQuery();
            conn.Close();

            //Discounted_Price
        }
        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            AddDiscount();

        }
    }
}
