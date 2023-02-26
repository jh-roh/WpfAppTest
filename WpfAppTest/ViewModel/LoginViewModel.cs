using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfAppTest.ViewModel
{
    [POCOViewModel]
    public class LoginViewModel
    {
        public static LoginViewModel Create()
        {
            return ViewModelSource.Create(() => new LoginViewModel());
        }
        protected LoginViewModel() { }

        public virtual string UserName { get; set; }
        public void Login()
        {
            this.GetService<IMessageBoxService>().Show("Login succeeded", "Login", MessageButton.OK, MessageIcon.Information, MessageResult.OK);
        }
        public bool CanLogin()
        {
            return !string.IsNullOrEmpty(UserName);
        }
    }
}
