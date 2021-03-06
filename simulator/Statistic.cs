﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;
using osadniciZKatanuAI;

namespace simulator
{
    public class Statistics
    {
        public const int PROGRESS_BAR_LENGTH = 50;

        int maxNumRound; // největší počet kol za hru
        int minNumRound; // nejmenší počet kol za hru
        int avrNumRound; // průměrný počet kol za hru
        int numGames; // počet her
        int actualGame; // statistika kolikáté hry se právě přidává
        int unfinishedGames; // počet nedokončených her (simulátor vyhodil výjimku buďto na moc tahů za kolo nebo na moc kol)
        public int RedWins { get; private set; } // počet hry kdy vyhrál červený
        public int BlueWins { get; private set; } // počet her kdy vyhrál modrý
        public int YellowWins { get; private set; } // počet her kdy vyhrál žlutý
        public int WhiteWins { get; private set; } // počet her kdy vyhrál bílý
        ILanguage curLang; // jazyk statistiky
        bool viewProgressBar;

        public Statistics(ILanguage curLang, int numGames, bool viewProgressBar)
        {
            this.curLang = curLang;
            maxNumRound = int.MinValue;
            minNumRound = int.MaxValue;
            this.numGames = numGames;
            actualGame = 0;
            avrNumRound = 0;
            unfinishedGames = 0;
            RedWins = 0;
            BlueWins = 0;
            YellowWins = 0;
            WhiteWins = 0;
            this.viewProgressBar = viewProgressBar;
            if (viewProgressBar)
            {
                ViewProgressBarPreparation();
            }
        }

        public void AddToStatistic()
        {
            unfinishedGames++;
            actualGame++;
            if (viewProgressBar)
            {
                ViewProgressBar();
            }
        }

        public void AddToStatistic(Game result)
        {
            if (result.GmProp.Round > maxNumRound) { maxNumRound = result.GmProp.Round; }
            if (result.GmProp.Round < minNumRound) { minNumRound = result.GmProp.Round; }
            avrNumRound = avrNumRound + result.GmProp.Round;
            actualGame++;

            if (result.ActualPlayer.PlProp.Points >= 10)
            {
                switch (result.ActualPlayer.PlProp.Color)
                {
                    case Game.color.red: RedWins++; break;
                    case Game.color.blue: BlueWins++; break;
                    case Game.color.yellow: YellowWins++; break;
                    case Game.color.white: WhiteWins++; break;
                }
            }

            if (viewProgressBar)
            {
                ViewProgressBar();
            }
        }

        public void PrintOverallStatistics(System.IO.TextWriter output)
        {
            output.WriteLine();
            output.WriteLine("Max kol: " + maxNumRound + ", min kol: " + minNumRound + ", prumerne kol: " + avrNumRound / numGames + ", nedohrane hry: " + unfinishedGames);
            output.WriteLine("Cerveny vyhral: " + RedWins + ", Modry vyhral: " + BlueWins + ", Zluty vyhral: " + YellowWins + ", Bily vyhral: " + WhiteWins);
        }

        public string GetOverallStatistic()
        {
            string res = "Max kol: " + maxNumRound + ", min kol: " + minNumRound + ", prumerne kol: " + avrNumRound / numGames + ", nedohrane hry: " + unfinishedGames;
            res+="\n";
            res += "Cerveny vyhral: " + RedWins + ", Modry vyhral: " + BlueWins + ", Zluty vyhral: " + YellowWins + ", Bily vyhral: " + WhiteWins;
            return res;
        }

        public void PrintGameResult(Game result, System.IO.TextWriter output)
        {
            output.WriteLine("počet kol: " + result.GmProp.Round);
            output.WriteLine("pořadí: ");
            foreach (Player plDes in result.Players)
            {
                output.WriteLine(curLang.ColorToString(plDes.PlProp.Color) + ": body " + plDes.PlProp.Points + ", rytíři " + plDes.PlProp.Knights);
            }
        }

        void ViewProgressBarPreparation()
        {
            Console.Write("|");
            for (int j = 0; j < PROGRESS_BAR_LENGTH; j++)
            {
                Console.Write(" ");
            }
            Console.Write("|");
        }

        void ViewProgressBar()
        {
            int tmpLeft, tmpTop;
            tmpLeft = Console.CursorLeft;
            tmpTop = Console.CursorTop;
            int ratio = numGames / PROGRESS_BAR_LENGTH;
            if (actualGame % ratio == 0)
            {
                Console.SetCursorPosition(actualGame / ratio , 0);
                Console.Write("=");
            }
            Console.SetCursorPosition(PROGRESS_BAR_LENGTH+2, 0);
            Console.Write(actualGame);
            Console.SetCursorPosition(tmpLeft, tmpTop);
        }

    }
}
