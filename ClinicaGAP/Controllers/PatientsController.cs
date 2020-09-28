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
using PagedList;

namespace ClinicaGAP.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private IPatientRepository patientRepository;

        public PatientsController()
        {
            this.patientRepository = new PatientRespository(new ClinicContext());
        }

        // GET: Patients
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
            ViewBag.MiddleNameSortParm = String.IsNullOrEmpty(sortOrder) ? "middle_name_desc" : "";
            ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";
            ViewBag.SecondLastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "second_last_name_desc" : "";
            ViewBag.DocumentIDSortParm = String.IsNullOrEmpty(sortOrder) ? "document_id_desc" : "";
            ViewBag.EmailSortParm = String.IsNullOrEmpty(sortOrder) ? "email_desc" : "";
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

            var patients = from s in patientRepository.GetPatients() select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString)
                                       || s.DocumentID.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "first_name_desc":
                    patients = patients.OrderByDescending(s => s.FirstName);
                    break;
                case "middle_name_desc":
                    patients = patients.OrderByDescending(s => s.MiddleName);
                    break;
                case "last_name_desc":
                    patients = patients.OrderByDescending(s => s.LastName);
                    break;
                case "second_last_name_desc":
                    patients = patients.OrderByDescending(s => s.SecondLastName);
                    break;
                case "document_id_desc":
                    patients = patients.OrderByDescending(s => s.DocumentID);
                    break;
                case "email_desc":
                    patients = patients.OrderByDescending(s => s.Email);
                    break;
                case "Date":
                    patients = patients.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    patients = patients.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    patients = patients.OrderBy(s => s.FirstName);
                    break;
            }
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(patients.ToPagedList(pageNumber, pageSize));
        }

        // GET: Patients/Details/X
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = patientRepository.GetPatientByID(id.Value);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patientRepository.InsertPatient(patient);
                patientRepository.Save();
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        // GET: Patients/Edit/X
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = patientRepository.GetPatientByID(id.Value);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/X
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patientRepository.UpdatePatient(patient);
                patientRepository.Save();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = patientRepository.GetPatientByID(id.Value);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            patientRepository.DeletePatient(id);
            patientRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                patientRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
