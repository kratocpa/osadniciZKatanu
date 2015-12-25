using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulator
{
    public class TooManyMovesException : Exception
    {
        public TooManyMovesException()
        {
        }

        public TooManyMovesException(string message)
            : base(message)
        {
        }
    }

    public class TooManyRoundsException : Exception
    {
        public TooManyRoundsException()
        {
        }

        public TooManyRoundsException(string message)
            : base(message)
        {
        }
    }

    public class WrongNumberOfPlayersException : Exception
    {
        public WrongNumberOfPlayersException()
        {
        }

        public WrongNumberOfPlayersException(string message)
            : base(message)
        {
        }
    }
}
