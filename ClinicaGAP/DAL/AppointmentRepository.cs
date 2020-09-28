using ClinicaGAP.Models.DataModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace ClinicaGAP.DAL
{
    public class AppointmentRespository : IAppointmentRepository, IDisposable
    {
        private ClinicContext context;

        public AppointmentRespository(ClinicContext context)
        {
            this.context = context;
        }

        public IEnumerable<Appointment> GetAppointments()
        {
            return context.Appointments.Include("Patient").Include("User").ToList();
        }

        public Appointment GetAppointmentByID(int id)
        {
            return context.Appointments.Include("Patient").Include("User").Where(a => a.AppointmentId == id).FirstOrDefault();
        }

        public void InsertAppointment(Appointment appointment)
        {
            context.Appointments.Add(appointment);
        }

        public void DeleteAppointment(int appointmentID)
        {
            Appointment appointment = context.Appointments.Find(appointmentID);
            context.Appointments.Remove(appointment);
        }

        public void UpdateAppointment(Appointment appointment)
        {
            context.Entry(appointment).State = EntityState.Modified;
        }

        public bool ValidForInsert(DateTime desiredDate)
        {
            if (context.Appointments.Where(a => DbFunctions.TruncateTime(a.AppointmentDate) == desiredDate.Date).FirstOrDefault() != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidForUpdate(DateTime desiredDate)
        {
            if (context.Appointments.Where(a => DbFunctions.TruncateTime(a.AppointmentDate) == desiredDate.Date).FirstOrDefault() != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidForDelete(Appointment desiredAppointment)
        {
            Appointment deletedAppointment = GetAppointmentByID(desiredAppointment.AppointmentId);
            double timeSpan = (deletedAppointment.AppointmentDate - DateTime.Now.AddDays(1)).TotalDays;
            if (timeSpan <= 24)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
