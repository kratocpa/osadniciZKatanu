using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    public interface IGameLogic
    {
        Move GenerateMove(GameProperties gmProp, PlayerProperties plProp);
        List<Move> GetAllPossibleMoves();
    }
}
