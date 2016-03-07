using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using simulator;
using osadniciZKatanu;
using osadniciZKatanuAI;

namespace evolution
{
    public class Common
    {
        //vytvoří pořadí čtyř hráčů na základě čísla hry (tak aby byl každý hráč spravedlivě rozdělen do pořadí)
        public static List<Player> SimulateFourPlayers(bool rotatePl, GameProperties gmProp, int gameNum)
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

        //vytvoří pořadí tří hráčů na základě čísla hry (tak aby byl každý hráč spravedlivě rozdělen do pořadí)
        public static List<Player> SimulateThreePlayers(bool rotatePl, GameProperties gmProp, int gameNum)
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

        //vytvoří pořadí dvou hráčů na základě čísla hry (tak aby byl každý hráč spravedlivě rozdělen do pořadí)
        public static List<Player> SimulateTwoPlayers(bool rotatePl, GameProperties gmProp, int gameNum)
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
    }
}
