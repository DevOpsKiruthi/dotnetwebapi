using System;

namespace dotnetapp.Exceptions
{
    public class FilmFestivalException : Exception
    {
        public FilmFestivalException(string message) : base(message)
        {
        }
    }
}
