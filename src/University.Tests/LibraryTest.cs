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
    public class LibraryViewModelTests
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
            using UniversityContext context = new UniversityContext(_options);
            {
                context.Database.EnsureDeleted();

                var libraries = new List<Library>
                {
                    new Library { LibraryId = 1, Name = "Library One", Address = "Address A", NumberOfFloors = 3, NumberOfRooms = 10, Description = "Description A", Librarian = "Librarian A"},
                    new Library { LibraryId = 2, Name = "Library Two", Address = "Address B", NumberOfFloors = 2, NumberOfRooms = 8, Description = "Description B", Librarian = "Librarian B"}
                };

                context.Librarys.AddRange(libraries);
                context.SaveChanges();
            }
        }

        #region Add


        [TestMethod]
        public void Add_library()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddLibraryViewModel addLibraryViewModel = new AddLibraryViewModel(context, _dialogService)
                {
                    Name = "New Library",
                    Address = "New Address",
                    NumberOfFloors = 2,
                    NumberOfRooms = 5,
                    Description = "New Description",
                    Librarian = "New Librarian"
                };

                addLibraryViewModel.Save.Execute(null);
                bool newLibraryExists = context.Librarys.Any(l => l.Name == "New Library" && l.Address == "New Address");
                Assert.IsTrue(newLibraryExists);
            }
        }


        [TestMethod]
        public void Add_library_without_name()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddLibraryViewModel addLibraryViewModel = new AddLibraryViewModel(context, _dialogService)
                {
                    Address = "Address without Name",
                    NumberOfFloors = 4,
                    NumberOfRooms = 12,
                    Description = "Description without Name",
                    Librarian = "Librarian without Name"
                };

                addLibraryViewModel.Save.Execute(null);

                bool newLibraryExists = context.Librarys.Any(l => l.Address == "Address without Name");
                Assert.IsFalse(newLibraryExists);
            }
        }

        [TestMethod]
        public void Add_library_without_floors()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddLibraryViewModel addLibraryViewModel = new AddLibraryViewModel(context, _dialogService)
                {
                    Name = "Library without Floors",
                    Address = "Address without Floors",
                    NumberOfRooms = 10,
                    Description = "Description without Floors",
                    Librarian = "Librarian without Floors"
                };

                addLibraryViewModel.Save.Execute(null);

                bool newLibraryExists = context.Librarys.Any(l => l.Name == "Library without Floors");
                Assert.IsFalse(newLibraryExists);
            }
        }

        #endregion

        #region Edit

        [TestMethod]
        public void Edit_library_with_valid_data()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditLibraryViewModel editLibraryViewModel = new EditLibraryViewModel(context, _dialogService)
                {
                    LibraryId = 1,
                    Name = "Updated Library One",
                    Address = "Updated Address A",
                    NumberOfFloors = 4,
                    NumberOfRooms = 12,
                    Description = "Updated Description A",
                    Librarian = "Updated Librarian A"
                };
                editLibraryViewModel.Save.Execute(null);

                var updatedLibrary = context.Librarys.FirstOrDefault(l => l.LibraryId == 1);

                Assert.IsNotNull(updatedLibrary);
                Assert.AreEqual("Updated Library One", updatedLibrary.Name);
                Assert.AreEqual("Updated Address A", updatedLibrary.Address);
                Assert.AreEqual(4, updatedLibrary.NumberOfFloors);
                Assert.AreEqual(12, updatedLibrary.NumberOfRooms);
                Assert.AreEqual("Updated Description A", updatedLibrary.Description);
                Assert.AreEqual("Updated Librarian A", updatedLibrary.Librarian);
            }
        }

        [TestMethod]
        public void Edit_library_without_name()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditLibraryViewModel editLibraryViewModel = new EditLibraryViewModel(context, _dialogService)
                {
                    LibraryId = 2,
                    Name = "",
                    Address = "Updated Address B",
                    NumberOfFloors = 2,
                    NumberOfRooms = 8,
                    Description = "Updated Description B",
                    Librarian = "Updated Librarian B"
                };
                editLibraryViewModel.Save.Execute(null);

                var updatedLibrary = context.Librarys.FirstOrDefault(l => l.LibraryId == 2);
                Assert.IsNotNull(updatedLibrary);
                Assert.AreNotEqual("Updated Address B", updatedLibrary.Address);
            }
        }

        [TestMethod]
        public void Edit_library_without_address()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditLibraryViewModel editLibraryViewModel = new EditLibraryViewModel(context, _dialogService)
                {
                    LibraryId = 1,
                    Name = "Updated Library One",
                    Address = "",
                    NumberOfFloors = 4,
                    NumberOfRooms = 12,
                    Description = "Updated Description A",
                    Librarian = "Updated Librarian A"
                };
                editLibraryViewModel.Save.Execute(null);

                var updatedLibrary = context.Librarys.FirstOrDefault(l => l.LibraryId == 1);
                Assert.IsNotNull(updatedLibrary);
                Assert.AreNotEqual(4, updatedLibrary.NumberOfFloors);
            }
        }


        [TestMethod]
        public void Edit_libraries()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditLibraryViewModel editLibraryViewModel = new EditLibraryViewModel(context, _dialogService)
                {
                    LibraryId = 1,
                    Name = "Updated Library One",
                    Address = "Updated Address A",
                    NumberOfFloors = 4,
                    NumberOfRooms = 12,
                    Description = "Updated Description A",
                    Librarian = "Updated Librarian A"
                };
                editLibraryViewModel.Save.Execute(null);

                var updatedLibrary = context.Librarys.FirstOrDefault(l => l.LibraryId == 1);

                Assert.IsNotNull(updatedLibrary);
            }
        }

        #endregion

    }
}
