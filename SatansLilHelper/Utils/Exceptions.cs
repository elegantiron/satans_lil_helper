using System;

namespace SatansLilHelper.Exceptions;

public class GameExitException : Exception { }

public class PathBlockedException : Exception
{
    public PathBlockedException()
        : base() { }

    public PathBlockedException(string message)
        : base(message) { }
}

public class MissingTargetException : Exception { }
