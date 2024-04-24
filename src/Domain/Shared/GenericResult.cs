using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Domain.Shared;

/// <summary>
///     Represents a generic result of the operation.
/// </summary>
/// <typeparam name="T">The type of the value that the result can hold.</typeparam>
public class Result<T> : Result where T : class
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    /// <param name="isSuccess">A boolean indicating whether the result is a success or failure.</param>
    /// <param name="error">An instance of the <see cref="Error"/> class representing the error if the result is a failure.</param>
    /// <param name="value">The value of type <typeparamref name="T"/> if the result is a success.</param>
    protected Result(bool isSuccess, Error error, T? value)
        : base(isSuccess, error)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the value associated with the result.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    ///     Creates a new instance of <see cref="Result{T}"/> representing a successful result with the provided value.
    /// </summary>
    /// <param name="value">The value of type <typeparamref name="T"/>.</param>
    /// <returns>A new instance of <see cref="Result{T}"/> representing a successful result.</returns>
    public static Result<T> Success(T value) => new(true, Error.None, value);

    /// <summary>
    ///     Creates a new instance of <see cref="Result{T}"/> representing a failure result with the provided error.
    /// </summary>
    /// <param name="error">An instance of the <see cref="Error"/> class representing the error.</param>
    /// <returns>A new instance of <see cref="Result{T}"/> representing a failure result.</returns>
    public static Result<T> Failure(Error error) => new(false, error, null);
}
