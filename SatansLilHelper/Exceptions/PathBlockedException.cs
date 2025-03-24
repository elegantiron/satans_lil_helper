using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatansLilHelper.Exceptions;

public class PathBlockedException : Exception
{
    public PathBlockedException()
        : base() { }

    public PathBlockedException(string message)
        : base(message) { }
}
