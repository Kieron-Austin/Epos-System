using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;



namespace Motapart_Core
{
    public partial class Customer : MaterialForm
    {


        public Customer()
        {
            InitializeComponent();
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Red700, Primary.Red700,
                Primary.Red700, Accent.Red700,
                TextShade.BLACK
            );
        }
        OpenFileDialog CustomerImage = new OpenFileDialog();
        private Image SelectedCustomerImage;
        private List<CustomerData> RetreieveUsers()
        {
            materialListView1.Items.Clear();

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

        private void ListCustomers()
        {
            List<CustomerData> users = RetreieveUsers();
            for (int i = 0; i < users.Count; i++)
            {
                CustomerData user = users[i];

                ListViewItem lvi = new ListViewItem();
                lvi.Text = user.id.ToString();
                lvi.SubItems.Add(user.name.ToString());
                lvi.SubItems.Add(user.address.ToString());
                lvi.SubItems.Add(user.number.ToString());
                materialListView1.Items.Add(lvi);
            }
        }
        public static int ToInt(string pStr)
        {
            return int.Parse(pStr);
        }
        private void AddCustomer()
        {
            MySqlConnection conn = new MySqlConnection("datasource=192.168.1.132;port=3306;username=root;password=;database=new_motapart;");
            conn.Open();
            MySqlCommand comm = conn.CreateCommand();
            comm.CommandText = "INSERT INTO customers(Customer_ID, Name, Address, Number, Image) VALUES('" + ToInt(materialSingleLineTextField4.Text) + "','" + materialSingleLineTextField1.Text.ToString() + "','" + materialSingleLineTextField2.Text.ToString() + "','" + materialSingleLineTextField3.Text.ToString() + "','" + Path.GetFileName(CustomerImage.FileName.ToString()) + "')";
            comm.ExecuteNonQuery();
            conn.Close();
            ListCustomers();
        }
        private void Customer_Load(object sender, EventArgs e)
        {
            ListCustomers();
        }
        private void materialListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            AddCustomer();
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

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerImage.Filter = "Image files (*.jpg, *.png) | *.jpg; *.png";

                if (CustomerImage.ShowDialog() == DialogResult.OK)
                {
                    SelectedCustomerImage = Image.FromFile(CustomerImage.FileName);
                    pictureBox1.Image = SelectedCustomerImage;

                    String Full_Image_Path = CustomerImage.FileName.Replace(Directory.GetCurrentDirectory(), "");
                    File.Copy(Full_Image_Path, "Images\\" + Path.GetFileName(CustomerImage.FileName.ToString()));

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void materialSingleLineTextField4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}