using Individual.Data;
using Individual.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Individual.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var subjects = _context.Subject.ToList();
            return View(subjects);
        }
        [HttpGet]
        public IActionResult Index(string searchSubjectCode)
        {
            var findSubject = from x in _context.Subject
                              select x;

            if (!string.IsNullOrEmpty(searchSubjectCode))
            {
                findSubject = findSubject.Where(x => x.SUBJCODE != null && x.SUBJCODE.Contains(searchSubjectCode));
            }

            return View(findSubject.ToList());
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(Subject addRequestSubject)
        {
            try
            {
                string errorMsg = FieldValidation(addRequestSubject);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    TempData["FieldError"] = errorMsg;
                    return View("Add", addRequestSubject);
                }

                if (string.IsNullOrEmpty(addRequestSubject.SUBJPREQ))
                {
                    TempData["RequisiteError"] = "Subject Requisite Error: Requisite Information must not be empty.";
                    return View("Add", addRequestSubject);
                }

                var existingSubject = _context.Subject.FirstOrDefault(s =>
                    s.SUBJCODE == addRequestSubject.SUBJCODE && s.SUBJCOURSECODE == addRequestSubject.SUBJCOURSECODE);

                if (existingSubject != null)
                {
                    TempData["DuplicateError"] = "Duplicate Error: Subject with the same code and course code already exists.";
                    return View(addRequestSubject);
                }
                if (addRequestSubject.SUBJPREQ == "CO")
                {
                    addRequestSubject.SUBJCODEPREQ = "n/a";
                }

                if (addRequestSubject.SUBJPREQ == "PREQ" && string.IsNullOrEmpty(addRequestSubject.SUBJCODEPREQ))
                {
                    TempData["PreqError"] = "Requisite Information Error: Subject Pre-requisite Code is required.";
                    return View("Add", addRequestSubject);
                }

                if (addRequestSubject.SUBJPREQ == "PRE")
                {
                    if (string.IsNullOrEmpty(addRequestSubject.SUBJCODEPREQ))
                    {
                        TempData["PreqError"] = "Requisite Information Error: Subject Pre-requisite Code is required.";
                        return View("Add", addRequestSubject);
                    }

                    var matchingSubject = _context.Subject.FirstOrDefault(s =>
                        s.SUBJCODE == addRequestSubject.SUBJCODEPREQ && s.SUBJCOURSECODE == addRequestSubject.SUBJCOURSECODE);

                    if (matchingSubject == null)
                    {
                        TempData["MismatchError"] = "Requisite Mismatch Error: No Prerequisite subject available.";
                        return View("Add", addRequestSubject);
                    }
                }

                SaveData(addRequestSubject);
                TempData["SuccessMessage"] = "Subject added successfully.";
                return View();
            }
            catch (DbUpdateException ex)
            {
                TempData["SqlError"] = "An error occurred while saving the subject. Please try again later.";
                return View("Add", addRequestSubject);
            }
        }

        [HttpGet]
        public IActionResult Edit(string? subcode)
        {
            var subject = _context.Subject.Where(s => s.SUBJCODE == subcode).FirstOrDefault();

            return View(subject);
        }

        [HttpPost]
        public IActionResult Edit(Subject subjectEdit)
        {
            var subject = _context.Subject
                .Where(s => s.SUBJCODE == subjectEdit.SUBJCODE)
                .FirstOrDefault();


            if (subject != null)
            {
                _context.Subject.Remove(subject);
                _context.Add(subjectEdit);
                _context.SaveChanges();
                TempData["UpMessage"] = "Subject Entry Updated Successfully!";
                return RedirectToAction("Index");
            }
            return View(subjectEdit);

        }

        [HttpPost]
        public IActionResult Delete(string subjcode)
        {
            if (string.IsNullOrEmpty(subjcode))
            {
                return NotFound();
            }

            var subject = _context.Subject.FirstOrDefault(s => s.SUBJCODE == subjcode);

            if (subject == null)
            {
                return NotFound();
            }

            _context.Subject.Remove(subject);
            _context.SaveChanges();
            TempData["DelMessage"] = "Subject Entry Deleted Successfully!";
            return RedirectToAction("Index");
        }

        private string FieldValidation(Subject subject)
        {
            List<string> missingFields = new List<string>();
            string[] fieldNames = { "Subject Code", "Subject Description", "Units", "Requisite Information", "Requisite Subject Code", "Offering", "Category", "Course Code", "Curriculum Code" };

            if (string.IsNullOrEmpty(subject.SUBJCODE))
            {
                missingFields.Add(fieldNames[0]);
            }
            if (string.IsNullOrEmpty(subject.SUBJDESC))
            {
                missingFields.Add(fieldNames[1]);
            }
            if (!IsNumeric(subject.SUBJUNITS.ToString()))
            {
                missingFields.Add(fieldNames[2]);
            }
            if (subject.SUBJUNITS == 0)
            {
                missingFields.Add(fieldNames[2]);
            }
            if (string.IsNullOrEmpty(subject.SUBJPREQ))
            {
                missingFields.Add(fieldNames[3]);
            }
            if (subject.SUBJPREQ != "CO" && string.IsNullOrEmpty(subject.SUBJCODEPREQ))
            {
                missingFields.Add(fieldNames[4]);
            }
            if (subject.SUBJREGOFRNG == 0)
            {
                missingFields.Add(fieldNames[5]);
            }
            if (string.IsNullOrEmpty(subject.SUBJCATEGORY))
            {
                missingFields.Add(fieldNames[6]);
            }
            if (string.IsNullOrEmpty(subject.SUBJCOURSECODE))
            {
                missingFields.Add(fieldNames[7]);
            }
            if (string.IsNullOrEmpty(subject.SUBJCURRCODE))
            {
                missingFields.Add(fieldNames[8]);
            }
            if (missingFields.Count > 0)
            {
                string fields = string.Join(", ", missingFields);
                string message = $"The following fields are missing or invalid values: {fields}";
                return message; 
            }
            return null;

        }

        private bool IsNumeric(string input)
        {
            return Regex.IsMatch(input, "^[0-9]+$");
        }

        private void SaveData(Subject addRequestSubject)
        {
            var subject = new Subject
            {
                SUBJCODE = addRequestSubject.SUBJCODE,
                SUBJDESC = addRequestSubject.SUBJDESC,
                SUBJUNITS = addRequestSubject.SUBJUNITS,
                SUBJREGOFRNG = addRequestSubject.SUBJREGOFRNG,
                SUBJCATEGORY = addRequestSubject.SUBJCATEGORY,
                SUBJSTATUS = "AC",
                SUBJCOURSECODE = addRequestSubject.SUBJCOURSECODE,
                SUBJCURRCODE = addRequestSubject.SUBJCURRCODE,
                SUBJPREQ = addRequestSubject.SUBJPREQ,
                SUBJCODEPREQ = addRequestSubject.SUBJCODEPREQ
            };

            _context.Subject.Add(subject);
            _context.SaveChanges();
        }
    }
}
