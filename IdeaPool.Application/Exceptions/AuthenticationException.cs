namespace IdeaPool.Application.Exceptions
{
    public class AuthenticationException: AppException
    {
        public AuthenticationException(string message): base(message)
        {
        }
    }
}