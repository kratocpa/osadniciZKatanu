using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class CantDeleteMaterialsException : Exception
    {
        public CantDeleteMaterialsException() { }
        public CantDeleteMaterialsException(string message) : base(message) { }
    }

    public class CantDeleteActionCardException : Exception
    {
        public CantDeleteActionCardException() { }
        public CantDeleteActionCardException(string message) : base(message) { }
    }

    public class NoRoadLeftException : Exception
    {
        public NoRoadLeftException() { }
        public NoRoadLeftException(string message) : base(message) { }
    }

    public class NoVillageLeftException : Exception
    {
        public NoVillageLeftException() { }
        public NoVillageLeftException(string message) : base(message) { }
    }

    public class NoTownLeftException : Exception
    {
        public NoTownLeftException() { }
        public NoTownLeftException(string message) : base(message) { }
    }

    public class WrongCoordinateException : Exception
    {
        public WrongCoordinateException() { }
        public WrongCoordinateException(string message) : base(message) { }
    }

    public class BuildingCollisionException : Exception
    {
        public BuildingCollisionException() { }
        public BuildingCollisionException(string message) : base(message) { }
    }

    public class WrongLocationForBuildingException : Exception
    {
        public WrongLocationForBuildingException() { }
        public WrongLocationForBuildingException(string message) : base(message) { }
    }

    public class WrongPlayerToRobbedException : Exception
    {
        public WrongPlayerToRobbedException() { }
        public WrongPlayerToRobbedException(string message) : base(message) { }
    }

    public class NoMaterialException : Exception
    {
        public NoMaterialException() { }
        public NoMaterialException(string message) : base(message) { }
    }

    public class NoActionCardException : Exception
    {
        public NoActionCardException() { }
        public NoActionCardException(string message) : base(message) { }
    }

    public class TooMuchActionsException : Exception
    {
        public TooMuchActionsException() { }
        public TooMuchActionsException(string message) : base(message) { }
    }

    public class IncorectMoveException : Exception
    {
        public IncorectMoveException() { }
        public IncorectMoveException(string message) : base(message) { }
    }
}
