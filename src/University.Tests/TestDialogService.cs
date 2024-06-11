using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Interfaces;

namespace University.Tests
{
    public class TestDialogService : IDialogService
    {
        private readonly bool _dialogResult;

        public TestDialogService(bool dialogResult)
        {
            _dialogResult = dialogResult;
        }

        public bool? Show(string itemName)
        {
            return _dialogResult;
        }
    }
}
