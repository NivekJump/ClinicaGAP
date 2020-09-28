using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaGAP.DAL;
using ClinicaGAP.Models.DataModels;
using ClinicaGAP.Models.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;

namespace ClinicaGAP.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private IAppointmentRepository appointmentRepository;
        private IPatientRepository patientRepository;

        public AppointmentsController()
        {
            this.appointmentRepository = new AppointmentRespository(new ClinicContext());
            this.patientRepository = new PatientRespository(new ClinicContext());
        }

        // GET: Appointments
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
            ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";
            ViewBag.DocumentIDSortParm = String.IsNullOrEmpty(sortOrder) ? "document_id_desc" : "";
            ViewBag.EmailSortParm = String.IsNullOrEmpty(sortOrder) ? "appointment_type_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var appointments = from s in appointmentRepository.GetAppointments() select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                appointments = appointments.Where(a => a.Patient.LastName.Contains(searchString)
                                       || a.Patient.FirstName.Contains(searchString)
                                       || a.Patient.DocumentID.Contains(searchString)
                                       || a.AppointmentType.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "first_name_desc":
                    appointments = appointments.OrderByDescending(a => a.Patient.FirstName);
                    break;
                case "last_name_desc":
                    appointments = appointments.OrderByDescending(a => a.Patient.LastName);
                    break;
                case "document_id_desc":
                    appointments = appointments.OrderByDescending(a => a.Patient.DocumentID);
                    break;
                case "appointment_type_desc":
                    appointments = appointments.OrderByDescending(a => a.AppointmentType.ToString());
                    break;
                case "Date":
                    appointments = appointments.OrderBy(a => a.AppointmentDate);
                    break;
                case "date_desc":
                    appointments = appointments.OrderByDescending(a => a.AppointmentDate);
                    break;
                default:
                    appointments = appointments.OrderBy(a => a.Patient.FirstName);
                    break;
            }
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(appointments.ToPagedList(pageNumber, pageSize));
        }

        // GET: Appointments/Details/X
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment Appointment = appointmentRepository.GetAppointmentByID(id.Value);
            if (Appointment == null)
            {
                return HttpNotFound();
            }
            return View(Appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment cratedAppointment)
        {
            Appointment newAppointment = new Appointment();
            if (appointmentRepository.ValidForInsert(cratedAppointment.AppointmentDate))
            {
                Patient assignedPatient = patientRepository.GetPatientByDocument(cratedAppointment.Patient.DocumentID);
                if (assignedPatient == null)
                {
                    ModelState.AddModelError("Patient.DocumentID", "There is no patient with that document");
                }
                if (ModelState.IsValid)
                {
                    newAppointment.AppointmentDate = cratedAppointment.AppointmentDate;
                    newAppointment.AppointmentType = cratedAppointment.AppointmentType;
                    newAppointment.PatientId = assignedPatient.PatientId;
                    newAppointment.UserId = User.Identity.GetUserId();
                    appointmentRepository.InsertAppointment(newAppointment);
                    appointmentRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            else 
            {
                ModelState.AddModelError("AppointmentDate", "Is not possible to create the appointment, the patient has already an appointment for that day.");
            }
            return View(newAppointment);
        }

        // GET: Appointments/Edit/X
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment Appointment = appointmentRepository.GetAppointmentByID(id.Value);
            if (Appointment == null)
            {
                return HttpNotFound();
            }
            return View(Appointment);
        }

        // POST: Appointments/Edit/X
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Appointment editedAppointment)
        {
            Appointment appointmentToEdit = appointmentRepository.GetAppointmentByID(editedAppointment.AppointmentId);
            if (appointmentRepository.ValidForUpdate(editedAppointment.AppointmentDate))
            {
                Patient editedPatient = patientRepository.GetPatientByDocument(editedAppointment.Patient.DocumentID);
                if (editedPatient == null)
                {
                    ModelState.AddModelError("Patient.DocumentID", "There is no patient with that document");
                }
                if (ModelState.IsValid)
                {
                    appointmentToEdit.AppointmentDate = editedAppointment.AppointmentDate;
                    appointmentToEdit.AppointmentType = editedAppointment.AppointmentType;
                    appointmentToEdit.PatientId = editedPatient.PatientId;
                    appointmentToEdit.UserId = User.Identity.GetUserId();
                    appointmentRepository.UpdateAppointment(appointmentToEdit);
                    appointmentRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            else 
            {
                ModelState.AddModelError("AppointmentDate", "Is not possible to update the appointment, the patient has already an appointment for that day.");
            }
            return View(appointmentToEdit);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment Appointment = appointmentRepository.GetAppointmentByID(id.Value);
            if (Appointment == null)
            {
                return HttpNotFound();
            }
            return View(Appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = appointmentRepository.GetAppointmentByID(id);
            if (appointmentRepository.ValidForDelete(appointment))
            {
                appointmentRepository.DeleteAppointment(id);
                appointmentRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("AppointmentDate", "Is not possible to cancel the appointment, you must cancel with less than 24 hours");
                return View(appointment);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                appointmentRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
