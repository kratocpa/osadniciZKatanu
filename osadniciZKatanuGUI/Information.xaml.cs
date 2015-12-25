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
using System.Windows.Shapes;
using osadniciZKatanu;

namespace osadniciZKatanuGUI
{
    /// <summary>
    /// Interaction logic for Information.xaml
    /// </summary>
    public partial class Information : Window
    {
        public Information()
        {
            InitializeComponent();
        }

        public void AddPossibleMoves(Move possibleMoves, Game gm)
        {
            InfoTextBlock.Text += possibleMoves.MoveDescription(gm.CurLang);
            InfoTextBlock.Text += String.Format("\n fitness: {0}", possibleMoves.fitnessMove);
            InfoTextBlock.Text += "\n ----------------------------------------------  \n";
        }

        public void ClearInfoText()
        {
            InfoTextBlock.Text = "";
        }
    }
}
