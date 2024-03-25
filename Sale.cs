using MaterialSkin;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Motapart_Core
{
    public partial class Sale : MaterialForm
    {
        public static string Invoice_ID = Random_Invoice_Numeric(7);
        public Sale()
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

        public static string Random_Invoice_Numeric(int length)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void Sale_Load(object sender, EventArgs e)
        {
           Invoice_ID = Random_Invoice_Numeric(7);
           billNoTextBox.Text = "MP-" + Invoice_ID;
        }

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

        private void billNoTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void supplierComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        void listsearch()
        {
            List<CustomerData> users = RetreieveUsers();
            for (int i = 0; i < users.Count; i++)
            {
                CustomerData user = users[i];
                ListViewItem lvi = new ListViewItem();

                if (tname.Text == user.id.ToString())
                {
                    label1.Text = "Customer Name  -  " + user.name.ToString();
                    label5.Text = "Customer Address  -  " + user.address.ToString();
                    label6.Text = "Customer Contact Number  -  " + user.number.ToString();

                    pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromFile(@"Images\\" + user.image.ToString());

                    Text_To_Speech.SpeechToMe("Customer Found");
                }
            }
        }
        private void tname_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tname.Text))
            {
                Text_To_Speech.SpeechToMe("User Not Found");
            }
            else
            {
                listsearch();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
