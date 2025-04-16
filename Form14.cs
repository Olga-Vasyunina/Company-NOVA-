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
    public partial class Form14 : Form
    {
        public Form14()
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

            // Подписка на события
            this.OnStatusChanged += form4.UpdateButtonStatus;
            form4.OnLogout += () =>
            {
                isAuthenticated = false; 
                button3.Text = "Вход"; 
                OnStatusChanged?.Invoke("Вход"); 
            };

            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            form4.Left = (screenWidth - form4.Width) / 2;
            form4.Top = (screenHeight - form4.Height) / 2;

            form4.Show();
            this.Close();
        }

        private void ShowForm20(object sender)
        {
            // Создаем экземпляр Form20 и передаем имя нажатой кнопки
            Button clickedButton = sender as Button;
            Form20 form20 = new Form20(clickedButton.Name);
            form20.ShowDialog(); 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ShowForm20(sender);
        }

        private void Form14_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(button11, "Стоимость аренды 150 рублей в месяц.");
            toolTip1.SetToolTip(button12, "Мы предоставляем гарантию.");
            toolTip1.SetToolTip(button13, "Возможна рассрочка, ежемесячный платёж 375 рублей.");
            toolTip1.SetToolTip(button8, "Мы предоставляем гарантию.");
            toolTip1.SetToolTip(button14, "Мы предоставляем гарантию.");
            toolTip1.SetToolTip(button10, "Возможна рассрочка, ежемесячный платёж 900 рублей.");
            toolTip1.SetToolTip(button15, "Возможна рассрочка, ежемесячный платёж 400 рублей.");
            toolTip1.SetToolTip(button9, "Возможна рассрочка, ежемесячный платёж 2000 рублей.");
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
            form13.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowForm20(sender);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowForm20(sender);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ShowForm20(sender);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ShowForm20(sender);
        }
    }
}
