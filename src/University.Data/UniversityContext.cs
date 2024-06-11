using University.Models;
using Microsoft.EntityFrameworkCore;

namespace University.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext()
        {
        }

        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }

       

        #region Pack
        public DbSet<StudentOrganization> StudentOrganizations { get; set; }
        public DbSet<Library> Librarys { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }

        #endregion



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("UniversityDb");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region StudentSubject
            modelBuilder.Entity<Subject>().Ignore(s => s.IsSelected);

            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1, Name = "Wieńczysław", LastName = "Nowakowicz", PESEL = "PESEL1", BirthDate = new DateTime(1987, 05, 22) },
                new Student { StudentId = 2, Name = "Stanisław", LastName = "Nowakowicz", PESEL = "PESEL2", BirthDate = new DateTime(2019, 06, 25) },
                new Student { StudentId = 3, Name = "Eugenia", LastName = "Nowakowicz", PESEL = "PESEL3", BirthDate = new DateTime(2021, 06, 08) });

            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjectId = 1, Name = "Matematyka", Semester = "1", Lecturer = "Michalina Warszawa" },
                new Subject { SubjectId = 2, Name = "Biologia", Semester = "2", Lecturer = "Halina Katowice" },
                new Subject { SubjectId = 3, Name = "Chemia", Semester = "3", Lecturer = "Jan Nowak" }
            );
            #endregion

            #region Pack

            #region StudentOrganization
            modelBuilder.Entity<StudentOrganization>().HasData(
                  new StudentOrganization
                  {
                      StudentOrganizationId = 1,
                      Name = "Student Organization 1",
                      Advisor = "Advisor 1",
                      President = "President 1",
                      Description = "Description of Student Organization 1",
                      MeetingSchedule = "Weekly",
                      Email = "org1@example.com"
                  },
                  new StudentOrganization
                  {
                      StudentOrganizationId = 2,
                      Name = "Student Organization 2",
                      Advisor = "Advisor 2",
                      President = "President 2",
                      Description = "Description of Student Organization 2",
                      MeetingSchedule = "Bi-weekly",
                      Email = "org2@example.com"
                  },
                  new StudentOrganization
                  {
                      StudentOrganizationId = 3,
                      Name = "Student Organization 3",
                      Advisor = "Advisor 3",
                      President = "President 3",
                      Description = "Description of Student Organization 3",
                      MeetingSchedule = "Monthly",
                      Email = "org3@example.com"
                  }
           );


            #endregion

            
            #region Library
            modelBuilder.Entity<Library>().HasData(
                new Library
                {
                    LibraryId = 1,
                    Name = "Main Library",
                    Address = "123 Main Street",
                    NumberOfFloors = 3,
                    NumberOfRooms = 10,
                    Description = "This is the main library of the university.",
                    Librarian = "Alice Librarian"
                },
                new Library
                {
                    LibraryId = 2,
                    Name = "Science Library",
                    Address = "456 Science Avenue",
                    NumberOfFloors = 2,
                    NumberOfRooms = 8,
                    Description = "This is the science library of the university.",
                    Librarian = "Bob Librarian"
                },
                 new Library
                 {
                     LibraryId = 3,
                     Name = "Arts Library",
                     Address = "789 Arts Boulevard",
                     NumberOfFloors = 4,
                     NumberOfRooms = 12,
                     Description = "This is the arts library of the university.",
                     Librarian = "Charlie Librarian"
                 }
            );
            #endregion

            #region Classroom
            modelBuilder.Entity<Classroom>().HasData(
                new Classroom
                {
                    ClassroomId = 1,
                    Location = "Building A, Room 101",
                    Capacity = 30,
                    AvailableSeats = 25,
                    Projector = true,
                    Whiteboard = true,
                    Microphone = false,
                    Description = "Standard classroom with projector and whiteboard."
                },
                new Classroom
                {
                    ClassroomId = 2,
                    Location = "Building B, Room 202",
                    Capacity = 50,
                    AvailableSeats = 50,
                    Projector = true,
                    Whiteboard = true,
                    Microphone = true,
                    Description = "Large lecture hall with full audio-visual equipment."
                },
                new Classroom
                {
                    ClassroomId = 3,
                    Location = "Building C, Room 303",
                    Capacity = 20,
                    AvailableSeats = 18,
                    Projector = false,
                    Whiteboard = true,
                    Microphone = false,
                    Description = "Small classroom with whiteboard."
                }
            );
            #endregion
            

            #endregion


        }
    }
}
