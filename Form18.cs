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
    public partial class Form18 : Form
    {
        private string buttonName;
        public Form18(string buttonName)
        {
            InitializeComponent();
            this.buttonName = buttonName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                MessageBox.Show("Пожалуйста, дайте согласие на обработку ваших персональных данных.");
                return;
            }

            if (!UserSession.IsAuthenticated)
            {
                MessageBox.Show("Пользователь не авторизован.");
                return;
            }

            int id_individ = UserSession.CurrentUserId;
            string fam = string.Empty;
            string name = string.Empty;
            string otch = string.Empty;

            // Получаем фамилию, имя и отчество из базы данных
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
            {
                con.Open();
                string query = "SELECT fam, name, otch FROM Individ WHERE id_individ = @id_individ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id_individ", id_individ);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fam = reader["fam"].ToString();
                            name = reader["name"].ToString();
                            otch = reader["otch"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }

            int Id_услуги;
            string Тип_услуги;
            string Название;
            decimal Цена;

            // Определяем данные в зависимости от нажатой кнопки
            switch (buttonName)
            {
                case "button4":
                    Id_услуги = 5;
                    Тип_услуги = "Телевидение";
                    Название = "Базовый";
                    Цена = 590;
                    break;
                case "button3":
                    Id_услуги = 6;
                    Тип_услуги = "Телевидение";
                    Название = "Средний";
                    Цена = 690;
                    break;
                case "button5":
                    Id_услуги = 7;
                    Тип_услуги = "Телевидение";
                    Название = "Оптимальный";
                    Цена = 790;
                    break;
                case "button6":
                    Id_услуги = 8;
                    Тип_услуги = "Телевидение";
                    Название = "Максимальный";
                    Цена = 1349;
                    break;
                default:
                    return; 
            }

            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
            {
                con.Open();

                // Проверяем, существует ли уже заявка с такими данными
                string checkQuery = "SELECT COUNT(*) FROM Заявки WHERE id_individ = @id_individ AND Тип_услуги = @Тип_услуги AND Название = @Название";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@id_individ", id_individ);
                    checkCmd.Parameters.AddWithValue("@Тип_услуги", Тип_услуги);
                    checkCmd.Parameters.AddWithValue("@Название", Название);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Заявка уже была отправлена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Вставляем новую заявку
                string insertQuery = "INSERT INTO Заявки (Дата_поступления, id_individ, fam, name, otch, Тип_услуги, Название, Цена, Статус) VALUES (@date, @id_individ, @fam, @name, @otch, @Тип_услуги, @Название, @Цена, @Статус)";
                using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                {
                    insertCmd.Parameters.AddWithValue("@date", DateTime.Now);
                    insertCmd.Parameters.AddWithValue("@id_individ", id_individ);
                    insertCmd.Parameters.AddWithValue("@fam", fam);
                    insertCmd.Parameters.AddWithValue("@name", name);
                    insertCmd.Parameters.AddWithValue("@otch", otch);
                    insertCmd.Parameters.AddWithValue("@Тип_услуги", Тип_услуги);
                    insertCmd.Parameters.AddWithValue("@Название", Название);
                    insertCmd.Parameters.AddWithValue("@Цена", Цена);
                    insertCmd.Parameters.AddWithValue("@Статус", "Новая");

                    insertCmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Заявка успешно отправлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
