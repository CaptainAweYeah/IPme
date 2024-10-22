using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MailKit.Net.Smtp;
using MimeKit;

namespace IPme
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            submitEmail.Click += SubmitEmail_Click;
        }

        // Click event (find way to simplify?)
        private void SubmitEmail_Click(object sender, RoutedEventArgs e)
        {
            OnClick(emailIn, emailOut, emailPass);
        }


        private void OnClick(TextBox mailIn, TextBox mailOut, TextBox ePass)
        {
            if (mailIn != null && mailOut != null) 
            {
                string ipOut = ipProcess();

                int atCnt1 = mailIn.Text.IndexOf('@');
                int atCnt2 = mailOut.Text.IndexOf('@');

                string eClient = mailIn.Text.Substring(atCnt1 + 1);

                string mailInName = mailIn.Text.Substring(0, atCnt1);
                string mailOutName = mailOut.Text.Substring(0, atCnt2);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress($"{mailInName}",$"{mailIn.Text}"));
                message.To.Add(new MailboxAddress($"{mailOutName}", $"{mailOut.Text}"));
                message.Subject = $"IPMe Output from {mailInName}!";

                message.Body = new TextPart("plain")
                {
                    Text = $"{ipOut}"
                };

                using var client = new SmtpClient();
                client.Connect($"smtp.{eClient}", 587, false);
                client.Authenticate($"{mailIn.Text}", $"{ePass.Text}");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private string ipProcess()
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "ipconfig.exe";
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            return output;
        }
    }
}
