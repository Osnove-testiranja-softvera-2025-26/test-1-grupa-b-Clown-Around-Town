using System;

namespace OTS2026_GrupaB.Exceptions
{
    public class PositionOutsideOfMapException: Exception
    {
        public PositionOutsideOfMapException() 
        {

        }

        public PositionOutsideOfMapException(string message) : base(message)
        {

        }
    }
}
