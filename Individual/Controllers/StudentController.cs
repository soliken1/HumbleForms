using Individual.Data;
using Individual.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Individual.Controllers
{
    public class StudentController : Controller
    {
        //CHANGES HERE WHICH IN GLOBAL VARIABLE AND IN CONSTRUCTOR
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment WebHostEnvironment;

        public StudentController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var students = _context.Student.ToList();
            return View(students);
        }
        [HttpGet]
        public IActionResult Index(string searchStudID)
        {
            var students = from x in _context.Student
                           select x;

            if (!string.IsNullOrEmpty(searchStudID) && long.TryParse(searchStudID, out long searchID))
            {
                students = students.Where(x => x.STFSTUDID == searchID);
            }

            return View(students.ToList());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(Student addRequestStudent)
        {
            string errorMsg = FieldValidation(addRequestStudent);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                TempData["FieldError"] = errorMsg;
                return View("Add", addRequestStudent);
            }

            if (_context.Student.Any(s => s.STFSTUDID == addRequestStudent.STFSTUDID))
            {
                TempData["DuplicateError"] = "Duplicate Error: The ID Number already exists. Cannot Proceed.";
                return View(addRequestStudent);
            }

            SaveData(addRequestStudent);

            TempData["SuccessMessage"] = "Student information saved successfully.";
            return View();
        }


        [HttpGet]
        public IActionResult Edit(long? code)
        {
            var studid = _context.Student.Where(s => s.STFSTUDID == code).FirstOrDefault();

            return View(studid);
        }
        [HttpPost]
        public IActionResult Edit(Student studentEdit)
        {
            var student = _context.Student.FirstOrDefault(s => s.STFSTUDID == studentEdit.STFSTUDID);

            if (student != null)
            {
                _context.Student.Remove(student);
                _context.Add(studentEdit);
                _context.SaveChanges();
                TempData["UpMessage"] = "Student Info Updated Successfully!";
                return RedirectToAction("Index");
            }
            return View(studentEdit);
        }

        [HttpPost]
        public IActionResult Delete(long? code)
        {
            if (code == null)
            {
                return NotFound();
            }

            var student = _context.Student.FirstOrDefault(s => s.STFSTUDID == code);

            if (student == null)
            {
                return NotFound();
            }

            _context.Student.Remove(student);
            _context.SaveChanges();
            TempData["DelMessage"] = "Student Info Deleted Successfully!";
            return RedirectToAction("Index");
        }

        private string FieldValidation(Student student)
        {
            List<string> missingFields = new List<string>();
            string[] fieldNames = { "ID Number", "First Name", "Last Name", "Course", "Year", "Remarks" };
            if (!IsNumeric(student.STFSTUDID.ToString()))
            {
                missingFields.Add(fieldNames[0]);
            }
            if (student.STFSTUDID == 0L)
            {
                missingFields.Add(fieldNames[0]);
            }
            if (string.IsNullOrEmpty(student.STFSTUDFNAME) || !IsAlphabetOnly(student.STFSTUDFNAME))
            {
                missingFields.Add(fieldNames[1]);
            }
            if (string.IsNullOrEmpty(student.STFSTUDLNAME) || !IsAlphabetOnly(student.STFSTUDLNAME))
            {
                missingFields.Add(fieldNames[2]);
            }
            if (string.IsNullOrEmpty(student.STFSTUDCOURSE))
            {
                missingFields.Add(fieldNames[3]);
            }
            if (student.STFSTUDYEAR == 0)
            {
                missingFields.Add(fieldNames[4]);
            }
            if (string.IsNullOrEmpty(student.STFSTUDREMARKS))
            {
                missingFields.Add(fieldNames[5]);
            }

            if (missingFields.Count > 0)
            {
                string fields = string.Join(", ", missingFields);
                string message = $"The following fields are missing or invalid values: {fields}";
                return message;
            }
            return null;

        }

        private bool IsAlphabetOnly(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, "^[a-zA-Z ]+$");
        }

        private bool IsNumeric(string input)
        {
            return Regex.IsMatch(input, "^[0-9]+$");
        }

        private void SaveData(Student addRequestStudent)
        {
            if (string.IsNullOrEmpty(addRequestStudent.STFSTUDMNAME))
            {
                addRequestStudent.STFSTUDMNAME = "n/a";
            }

            string fileName = UploadFile(addRequestStudent);

            var student = new Student()
            {
                STFSTUDID = addRequestStudent.STFSTUDID,
                //CHANGES HERE THE STFSTUDPICPATH
                STFSTUDPICPATH = fileName,
                STFSTUDFNAME = addRequestStudent.STFSTUDFNAME,
                STFSTUDMNAME = addRequestStudent.STFSTUDMNAME,
                STFSTUDLNAME = addRequestStudent.STFSTUDLNAME,
                STFSTUDCOURSE = addRequestStudent.STFSTUDCOURSE,
                STFSTUDYEAR = addRequestStudent.STFSTUDYEAR,
                STFSTUDREMARKS = addRequestStudent.STFSTUDREMARKS,
                STFSTUDSTATUS = "AC"
            };
            _context.Student.Add(student);
            _context.SaveChanges();
        }

        //DOES NOT SAVE THE IMAGE BUT ONLY THE PATH FOR THE IMAGE, THIS BLOCK 
        //CREATES A PATH FOR THE IMAGE TO SAVE IT ON THE ROOT FOLDER CALLED "images" in wwwroot
        //THIS STATEMENT RETURNS THE PATH OF THE CREATED OR GENERATED IMAGE TO THE USER'S 
        //SUBMITTED IMAGE
        private string UploadFile(Student addRequestStudent)
        {
            string fileName = null;
            if(addRequestStudent.STFSTUDPIC != null)
            {
                string uploadDir= Path.Combine(WebHostEnvironment.WebRootPath, "images");
                fileName = Guid.NewGuid().ToString() + "-" + addRequestStudent.STFSTUDPIC.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    addRequestStudent.STFSTUDPIC.CopyTo(fileStream);
                }
            }
            return fileName;
        } 
    }
}
