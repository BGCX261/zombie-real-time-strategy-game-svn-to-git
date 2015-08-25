using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Exception
{
    /// <summary>
    /// This class will represent an exception that will be thrown by one of the "Factory" classes 
    /// (TileFactory, UnitFactory, ect.) when a factory encounters a fatal error.
    /// </summary>
    public class FactoryException : SystemException
    {
        public FactoryException() : base() { }

        public FactoryException(string message) : base(message) { }

        public FactoryException(string message, SystemException inner) : base(message, inner) { }
    }
}
