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
            GameProperties gmProp;
            int i = 0;
           
            while (i < rounds)
            {
                gmProp = new GameProperties(rndGmBr, new EngLanguage());

                List<Player> players = new List<Player>();
                players.Add(new Player(Game.color.red, false, gmProp));
                players.Add(new Player(Game.color.blue, false, gmProp));
                players.Add(new Player(Game.color.yellow, false, gmProp));
                players.Add(new Player(Game.color.white, false, gmProp));

                Simulator simul = new Simulator(players, gmProp);
                if (fsPl != "") { simul.firstPl = new MyGameLogic(fsPl); } else { simul.firstPl = new MyGameLogic(); }
                if (scPl != "") { simul.secondPl = new MyGameLogic(scPl); } else { simul.secondPl = new MyGameLogic(); }
                if (thPl != "") { simul.thirdPl = new MyGameLogic(thPl); } else { simul.thirdPl = new MyGameLogic(); }
                if (foPl != "") { simul.fourthPl = new MyGameLogic(foPl); } else { simul.fourthPl = new MyGameLogic(); }
                
                try
                {
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


                progressBar.Dispatcher.BeginInvoke(new Action(() => { progressBar.Value = i; }));

                i++;
            }
            
            resultStr = statistic.GetOverallStatistic();
            //System.Windows.MessageBox.Show(resultStr);

            resultLabel.Dispatcher.BeginInvoke(new Action(() => { resultLabel.Content = resultStr; }));
            startButton.Dispatcher.BeginInvoke(new Action(() => { startButton.IsEnabled = true; }));
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            

            fsPl = fsPlayerTextBox.Text;
            scPl = scPlayerTextBox.Text;
            thPl = thPlayerTextBox.Text;
            foPl = foPlayerTextBox.Text;
            rounds=Int32.Parse(roundTextBox.Text);
            rndGmBr=false;
            if (rndCheckBox.IsChecked == true) { rndGmBr = true; }
            progressBar.Minimum = 0;
            progressBar.Maximum = rounds;
            progressBar.Value = 0;
            Thread t = new Thread(new ThreadStart(simulate));
            t.Start();
            startButton.IsEnabled = false;
        }

    }
}
