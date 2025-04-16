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
    public partial class Form5 : Form
    {
        private DataTable dataTable;
        private DataTable filteredDataTable; // Данные для отображения после фильтрации
        public Form5()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
            {
                string query = @"
            SELECT i.*, r.role 
            FROM Individ i
            LEFT JOIN Role r ON i.id_role = r.id_role";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    sampleDatabaseDataSet16.Tables["Individ"].Clear();

                    try
                    {
                        con.Open();

                        adapter.Fill(sampleDatabaseDataSet16, "IndividWithRole");

                        dataGridView1.DataSource = sampleDatabaseDataSet16.Tables["IndividWithRole"];

                        if (dataGridView1.Columns.Contains("role"))
                        {
                            dataGridView1.Columns.Remove("role");
                        }

                        // Создаем новый столбец выпадающего списка для ролей
                        DataGridViewComboBoxColumn roleColumn = new DataGridViewComboBoxColumn();
                        roleColumn.Name = "role";
                        roleColumn.HeaderText = "Роль";
                        roleColumn.DataPropertyName = "role"; 

                        roleColumn.Items.Add("Администратор");
                        roleColumn.Items.Add("Пользователь");

                        roleColumn.Width = 115;

                        // Добавляем столбец в DataGridView
                        dataGridView1.Columns.Add(roleColumn);

                        dataGridView1.Columns["role"].SortMode = DataGridViewColumnSortMode.NotSortable;

                        dataGridView1.Columns["role"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopCenter;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            row.Cells["role"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter; 
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
                    }
                }
            }
        }
        private void предоставляемыеУслугиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form15 form15 = new Form15();
            form15.Show();
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            form15.Left = (screenWidth - form15.Width) / 2;
            form15.Top = (screenHeight - form15.Height) / 2;

            form15.Show();
            this.Close();
        }

        private void оплатаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form17 form17 = new Form17();
            form17.Show();
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            form17.Left = (screenWidth - form17.Width) / 2;
            form17.Top = (screenHeight - form17.Height) / 2;

            form17.Show();
            this.Close();
        }

        private void поступившиеЗаявкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form12 form12 = new Form12();
            form12.Show();
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            form12.Left = (screenWidth - form12.Width) / 2;
            form12.Top = (screenHeight - form12.Height) / 2;

            form12.Show();
            this.Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sampleDatabaseDataSet16.Individ". При необходимости она может быть перемещена или удалена.
            this.individTableAdapter.Fill(this.sampleDatabaseDataSet16.Individ);
            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            try
            {
                DataTable individTable = sampleDatabaseDataSet16.Tables["IndividWithRole"];

                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
                {
                    con.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        // Команда для обновления
                        adapter.UpdateCommand = new SqlCommand("UPDATE Individ SET fam = @fam, name = @name, otch = @otch, login = @login, password = @password, id_role = @id_role WHERE id_individ = @id_individ", con);
                        adapter.UpdateCommand.Parameters.Add("@fam", SqlDbType.NVarChar, 50, "fam");
                        adapter.UpdateCommand.Parameters.Add("@name", SqlDbType.NVarChar, 50, "name");
                        adapter.UpdateCommand.Parameters.Add("@otch", SqlDbType.NVarChar, 50, "otch");
                        adapter.UpdateCommand.Parameters.Add("@login", SqlDbType.NVarChar, 50, "login");
                        adapter.UpdateCommand.Parameters.Add("@password", SqlDbType.NVarChar, 50, "password");
                        adapter.UpdateCommand.Parameters.Add("@id_role", SqlDbType.Int, 0, "id_role");
                        adapter.UpdateCommand.Parameters.Add("@id_individ", SqlDbType.Int, 0, "id_individ");

                        // Команда для вставки
                        adapter.InsertCommand = new SqlCommand("INSERT INTO Individ (fam, name, otch, login, password, id_role) VALUES (@fam, @name, @otch, @login, @password, @id_role)", con);
                        adapter.InsertCommand.Parameters.Add("@fam", SqlDbType.NVarChar, 50, "fam");
                        adapter.InsertCommand.Parameters.Add("@name", SqlDbType.NVarChar, 50, "name");
                        adapter.InsertCommand.Parameters.Add("@otch", SqlDbType.NVarChar, 50, "otch");
                        adapter.InsertCommand.Parameters.Add("@login", SqlDbType.NVarChar, 50, "login");
                        adapter.InsertCommand.Parameters.Add("@password", SqlDbType.NVarChar, 50, "password");
                        adapter.InsertCommand.Parameters.Add("@id_role", SqlDbType.Int, 0, "id_role");

                        // Команда для удаления
                        adapter.DeleteCommand = new SqlCommand("DELETE FROM Individ WHERE id_individ = @id_individ", con);
                        adapter.DeleteCommand.Parameters.Add("@id_individ", SqlDbType.Int, 0, "id_individ");

                        // Сопоставляем столбец "role" с id_role в базе данных
                        foreach (DataRow row in individTable.Rows)
                        {
                            if (row.RowState == DataRowState.Modified || row.RowState == DataRowState.Added)
                            {
                                if (row["role"] != DBNull.Value)
                                {
                                    row["id_role"] = GetRoleId(row["role"].ToString());
                                }
                            }
                        }

                        adapter.Update(individTable);
                    }
                }

                MessageBox.Show("Изменения успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении изменений: " + ex.Message);
            }
        }

        // Метод для получения ID роли по названию
        private int GetRoleId(string roleName)
        {
            switch (roleName)
            {
                case "Администратор":
                    return 1; 
                case "Пользователь":
                    return 2; 
                default:
                    return 0; 
            }
        }

    }
}
