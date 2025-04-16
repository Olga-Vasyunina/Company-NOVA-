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
    public partial class Form12 : Form
    {
        private DataTable dataTable;

        public Form12()
        {
            InitializeComponent();
            LoadStatusOptions(); 
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sampleDatabaseDataSet6.Заявки". При необходимости она может быть перемещена или удалена.
            this.заявкиTableAdapter1.Fill(this.sampleDatabaseDataSet6.Заявки);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sampleDatabaseDataSet7.Заявки". При необходимости она может быть перемещена или удалена.
            this.заявкиTableAdapter2.Fill(this.sampleDatabaseDataSet7.Заявки);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sampleDatabaseDataSet5.Заявки". При необходимости она может быть перемещена или удалена.
            this.заявкиTableAdapter.Fill(this.sampleDatabaseDataSet5.Заявки);
            // Подключаем обработчик события для клика по заголовку столбца
            LoadData();
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
            {
                SqlDataAdapter adap = new SqlDataAdapter("SELECT * FROM Заявки", con);
                dataTable = new DataTable();
                adap.Fill(dataTable);

                // Привязка данных к DataGridView
                заявкиBindingSource3.DataSource = dataTable;
                dataGridView1.DataSource = заявкиBindingSource3;
            }
        }

        private void LoadDataGridView()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Заявки", connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус заявки");
                return; 
            }

            int selectedId = (int)comboBox1.SelectedValue; 
            string selectedStatus = comboBox2.SelectedItem.ToString(); 

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
            {
                connection.Open();

                // SQL запрос для обновления статуса заявки
                string query = "UPDATE Заявки SET Статус = @Статус WHERE Id_заявки = @Id_заявки";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Статус", selectedStatus);
                    command.Parameters.AddWithValue("@Id_заявки", selectedId);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery(); 
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Статус успешно обновлен");

                            LoadDataGridView(); 
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить статус. Проверьте Id");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении статуса: {ex.Message}");
                    }
                }
            }
        }

        private void LoadStatusOptions()
        {
            comboBox2.Items.Add("Новая");
            comboBox2.Items.Add("В обработке");
            comboBox2.Items.Add("Завершена");
        }

        private void оплаченноToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1) 
            {
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
                ShowFilterMenu(column);
            }
        }
        private void ShowFilterMenu(DataGridViewColumn column)
        {
            ContextMenuStrip filterMenu = new ContextMenuStrip();

            filterMenu.Items.Add("Фильтр по значению", null, (s, e) => ApplyFilter(column));

            filterMenu.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
        }
        private void ApplyFilter(DataGridViewColumn column)
        {
            // Получаем правильное имя столбца из DataTable
            string columnName = column.DataPropertyName;

            var uniqueValues = dataTable.AsEnumerable()
                .Select(row => row[columnName].ToString())
                .Distinct()
                .OrderBy(value => value)
                .ToList();

            // Создание меню
            ContextMenuStrip valueMenu = new ContextMenuStrip();

            foreach (var value in uniqueValues)
            {
                valueMenu.Items.Add(new ToolStripMenuItem(value, null, (s, e) =>
                {
                    заявкиBindingSource3.Filter = $"{columnName} = '{value}'";
                }));
            }

            valueMenu.Items.Add(new ToolStripMenuItem("Сбросить фильтр", null, (s, e) =>
            {
                заявкиBindingSource3.RemoveFilter();
            }));

            valueMenu.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                int selectedId = (int)selectedRow.Cells["id_individ"].Value;
                string selectedName = selectedRow.Cells["Название"].Value.ToString();
                string selectedStatus = selectedRow.Cells["Статус"].Value.ToString();

                comboBox2.SelectedValue = selectedId; 
                comboBox1.SelectedValue = selectedStatus; 

                UpdateRichTextBox(selectedRow);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Убираем выделение текста при получении фокуса
            richTextBox1.Enter += richTextBox1_Enter;
        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            // Убираем фокус с textBox1, чтобы он не выделялся
            this.ActiveControl = null;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что курсор находится над допустимой ячейкой
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];

                UpdateRichTextBox(row);
            }
        }

        private void UpdateRichTextBox(DataGridViewRow row)
        {
            // Извлекаем необходимые данные из строки
            string фамилия = row.Cells["fam"].Value?.ToString();
            string имя = row.Cells["name"].Value?.ToString();
            string отчество = row.Cells["otch"].Value?.ToString();
            string датаПоступления = row.Cells["Дата_поступления"].Value?.ToString();
            string типУслуги = row.Cells["Тип_услуги"].Value?.ToString();
            string цена = row.Cells["Цена"].Value?.ToString();

            richTextBox1.Clear();

            AppendFormattedText(richTextBox1, "Фамилия: ", фамилия);
            AppendFormattedText(richTextBox1, "Имя: ", имя);
            AppendFormattedText(richTextBox1, "Отчество: ", отчество);
            AppendFormattedText(richTextBox1, "Дата поступления: ", датаПоступления);
            AppendFormattedText(richTextBox1, "Тип услуги: ", типУслуги);
            AppendFormattedText(richTextBox1, "Цена: ", цена);

            CenterAlignRichTextBox(richTextBox1);
        }

        private void AppendFormattedText(RichTextBox richTextBox, string label, string value)
        {
            // Добавляем подчеркивающий текст
            richTextBox.SelectionColor = Color.Blue;
            richTextBox.SelectionFont = new Font(richTextBox.Font, FontStyle.Underline);
            richTextBox.AppendText(label);

            // Сбрасываем стиль к обычному
            richTextBox.SelectionColor = Color.Black;
            richTextBox.SelectionFont = new Font(richTextBox.Font, FontStyle.Regular);
            richTextBox.AppendText(value + "\n");
        }

        private void CenterAlignRichTextBox(RichTextBox richTextBox)
        {
            // Установка текста по центру
            richTextBox.SelectAll();
            richTextBox.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox.DeselectAll();
        }

        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            form5.Left = (screenWidth - form5.Width) / 2;
            form5.Top = (screenHeight - form5.Height) / 2;

            form5.Show();
            this.Close();
        }
    }
}
