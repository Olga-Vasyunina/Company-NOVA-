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
    public partial class Form16 : Form
    {
        private int CurrentUserId;
        private DataTable dataTable;
        public Form16(int userId)
        {
            InitializeComponent();
            CurrentUserId = userId; // Сохраняем переданный ID
            FilterServices(); // Применение фильтра к услугам
        }

        private void LoadData2()
        {
            if (UserSession.IsAuthenticated)
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
                {
                    string query = "SELECT * FROM Оплаченные WHERE id_individ = @id_individ"; 
                    SqlDataAdapter adap = new SqlDataAdapter(query, con);
                    adap.SelectCommand.Parameters.AddWithValue("@id_individ", UserSession.CurrentUserId);

                    DataTable dt = new DataTable();
                    adap.Fill(dt);

                    оплаченныеBindingSource2.DataSource = dt;
                    dataGridView1.DataSource = оплаченныеBindingSource2;
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в систему для просмотра данных.");
            }
        }

        private void FilterServices()
        {
            if (UserSession.IsAuthenticated)
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
                {
                    string query = "SELECT * FROM Заявки WHERE id_individ = @id_individ";
                    SqlDataAdapter adap = new SqlDataAdapter(query, con);
                    adap.SelectCommand.Parameters.AddWithValue("@id_individ", UserSession.CurrentUserId);

                    DataTable dt = new DataTable();
                    adap.Fill(dt);

                    заявкиBindingSource1.DataSource = dt;
                    dataGridView2.DataSource = заявкиBindingSource1;
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в систему для просмотра данных.");
            }
        }

        private void заявкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(UserSession.CurrentUserId);
            form6.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView2.SelectedRows[0];

                if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                    string.IsNullOrWhiteSpace(textBox2.Text) ||
                    string.IsNullOrWhiteSpace(textBox3.Text) ||
                    string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля для оплаты.");
                    return;
                }

                string month = textBox2.Text.Trim();
                if (month.Length != 2 || !IsDigitsOnly(month))
                {
                    MessageBox.Show("Неправильный ввод срока действия (ММ)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; 
                }

                string year = textBox3.Text.Trim();
                if (year.Length != 2 || !IsDigitsOnly(year))
                {
                    MessageBox.Show("Неправильный ввод срока действия (ГГ)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; 
                }

                string cvv = textBox4.Text.Trim();
                if (cvv.Length != 3 || !IsDigitsOnly(cvv))
                {
                    MessageBox.Show("Неправильный ввод кода CVV", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; 
                }

                var newRow = sampleDatabaseDataSet141.Оплаченные.NewОплаченныеRow();

                // Получаем максимальное значение Id_заявки из таблицы Оплаченные
                int maxId = sampleDatabaseDataSet141.Оплаченные
                                  .Select("Id_заявки IS NOT NULL")
                                  .Length > 0
                              ? sampleDatabaseDataSet141.Оплаченные.Max(r => r.Id_заявки)
                              : 0;

                newRow.Id_заявки = maxId + 1;

                // Переносим данные из выбранной строки в новую строку оплаченных
                newRow.id_individ = (int)((DataRowView)selectedRow.DataBoundItem)["id_individ"];
                newRow.fam = (string)((DataRowView)selectedRow.DataBoundItem)["fam"];
                newRow.name = (string)((DataRowView)selectedRow.DataBoundItem)["name"];
                newRow.otch = (string)((DataRowView)selectedRow.DataBoundItem)["otch"];
                newRow.Тип_услуги = (string)((DataRowView)selectedRow.DataBoundItem)["Тип_услуги"];
                newRow.Название = (string)((DataRowView)selectedRow.DataBoundItem)["Название"];
                newRow.Цена = (decimal)((DataRowView)selectedRow.DataBoundItem)["Цена"];
                newRow.Статус = "Оплачено"; 
                newRow.Дата_оплаты = DateTime.Now; 

                sampleDatabaseDataSet141.Оплаченные.AddОплаченныеRow(newRow);

                // Получаем идентификатор строки для удаления
                var rowToRemove = (DataRow)((DataRowView)selectedRow.DataBoundItem).Row;
                int idToDelete = Convert.ToInt32(rowToRemove["Id_заявки"]); 

                // Удаляем запись из базы данных
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Заявки WHERE Id_заявки = @Id_заявки", con))
                    {
                        cmd.Parameters.AddWithValue("@Id_заявки", idToDelete);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            var foundRows = sampleDatabaseDataSet15.Заявки.Select($"Id_заявки = {idToDelete}");
                            if (foundRows.Length > 0)
                            {
                                sampleDatabaseDataSet15.Заявки.Rows.Remove(foundRows[0]);
                                MessageBox.Show("Оплата успешно произведена!");
                                SaveChanges(); 

                                dataGridView1.DataSource = sampleDatabaseDataSet141.Оплаченные;
                                dataGridView2.DataSource = заявкиBindingSource1;
                            }
                            else
                            {
                                MessageBox.Show("Ошибка: строка не найдена в DataSet.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при удалении записи из базы данных.");
                        }
                    }
                }
            }
        }

        // Метод для проверки, состоит ли строка только из цифр
        private bool IsDigitsOnly(string str)
        {
            return str.All(char.IsDigit);
        }

        private void SaveChanges()
        {
            try
            {
                заявкиTableAdapter1.Update(sampleDatabaseDataSet15.Заявки);

                оплаченныеTableAdapter2.Update(sampleDatabaseDataSet141.Оплаченные);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message);
            }
        }

        private void Form16_Load(object sender, EventArgs e)
        {
                // TODO: данная строка кода позволяет загрузить данные в таблицу "sampleDatabaseDataSet15.Заявки". При необходимости она может быть перемещена или удалена.
                this.заявкиTableAdapter1.Fill(this.sampleDatabaseDataSet15.Заявки);
                // TODO: данная строка кода позволяет загрузить данные в таблицу "sampleDatabaseDataSet141.Оплаченные". При необходимости она может быть перемещена или удалена.
                this.оплаченныеTableAdapter2.Fill(this.sampleDatabaseDataSet141.Оплаченные);
            LoadData2();
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1) // Проверяем, что нажата строка заголовка
            {
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
                ShowFilterMenu(column);
            }
        }

        private void ShowFilterMenu(DataGridViewColumn column)
        {
            ContextMenuStrip filterMenu = new ContextMenuStrip();

            // Получаем уникальные значения для выбранного столбца
            DataTable dataTable;

            // Проверяем, является ли DataSource DataView
            if (dataGridView1.DataSource is DataView dataView)
            {
                dataTable = dataView.Table; // Получаем оригинальный DataTable
            }
            else if (dataGridView1.DataSource is DataTable dt)
            {
                dataTable = dt; // Если это уже DataTable
            }
            else
            {
                return; 
            }

            var uniqueValues = dataTable.AsEnumerable()
                .Select(row => row[column.DataPropertyName].ToString())
                .Distinct()
                .ToList();

            // Добавляем уникальные значения в контекстное меню
            foreach (var value in uniqueValues)
            {
                filterMenu.Items.Add(value, null, (s, e) => ApplyFilter(column, value));
            }

            filterMenu.Items.Add("Сбросить фильтр", null, (s, e) => ResetFilter());

            filterMenu.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
        }

        private void ApplyFilter(DataGridViewColumn column, string filterValue)
        {
            DataTable dataTable = (DataTable)dataGridView1.DataSource;

            // Применяем фильтрацию
            DataView dataView = new DataView(dataTable);
            dataView.RowFilter = $"{column.DataPropertyName} = '{filterValue}'"; 

            dataGridView1.DataSource = dataView;
        }

        private void ResetFilter()
        {
            DataTable originalDataTable;

            // Проверяем, является ли DataSource DataView
            if (dataGridView1.DataSource is DataView dataView)
            {
                originalDataTable = dataView.Table; 
            }
            else if (dataGridView1.DataSource is DataTable dt)
            {
                originalDataTable = dt; 
            }
            else
            {
                return; 
            }

            // Если у нас есть исходные данные, то просто возвращаем их
            if (originalDataTable != null)
            {
                dataGridView1.DataSource = originalDataTable.Copy(); 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (UserSession.IsAuthenticated)
            {
                // Фильтруем данные по id_individ
                var filteredRows = sampleDatabaseDataSet15.Заявки
                    .Where(row => row.id_individ == UserSession.CurrentUserId);

                DataTable filteredDataTable = new DataTable();

                filteredDataTable = sampleDatabaseDataSet15.Заявки.Clone();

                // Заполняем отфильтрованные данные
                foreach (var row in filteredRows)
                {
                    filteredDataTable.ImportRow(row);
                }

                // Привязываем отфильтрованные данные к dataGridView2
                dataGridView2.DataSource = filteredDataTable;
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в систему для просмотра данных.");
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length > 3)
            {
                textBox4.Text = textBox4.Text.Substring(0, 3);

                textBox4.SelectionStart = textBox4.Text.Length;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 2)
            {
                textBox2.Text = textBox2.Text.Substring(0, 2);

                textBox2.SelectionStart = textBox2.Text.Length;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length > 2)
            {
                textBox3.Text = textBox3.Text.Substring(0, 2);

                textBox3.SelectionStart = textBox3.Text.Length;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string textWithoutSpaces = textBox1.Text.Replace(" ", "");

            if (textWithoutSpaces.Length > 16)
            {
                textWithoutSpaces = textWithoutSpaces.Substring(0, 16);
            }

            // Форматируем текст, добавляя пробелы через каждые 4 цифры
            string formattedText = string.Empty;
            for (int i = 0; i < textWithoutSpaces.Length; i++)
            {
                if (i > 0 && i % 4 == 0)
                {
                    formattedText += " "; 
                }
                formattedText += textWithoutSpaces[i];
            }

            // Обновляем текст в TextBox и устанавливаем курсор в конец
            textBox1.TextChanged -= textBox1_TextChanged; // временно отключаем обработчик события, чтобы избежать рекурсии
            textBox1.Text = formattedText;
            textBox1.SelectionStart = textBox1.Text.Length; // устанавливаем курсор в конец текста
            textBox1.TextChanged += textBox1_TextChanged; // снова подключаем обработчик события
        }
    }
}
