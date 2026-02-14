using ClassUP.ApplicationCore.Exeptions;

namespace ClassUP.ApplicationCore.Exceptions
{
    public class IdentityOperationException : ApplicationExceptionBase
    {
        public IEnumerable<string> Errors { get; }

        public IdentityOperationException(IEnumerable<string> errors)
            : base(string.Join(", ", errors))
        {
            Errors = errors;
        }
    }
}
