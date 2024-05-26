using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Application.DTOs.FeedbackDTOs;
public class FeedbackForCreationDto
{
    public decimal Rating { get; set; }
    public string Text { get; set; }
    public int AppointmentId { get; set; }
}
