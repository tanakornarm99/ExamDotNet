using BasicCSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicCSharp.DataAccess
{
    public class DACustomer
    {
        private ExecuteQuery _exec;

        public DACustomer(string connectionString)
        {
            _exec = new ExecuteQuery(connectionString);
        }

        public void AddCustomer(string firstName, string sureName, string contact, string email)
        {
            string cmd = "INSERT INTO [Customer] (Firstname,Surename,Contact,Email) VALUES (@firstName,@sureName,@contact,@email)";
            List<Param> paramety = new List<Param>();
            paramety.Add(_exec.SetParam("firstName", firstName));
            paramety.Add(_exec.SetParam("sureName", sureName));
            paramety.Add(_exec.SetParam("contact", contact));
            paramety.Add(_exec.SetParam("email", email));
            _exec.ExecuteNonQuery(cmd, paramety);
        }


    }
}