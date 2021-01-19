using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Models
{
    public class Account : BsonData.Document
    {
        
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        
        public Permission Role { get; set; }
    }
    //public class AccountBinding : Account
    //{
    //    public string UserName { get; set; }

    //}

    public class UserCreate: BsonData.Document
    {
        public UserCreate()
        {
            Role = Permission.USER;
        }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public static Permission Role { get; set; }
    }

    public class UserInfo : BsonData.Document
    {
        public string Name { get; set; }
        public Account Account { get; set; }
    }
    public enum Permission
    {
        NONE, USER, MOD, ADMIN
    }
}