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
    public partial class Form15 : Form
    {
        private DataGridViewComboBoxColumn typeColumn;
        private DataGridViewComboBoxColumn nameColumn;
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataTable dataTable;
        private DataTable filteredDataTable; // Данные для отображения после фильтрации
        public Form15()
        {
            InitializeComponent();
        }

        private List<string> uniqueTypes = new List<string>();
        private List<string> uniqueNames = new List<string>();

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
            {
                SqlDataAdapter adap = new SqlDataAdapter("SELECT * FROM Предоставляемые_услуги", con);
                dataTable = new DataTable();
                adap.Fill(dataTable);

                // Копируем данные в новую таблицу для фильтрации
                filteredDataTable = dataTable.Copy();

                предоставляемыеуслугиBindingSource.DataSource = filteredDataTable;
                dataGridView1.DataSource = предоставляемыеуслугиBindingSource;

                // Получаем уникальные значения для комбинированного столбца
                uniqueTypes = dataTable.AsEnumerable()
                    .Select(row => row.Field<string>("Тип_услуги"))
                    .Distinct()
                    .ToList();

                // Настройка ComboBox для "Тип услуги"
                DataGridViewComboBoxColumn typeColumn = new DataGridViewComboBoxColumn
                {
                    HeaderText = "Тип услуги",
                    Name = "Тип_услуги",
                    DataPropertyName = "Тип_услуги", 
                    Width = 200 
                };
                typeColumn.Items.AddRange(uniqueTypes.ToArray());
                dataGridView1.Columns.Add(typeColumn);
                typeColumn.DisplayIndex = 0;
                
                typeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

                // Настройка TextBox для "Название"
                DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn
                {
                    HeaderText = "Название",
                    Name = "Название",
                    DataPropertyName = "Название", 
                    Width = 200
                };
                dataGridView1.Columns.Add(nameColumn);
                nameColumn.DisplayIndex = 1;
                
                nameColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

                // Настройка столбца для цены
                DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn
                {
                    HeaderText = "Цена",
                    Name = "Цена",
                    DataPropertyName = "Цена"
                };
                priceColumn.DisplayIndex = 2;
                dataGridView1.Columns.Add(priceColumn);
                
                priceColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

                
                dataGridView1.Columns["Id_услуги"].Visible = false;

                // Установка значений для ComboBox в каждой строке
                foreach (DataRow row in filteredDataTable.Rows)
                {
                    int rowIndex = filteredDataTable.Rows.IndexOf(row);
                    dataGridView1.Rows[rowIndex].Cells["Тип_услуги"].Value = row["Тип_услуги"];
                    dataGridView1.Rows[rowIndex].Cells["Название"].Value = row["Название"];
                }

                // Добавление обработчика события для заголовков столбцов
                dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
            }
        }

        private void Form15_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sampleDatabaseDataSet9.Предоставляемые_услуги". При необходимости она может быть перемещена или удалена.
            this.предоставляемые_услугиTableAdapter1.Fill(this.sampleDatabaseDataSet9.Предоставляемые_услуги);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sampleDatabaseDataSet8.Предоставляемые_услуги". При необходимости она может быть перемещена или удалена.
            this.предоставляемые_услугиTableAdapter.Fill(this.sampleDatabaseDataSet8.Предоставляемые_услуги);
            LoadData();
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
        }

        private void заявкиToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e) 
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
            {
                // Создаем SqlDataAdapter для обновления данных
                SqlDataAdapter adap = new SqlDataAdapter("SELECT * FROM Предоставляемые_услуги", con);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adap);

                try
                {
                    adap.Update(filteredDataTable);
                    MessageBox.Show("Изменения успешно сохранены.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении изменений: " + ex.Message);
                }
            }
        }

        private int GetMaxId()
        {
            int maxId = 0;

            string query = "SELECT MAX(Id_услуги) FROM Предоставляемые_услуги";

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        maxId = Convert.ToInt32(result);
                    }
                }
            }

            return maxId;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что нажата не заголовочная ячейка и не пустая ячейка
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Начинаем редактирование выбранной ячейки
                dataGridView1.BeginEdit(true);
            }
        }

        private void button3_Click(object sender, EventArgs e) // Удаление
        {
            if (dataGridView1.CurrentRow != null)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
            }
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

            // Добавление условий фильтрации
            filterMenu.Items.Add("Фильтр по значению", null, (s, e) => ApplyFilter(column));

            filterMenu.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
        }

        private void ApplyFilter(DataGridViewColumn column)
        {
            // Получаем правильное имя столбца из DataTable
            string columnName = column.DataPropertyName;

            // Получаем все уникальные значения из выбранного столбца
            var uniqueValues = dataTable.AsEnumerable()
                .Select(row => row[columnName].ToString())
                .Distinct()
                .OrderBy(value => value)
                .ToList();

            // Создаем меню для выбора значений фильтрации
            ContextMenuStrip valueMenu = new ContextMenuStrip();

            foreach (var value in uniqueValues)
            {
                valueMenu.Items.Add(new ToolStripMenuItem(value, null, (s, e) =>
                {
                    FilterData(columnName, value);
                }));
            }

            // Добавляем пункт для сброса фильтра
            valueMenu.Items.Add(new ToolStripMenuItem("Сбросить фильтр", null, (s, e) =>
            {
                FilterData(columnName, null); 
            }));

            valueMenu.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
        }

        private void FilterData(string columnName, string filterValue)
        {
            if (filterValue != null)
            {
                предоставляемыеуслугиBindingSource.Filter = $"{columnName} = '{filterValue}'";
            }
            else
            {
                предоставляемыеуслугиBindingSource.RemoveFilter();
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            // Обработка нажатия клавиши Delete для удаления строки
            if (e.KeyCode == Keys.Delete)
            {
                button3.PerformClick(); 
                e.Handled = true; 
            }
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
