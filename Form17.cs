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
    public partial class Form17 : Form
    {
        private DataTable dataTable;
        public Form17()
        {
            InitializeComponent();
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

        private void Form17_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sampleDatabaseDataSet14.Оплаченные". При необходимости она может быть перемещена или удалена.
            this.оплаченныеTableAdapter.Fill(this.sampleDatabaseDataSet14.Оплаченные);
            // Подключаем обработчик события для клика по заголовку столбца
            LoadData();
            dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SampleDatabaseConnectionString))
            {
                SqlDataAdapter adap = new SqlDataAdapter("SELECT * FROM Оплаченные", con);
                dataTable = new DataTable();
                adap.Fill(dataTable);

                // Привязка данных к DataGridView
                оплаченныеBindingSource.DataSource = dataTable;
                dataGridView1.DataSource = оплаченныеBindingSource;
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
                    // Применяем фильтр при выборе значения
                    оплаченныеBindingSource.Filter = $"{columnName} = '{value}'";
                }));
            }

            valueMenu.Items.Add(new ToolStripMenuItem("Сбросить фильтр", null, (s, e) =>
            {
                оплаченныеBindingSource.RemoveFilter();
            }));

            valueMenu.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
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
