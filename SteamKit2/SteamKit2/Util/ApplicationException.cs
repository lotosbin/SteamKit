#if !APPLICATIONEXCEPTION_AVAILABLE
namespace System
{
#if BINARY_SERIALIZATION_AVAILABLE
    [Serializable]
#endif
    public class ApplicationException : Exception
    {
        public ApplicationException() { }
        public ApplicationException(string message) : base(message) { }
        public ApplicationException(string message, Exception inner) : base(message, inner) { }

#if BINARY_SERIALIZATION_AVAILABLE
        protected ApplicationException(
          Runtime.Serialization.SerializationInfo info,
          Runtime.Serialization.StreamingContext context) : base(info, context) { }
#endif
    }
}
#endif