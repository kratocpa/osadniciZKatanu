using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Xml;
using System.Threading;
using evolution;
using osadniciZKatanuAI;

namespace evolutionGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void evaluate(int generationCount, int popSize, int gameCount, double xOverProb, double mutationProb, double genChangeProb, string fs, string sc, string th)
        {
            try
            {
                GenerateMovesProperties gmMovProp = new GenerateMovesProperties();
                int individumSize = gmMovProp.Parameters.Count;
                EvolutionAlgorithm eva;
                ISelector rouSel = new RouletteWheelSelector();
                IOperator mating = new OnePtXOver(xOverProb);
                IOperator mutation = new IntegerMutation(mutationProb, genChangeProb);

                Population parents = new Population(individumSize, 0, 300);
                Population offspring;
                IFitnessEvaluator fitEval = new OneStrategyEvaluator(fs, sc, th, gameCount, 4);
                parents.GenerateRandomPopulation(popSize);
                eva = new EvolutionAlgorithm(popSize);

                eva.matingSelectors = rouSel;
                eva.operators.Add(mating);
                eva.operators.Add(mutation);
                eva.eval = fitEval;

                string folderName="bestParam";
                System.IO.Directory.CreateDirectory(folderName);

                while (eva.generationNo < generationCount)
                {
                    fitEval.Evaluate(parents);
                    XmlDocument doc = new XmlDocument();
                    Printer.PrintBest(parents, doc, eva.generationNo, folderName);
                    progressBar.Dispatcher.BeginInvoke(new Action(() => { progressBar.Value = eva.generationNo; }));
                    //PrintPopulation(parents, evolutionPop, eva.generationNo);
                    offspring = eva.Evolve(parents);
                    parents = offspring;
                }
                startButton.Dispatcher.BeginInvoke(new Action(() => { startButton.IsEnabled = true; }));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
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

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            string fsPl = fsPlayerTextBox.Text;
            string scPl = scPlayerTextBox.Text;
            string thPl = thPlayerTextBox.Text;
            try
            {
                int genCount = Int32.Parse(genCountTextBox.Text);
                int popSize = Int32.Parse(popSizeTextBox.Text);
                int gmCountEval = Int32.Parse(gameCountEvalTextBox.Text);
                double mutProb = double.Parse(mutProbTextBox.Text, System.Globalization.CultureInfo.InvariantCulture);
                double genChangeProb = double.Parse(genChangeProbtextBox.Text, System.Globalization.CultureInfo.InvariantCulture);
                double matProb = double.Parse(matProbTextBox.Text, System.Globalization.CultureInfo.InvariantCulture);
                if (mutProb > 1 || genChangeProb > 1 || matProb > 1) { throw new Exception("Wrong probabilities value"); }
                if (mutProb < 0 || genChangeProb < 0 || matProb < 0) { throw new Exception("Wrong probabilities value"); }
                if (genCount < 0 || popSize < 0 || gmCountEval < 0) { throw new Exception("genCount, popSize and gmCountEval can't be negative"); }

                progressBar.Minimum = 0;
                progressBar.Maximum = genCount;
                progressBar.Value = 0;
                Thread t = new Thread(new ThreadStart(() => evaluate(genCount, popSize, gmCountEval, matProb, mutProb, genChangeProb, fsPl, scPl, thPl)));
                t.Start();
                startButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

    }
}
