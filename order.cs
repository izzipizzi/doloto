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
    public partial class Order : Form
    {

        private string login;
        public Order()
        {
            InitializeComponent();
        }

        public Order(string login)
        {


            this.login = login;

            InitializeComponent();
            if (login != "admin")
            {
                MessageBox.Show(login);
                tabControl1.TabPages.Remove(addDetailTab);
                label11.Visible = false;
                textBox7.Visible = false;
                button3.Visible = false;
            }
        }


        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dolotoDB.mdf;Integrated Security=True;Connect Timeout=30");

        DataSet data = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter();


        public void connect()
        {
            try
            {
                connection.Open();

                if (connection.State == ConnectionState.Open)
                {
                    // MessageBox.Show("Зєднання встановленно!");
                }
            }
            catch (Exception)
            {
                if (connection.State != ConnectionState.Open)
                {
                    MessageBox.Show("Зєднання відсутнє");
                }
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public int detail_ID;
        public int getID()
        {
            string id;
            connect();
            try
            {
                SqlCommand com = new SqlCommand("Select MAX(Id) from  details", connection);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();

                    detail_ID = Convert.ToInt32(id);
                    detail_ID++;

                }
                reader.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Сталсь помилка на сервері повторіть будь-ласка" + detail_ID);


            }
            return detail_ID;

        }
        private void button2_Click(object sender, EventArgs e)
        {

            string name = textBox4.Text;
            string type = comboBox3.Text;
            int id = getID();



            int price;
            int count;



            try
            {

                price = Convert.ToInt32(textBox6.Text);
                count = Convert.ToInt32(textBox5.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Значення  має бути записане у цифрах");
                return;
            }

            try
            {
                connect();

                SqlCommand commandText = connection.CreateCommand();
                commandText.CommandText = " Insert into details (Id,name,type,price,count) " +
                    "VALUES (@id, @name, @type, @price,@count)";
                commandText.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                commandText.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
                commandText.Parameters.Add("@type", SqlDbType.NVarChar).Value = type;
                commandText.Parameters.Add("@price", SqlDbType.Int).Value = price;
                commandText.Parameters.Add("@count", SqlDbType.Float).Value = count;



                dataAdapter.SelectCommand = commandText;
                dataAdapter.Fill(data, "details");
                MessageBox.Show("Успішно добавлено");

            }
            catch (SqlException ex)
            {
                MessageBox.Show("помилка додавання" + ex);
            }
        }

        private void Order_Load(object sender, EventArgs e)
        {
            loadDetailTypes();
           

        }

 

        void loadDetailTypes()
        {
            connect();
            string strCmd = "select type from types";
            SqlCommand cmd = new SqlCommand(strCmd, connection);
            SqlDataAdapter da = new SqlDataAdapter(strCmd, connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteNonQuery();
            connection.Close();

            comboBox3.DisplayMember = ds.Tables[0].Columns[0].ToString();
            comboBox3.ValueMember = "Тип деталі";
            comboBox3.DataSource = ds.Tables[0];

            comboBox3.Enabled = true;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox1.DisplayMember = ds.Tables[0].Columns[0].ToString();
            comboBox1.ValueMember = "Тип деталі";
            comboBox1.DataSource = ds.Tables[0];

            comboBox1.Enabled = true;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;


        }

        private void Order_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = true;
            connection.Open();
            SqlCommand cmd = new SqlCommand("select name from details where type = @type", connection);
            cmd.Parameters.AddWithValue("@type", comboBox1.Text);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteNonQuery();
            connection.Close();


            comboBox2.DisplayMember = ds.Tables[0].Columns[0].ToString();
            comboBox2.ValueMember = "Виберіть деталь";
            comboBox2.DataSource = ds.Tables[0];

            comboBox2.Enabled = true;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            connection.Close();
        }

        private void orderTab_Enter(object sender, EventArgs e)
        {
            loadDetailTypes();

        }


        public int getDetailID(string name)
        {
            string id;
            connect();
            try
            {
                SqlCommand com = new SqlCommand("Select Id from  details where name = @name", connection);
                com.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();

                    detail_ID = Convert.ToInt32(id);
                   

                }
                reader.Close();
                connection.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Сталсь помилка на сервері повторіть будь-ласка" + detail_ID);


            }
            return detail_ID;
        }
        public int user_ID;
        int getUserID()
        {

            string id;
            connect();
            try
            {
                SqlCommand com = new SqlCommand("Select Id from  users where login = @name", connection);
                com.Parameters.Add("@name", SqlDbType.NVarChar).Value = login;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();

                    user_ID = Convert.ToInt32(id);
                    user_ID++;

                }
                reader.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Сталсь помилка на сервері повторіть будь-ласка" + user_ID);


            }
            return user_ID;

        }

        public int order_ID;
        public int getOrderId()
        {
            string id;
            connect();
            try
            {
                SqlCommand com = new SqlCommand("Select MAX(Id) from  orders", connection);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();

                    order_ID = Convert.ToInt32(id);
                    order_ID++;

                }
                reader.Close();

            }
            catch (SqlException)
            {
                MessageBox.Show("Сталась помилка при замовленні. Будь ласка повторіть пізніше.");


            }
            return order_ID;


        }
        private void button1_Click(object sender, EventArgs e)
        {

            int detail_id = getDetailID(comboBox2.Text);
            int user_id = getUserID();
            int id = getOrderId();
            int count;
            int total_price;
            string phone = textBox2.Text;
            try
            {


                count = Convert.ToInt32(textBox1.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("Значення має бути записане у цифрах");
                return;
            }
            try
            {
                connect();

                SqlCommand commandText = connection.CreateCommand();
                commandText.CommandText = " Insert into orders (Id,detail_id,user_id,date,phone,count,total_price) " +
                    "VALUES (@id, @detail_id, @user_id, @date,@phone,@count,((select price from details where Id = @detail_id) * @count))";
                commandText.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                commandText.Parameters.Add("@detail_id", SqlDbType.Int).Value = detail_id;
                commandText.Parameters.Add("@user_id", SqlDbType.Int).Value = user_id;
                commandText.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Now;
                commandText.Parameters.Add("@phone", SqlDbType.NVarChar).Value = phone;
                commandText.Parameters.Add("@count", SqlDbType.Int).Value = count;
                //commandText.Parameters.Add("@total_price", SqlDbType.Int).Value = total_price;


                commandText.ExecuteNonQuery();

                int dif_count = 0;
                SqlCommand updateCount = connection.CreateCommand();
                updateCount.CommandText = "update details set count = ((select count from details where Id = @id) - @count) where Id ='"+detail_id+"'";
                updateCount.Parameters.Add("@count", SqlDbType.Int).Value = count;
                updateCount.Parameters.Add("@id", SqlDbType.Int).Value = detail_id;
                updateCount.ExecuteNonQuery();

                getCount();
                MessageBox.Show("Замовленно");
                

            }
            catch (SqlException ex)
            {
                MessageBox.Show("помилка додавання" + ex);
            }

        }
        public void Clear(DataGridView dataGridView)
        {
            while (dataGridView.Rows.Count > 1)
                for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                    dataGridView.Rows.Remove(dataGridView.Rows[i]);
        }

        private void ordersTab_Enter(object sender, EventArgs e)
        {
            Clear(dataGridView1);
            SqlCommand com = new SqlCommand("Select * from orders LEFT JOIN details on" +
                " Orders.detail_id = details.Id where user_id='" + getUserID() + "'", connection);
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet ds = new DataSet();
            da.SelectCommand = com;
            da.Fill(ds, "Orders");

            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "Orders";

            dataGridView1.Columns["detail_id"].Visible = false;
            dataGridView1.Columns["user_id"].Visible = false;
            dataGridView1.Columns["Id1"].Visible = false;
            dataGridView1.Columns["count1"].Visible = false;


            dataGridView1.Columns["Id"].HeaderText = "Номер замовлення";
            dataGridView1.Columns["date"].HeaderText = "Дата";
            dataGridView1.Columns["phone"].HeaderText = "Номер телефону";
            dataGridView1.Columns["count"].HeaderText = "Кількість";
            dataGridView1.Columns["name"].HeaderText = "Назва деталі";
            dataGridView1.Columns["type"].HeaderText = "Тип";
            dataGridView1.Columns["price"].HeaderText = "Ціна за 1 шт.";
            dataGridView1.Columns["total_price"].HeaderText = "Сума до оплати";

            /* dataGridView3.Columns["detail_id1"].Visible = false;
             dataGridView3.Columns["detail_id"].Visible = false;
             dataGridView3.Columns["client_id"].Visible = false;
             dataGridView3.Columns["Id1"].Visible = false;

             dataGridView3.Columns["price1"].HeaderText = "Ціна за 1 шт";
             dataGridView3.Columns["Id"].HeaderText = "Номер замовлення";
             dataGridView3.Columns["date"].HeaderText = "Дата/Час замовлення";
             dataGridView3.Columns["count"].HeaderText = "К-сть деталей";
             dataGridView3.Columns["detail_name"].HeaderText = "Назва деталі";
             dataGridView3.Columns["detail_type"].HeaderText = "Тип деталі";
             dataGridView3.Columns["price"].HeaderText = "Загальна ціна";*/
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            if (textBox3.Text == String.Empty)
            {
                Clear(dataGridView1);
                dataGridView1.Update();
                SqlDataAdapter OrddersDA = new SqlDataAdapter("Select * from orders LEFT JOIN details on" +
                " orders.detail_id = details.Id where user_id='" + getUserID() + "'", connection);
                OrddersDA.TableMappings.Add("Table", "Orders");
                OrddersDA.Fill(data);
                dataGridView1.DataSource = data;
                dataGridView1.DataMember = "Orders";
            }
            else
            {
                Clear(dataGridView1);
                dataGridView1.Update();
                SqlCommand com = new SqlCommand("select * from orders " +
                   "LEFT JOIN details on orders.detail_id = details.Id where  user_id='" + getUserID() + "' and  details.name like @name ", connection);
                com.Parameters.Add("@name", SqlDbType.NVarChar).Value = textBox3.Text + "%";
                SqlDataAdapter OrddersDA = new SqlDataAdapter();
                OrddersDA.SelectCommand = com;
                OrddersDA.TableMappings.Add("Table", "Orders");
                OrddersDA.Fill(data);
                dataGridView1.DataSource = data;
                dataGridView1.DataMember = "Orders";

            }
        }
        void getCount()
        {
            label10.Text = "Лишилось на складі: ";


            connect();
            string count;
            try
            {
                SqlCommand cmd = new SqlCommand("select count from details where name = @name", connection);
                cmd.Parameters.AddWithValue("@name", comboBox2.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    count = reader[0].ToString();

                    /*  order_ID = Convert.ToInt32(id);
                      order_ID++;*/
                    int c = Convert.ToInt32(count);
                    if(c == 0)
                    {
                        label10.Text = "Деталі закінчились";
                        button1.Enabled = false;
                        return;
                    }
                    else
                    {
                        label10.Text += count;
                        button1.Enabled = true;
                    }

                }
                reader.Close();

            }
            catch (SqlException)
            {
                MessageBox.Show("Сталась помилка. Будь ласка повторіть пізніше.");


            }
            connection.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            getCount();          
           
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {


            int count;



            try
            {

              
                count = Convert.ToInt32(textBox7.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Значення  має бути записане у цифрах");
                return;
            }

            try
            {
                connect();

                SqlCommand commandText = connection.CreateCommand();
                commandText.CommandText = " update details set count = @count where name = @name";
               
                commandText.Parameters.Add("@name", SqlDbType.NVarChar).Value = comboBox2.Text;
              
                commandText.Parameters.Add("@count", SqlDbType.Int).Value = count;


                commandText.ExecuteNonQuery();
                MessageBox.Show("Успішно оновлено");


            }
            catch (SqlException ex)
            {
                MessageBox.Show("помилка додавання" + ex);
            }
            getCount();
        }
    }
}
