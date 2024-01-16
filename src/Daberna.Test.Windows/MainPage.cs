using Daberna.Common;

namespace Daberna.Test.Windows
{
    public partial class MainPage : Form
    {
        
        public MainPage()
        {

            InitializeComponent();
            Program.Client = new TCPClient(SocketSettings.ServerIP, int.Parse(SocketSettings.ServerPort));

            Program.Client.Connected += OnConnected;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Connect_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Name_txt.Text))
            {
                MessageBox.Show("Name is empty!");
                return;
            }

            Program.Client.EnterNameToConnect(Name_txt.Text);
        }

        private void OnConnected()
        {
            MessageBox.Show("Client has been connected");
            GamePage gamePage = new GamePage(this);

            gamePage.Show();
        }

        private void MainPage_Load(object sender, EventArgs e)
        {

        }
    }
}