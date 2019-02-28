#region

using System;
using System.Globalization;

#endregion

namespace IdeaPool.Application.Exceptions
{
    public class AppException: Exception
    {
        public AppException(string message): base(message)
        {
        }

        public AppException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}