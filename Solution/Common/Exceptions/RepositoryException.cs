
namespace Common.Exceptions
{
    public class RepositoryException: Exception
    {
        public RepositoryException(string errorMessage): base(errorMessage) {}
    }
}
