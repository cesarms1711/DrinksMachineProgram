using DrinksMachineProgram.Entities;
using DrinksMachineProgram.Resources;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DrinksMachineProgram.BusinessLayer
{

    public class UsersBL : IEntityBL<User, short>
    {

        #region Private Attributes

        private short MaxId = 0;

        private static List<User> Users = new();

        #endregion Private Attributes

        #region CTOR

        private UsersBL() { }

        #endregion CTOR

        #region Singleton Instance

        private static UsersBL _instance;

        public static UsersBL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UsersBL();
                }

                return _instance;
            }
        }

        #endregion Singleton Instance

        #region Public Methods

        public List<User> List()
        {
            return Users;
        }

        public User Detail(short id)
        {
            User user = Users.First(u => u.Id == id);

            if (user == null) throw new Exception(TextResources.MessageErrorRecordDoesNotExist);

            return user;
        }

        public User Detail(
             string userName,
             byte[] passwordHash)
        {
            var users = Users
                .Where(u => u.UserName == userName)
                .ToList();

            users = users
                .Where(u => StructuralComparisons.StructuralEqualityComparer.Equals(
                    u.PasswordHash,
                    passwordHash))
                .ToList();

            return users.FirstOrDefault();
        }

        public void Create(User user)
        {
            MaxId++;

            user.Id = MaxId;

            Users.Add(user);
        }

        public void Edit(User user)
        {
            User modifiedUser = Users.First(u => u.Id == user.Id);

            modifiedUser.UserName = user.UserName;
            modifiedUser.FirstName = user.FirstName;
            modifiedUser.LastName = user.LastName;
            modifiedUser.PasswordHash = user.PasswordHash;
        }

        public void Delete(short id)
        {
            Users = Users
                .Where(u => u.Id != id)
                .ToList();
        }

#endregion Public Methods

    }

}