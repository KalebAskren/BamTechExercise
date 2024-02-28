namespace StargateAPI.Business.Data.Helpers
{
    public interface IExceptionLoggingHelper
    {
        public void PersistException(Exception exception);
    }
}
