namespace ExchangeRateUpdater.Domain
{
    //This classes would usually live in a shared Framework library that multiple teams can use 
    public class Result 
    {
        public Result(bool success, IEnumerable<string> failureResons)
        {
            Succsess = success;
            FailureResons = failureResons;
        }

        public bool Succsess { get; }
        public IEnumerable<string> FailureResons { get; }
        public static Result Combine(params Result[] results)
        {
            var failureResons = results.SelectMany(r => r.FailureResons);
            return failureResons.Any() ? Fail(failureResons) : OK();
        }

        private static Result OK() => new(true, new List<string>());
        private static Result Fail(IEnumerable<string> failureResons) => new(false, failureResons);

        public static Result<T> OK<T> (T? value) => new(value, true, new List<string>());
        public static Result<T> Fail<T> (IEnumerable<string> failureResons) => new(default, false, failureResons);
    }

    public class Result<T> : Result
    {
        private readonly T? _value;
        public T Value { get { return _value!; } }

        public Result(T? value, bool success, IEnumerable<string> failureResons) : base(success, failureResons)
        {
            _value = value;
        }
    }

    public static class ResultExtensions
    {
        public static Result<Type> ToResult<Type>(this Type? optionalValue, string failureReason) where Type : class
           => optionalValue is not null ? Result.OK(optionalValue) : Result.Fail<Type>(new List<string>() { failureReason });

        public static Result<Type> ToResult<Type>(this Type? optionalValue, string failureReason) where Type : struct
           => optionalValue.HasValue ? Result.OK(optionalValue.Value) : Result.Fail<Type>(new List<string>() { failureReason });

        public static Result<U> OnSuccess<Type, U>(this Result<Type> result, Func<Type, U> transformationFunc)
            => result.Succsess ? Result.OK(transformationFunc(result.Value)) : Result.Fail<U>(result.FailureResons);

        public static Result<Type> Ensure<Type>(this Result<Type> result, Func<Type, bool> validationFunc, string failureReason)
        {
            if (result.Succsess is false)
            {
                return result;
            }
            if (validationFunc(result.Value) is false)
            {
                return Result.Fail<Type>(new List<string>() { failureReason });
            }
            return result;
        }

        public static Result<TMapped> Map<T, TMapped>(this Result<T> result, Func<T, TMapped> mappingFunction)
            => result.Succsess ? Result.OK(mappingFunction(result.Value)) : Result.Fail<TMapped>(result.FailureResons);
    }
}
