using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doloto
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dolotoDB.mdf;Integrated Security=True;Connect Timeout=30");


        public int cid;
      
        public void connect()
        {
            try
            {
                connection.Open();

                if (connection.State == ConnectionState.Open)
                {
                    // MessageBox.Show("з'єднання встановлено");
                }
            }
            catch (Exception)
            {
                if (connection.State != ConnectionState.Open)
                {
                    MessageBox.Show("відсутнє  з'єднання ");
                }
            }
        }
        public int getId()
        {
            string id;
            connect();
            try
            {
                //connection.Open();
                SqlCommand com = new SqlCommand("Select MAX(Id) from  users", connection);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();

                    cid = Convert.ToInt32(id);
                    cid++;

                }
                reader.Close();
                // connection.Close();



            }
            catch (SqlException)
            {
                MessageBox.Show("error id" + cid);


            }
            return cid;


        }
      
        public void register()
        {
            String login, password,email;
            login = loginBox.Text;
            password = passwordBox.Text;
            email = textBox1.Text;
            int id = getId();
            //int user_id = getUserId();

            //connection.Open();
            connect();
            SqlCommand select = new SqlCommand("Select login from users where login ='" + login + "'", connection);
            SqlDataReader reader = select.ExecuteReader();
            if (reader.HasRows)
            {
                MessageBox.Show("Цей користувач вже зареєстрованний");
                registerBtn.Enabled = true;
                //statusBox.Enabled = true;
                reader.Close();
            }
            else
            {
                reader.Close();
                
                SqlCommand command = new SqlCommand("Insert into Users(Id,login,password,email) values('" + id + "' , '"
                + login + "' , '" + password + "', '" + email + "')", connection);

                if (command.ExecuteNonQuery() != 0)
                    MessageBox.Show("Виконано");
                else
                    MessageBox.Show("ПОМИЛКА");




            }
            reader.Close();
            connection.Close();

        }

        public void check()
        {
            connect();
            SqlCommand command;

            SqlDataReader read;
            String login = loginBox.Text;
            String password = passwordBox.Text;
           // string status = statusBox.Text;

            try
            {

                command = new SqlCommand("select * from  Users where login=" +
                   "'" + login + "' and password ='" + password + "'", connection);

                read = command.ExecuteReader();



                if ((loginBox.Text != "") && (passwordBox.Text != ""))
                {

                    string check_password = "";
                    int i = 0;
                    while (read.Read())
                    {
                        i++;
                        login = read.GetString(1);
                        check_password = read.GetString(2);
                       // status = read.GetString(3);


                    }
                    if (i == 1)
                    {
                        Order form = new Order(login);
                         form.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Перевірте коректність логіна та пароля", "Такого користувача не знайдено", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        passwordBox.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Заповніть всі поля!", "Ідентифікація неможлива", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show("Error" + e);
            }
            finally
            {
                connection.Close();
            }


        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {

            label3.Enabled = false;
            textBox1.Enabled = false;
            registerBtn.Enabled = false;
            LoginInBtn.Enabled = true;
            radioButton1.Checked = true;
            register();
            passwordBox.Text = "";

        }

        private void LoginInBtn_Click(object sender, EventArgs e)
        {
            check();
        }

        private void Authorization_Load(object sender, EventArgs e)
        {
            connect();
        }

        private void Authorization_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            registerBtn.Enabled = false;
            textBox1.Enabled = false;
            label3.Enabled = false;
            LoginInBtn.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            registerBtn.Enabled = true;
            LoginInBtn.Enabled = false;
            textBox1.Enabled = true;
            label3.Enabled = true;
        }
    }
}
