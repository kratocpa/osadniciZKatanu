using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.ComponentModel;
using osadniciZKatanu;
using osadniciZKatanuAI;
using simulator;

namespace simulatorGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fsPl;
        string scPl;
        string thPl;
        string foPl;
        int rounds;
        bool rndGmBr;
        bool rotatePl;
        int plCt;
        string resultStr;

        int pBar = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void fsPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    fsPlayerTextBox.Text = file;
                    fsPlayerTextBox.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    fsPlayerTextBox.Text = null;
                    fsPlayerTextBox.ToolTip = null;
                    break;
            }
        }

        private void scPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    scPlayerTextBox.Text = file;
                    scPlayerTextBox.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    scPlayerTextBox.Text = null;
                    scPlayerTextBox.ToolTip = null;
                    break;
            }
        }

        private void thPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
          
            fileDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    thPlayerTextBox.Text = file;
                    thPlayerTextBox.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    thPlayerTextBox.Text = null;
                    thPlayerTextBox.ToolTip = null;
                    break;
            }
        }

        private void foPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    foPlayerTextBox.Text = file;
                    foPlayerTextBox.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    foPlayerTextBox.Text = null;
                    foPlayerTextBox.ToolTip = null;
                    break;
            }
        }

        private void simulate()
        {
            Statistics statistic = new Statistics(new EngLanguage(), rounds, false);
            GameProperties GmProp = new GameProperties(rndGmBr, new EngLanguage());
            GmProp.LoadFromXml();
            int i = 0;

            while (i < rounds)
            {
                GameProperties gmProp = (GameProperties)GmProp.Clone();
                List<Player> players = new List<Player>();
                Simulator simul;
                try
                {
                    if (plCt == 2)
                    {
                        simul = new Simulator(simulateTwoPlayers(rotatePl, gmProp, i), gmProp);
                        if (fsPl != "") { simul.redPl = new MyGameLogic(fsPl); } else { simul.redPl = new MyGameLogic(); }
                        if (scPl != "") { simul.bluePl = new MyGameLogic(scPl); } else { simul.bluePl = new MyGameLogic(); }
                    }
                    else if (plCt == 3)
                    {
                        simul = new Simulator(simulateThreePlayers(rotatePl, gmProp, i), gmProp);
                        if (fsPl != "") { simul.redPl = new MyGameLogic(fsPl); } else { simul.redPl = new MyGameLogic(); }
                        if (scPl != "") { simul.bluePl = new MyGameLogic(scPl); } else { simul.bluePl = new MyGameLogic(); }
                        if (thPl != "") { simul.yellowPl = new MyGameLogic(thPl); } else { simul.yellowPl = new MyGameLogic(); }
                    }
                    else
                    {
                        simul = new Simulator(simulateFourPlayers(rotatePl, gmProp, i), gmProp);
                        if (fsPl != "") { simul.redPl = new MyGameLogic(fsPl); } else { simul.redPl = new MyGameLogic(); }
                        if (scPl != "") { simul.bluePl = new MyGameLogic(scPl); } else { simul.bluePl = new MyGameLogic(); }
                        if (thPl != "") { simul.yellowPl = new MyGameLogic(thPl); } else { simul.yellowPl = new MyGameLogic(); }
                        if (foPl != "") { simul.whitePl = new MyGameLogic(foPl); } else { simul.whitePl = new MyGameLogic(); }
                    }


                    var result = simul.run();
                    statistic.AddToStatistic(result);
                }
                catch (TooManyMovesException ex)
                {
                    resultLabel.Content = ("\n" + ex.Message + " in game number " + i);
                    statistic.AddToStatistic();
                }
                catch (TooManyRoundsException ex)
                {
                    resultLabel.Content = ("\n" + ex.Message + " in game number " + i);
                    statistic.AddToStatistic();
                }
                catch (WrongNumberOfPlayersException ex)
                {
                    resultLabel.Content = ("\n" + ex.Message + " in game number " + i);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                    break;
                }


                progressBar.Dispatcher.BeginInvoke(new Action(() => { progressBar.Value = i; }));

                i++;
            }
            
            resultStr = statistic.GetOverallStatistic();
            //System.Windows.MessageBox.Show(resultStr);

            resultLabel.Dispatcher.BeginInvoke(new Action(() => { resultLabel.Content = resultStr; }));
            startButton.Dispatcher.BeginInvoke(new Action(() => { startButton.IsEnabled = true; }));
        }

        private List<Player> simulateFourPlayers(bool rotatePl, GameProperties gmProp, int gameNum)
        {
            List<Player> players = new List<Player>();
            if (rotatePl)
            {
                if (gameNum % 4 == 0)
                {
                    players.Add(new Player(Game.color.red, false, gmProp));
                    players.Add(new Player(Game.color.blue, false, gmProp));
                    players.Add(new Player(Game.color.yellow, false, gmProp));
                    players.Add(new Player(Game.color.white, false, gmProp));
                }
                else if (gameNum % 4 == 1)
                {
                    players.Add(new Player(Game.color.white, false, gmProp));
                    players.Add(new Player(Game.color.red, false, gmProp));
                    players.Add(new Player(Game.color.blue, false, gmProp));
                    players.Add(new Player(Game.color.yellow, false, gmProp));
                }
                else if (gameNum % 4 == 2)
                {
                    players.Add(new Player(Game.color.yellow, false, gmProp));
                    players.Add(new Player(Game.color.white, false, gmProp));
                    players.Add(new Player(Game.color.red, false, gmProp));
                    players.Add(new Player(Game.color.blue, false, gmProp));
                }
                else
                {
                    players.Add(new Player(Game.color.blue, false, gmProp));
                    players.Add(new Player(Game.color.yellow, false, gmProp));
                    players.Add(new Player(Game.color.white, false, gmProp));
                    players.Add(new Player(Game.color.red, false, gmProp));
                }
            }
            else
            {
                players.Add(new Player(Game.color.red, false, gmProp));
                players.Add(new Player(Game.color.blue, false, gmProp));
                players.Add(new Player(Game.color.yellow, false, gmProp));
                players.Add(new Player(Game.color.white, false, gmProp));
            }
            return players;
        }

        private List<Player> simulateThreePlayers(bool rotatePl, GameProperties gmProp, int gameNum)
        {
            List<Player> players = new List<Player>();
            if (rotatePl)
            {
                if (gameNum % 3 == 0)
                {
                    players.Add(new Player(Game.color.red, false, gmProp));
                    players.Add(new Player(Game.color.blue, false, gmProp));
                    players.Add(new Player(Game.color.yellow, false, gmProp));
                }
                else if (gameNum % 3 == 1)
                {
                    players.Add(new Player(Game.color.yellow, false, gmProp));
                    players.Add(new Player(Game.color.red, false, gmProp));
                    players.Add(new Player(Game.color.blue, false, gmProp));
                }
                else if (gameNum % 3 == 2)
                {
                    players.Add(new Player(Game.color.blue, false, gmProp));
                    players.Add(new Player(Game.color.yellow, false, gmProp));
                    players.Add(new Player(Game.color.red, false, gmProp));                   
                }
            }
            else
            {
                players.Add(new Player(Game.color.red, false, gmProp));
                players.Add(new Player(Game.color.blue, false, gmProp));
                players.Add(new Player(Game.color.yellow, false, gmProp));
            }
            return players;
        }

        private List<Player> simulateTwoPlayers(bool rotatePl, GameProperties gmProp, int gameNum)
        {
            List<Player> players = new List<Player>();
            if (rotatePl)
            {
                if (gameNum % 2 == 0)
                {
                    players.Add(new Player(Game.color.red, false, gmProp));
                    players.Add(new Player(Game.color.blue, false, gmProp));
                }
                else if (gameNum % 2 == 1)
                {
                    players.Add(new Player(Game.color.blue, false, gmProp));
                    players.Add(new Player(Game.color.red, false, gmProp));      
                }
            }
            else
            {
                players.Add(new Player(Game.color.red, false, gmProp));
                players.Add(new Player(Game.color.blue, false, gmProp));
            }
            return players;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            fsPl = fsPlayerTextBox.Text;
            scPl = scPlayerTextBox.Text;
            thPl = thPlayerTextBox.Text;
            foPl = foPlayerTextBox.Text;
            try
            {
                rounds = Int32.Parse(roundTextBox.Text);
                rndGmBr = false;
                if (rndCheckBox.IsChecked == true) { rndGmBr = true; }
                rotatePl = false;
                if (rotatePlCheckBox.IsChecked == true) { rotatePl = true; }
                progressBar.Minimum = 0;
                progressBar.Maximum = rounds;
                progressBar.Value = 0;
                Thread t = new Thread(new ThreadStart(simulate));
                t.Start();
                startButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void playerCountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (playerCountComboBox.SelectedIndex == 0)
            {
                fsPlayerButton.IsEnabled = true;
                fsPlayerTextBox.IsEnabled = true;
                scPlayerButton.IsEnabled = true;
                scPlayerTextBox.IsEnabled = true;
                thPlayerButton.IsEnabled = false;
                thPlayerTextBox.IsEnabled = false;
                foPlayerButton.IsEnabled = false;
                foPlayerTextBox.IsEnabled = false;
                plCt = 2;
            }
            else if (playerCountComboBox.SelectedIndex == 1)
            {
                fsPlayerButton.IsEnabled = true;
                fsPlayerTextBox.IsEnabled = true;
                scPlayerButton.IsEnabled = true;
                scPlayerTextBox.IsEnabled = true;
                thPlayerButton.IsEnabled = true;
                thPlayerTextBox.IsEnabled = true;
                foPlayerButton.IsEnabled = false;
                foPlayerTextBox.IsEnabled = false;
                plCt = 3;
            }
            else if (playerCountComboBox.SelectedIndex == 2)
            {
                fsPlayerButton.IsEnabled = true;
                fsPlayerTextBox.IsEnabled = true;
                scPlayerButton.IsEnabled = true;
                scPlayerTextBox.IsEnabled = true;
                thPlayerButton.IsEnabled = true;
                thPlayerTextBox.IsEnabled = true;
                foPlayerButton.IsEnabled = true;
                foPlayerTextBox.IsEnabled = true;
                plCt = 4;
            }
            else
            {
                fsPlayerButton.IsEnabled = false;
                fsPlayerTextBox.IsEnabled = false;
                scPlayerButton.IsEnabled = false;
                scPlayerTextBox.IsEnabled = false;
                thPlayerButton.IsEnabled = false;
                thPlayerTextBox.IsEnabled = false;
                foPlayerButton.IsEnabled = false;
                foPlayerTextBox.IsEnabled = false;
            }
            
        }
    }
}
