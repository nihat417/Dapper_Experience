using Dapper;
using Dapper_Experience.DbGenerator;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection.Metadata;

namespace Dupper_Experience
{
    public partial class Form1 : Form
    {

        SqlConnection? connection = null;
        IConfigurationRoot? root = null;
        string command = String.Empty;
        DataTable? dataTable = null;
        public Form1()
        {
            InitializeComponent();
            var connectionString = "Data Source=NIKO\\NIKO;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            connection = new SqlConnection(connectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Generator.CreateDb(connection);

            root = new ConfigurationBuilder().AddJsonFile("appsittings.json").Build();

            connection.ConnectionString = root.GetConnectionString("db1");


            command = "SELECT * FROM BOOK";
            var reader = connection.ExecuteReader(command);
            dataTable = new DataTable();
            dataTable.Load(reader);

            dataGridView1.DataSource= dataTable;

         
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            int selectedIndex = dataGridView1.CurrentRow.Index;

            if (selectedIndex != -1)
            {
                int selectedItemID = (int)dataGridView1.Rows[selectedIndex].Cells["ID"].Value;

                
                //using (SqlConnection connection = new SqlConnection(connectionString))
                
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM Book WHERE ID = @ID", connection))
                    {
                        command.Parameters.AddWithValue("@ID", selectedItemID);
                        command.ExecuteNonQuery();
                    }
                

                
                dataGridView1.Rows.RemoveAt(selectedIndex);
            }
        }

        private void clear_btn_Click(object sender, EventArgs e)
        {
            var sql = "DELETE FROM Book";
            connection.Execute(sql, null);
            
            MessageBox.Show("All list deleted");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            command = "SELECT * FROM BOOK";
            var reader = connection.ExecuteReader(command);
            dataTable = new DataTable();
            dataTable.Load(reader);
            dataGridView1.DataSource= dataTable;
            dataGridView1.Refresh();
        }

        private void edit_btn_Click(object sender, EventArgs e)
        {
            
            string name = dataGridView1.CurrentRow.Cells["Name"].Value.ToString();
            int page = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Page"].Value);
            string author = dataGridView1.CurrentRow.Cells["Author"].Value.ToString();
            decimal price = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Price"].Value);
            int stock= Convert.ToInt32(dataGridView1.CurrentRow.Cells["Stock"].Value);


            var parameters = new DynamicParameters();
            
            parameters.Add("@Name", name);
            parameters.Add("@Page", page);
            parameters.Add("@Author", author);
            parameters.Add("@Price", price);
            parameters.Add("@Stock", stock);
            


            connection.Execute("sp_AddBook", parameters, commandType: CommandType.StoredProcedure);

            

            
            connection.Close();
        }
    }
}