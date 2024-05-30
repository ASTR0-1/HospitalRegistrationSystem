﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Application.DTOs.FeedbackDTOs;

public class FeedbackDto
{
    public decimal Rating { get; set; }
    public string Text { get; set; }

    public int AppointmentId { get; set; }
    public DateTime FeedbackDate { get; set; }
}