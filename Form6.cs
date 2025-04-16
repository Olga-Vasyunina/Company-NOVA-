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
    public partial class Form6 : Form
    {
        private int CurrentUserId;
        public Form6(int userId)
        {
            InitializeComponent();
            CurrentUserId = userId; // Сохраняем переданный ID
            LoadData(); 
        }

        private void LoadData()
        {
            string query = "SELECT * FROM Заявки WHERE id_individ = @id_individ";

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_individ", CurrentUserId);

                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при загрузке заявок: " + ex.Message);
                    }
                }
            }
        }
        
        private void Form6_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sampleDatabaseDataSet11.Заявки". При необходимости она может быть перемещена или удалена.
            this.заявкиTableAdapter.Fill(this.sampleDatabaseDataSet11.Заявки);
            LoadData();
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1) // нажата строка заголовка
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

        private void ResetFilter()
        {
            DataTable originalDataTable;

            // Проверяем, является ли DataSource DataView
            if (dataGridView1.DataSource is DataView dataView)
            {
                originalDataTable = dataView.Table; // Получаем оригинальный DataTable
            }
            else if (dataGridView1.DataSource is DataTable dt)
            {
                originalDataTable = dt; // Если это уже DataTable
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

        private void ApplyFilter(DataGridViewColumn column, string filterValue)
        {
            DataTable dataTable = (DataTable)dataGridView1.DataSource;

            // Применяем фильтрацию
            DataView dataView = new DataView(dataTable);
            dataView.RowFilter = $"{column.DataPropertyName} = '{filterValue}'"; 

            dataGridView1.DataSource = dataView;
        }

        private void оплатаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form16 form16 = new Form16(UserSession.CurrentUserId);
            form16.Show();
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            form16.Left = (screenWidth - form16.Width) / 2;
            form16.Top = (screenHeight - form16.Height) / 2;

            form16.Show();
            this.Close();
        }
    }
}
