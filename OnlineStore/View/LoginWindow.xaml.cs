using System;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OnlineStore.View
{
    public partial class LoginWindow : Window
    {
        private string captchaCode;

        public LoginWindow()
        {
            InitializeComponent();
            UpdateCaptchaImage();
        }

        private void UpdateCaptchaImage()
        {
            GenerateCaptcha();
        }


        private void GenerateCaptcha()
        {
            captchaCode = GenerateRandomCode();
            RenderCaptchaImage(captchaCode);
        }

        private string GenerateRandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void RenderCaptchaImage(string code)
        {
            using (Bitmap bitmap = new Bitmap(200, 50))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Font font = new Font("Arial",
                                     20,
                                     System.Drawing.FontStyle.Bold);

                graphics.DrawString(code, font, Brushes.Black, new PointF(10, 10));

                MemoryStream memoryStream = new MemoryStream();
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                imgCaptcha.Source = bitmapImage;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;
            string enteredCaptcha = txtCaptcha.Text;

            if (enteredCaptcha.Equals(captchaCode, StringComparison.OrdinalIgnoreCase))
            {
                if (CheckCredentials(login, password, out int userRole))
                {
                    MessageBox.Show("Login successful!");

                    OpenWindowBasedOnRole(userRole);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid login or password. Please try again.");
                }
            }
            else
            {
                MessageBox.Show("Invalid captcha. Please try again.");
            }

            // Обновляем изображение капчи после каждой попытки входа
            UpdateCaptchaImage();
        }


        private bool CheckCredentials(string login, string password, out int userRole)
        {
            userRole = 0; // Значение по умолчанию

            // Реализуйте проверку логина и пароля в базе данных
            string connectionString = "Data Source=DESKTOP-KVMUOKD\\SQLEXPRESS;Initial Catalog=OnlineStoreDB;Integrated Security=True"; // Замените на свою строку подключения
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT TypeUserID FROM [User] WHERE login = @login AND password = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out userRole))
                {
                    return true;
                }
            }

            return false;
        }

        private void OpenWindowBasedOnRole(int userRole)
        {
            // Открывайте окно в зависимости от роли пользователя
            switch (userRole)
            {
                case 1: // Роль 1
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    break;

                case 2: // Роль 2
                    UserWindow userWindow = new UserWindow();
                    userWindow.Show();
                    break;

                default:
                    MessageBox.Show("Unknown user role. Cannot determine which window to open.");
                    break;
            }
        }

        private void loh_click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Close();
        }

        private void loh1_click(object sender, RoutedEventArgs e)
        {
            UserWindow userWindow = new UserWindow();
            userWindow.Show();
        }
    }
}
