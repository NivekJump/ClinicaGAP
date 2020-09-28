using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicaGAP.Models.DataModels
{
    public enum AppointmentType
    {
        [Display(Name = "General")]
        General,
        [Display(Name = "Odontology")]
        Odontology,
        [Display(Name = "Pediatrics")]
        Pediatrics,
        [Display(Name = "Neurology")]
        Neurology
    }

    public class Appointment
    {
        public int AppointmentId { get; set; }
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }
        [Display(Name = "Appointment Type")]
        public AppointmentType AppointmentType { get; set; }

        [Display(Name = "Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [Display(Name = "Created By:")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
