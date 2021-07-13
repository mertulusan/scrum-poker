using System;

namespace ScrumPoker.Exceptions
{
    public class PredictionException : Exception
    {
        public PredictionException() { }

        public PredictionException(string message) : base(message) { }
    }
}
