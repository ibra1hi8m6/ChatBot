using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Data.ViewModel.UserViewModel
{
    public class LoginDTOs
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterDTOs
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public string PhoneNumber { get; set; }
      
    }
    public class RegisterResponse
    {
        public bool Successed { get; set; }
        public string Message { get; set; }
    }
}
