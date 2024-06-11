using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using University.Data;
using University.Interfaces;
using University.Models;
using University.Services;
using University.ViewModels;

namespace University.Tests
{
    [TestClass]
    public class ClassroomViewModelTests
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
            using (UniversityContext context = new UniversityContext(_options))
            {
                context.Database.EnsureDeleted();

                var classrooms = new[]
                {
                    new Classroom { ClassroomId = 1, Location = "Room 101", Capacity = 30, AvailableSeats = 30, Projector = true, Whiteboard = true, Microphone = false, Description = "Lecture Hall" },
                    new Classroom { ClassroomId = 2, Location = "Room 102", Capacity = 20, AvailableSeats = 20, Projector = false, Whiteboard = true, Microphone = true, Description = "Seminar Room" }
                };

                context.Classrooms.AddRange(classrooms);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void Add_classroom_with_all_details()
        {
            using (UniversityContext context = new UniversityContext(_options))
            {
                AddClassroomViewModel addClassroomViewModel = new AddClassroomViewModel(context, _dialogService)
                {
                    Location = "Room 103",
                    Capacity = 40,
                    AvailableSeats = 40,
                    Projector = true,
                    Whiteboard = true,
                    Microphone = true,
                    Description = "New Classroom"
                };

                addClassroomViewModel.Save.Execute(null);

                bool newClassroomExists = context.Classrooms.Any(c => c.Location == "Room 103" && c.Capacity == 40);
                Assert.IsTrue(newClassroomExists);
            }
        }

        [TestMethod]
        public void Add_classroom_without_location()
        {
            using (UniversityContext context = new UniversityContext(_options))
            {
                AddClassroomViewModel addClassroomViewModel = new AddClassroomViewModel(context, _dialogService)
                {
                    Capacity = 25,
                    AvailableSeats = 25,
                    Projector = false,
                    Whiteboard = true,
                    Microphone = false,
                    Description = "Classroom without Location"
                };

                addClassroomViewModel.Save.Execute(null);

                bool newClassroomExists = context.Classrooms.Any(c => c.Capacity == 25);
                Assert.IsFalse(newClassroomExists);
            }
        }

        [TestMethod]
        public void Add_classroom_without_capacity()
        {
            using (UniversityContext context = new UniversityContext(_options))
            {
                AddClassroomViewModel addClassroomViewModel = new AddClassroomViewModel(context, _dialogService)
                {
                    Location = "Room 104",
                    AvailableSeats = 25,
                    Projector = false,
                    Whiteboard = true,
                    Microphone = false,
                    Description = "Classroom without Capacity"
                };

                addClassroomViewModel.Save.Execute(null);

                bool newClassroomExists = context.Classrooms.Any(c => c.Location == "Room 104");
                Assert.IsTrue(newClassroomExists);
            }
        }

        [TestMethod]
        public void Add_Classroom_With_Valid_Data_Positive()
        {
            using (UniversityContext context = new UniversityContext(_options))
            {
                var viewModel = new AddClassroomViewModel(context, _dialogService)
                {
                    Location = "Room 105",
                    Capacity = 50,
                    AvailableSeats = 50,
                    Projector = true,
                    Whiteboard = true,
                    Microphone = true,
                    Description = "New Valid Classroom"
                };

                viewModel.Save.Execute(null);

                bool newClassroomExists = context.Classrooms.Any(c => c.Location == "Room 105" && c.Capacity == 50);
                Assert.IsTrue(newClassroomExists);
            }
        }

        [TestMethod]
        public void Add_Classroom_With_Invalid_Data_Negative()
        {
            using (UniversityContext context = new UniversityContext(_options))
            {
                var viewModel = new AddClassroomViewModel(context, _dialogService)
                {
                    // No location provided, invalid data
                    Capacity = 50,
                    AvailableSeats = 50,
                    Projector = true,
                    Whiteboard = true,
                    Microphone = true,
                    Description = "Invalid Classroom Without Location"
                };

                viewModel.Save.Execute(null);

                bool newClassroomExists = context.Classrooms.Any(c => c.Capacity == 50 && c.Description == "Invalid Classroom Without Location");
                Assert.IsFalse(newClassroomExists);
            }
        }

        #region Edit 

        [TestMethod]
        public void Edit_Classroom_With_All_Details()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new EditClassroomViewModel(context, _dialogService)
                {
                    ClassroomId = 1,
                    Location = "Updated Room 101",
                    Capacity = 35,
                    AvailableSeats = 35,
                    Projector = false,
                    Whiteboard = false,
                    Microphone = true,
                    Description = "Updated Lecture Hall",
                };

                viewModel.Save.Execute(null);

                bool classroomExists = context.Classrooms.Any(c => c.ClassroomId == 1 && c.Location == "Updated Room 101" && c.Capacity == 35);
                Assert.IsTrue(classroomExists);
            }
        }

        [TestMethod]
        public void Edit_Classroom_Without_Location()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new EditClassroomViewModel(context, _dialogService)
                {
                    ClassroomId = 1,
                    Capacity = 30,
                    AvailableSeats = 30,
                    Projector = true,
                    Whiteboard = true,
                    Microphone = false,
                    Description = "Lecture Hall without Updated Location"
                };

                viewModel.Save.Execute(null);

                bool classroomExists = context.Classrooms.Any(c => c.ClassroomId == 1 && c.Location == "Updated Room 101");
                Assert.IsFalse(classroomExists);
            }
        }

        [TestMethod]
        public void Edit_Classroom_With_Location()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new EditClassroomViewModel(context, _dialogService)
                {
                    ClassroomId = 1,
                    Capacity = 30,
                    Location = "Test",
                    AvailableSeats = 30,
                    Projector = true,
                    Whiteboard = true,
                    Microphone = false,
                    Description = "Lecture Hall without Updated Location"
                };

                viewModel.Save.Execute(null);

                bool classroomExists = context.Classrooms.Any(c => c.ClassroomId == 1 && c.Location == "Test");
                Assert.IsTrue(classroomExists);
            }
        }


        [TestMethod]
        public void Edit_Classroom_Without_Capacity()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new EditClassroomViewModel(context, _dialogService)
                {
                    ClassroomId = 1,
                    Location = "Updated Room 101",
                    AvailableSeats = 30,
                    Projector = true,
                    Whiteboard = true,
                    Microphone = false,
                    Description = "Lecture Hall without Updated Capacity"
                };

                viewModel.Save.Execute(null);

                bool classroomExists = context.Classrooms.Any(c => c.ClassroomId == 1 && c.Location == "Updated Room 101");
                Assert.IsTrue(classroomExists);
            }
        }

        [TestMethod]
        public void Edit_Classroom_With_Valid_Data_Positive()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new EditClassroomViewModel(context, _dialogService)
                {
                    ClassroomId = 1,
                    Location = "Updated Room 101",
                    Capacity = 35,
                    AvailableSeats = 35,
                    Projector = true,
                    Whiteboard = true,
                    Microphone = true,
                    Description = "Updated Valid Classroom"
                };

                viewModel.Save.Execute(null);

                bool classroomExists = context.Classrooms.Any(c => c.ClassroomId == 1 && c.Location == "Updated Room 101" && c.Capacity == 35);
                Assert.IsTrue(classroomExists);
            }
        }

        [TestMethod]
        public void Edit_Classroom_With_Invalid_Data_Negative()
        {
            using (var context = new UniversityContext(_options))
            {
                var viewModel = new EditClassroomViewModel(context, _dialogService)
                {
                    ClassroomId = 1,
                    // No location provided, invalid data
                    Capacity = 35,
                    AvailableSeats = 35,
                    Projector = true,
                    Whiteboard = true,
                    Microphone = true,
                    Description = "Invalid Update Without Location"
                };

                viewModel.Save.Execute(null);

                bool classroomExists = context.Classrooms.Any(c => c.ClassroomId == 1 && c.Description == "Invalid Update Without Location");
                Assert.IsTrue(classroomExists);
            }
        }

        #endregion

        #region Remove

        [TestMethod]
        public void Remove_Classroom_With_Confirmation()
        {
            using (var context = new UniversityContext(_options))
            {
                var initialCount = context.Classrooms.Count();
                var dialogService = new TestDialogService(true);
                var viewModel = new ClassroomViewModel(context, dialogService);

                viewModel.Remove.Execute((int)1);

                var classroomExists = context.Classrooms.Any(c => c.ClassroomId == 1);
                Assert.IsFalse(classroomExists);
                Assert.AreEqual(initialCount - 1, context.Classrooms.Count());
            }
        }

        [TestMethod]
        public void Remove_Classroom_Without_Confirmation()
        {
            using (var context = new UniversityContext(_options))
            {
                var initialCount = context.Classrooms.Count();
                var dialogService = new TestDialogService(false);
                var viewModel = new ClassroomViewModel(context, dialogService);

                viewModel.Remove.Execute((int)1);

                var classroomExists = context.Classrooms.Any(c => c.ClassroomId == 1);
                Assert.IsTrue(classroomExists);
                Assert.AreEqual(initialCount, context.Classrooms.Count());
            }
        }

        [TestMethod]
        public void Remove_Nonexistent_Classroom()
        {
            using (var context = new UniversityContext(_options))
            {
                var initialCount = context.Classrooms.Count();
                var dialogService = new TestDialogService(true);
                var viewModel = new ClassroomViewModel(context, dialogService);

                viewModel.Remove.Execute((int)9999);

                var classroomExists = context.Classrooms.Any(c => c.ClassroomId == 9999);
                Assert.IsFalse(classroomExists);
                Assert.AreEqual(initialCount, context.Classrooms.Count());
            }
        }
        #endregion

    }
}