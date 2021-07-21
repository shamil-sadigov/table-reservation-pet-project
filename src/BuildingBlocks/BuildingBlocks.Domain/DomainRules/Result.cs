#region

using System;
using System.Collections.Generic;

#endregion

namespace BuildingBlocks.Domain.DomainRules
{
    public class Result
    {
        protected Result()
        {
        }

        public bool Succeeded { get; protected init; }

        public bool Failed => !Succeeded;

        public IReadOnlyCollection<Error>? Errors { get; protected init; }

        public static Result Success()
        {
            return new()
            {
                Succeeded = true
            };
        }

        public static Result Failure(Error error)
        {
            return new()
            {
                Errors = new[] {error}
            };
        }

        public static Result Failure(IReadOnlyCollection<Error> errors)
        {
            return new()
            {
                Errors = errors
            };
        }

        public Result<T> WithResponse<T>(T response)
        {
            return Succeeded
                ? Result<T>.Success(response)
                : Result<T>.Failure(Errors!);
        }

        public static implicit operator Result(Error error) => Failure(error);
    }


    public class Result<T> : Result
    {
        public T? Value { get; private init; }

        public static Result<T> Success(T response)
        {
            if (response is null) 
                throw new ArgumentNullException(nameof(response));

            return new()
            {
                Succeeded = true,
                Value = response
            };
        }

        public new static Result<T> Failure(Error error)
        {
            return new()
            {
                Errors = new[] {error}
            };
        }

        public new static Result<T> Failure(IReadOnlyCollection<Error> errors)
        {
            return new()
            {
                Errors = errors
            };
        }
        
        public static implicit operator Result<T>(Error error) => Failure(error);
        public static implicit operator Result<T>(T value) => Success(value);

    }
}