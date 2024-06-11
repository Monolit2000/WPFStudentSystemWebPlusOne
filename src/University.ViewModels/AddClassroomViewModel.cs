using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class AddClassroomViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

        public string Error => string.Empty;

        #region props

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Location")
                {
                    if (string.IsNullOrEmpty(Location))
                    {
                        return "Location is Required";
                    }
                }
                if (columnName == "Description")
                {
                    if (string.IsNullOrEmpty(Description))
                    {
                        return "Description is Required";
                    }
                }
                return string.Empty;
            }
        }

        private string _location = string.Empty;
        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private int _capacity;
        public int Capacity
        {
            get => _capacity;
            set
            {
                _capacity = value;
                OnPropertyChanged(nameof(Capacity));
            }
        }

        private int _availableSeats;
        public int AvailableSeats
        {
            get => _availableSeats;
            set
            {
                _availableSeats = value;
                OnPropertyChanged(nameof(AvailableSeats));
            }
        }

        private bool _projector;
        public bool Projector
        {
            get => _projector;
            set
            {
                _projector = value;
                OnPropertyChanged(nameof(Projector));
            }
        }

        private bool _whiteboard;
        public bool Whiteboard
        {
            get => _whiteboard;
            set
            {
                _whiteboard = value;
                OnPropertyChanged(nameof(Whiteboard));
            }
        }

        private bool _microphone;
        public bool Microphone
        {
            get => _microphone;
            set
            {
                _microphone = value;
                OnPropertyChanged(nameof(Microphone));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _response = string.Empty;
        public string Response
        {
            get => _response;
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }

        #endregion

        #region Navigation

        private ICommand? _back = null;
        public ICommand Back
        {
            get
            {
                if (_back is null)
                {
                    _back = new RelayCommand<object>(NavigateBack);
                }
                return _back;
            }
        }

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.ClassroomSubView = new ClassroomViewModel(_context, _dialogService);
            }
        }

        private ICommand? _save = null;
        public ICommand Save
        {
            get
            {
                if (_save is null)
                {
                    _save = new RelayCommand<object>(SaveData);
                }
                return _save;
            }
        }

        private void SaveData(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            Classroom classroom = new Classroom
            {
                Location = this.Location,
                Capacity = this.Capacity,
                AvailableSeats = this.AvailableSeats,
                Projector = this.Projector,
                Whiteboard = this.Whiteboard,
                Microphone = this.Microphone,
                Description = this.Description
            };

            _context.Classrooms.Add(classroom);
            _context.SaveChanges();

            Response = "Data Saved";
        }

        #endregion

        #region Basic
        public AddClassroomViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private bool IsValid()
        {
            string[] properties = { "Location", "Description" };
            foreach (string property in properties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
