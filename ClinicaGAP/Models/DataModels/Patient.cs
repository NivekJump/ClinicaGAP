using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaGAP.Models.DataModels
{
    public enum BloodType
    {
        [Display(Name = "O+")]
        OPositive,
        [Display(Name = "A+")]
        APositive,
        [Display(Name = "B+")]
        BPositive,
        [Display(Name = "AB+")]
        ABPositive,
        [Display(Name = "AB-")]
        ABNegative,
        [Display(Name = "A-")]
        ANegative,
        [Display(Name = "B-")]
        BNegative,
        [Display(Name = "O-")]
        ONegative
    }

    public class Patient
    {
        public int PatientId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Second Last Name")]
        public string SecondLastName { get; set; }
        [Display(Name = "Document ID")]
        public string DocumentID { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "BloodType")]
        public BloodType BloodType { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
