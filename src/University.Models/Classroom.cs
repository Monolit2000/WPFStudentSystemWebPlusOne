using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Models
{
    public class Classroom
    {
        public int ClassroomId { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public int AvailableSeats { get; set; }
        public bool Projector { get; set; }
        public bool Whiteboard { get; set; }
        public bool Microphone { get; set; }
        public string Description { get; set; }
    }
}
