using Daberna.Common;
using static System.Windows.Forms.ListView;

namespace Daberna.Test.Windows
{
    public partial class GamePage : Form
    {
        //private readonly TCPClient Client;
        private readonly MainPage MainPage;

        public GamePage(
            MainPage mainPage)
        {
            //Client = tCPClient;
            MainPage = mainPage;

            Program.Client.GamesHasTaken += OnGamesRecived;

            InitializeComponent();

        }

        private void OnGamesRecived(List<Game> games)
        {
            if (Games_listview.InvokeRequired)
            {
                // If it is, use Invoke to marshal the call back to the UI thread
                Games_listview.Invoke(new MethodInvoker(() =>
                {
                    UpdateGamesList(games);
                }));
            }
            else
            {
                // If it's already on the UI thread, update directly
                UpdateGamesList(games);
            }

        }

        private void GamePage_Load(object sender, EventArgs e)
        {
            SendGetListOfGamesMessage();
        }

        private void SendGetListOfGamesMessage()
        {
            Program.Client.SendGetAllGamesMessage();
        }

        private void Create_Btn_Click(object sender, EventArgs e)
        {
            Program.Client.SendCreateGameMessage(GameName_Txt.Text);
        }

        private void UpdateGamesList(List<Game> games)
        {
            Games_listview.Items.Clear(); // Clear existing items before adding new ones

            foreach (var game in games)
            {
                var lv = new ListViewItem { Text = game.Name };
                Games_listview.Items.Add(lv);
            }
        }
    }
}
