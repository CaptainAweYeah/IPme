using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
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

        // Click event
        private void SubmitEmail_Click(object sender, RoutedEventArgs e)
        {
            IPAddress server = CheckIP(supportIP.Text);

            OnClick(server);
        }

        public static IPAddress CheckIP(string server)
        {
            if (server != null)
            {
                if (IPAddress.TryParse(server, out IPAddress ip))
                {
                    return IPAddress.Parse(server);
                }
            }
            throw new Exception("Code not valid!");
        }


        private void OnClick(IPAddress serverIP)
        {
            ClientLink link = new ClientLink();
            string ipConfigOut = ipProcess();
            link.Client(ipConfigOut, serverIP);
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
