using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleDatabaseWalkthrough
{
    public partial class Form3 : Form
    {
        private bool isAuthenticated = false; // Переменная для отслеживания состояния авторизации
        public event Action<string> OnStatusChanged; // Событие для передачи статуса кнопки
        public string UserRole { get; private set; }
        public Form3()
        {
            InitializeComponent();
        }

        // Личный кабинет
        private void button1_Click(object sender, EventArgs e)
        {
            if (IsUserAuthenticated())
            {
                OpenUserForm(UserRole); 
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в систему.");
                button3.PerformClick(); 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();

            if (!isAuthenticated) 
            {
                if (form1.ShowDialog() == DialogResult.OK)
                {
                    if (UserSession.IsAuthenticated)
                    {
                        isAuthenticated = true; 
                        UserRole = UserSession.UserRole; 
                        button3.Text = "Выход"; 
                        OnStatusChanged?.Invoke("Выход");
                    }
                }
            }
            else 
            {
                isAuthenticated = false; 
                button3.Text = "Вход"; 
                MessageBox.Show("Вы вышли из системы."); 
                OnStatusChanged?.Invoke("Вход");
            }
        }

    private void OpenUserForm(string userRole)
        {
            if (userRole == "1") // Администратор
            {
                Form12 form12 = new Form12();
                form12.Show();
            }
            else if (userRole == "2") // Обычный пользователь
            {
                Form6 form6 = new Form6(UserSession.CurrentUserId);
                form6.Show();
            }
        }

        private bool IsUserAuthenticated()
        {
            return !string.IsNullOrEmpty(UserRole); // Проверяем, установлена ли роль пользователя
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show();
        }

        private void ShowForm11(object sender)
        {
            // Создаем экземпляр Form11 и передаем имя нажатой кнопки
            Button clickedButton = sender as Button;
            Form11 form11 = new Form11(clickedButton.Name);
            form11.ShowDialog(); 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (isAuthenticated) 
            {
                ShowForm11(sender); 
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в аккаунт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isAuthenticated) 
            {
                ShowForm11(sender); 
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в аккаунт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (isAuthenticated) 
            {
                ShowForm11(sender); 
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в аккаунт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (isAuthenticated) 
            {
                ShowForm11(sender); 
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в аккаунт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }
        }

        private void телевидениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();

            // Подписка на события
            this.OnStatusChanged += form4.UpdateButtonStatus;
            form4.OnLogout += () =>
            {
                isAuthenticated = false; 
                button3.Text = "Вход"; 
                OnStatusChanged?.Invoke("Вход"); // Передаем статус
            };

            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            form4.Left = (screenWidth - form4.Width) / 2;
            form4.Top = (screenHeight - form4.Height) / 2;

            form4.Show();
        }

        private void видеонаблюдениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form13 form13 = new Form13();
            form13.Show();
        }

        private void оборудованиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form14 form14 = new Form14();
            form14.Show();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
