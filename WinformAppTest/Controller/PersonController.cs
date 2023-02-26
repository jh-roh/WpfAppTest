using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinformAppTest.Model;

namespace WinformAppTest.Controller
{
    public class PersonController
    {
        private MainForm _view;
        private PersonModel _model;

        public PersonController(MainForm view)
        {
            _view = view;
            _model = new PersonModel();
        }

        public void LoadPersons()
        {
            var persons = _model.GetPersons();
            _view.SetGridDataSource(persons);
        }
    }
}
