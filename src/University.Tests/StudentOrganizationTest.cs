using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using University.Data;
using University.Interfaces;
using University.Models;
using University.Services;
using University.ViewModels;

namespace University.Tests
{
    [TestClass]
    public class StudentOrganizationViewModelTests
    {
        private IDialogService _dialogService;
        private DbContextOptions<UniversityContext> _options;

        [TestInitialize()]
        public void Initialize()
        {
            _options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "UniversityTestDB")
                .Options;
            SeedTestDB();
            _dialogService = new DialogService();
        }

        private void SeedTestDB()
        {
            using (var context = new UniversityContext(_options))
            {
                context.Database.EnsureDeleted();
                List<Student> students = new List<Student>
                {
                new Student { StudentId = 1, Name = "Student A", LastName = "Nowakowicz", PESEL="PESEL1", BirthDate = new DateTime(1987, 05, 22) },
                new Student { StudentId = 2, Name = "Student B", LastName = "Nowakowicz", PESEL = "PESEL2", BirthDate = new DateTime(2019, 06, 25) },
                new Student { StudentId = 3, Name = "Student C", LastName = "Nowakowicz", PESEL = "PESEL3", BirthDate = new DateTime(2021, 06, 08) }
                };

                var studentOrganizations = new[]
                {
                    new StudentOrganization { StudentOrganizationId = 1, Name = "Organization A", Advisor = "Advisor A", President = "President A", Description = "Description A", MeetingSchedule = "Schedule A", Email = "orgA@example.com" },
                    new StudentOrganization { StudentOrganizationId = 2, Name = "Organization B", Advisor = "Advisor B", President = "President B", Description = "Description B", MeetingSchedule = "Schedule B", Email = "orgB@example.com" }
                };

                context.Students.AddRange(students);
                context.StudentOrganizations.AddRange(studentOrganizations);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void Add_StudentOrganization_With_Valid_Data()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new AddStudentOrganizationViewModel(context, _dialogService)
                {
                    Name = "Organization C",
                    Advisor = "Advisor C",
                    President = "President C",
                    Description = "Description C",
                    MeetingSchedule = "Schedule C",
                    Email = "orgC@example.com"
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.Name == "Organization C" && org.Advisor == "Advisor C");
                Assert.IsTrue(organizationExists);
            }
        }

        [TestMethod]
        public void Add_StudentOrganization_Without_Name()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new AddStudentOrganizationViewModel(context, _dialogService)
                {
                    Advisor = "Advisor D",
                    President = "President D",
                    Description = "Description D",
                    MeetingSchedule = "Schedule D",
                    Email = "orgD@example.com"
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.Advisor == "Advisor D");
                Assert.IsFalse(organizationExists);
                Assert.AreEqual("Name is Required", viewModel["Name"]);
            }
        }

        [TestMethod]
        public void Add_StudentOrganization_Without_Advisor()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new AddStudentOrganizationViewModel(context, _dialogService)
                {
                    Name = "Organization E",
                    President = "President E",
                    Description = "Description E",
                    MeetingSchedule = "Schedule E",
                    Email = "orgE@example.com"
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.Name == "Organization E");
                Assert.IsFalse(organizationExists);
                Assert.AreEqual("Advisor is Required", viewModel["Advisor"]);
            }
        }

        [TestMethod]
        public void Add_StudentOrganization_Without_President()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new AddStudentOrganizationViewModel(context, _dialogService)
                {
                    Name = "Organization F",
                    Advisor = "Advisor F",
                    Description = "Description F",
                    MeetingSchedule = "Schedule F",
                    Email = "orgF@example.com"
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.Name == "Organization F");
                Assert.IsFalse(organizationExists);
                Assert.AreEqual("President is Required", viewModel["President"]);
            }
        }

        [TestMethod]
        public void Add_StudentOrganization_Without_Description()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new AddStudentOrganizationViewModel(context, _dialogService)
                {
                    Name = "Organization G",
                    Advisor = "Advisor G",
                    President = "President G",
                    MeetingSchedule = "Schedule G",
                    Email = "orgG@example.com"
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.Name == "Organization G");
                Assert.IsFalse(organizationExists);
                Assert.AreEqual("Description is Required", viewModel["Description"]);
            }
        }

        [TestMethod]
        public void Add_StudentOrganization_Without_MeetingSchedule()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new AddStudentOrganizationViewModel(context, _dialogService)
                {
                    Name = "Organization H",
                    Advisor = "Advisor H",
                    President = "President H",
                    Description = "Description H",
                    Email = "orgH@example.com"
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.Name == "Organization H");
                Assert.IsFalse(organizationExists);
                Assert.AreEqual("Meeting Schedule is Required", viewModel["MeetingSchedule"]);
            }
        }

        [TestMethod]
        public void Add_StudentOrganization_Without_Email()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new AddStudentOrganizationViewModel(context, _dialogService)
                {
                    Name = "Organization I",
                    Advisor = "Advisor I",
                    President = "President I",
                    Description = "Description I",
                    MeetingSchedule = "Schedule I"
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.Name == "Organization I");
                Assert.IsFalse(organizationExists);
                Assert.AreEqual("Email is Required", viewModel["Email"]);
            }
        }

        [TestMethod]
        public void Add_Student_To_StudentOrganization()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new AddStudentOrganizationViewModel(context, _dialogService)
                {
                    Name = "Organization C",
                    Advisor = "Advisor C",
                    President = "President C",
                    Description = "Description C",
                    MeetingSchedule = "Schedule C",
                    Email = "orgC@example.com"
                };

                var student = context.Students.First();
                viewModel.Add.Execute(student);
                viewModel.Save.Execute(null);

                var organization = context.StudentOrganizations.Include(o => o.Students).FirstOrDefault(o => o.Name == "Organization C");
                Assert.IsNotNull(organization);
                Assert.IsTrue(organization.Students.Any(s => s.StudentId == student.StudentId));
            }
        }


        #region Edit 


        [TestMethod]
        public void Edit_StudentOrganization_With_Valid_Data()
        {
            using (var context = new UniversityContext(_options))
            {
                EditStudentOrganizationViewModel viewModel = new EditStudentOrganizationViewModel(context, _dialogService)
                {
                    StudentOrganizationId = 1,
                    Name = "Updated Organization A",
                    Advisor = "Updated Advisor A",
                    President = "Updated President A",
                    Description = "Updated Description A",
                    MeetingSchedule = "Updated Schedule A",
                    Email = "updated_orgA@example.com"
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.Name == "Updated Organization A" && org.Advisor == "Updated Advisor A");
                Assert.IsTrue(organizationExists);
            }
        }

        [TestMethod]
        public void Edit_StudentOrganization_Without_Name()
        {
            using (var context = new UniversityContext(_options))
            {
                EditStudentOrganizationViewModel viewModel = new EditStudentOrganizationViewModel(context, _dialogService)
                {
                    StudentOrganizationId = 2,
                    Name = "",
                    Advisor = "Updated Advisor B",
                    President = "Updated President B",
                    Description = "Updated Description B",
                    MeetingSchedule = "Updated Schedule B",
                    Email = "updated_orgB@example.com"
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.StudentOrganizationId == 2 && org.Advisor == "Updated Advisor B");
                Assert.IsFalse(organizationExists);
                Assert.AreEqual("Name is Required", viewModel["Name"]);
            }
        }

        [TestMethod]
        public void Edit_StudentOrganization_Without_Advisor()
        {
            using (var context = new UniversityContext(_options))
            {
                EditStudentOrganizationViewModel viewModel = new EditStudentOrganizationViewModel(context, _dialogService)
                {
                    StudentOrganizationId = 2,
                    Name = "Updated Organization B",
                    Advisor = "",
                    President = "Updated President B",
                    Description = "Updated Description B",
                    MeetingSchedule = "Updated Schedule B",
                    Email = "updated_orgB@example.com"
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.StudentOrganizationId == 2 && org.Name == "Updated Organization B");
                Assert.IsFalse(organizationExists);
                Assert.AreEqual("Advisor is Required", viewModel["Advisor"]);
            }
        }


        [TestMethod]
        public void Edit_StudentOrganization_Without_MeetingSchedule()
        {
            using (var context = new UniversityContext(_options))
            {
                EditStudentOrganizationViewModel viewModel = new EditStudentOrganizationViewModel(context, _dialogService)
                {
                    StudentOrganizationId = 2,
                    Name = "Updated Organization B",
                    Advisor = "Updated Advisor B",
                    President = "Updated President B",
                    Description = "Updated Description B",
                    MeetingSchedule = "",
                    Email = "updated_orgB@example.com"
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.StudentOrganizationId == 2 && org.Name == "Updated Organization B");
                Assert.IsTrue(organizationExists);
            }
        }

        [TestMethod]
        public void Edit_StudentOrganization_Without_Email()
        {
            using (var context = new UniversityContext(_options))
            {
                EditStudentOrganizationViewModel viewModel = new EditStudentOrganizationViewModel(context, _dialogService)
                {
                    StudentOrganizationId = 2,
                    Name = "Updated Organization B",
                    Advisor = "Updated Advisor B",
                    President = "Updated President B",
                    Description = "Updated Description B",
                    MeetingSchedule = "Updated Schedule B",
                    Email = "" // Empty email
                };

                viewModel.Save.Execute(null);

                bool organizationExists = context.StudentOrganizations.Any(org => org.StudentOrganizationId == 2 && org.Name == "Updated Organization B");
                Assert.IsFalse(organizationExists);
                Assert.AreEqual("Email is Required", viewModel["Email"]);
            }
        }

        [TestMethod]
        public void Edit_StudentOrganization_Add_Student()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new EditStudentOrganizationViewModel(context, _dialogService)
                {
                    StudentOrganizationId = 1
                };

                var student = context.Students.First(s => s.StudentId == 3);
                viewModel.Add.Execute(student);
                viewModel.Save.Execute(null);

                var organization = context.StudentOrganizations.Include(o => o.Students).FirstOrDefault(o => o.StudentOrganizationId == 1);
                Assert.IsNotNull(organization);
                Assert.IsTrue(organization.Students.Any(s => s.StudentId == student.StudentId));
            }
        }

        [TestMethod]
        public void Edit_StudentOrganization_Remove_Student()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new EditStudentOrganizationViewModel(context, _dialogService)
                {
                    StudentOrganizationId = 1
                };

                var student = context.Students.First(s => s.StudentId == 1);
                viewModel.Remove.Execute(student);
                viewModel.Save.Execute(null);

                var organization = context.StudentOrganizations.Include(o => o.Students).FirstOrDefault(o => o.StudentOrganizationId == 1);
                Assert.IsNotNull(organization);
                Assert.IsFalse(organization.Students.Any(s => s.StudentId == student.StudentId));
            }
        }

        #endregion

        #region Remuve

        [TestMethod]
        public void Remove_StudentOrganization_With_Valid_Id()
        {
            using (var context = new UniversityContext(_options))
            {
                // Arrange
                var dialogService = new TestDialogService(true);
                var viewModel = new StudentOrganizationViewModel(context, dialogService);
                var initialCount = context.StudentOrganizations.Count();

                // Act
                viewModel.Remove.Execute((long)1); // Removing the organization with ID 1

                // Assert
                var organizationExists = context.StudentOrganizations.Any(org => org.StudentOrganizationId == 1);
                Assert.IsFalse(organizationExists);
                Assert.AreEqual(initialCount - 1, context.StudentOrganizations.Count());
            }
        }

        [TestMethod]
        public void Remove_StudentOrganization_With_Invalid_Id()
        {
            using (var context = new UniversityContext(_options))
            {
                // Arrange
                var initialCount = context.StudentOrganizations.Count();
                var dialogService = new TestDialogService(true);
                var viewModel = new StudentOrganizationViewModel(context, dialogService);

                // Act
                viewModel.Remove.Execute((long)9999); // Cast the invalid ID to long

                // Assert
                var organizationExists = context.StudentOrganizations.Any(org => org.StudentOrganizationId == 9999);
                Assert.IsFalse(organizationExists); // Organization should not exist
                Assert.AreEqual(initialCount, context.StudentOrganizations.Count()); // Count should remain the same
            }
        }

        [TestMethod]
        public void Remove_StudentOrganization_With_Confirmation_Dialog()
        {
            using (var context = new UniversityContext(_options))
            {
                // Arrange
                var initialCount = context.StudentOrganizations.Count();

                // Simulate dialog returning false
                var dialogService = new TestDialogService(false);
                var viewModel = new StudentOrganizationViewModel(context, dialogService);

                // Act
                viewModel.Remove.Execute((long)1); // Cast the ID to long

                // Assert
                var organizationExists = context.StudentOrganizations.Any(org => org.StudentOrganizationId == 1);
                Assert.IsTrue(organizationExists); // Organization should still exist
                Assert.AreEqual(initialCount, context.StudentOrganizations.Count());

                // Simulate dialog returning true
                dialogService = new TestDialogService(true);
                viewModel = new StudentOrganizationViewModel(context, dialogService);

                // Act again
                viewModel.Remove.Execute((long)1); // Cast the ID to long

                // Assert again
                organizationExists = context.StudentOrganizations.Any(org => org.StudentOrganizationId == 1);
                Assert.IsFalse(organizationExists); // Organization should be removed now
                Assert.AreEqual(initialCount - 1, context.StudentOrganizations.Count());
            }
        }

        #endregion
    }
}
