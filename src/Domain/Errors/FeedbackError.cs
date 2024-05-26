using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Domain.Shared.ResultPattern;

namespace HospitalRegistrationSystem.Domain.Errors;

public static class FeedbackError
{
    public static Error FeedbackOnNotVisitedAppointmentError(int appointmentId) => new("Feedback cannot be given for a not visited appointment.", $"Feedback cannot be given for a not visited appointment with ID '{appointmentId}'.");
}
