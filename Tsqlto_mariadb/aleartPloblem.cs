using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;

using MySql.Data;//add refferent เอา dll จากโปรเจ็กนี้ได้เลย
using MySql.Data.MySqlClient;
using MySql.Data.EntityFramework;

using System.IO;
using Color = Google.Apis.Sheets.v4.Data.Color;


namespace Tsqlto_mariadb
{
    public partial class aleartPloblem : Form
    {
        DataTable Data_1;
        public aleartPloblem(DataTable dt)
        {
            Data_1 = dt;
            InitializeComponent();
        }
       // string connectionString_tsql = "Data Source=M-WATER;Initial Catalog=M_Water;User ID=business;Password=Sy$temB+";
        int month_last; int this_year;
        string sql = "";
  

        private void aleartPloblem_Load(object sender, EventArgs e)
        {
            
            label2.Text = "Date Aleart : " + DateTime.Now.ToString("MM");


            string g = DateTime.Now.ToString("MM");
            string h = DateTime.Now.ToString("yyyy");
            //    month_last = int.Parse(g);
            //   month_last -= 1;
            month_last= DateTime.Now.AddMonths(-1).Month;

            this_year = int.Parse(h);

            //   GoogleSheetsHelperApp.GetDataFromSheet(googleSheetParameters);
            // forgotpunchcard_everyday aa = new forgotpunchcard_everyday();
            //string query_everyday =  aa.SelectData_sql();//เอา query จาก class ไปใส่ method
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Data_1;

             InserttoMariadb(sender,e);
            Data_1 = null;
            // MessageBox.Show("Records inserted " + DateTime.Now.ToString("d-MM-yyyy") + " " + (dataGridView1.Rows.Count - 1).ToString() + "Row");

          //  Application.Exit();
     }

          void api_googleapp(object sender, EventArgs e)
        {
        

        }
        void InserttoMariadb(object sender, EventArgs e)
        {
            string MyConnectionString = "Server=134.209.97.46;Port=3306;Database=driver_delivery_cost;Uid=tey_app;Pwd=aHc9souxLtk34;CharSet=utf8;";
            MySqlConnection connection = new MySqlConnection(MyConnectionString);
           
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd = connection.CreateCommand();
                    if (row.IsNewRow) continue;
                    cmd.Parameters.AddWithValue("@YEAR", this_year);
                    cmd.Parameters.AddWithValue("@MONTH", month_last);
                    cmd.Parameters.AddWithValue("@car_code", row.Cells["CarNo"].Value);
                    cmd.Parameters.AddWithValue("@emp_code", row.Cells["EmployeeCode"].Value);
                    cmd.Parameters.AddWithValue("@distance_km", row.Cells["Distance"].Value);
                    cmd.Parameters.AddWithValue("@NumberOfMembers", Convert.ToInt32(row.Cells["NumberOfMembers"].Value.ToString().Replace(",", "")));
                    cmd.Parameters.AddWithValue("@DeliveryAmount", Convert.ToInt32(row.Cells["DeliveryAmount"].Value.ToString().Replace(",", "")));


                    cmd.CommandText = "INSERT INTO driver_delivery_cost.driving_distance(YEAR,MONTH,car_code,emp_code,distance_km,total_member,total_bottle_delivered)" +
                         "VALUES(@YEAR,@MONTH,@car_code,@emp_code,@distance_km,@NumberOfMembers,@DeliveryAmount)";

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
    
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
        //    string connectionString_mysql = "Server=159.65.3.13;Port=3306;Database=punch_card_noti;Uid=root;Pwd=gYdkw0KDiwasd5op";
         
        //    string StrQuery;
        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(connectionString_mysql))
        //        {
        //            using (MySqlCommand comm = new MySqlCommand())
        //            {
        //                comm.Connection = conn;
        //                conn.Open();
        //                StrQuery = @"INSERT INTO forgot_punchcard (SF_CODE,SF_NAME) VALUES ("
        //                        + textBox1.Text + ", "

        //                        + textBox1.Text + ");";
        //                comm.CommandText = StrQuery;
        //                comm.ExecuteNonQuery();
        //            }
        //        }
        //    }


        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            
        }







    }
}
