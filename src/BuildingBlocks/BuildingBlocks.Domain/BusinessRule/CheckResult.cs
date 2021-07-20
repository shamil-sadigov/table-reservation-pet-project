#region

using System;
using System.Collections.Generic;

#endregion

namespace BuildingBlocks.Domain.BusinessRule
{
    public class CheckResult
    {
        protected CheckResult()
        {
        }

        public bool Succeeded { get; protected init; }

        public bool Failed => !Succeeded;

        public IReadOnlyCollection<Error>? Errors { get; protected init; }

        public static CheckResult Success() => new()
            {
                Succeeded = true
            };

        public static CheckResult Failure(Error error) => 
            new()
            {
                Errors = new[] {error}
            };

        public static CheckResult Failure(IReadOnlyCollection<Error> errors) =>
            new()
            {
                Errors = errors
            };

        public CheckResult<T> WithResponse<T>(T response) =>
            Succeeded
                ? CheckResult<T>.Success(response)
                : CheckResult<T>.Failure(Errors!);
    }


    public class CheckResult<T> : CheckResult
    {
        public T? Response { get; private init; }

        public static CheckResult<T> Success(T response)
        {
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            
            return new()
            {
                Succeeded = true,
                Response = response
            };
        }

        public new static CheckResult<T> Failure(Error error)
        {
            return new()
            {
                Errors = new[] {error}
            };
        }

        public new static CheckResult<T> Failure(IReadOnlyCollection<Error> errors)
        {
            return new()
            {
                Errors = errors
            };
        }
    }
}