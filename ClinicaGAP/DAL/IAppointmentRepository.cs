using System;
using System.Collections.Generic;
using ClinicaGAP.Models.DataModels;

namespace ClinicaGAP.DAL
{
    public interface IAppointmentRepository : IDisposable
    {
        IEnumerable<Appointment> GetAppointments();
        Appointment GetAppointmentByID(int appointmentId);
        void InsertAppointment(Appointment appointment);
        void DeleteAppointment(int studentID);
        void UpdateAppointment(Appointment appointment);
        bool ValidForInsert(DateTime desiredDate);
        bool ValidForUpdate(DateTime desiredDate);
        bool ValidForDelete(Appointment desiredAppointment);
        void Save();
    }
}
