namespace DigitalWallet.Application.Base
{
    public class ServiceResult
    {
        public bool IsSuccess => Status == ResultStatus.Ok;

        public int? ErrorCode { get; private set; }

        public ResultStatus Status { get; private set; }

        public static ServiceResult Ok()
        {
            return new ServiceResult
            {
                Status = ResultStatus.Ok,
            };
        }

        public static ServiceResult Error(int code)
        {
            return new ServiceResult
            {
                Status = ResultStatus.ClientError,
                ErrorCode = code
            };
        }

        public static ServiceResult NotFound()
        {
            return new ServiceResult
            {
                Status = ResultStatus.NotFound
            };
        }
    }

    public class ServiceResult<T>
    {
        public bool IsSuccess => Status == ResultStatus.Ok;

        public T Value { get; private set; }

        public int? ErrorCode { get; private set; }

        public ResultStatus Status { get; private set; }

        public static ServiceResult<T> Ok(T value)
        {
            return new ServiceResult<T>
            {
                Status = ResultStatus.Ok,
                Value = value
            };
        }

        public static implicit operator ServiceResult<T>(T result)
        {
            return Ok(result);
        }

        public static ServiceResult<T> Error(int code)
        {
            return new ServiceResult<T>
            {
                Status = ResultStatus.ClientError,
                ErrorCode = code
            };
        }

        public static ServiceResult<T> NotFound()
        {
            return new ServiceResult<T>
            {
                Status = ResultStatus.NotFound
            };
        }
    }
}
