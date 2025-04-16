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
    public partial class Form13 : Form
    {
        public Form13()
        {
            InitializeComponent();
            // Проверяем, был ли пользователь уже авторизован
            if (isAuthenticated)
            {
                button1.Text = "Выход"; 
            }
            else
            {
                button1.Text = "Вход"; 
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
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

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Close();
        }

        private void ShowForm19(object sender)
        {
            // Создаем экземпляр Form19 и передаем имя нажатой кнопки
            Button clickedButton = sender as Button;
            Form19 form19 = new Form19(clickedButton.Name);
            form19.ShowDialog(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowForm19(sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowForm19(sender);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ShowForm19(sender);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();

            if (!isAuthenticated) 
            {
                if (form1.ShowDialog() == DialogResult.OK)
                {
                    // Если вход успешен, получаем роль пользователя
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
        public Action<string> UpdateButtonStatus { get; internal set; }
        public event Action OnLogout; // Событие для выхода
        public event Action<string> OnStatusChanged; // Событие для передачи статуса кнопки
        private bool isAuthenticated = true; 


        private void button2_Click(object sender, EventArgs e)
        {
            // Проверяем, авторизован ли пользователь
            if (IsUserAuthenticated())
            {
                OpenUserForm(UserRole); 
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в систему.");
                button1.PerformClick(); 
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

        private void оборудованиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form14 form14 = new Form14();
            form14.Show();
            this.Close();
        }

    }
}
