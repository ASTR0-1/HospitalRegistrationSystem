using System;

namespace HospitalRegistrationSystem.Domain.Shared.ResultPattern;

/// <summary>
///     Represents the result of an operation.
/// </summary>
public class Result
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Result" /> class.
    /// </summary>
    /// <param name="isSuccess">A boolean indicating whether the result is a success or failure.</param>
    /// <param name="error">An instance of the <see cref="Error" /> class representing the error if the result is a failure.</param>
    protected Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None)
            || (!isSuccess && error == Error.None))
            throw new ArgumentException("Invalid error", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    ///     Gets a value indicating whether the operation is successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    ///     Gets a value indicating whether the operation is a failure.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    ///     Gets the error associated with the result.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    ///     Creates a new instance of <see cref="Result" /> representing a successful result.
    /// </summary>
    /// <returns>A new instance of <see cref="Result" /> representing a successful result.</returns>
    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    /// <summary>
    ///     Creates a new instance of <see cref="Result" /> representing a failure result with the provided error.
    /// </summary>
    /// <param name="error">An instance of the <see cref="Error" /> class representing the error.</param>
    /// <returns>A new instance of <see cref="Result" /> representing a failure result.</returns>
    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }
}