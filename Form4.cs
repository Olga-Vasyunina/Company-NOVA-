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
    public partial class Form4 : Form
    {
        public Action<string> UpdateButtonStatus { get; internal set; }

        public Form4()
        {
            InitializeComponent();
            if (isAuthenticated)
            {
                button1.Text = "Выход"; 
            }
            else
            {
                button1.Text = "Вход"; 
            }
        }

        private void UpdateButtonText(string status)
        {
            button1.Text = status;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 avtoForm = new Form1();

            // Подписываемся на событие закрытия формы
            avtoForm.FormClosed += (s, args) =>
            {
                // Очищаем для логина и пароля
                avtoForm.ClearTextFields();
            };

            avtoForm.Show();
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            avtoForm.Left = (screenWidth - avtoForm.Width) / 2;
            avtoForm.Top = (screenHeight - avtoForm.Height) / 2;

            avtoForm.Show();

            this.Close();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show();

            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            form9.Left = (screenWidth - form9.Width) / 2;
            form9.Top = (screenHeight - form9.Height) / 2;

            form9.Show();
            this.Close();
        }

        public event Action OnLogout; // Событие для выхода
        public event Action<string> OnStatusChanged; // Событие для передачи статуса кнопки
        private bool isAuthenticated = true; 

        private void button1_Click(object sender, EventArgs e)
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
                        button1.Text = "Выход"; 
                        OnStatusChanged?.Invoke("Выход");
                    }
                }
            }
            else 
            {
                isAuthenticated = false; 
                button1.Text = "Вход"; 
                MessageBox.Show("Вы вышли из системы."); 
                OnStatusChanged?.Invoke("Вход");
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (IsUserAuthenticated())
            {
                OpenUserForm(UserRole); // Открываем форму в зависимости от роли
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в систему.");
                button1.PerformClick(); // Открываем форму входа
            }
        }

        public string UserRole { get; private set; }

        private void OpenUserForm(string userRole)
        {
            if (userRole == "1") // Администратор
            {
                Form12 form12 = new Form12();
                form12.Show();
                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

                form12.Left = (screenWidth - form12.Width) / 2;
                form12.Top = (screenHeight - form12.Height) / 2;

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

        private void видеонаблюдениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form13 form13 = new Form13();

            // Подписка на события
            this.OnStatusChanged += form13.UpdateButtonStatus;
            form13.OnLogout += () =>
            {
                isAuthenticated = false; 
                button1.Text = "Вход"; 
                OnStatusChanged?.Invoke("Вход"); 
            };

            form13.Show();
            this.Close();
        }

        private void оборудованиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form14 form14 = new Form14();
            form14.Show();
            this.Close();
        }

        private void ShowForm18(object sender)
        {
            // Создаем экземпляр Form11 и передаем имя нажатой кнопки
            Button clickedButton = sender as Button;
            Form18 form18 = new Form18(clickedButton.Name);
            form18.ShowDialog(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowForm18(sender);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            ShowForm18(sender);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ShowForm18(sender);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ShowForm18(sender);
        }
    }
}
