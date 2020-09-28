using System;
using System.Collections.Generic;
using ClinicaGAP.Models.DataModels;

namespace ClinicaGAP.DAL
{
    public interface IPatientRepository : IDisposable
    {
        IEnumerable<Patient> GetPatients();
        Patient GetPatientByID(int patientId);
        Patient GetPatientByDocument(string document);
        void InsertPatient(Patient patient);
        void DeletePatient(int patientId);
        void UpdatePatient(Patient patient);
        void Save();
    }
}
