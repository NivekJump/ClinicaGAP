namespace ClinicaGAP.Migrations
{
    using ClinicaGAP.DAL;
    using ClinicaGAP.Models.DataModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Cryptography;

    internal sealed class Configuration : DbMigrationsConfiguration<ClinicaGAP.DAL.ClinicContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ClinicaGAP.DAL.ClinicContext";
        }

        protected override void Seed(ClinicaGAP.DAL.ClinicContext context)
        {
            SeedUsers(context);
            List<int> newPatientsIds = SeedPatients(context);
            List<string> newUsersIds = SeedUsers(context);
            SeedAppointments(context, newPatientsIds, newUsersIds);
        }

        private List<string> SeedUsers(ClinicContext context)
        {
            List<(User user, String password)> seedUsers = new List<(User, string)>();
            seedUsers.Add(new(new User { UserName = "Nivekjump@gmail.com", Email = "Nivekjump@gmail.com" }, "GAP123!"));
            seedUsers.Add(new(new User { UserName = "Lulu@gmail.com", Email = "Lulu@gmail.com" }, "GAP123!"));
            seedUsers.Add(new(new User { UserName = "Gabriela@gmail.com", Email = "Gabriela@gmail.com" }, "GAP123!"));
            seedUsers.Add(new(new User { UserName = "GAP1@gmail.com", Email = "GAP1@gmail.com" }, "GAP123!"));
            seedUsers.Add(new(new User { UserName = "GAP2@gmail.com", Email = "GAP2@gmail.com" }, "GAP123!"));

            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

            foreach ((User, String) tuple in seedUsers)
            {
                userManager.Create(tuple.Item1, tuple.Item2);
            }

            context.SaveChanges();

            return context.Users.Select(sU => sU.Id).ToList();
        }

        public List<int> SeedPatients(ClinicContext context)
        {
            Random random = new Random();
            List<Patient> seedPatients = new List<Patient>();

            seedPatients.Add(new Patient { FirstName = "Pedro", MiddleName = "Paricio", LastName = "Panzon", SecondLastName = "Picapiedra", DocumentID = "987654321", Email = "PPanzon@gmail.com", BloodType = (BloodType)random.Next(0, Enum.GetNames(typeof(BloodType)).Length), EnrollmentDate = DateTime.Now });
            seedPatients.Add(new Patient { FirstName = "Juliana", MiddleName = "Solaris", LastName = "Nueva", SecondLastName = "Dehli", DocumentID = "11223344556", Email = "JD@gmail.com", BloodType = (BloodType)random.Next(0, Enum.GetNames(typeof(BloodType)).Length), EnrollmentDate = DateTime.Now });
            seedPatients.Add(new Patient { FirstName = "Pablo", MiddleName = "Clavo", LastName = "Clavito", SecondLastName = "Martillo", DocumentID = "99887766554", Email = "PClavito@gmail.com", BloodType = (BloodType)random.Next(0, Enum.GetNames(typeof(BloodType)).Length), EnrollmentDate = DateTime.Now });
            seedPatients.Add(new Patient { FirstName = "Kenny", MiddleName = "Bell", LastName = "Maribelle", SecondLastName = "Lit", DocumentID = "123987654", Email = "KB@gmail.com", BloodType = (BloodType)random.Next(0, Enum.GetNames(typeof(BloodType)).Length), EnrollmentDate = DateTime.Now });
            seedPatients.Add(new Patient { FirstName = "Diego", MiddleName = "Tortulio", LastName = "Parra", SecondLastName = "Ayala", DocumentID = "558822334", Email = "DTPA@gmail.com", BloodType = (BloodType)random.Next(0, Enum.GetNames(typeof(BloodType)).Length), EnrollmentDate = DateTime.Now });

            context.Patients.AddRange(seedPatients);

            context.SaveChanges();

            return context.Patients.Select(p => p.PatientId).ToList();
        }

        public void SeedAppointments(ClinicContext context, List<int> newPatients, List<string> newUsers)
        {
            Random random = new Random();
            List<Appointment> seedAppointments = new List<Appointment>();

            seedAppointments.Add(new Appointment { AppointmentDate = DateTime.Now.AddDays(1), AppointmentType = (AppointmentType)random.Next(0, Enum.GetNames(typeof(AppointmentType)).Length), PatientId = newPatients[random.Next(0, newPatients.Count)], UserId = newUsers[random.Next(0, newUsers.Count)] });
            seedAppointments.Add(new Appointment { AppointmentDate = DateTime.Now.AddDays(3), AppointmentType = (AppointmentType)random.Next(0, Enum.GetNames(typeof(AppointmentType)).Length), PatientId = newPatients[random.Next(0, newPatients.Count)], UserId = newUsers[random.Next(0, newUsers.Count)] });
            seedAppointments.Add(new Appointment { AppointmentDate = DateTime.Now.AddDays(5), AppointmentType = (AppointmentType)random.Next(0, Enum.GetNames(typeof(AppointmentType)).Length), PatientId = newPatients[random.Next(0, newPatients.Count)], UserId = newUsers[random.Next(0, newUsers.Count)] });
            seedAppointments.Add(new Appointment { AppointmentDate = DateTime.Now.AddDays(7), AppointmentType = (AppointmentType)random.Next(0, Enum.GetNames(typeof(AppointmentType)).Length), PatientId = newPatients[random.Next(0, newPatients.Count)], UserId = newUsers[random.Next(0, newUsers.Count)] });
            seedAppointments.Add(new Appointment { AppointmentDate = DateTime.Now.AddDays(11), AppointmentType = (AppointmentType)random.Next(0, Enum.GetNames(typeof(AppointmentType)).Length), PatientId = newPatients[random.Next(0, newPatients.Count)], UserId = newUsers[random.Next(0, newUsers.Count)] });

            context.Appointments.AddRange(seedAppointments);

            context.SaveChanges();
        }
    }
}
