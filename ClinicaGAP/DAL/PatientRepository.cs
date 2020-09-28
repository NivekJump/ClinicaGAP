using ClinicaGAP.Models.DataModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace ClinicaGAP.DAL
{
    public class PatientRespository : IPatientRepository, IDisposable
    {
        private ClinicContext context;

        public PatientRespository(ClinicContext context)
        {
            this.context = context;
        }

        public IEnumerable<Patient> GetPatients()
        {
            return context.Patients.ToList();
        }

        public Patient GetPatientByID(int id)
        {
            return context.Patients.Find(id);
        }

        public Patient GetPatientByDocument(string document) 
        {
            return context.Patients.Where(p => p.DocumentID.Contains(document)).FirstOrDefault();
        }

        public void InsertPatient(Patient patient)
        {
            context.Patients.Add(patient);
        }

        public void DeletePatient(int patientID)
        {
            Patient patient = context.Patients.Find(patientID);
            context.Patients.Remove(patient);
        }

        public void UpdatePatient(Patient patient)
        {
            context.Entry(patient).State = EntityState.Modified;
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
