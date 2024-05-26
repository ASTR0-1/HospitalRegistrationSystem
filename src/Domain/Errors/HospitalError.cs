﻿using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public class HospitalError
{
    public static Error HospitalIdNotFound(int hospitalId) => new($"Hospital {hospitalId} not found.", $"The hospital with ID '{hospitalId}' does not exist.");

    public static Error HospitalFeePercentInvalid(string name, decimal hospitalFeePercent) => new ($"Hospital fee percent is invalid for hospital '{name}'.", $"The hospital fee percent '{hospitalFeePercent}' for hospital '{name}' is invalid.");
}
