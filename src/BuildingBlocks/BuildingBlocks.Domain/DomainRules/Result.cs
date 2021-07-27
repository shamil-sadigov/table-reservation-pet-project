#region

using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using MoreLinq.Extensions;

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
        
        public Result<T> WithValue<T>(T? response)
        {
            return Succeeded
                ? Result<T>.Success(response!)
                : Result<T>.Failure(Errors!);
        }
        
        public Result<T> WithoutValue<T>() 
            => Result<T>.Failure(Errors!);
        
        public Result CombineWith(params Result[] otherResults)
        {
            var errors = new List<Error>();
            
            var allResultAreSuccessful = otherResults.Aggregate(seed: true, (isSuccessful, result) =>
            {
                if (result.Failed)
                    errors.AddRange(result.Errors!);

                return isSuccessful && result.Succeeded;
            });

            return allResultAreSuccessful ? Success() : Failure(errors);
        }
        

        public static implicit operator Result(Error error) => Failure(error);
        public static implicit operator Result(List<Error> errors) => Failure(errors);
        
    }


    public class Result<T> : Result
    {
        /// <summary>
        /// Has value when Succeeded is true
        /// </summary>
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
        public static implicit operator Result<T>(List<Error> errors) => Failure(errors);
        public static implicit operator Result<T>(T value) => Success(value);

    }
}