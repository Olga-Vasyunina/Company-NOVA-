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
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
            textBox1.ReadOnly = true; 
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

        private void Form9_Load(object sender, EventArgs e)
        {
            // Вызываем метод для отображения информации из label2
            DisplayInformationForLabel2();
            lastClickedLabel = label2; // Инициализируем последним нажатым Label
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // При клике на label2 также отображаем информацию
            DisplayInformationForLabel2();
            UpdateLabelHighlight(label2);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.Black;
            // Убираем выделение текста при получении фокуса
            textBox1.Enter += TextBox1_Enter;
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            // Убираем фокус с textBox1, чтобы он не выделялся
            this.ActiveControl = null;
        }

        private void DisplayInformationForLabel2()
        {
            textBox1.Text = "ПАО «NOVA» — один из ведущих интегрированных провайдеров цифровых услуг в России, " +
                            "который активно работает во всех сегментах рынка и обслуживает миллионы домохозяйств, " +
                            "а также государственные и частные организации.\n\n" +
                            "Компания занимает значительные позиции на рынке услуг высокоскоростного интернета и цифрового телевидения, " +
                            "а также в области мобильной связи.\n\n" +
                            "Количество клиентов, использующих услуги доступа в интернет на основе современных технологий, превышает 10 млн. " +
                            "«NOVA» входит в число крупнейших мобильных операторов страны с более чем 35 млн абонентов.\n" +
                            "В сотрудничестве с партнерами компания развивает видеосервис Wink, " +
                            "который занимает третье место среди онлайн-кинотеатров России по количеству активных подписчиков.\n\n" +
                            "«NOVA» является признанным экспертом в области инновационных решений для цифровых государственных сервисов, " +
                            "кибербезопасности, цифровизации регионов, здравоохранения, образования и ЖКХ. " +
                            "Компания активно занимается разработкой собственных программных решений и производством телекоммуникационного оборудования, " +
                            "уделяя особое внимание импортозамещению и развитию новых технологий.";

            label2.BackColor = SystemColors.ButtonFace;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "История компании NOVA начинается с 2000 года, когда она была основана с целью предоставления высококачественных услуг связи. " +
                            "С тех пор компания активно развивалась и расширяла спектр своих услуг, включая интернет, телевидение и мобильную связь.";
            UpdateLabelHighlight(label3);
        }

        private Label lastClickedLabel; // Переменная для хранения последнего нажатого Label

        private void UpdateLabelHighlight(Label clickedLabel)
        {
            if (lastClickedLabel != null && lastClickedLabel != clickedLabel)
            {
                lastClickedLabel.BackColor = SystemColors.ButtonHighlight; 
            }

            clickedLabel.BackColor = SystemColors.ButtonFace;

            lastClickedLabel = clickedLabel;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Миссия компании NOVA — обеспечить доступ к цифровым услугам для каждого гражданина России, " +
                            "предоставляя высококачественные и доступные решения для связи и развлечений.";
            UpdateLabelHighlight(label4);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Компания NOVA имеет все необходимые лицензии для предоставления услуг связи на территории Российской Федерации. " +
                            "Лицензии подтверждают высокие стандарты качества и надежности предоставляемых услуг.";
            UpdateLabelHighlight(label6);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "NOVA имеет ряд сертификатов, подтверждающих соответствие её услуг международным стандартам качества. " +
                            "Компания регулярно проходит аудит и сертификацию своих процессов.";
            UpdateLabelHighlight(label7);
        }

        private void label8_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Компания NOVA предлагает услуги с сертификатами качества, что подтверждает их надежность и безопасность для пользователей. " +
                            "Каждый продукт проходит строгую проверку перед выходом на рынок.";
            UpdateLabelHighlight(label8);
        }

        private void дляОператоровToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Close();
        }

        public Action<string> UpdateButtonStatus { get; internal set; }
        public event Action OnLogout; // Событие для выхода
        public event Action<string> OnStatusChanged; // Событие для передачи статуса кнопки
        private bool isAuthenticated = true; // Предполагаем, что пользователь авторизован

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

        private void button2_Click(object sender, EventArgs e)
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
            form13.Show();
            this.Close();
        }

        private void оборудованиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form14 form14 = new Form14();
            form14.Show();
            this.Close();
        }
    }
}
