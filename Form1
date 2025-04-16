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

namespace SampleDatabaseWalkthrough
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (Properties.Settings.Default.RememberMe)
            {
                textBox1.Text = Properties.Settings.Default.Username;
                textBox2.Text = Properties.Settings.Default.Password;
                checkBox1.Checked = true;
            }
        }

        public void ClearTextFields()
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString);
            SqlDataAdapter adap = new SqlDataAdapter("SELECT * FROM individ WHERE login=@login AND password=@password", con);

            adap.SelectCommand.Parameters.AddWithValue("@login", textBox1.Text);
            adap.SelectCommand.Parameters.AddWithValue("@password", textBox2.Text);

            DataSet ds = new DataSet();

            try
            {
                con.Open();
                adap.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
                {
                    UserSession.IsAuthenticated = true; 
                    string roleMessage = ""; 

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        UserSession.CurrentUserId = Convert.ToInt32(ds.Tables[0].Rows[i]["id_individ"]); // Сохраняем ID пользователя

                        if (ds.Tables[0].Rows[i]["id_role"] != DBNull.Value)
                        {
                            UserSession.UserRole = ds.Tables[0].Rows[i]["id_role"].ToString().Trim(); // Сохраняем роль
                            if (UserSession.UserRole == "1")
                            {
                                roleMessage = "Вы вошли в качестве администратора.";
                            }
                            else if (UserSession.UserRole == "2")
                            {
                                roleMessage = "Вы вошли в качестве обычного пользователя.";
                            }
                        }
                    }

                    if (checkBox1.Checked)
                    {
                        Properties.Settings.Default.Username = textBox1.Text;
                        Properties.Settings.Default.Password = textBox2.Text;
                        Properties.Settings.Default.RememberMe = true;
                        Properties.Settings.Default.Save();
                    }

                    MessageBox.Show($"Успешная авторизация! {roleMessage}");

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else 
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void CenterForm(Form form)
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            form.Left = (screenWidth - form.Width) / 2;
            form.Top = (screenHeight - form.Height) / 2;
        }
            private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }
    }
}
