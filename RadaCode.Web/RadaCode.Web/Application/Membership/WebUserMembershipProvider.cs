using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using RadaCode.Web.Data.Entities;
using RadaCode.Web.Data.Repositories;
using RadaCode.Web.Data.Utils;

namespace RadaCode.Web.Application.Membership
{
    public class WebUserMembershipProvider: MembershipProvider
    {
        private readonly IWebUserRepository _userRepository;

        public WebUserMembershipProvider()
        {
            _userRepository = DependencyResolver.Current.GetService<IWebUserRepository>();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (string.IsNullOrEmpty(username))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }
            if (string.IsNullOrEmpty(password))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            string hashedPassword = Crypto.HashPassword(password);
            if (hashedPassword.Length > 128)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (_userRepository.UserNameTaken(username))
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            var newBorn = _userRepository.CreateUser(username, hashedPassword, email);
            status = MembershipCreateStatus.Success;

            //TODO: Schedule E-mail notification in case email is provided. Also set up a default user settings.

            return new MembershipUser(System.Web.Security.Membership.Provider.Name, newBorn.UserName, newBorn.Id,
                newBorn.Email, null, null, true, newBorn.IsLockedOut, newBorn.CreateDate.Value, newBorn.LastLoginDate.Value,
                newBorn.LastActivityDate.Value, newBorn.LastPasswordChangedDate.Value, newBorn.LastLockoutDate.Value);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(oldPassword))
            {
                return false;
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return false;
            }

            var user = _userRepository.GetUser(username);
            if (user == null)
            {
                return false;
            }
            var HashedPassword = user.Password;
            var VerificationSucceeded = (HashedPassword != null && Crypto.VerifyHashedPassword(HashedPassword, oldPassword));
            if (VerificationSucceeded)
            {
                user.PasswordFailuresSinceLastSuccess = 0;
            }
            else
            {
                int Failures = user.PasswordFailuresSinceLastSuccess;
                if (Failures < MaxInvalidPasswordAttempts)
                {
                    user.PasswordFailuresSinceLastSuccess += 1;
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                }
                else if (Failures >= MaxInvalidPasswordAttempts)
                {
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                    user.LastLockoutDate = DateTime.UtcNow;
                    user.IsLockedOut = true;
                }
                _userRepository.SaveChanges();
                return false;
            }
            var NewHashedPassword = Crypto.HashPassword(newPassword);
            if (NewHashedPassword.Length > 128)
            {
                return false;
            }
            user.Password = NewHashedPassword;
            user.LastPasswordChangedDate = DateTime.UtcNow;
            _userRepository.SaveChanges();
            return true;
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            
            var user = _userRepository.GetUser(username);
            if (user == null)
            {
                return false;
            }
            
            if (user.IsLockedOut)
            {
                return false;
            }
            String HashedPassword = user.Password;
            Boolean VerificationSucceeded = (HashedPassword != null && Crypto.VerifyHashedPassword(HashedPassword, password));
            if (VerificationSucceeded)
            {
                user.PasswordFailuresSinceLastSuccess = 0;
                user.LastLoginDate = DateTime.UtcNow;
                user.LastActivityDate = DateTime.UtcNow;
            }
            else
            {
                int Failures = user.PasswordFailuresSinceLastSuccess;
                if (Failures < MaxInvalidPasswordAttempts)
                {
                    user.PasswordFailuresSinceLastSuccess += 1;
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                }
                else if (Failures >= MaxInvalidPasswordAttempts)
                {
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                    user.LastLockoutDate = DateTime.UtcNow;
                    user.IsLockedOut = true;
                }
            }
            _userRepository.SaveChanges();
            return VerificationSucceeded;
        }

        public override bool UnlockUser(string userName)
        {
            var user = _userRepository.GetUser(userName);
            if (user != null)
            {
                user.IsLockedOut = false;
                user.PasswordFailuresSinceLastSuccess = 0;
                _userRepository.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (!(providerUserKey is Guid)) return null;
            
            var user = _userRepository.GetUser((Guid)providerUserKey);
            if (user == null)
            {
                return null;
            }
            else
            {
                if (userIsOnline)
                {
                    user.LastActivityDate = DateTime.UtcNow;
                    _userRepository.SaveChanges();
                }
                return new MembershipUser(System.Web.Security.Membership.Provider.Name, user.UserName, user.Id,
                                          user.Email, null, null, true, user.IsLockedOut, user.CreateDate.Value,
                                          user.LastLoginDate.Value, user.LastActivityDate.Value,
                                          user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value);
            }
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            var user = _userRepository.GetUser(username);
            if (user == null)
            {
                return null;
            }
            else
            {
                if (userIsOnline)
                {
                    user.LastActivityDate = DateTime.UtcNow;
                    _userRepository.SaveChanges();
                }
                return new MembershipUser(System.Web.Security.Membership.Provider.Name, user.UserName, user.Id,
                                          user.Email, null, null, true, user.IsLockedOut, user.CreateDate.Value,
                                          user.LastLoginDate.Value, user.LastActivityDate.Value,
                                          user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value);
            }
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            var user = _userRepository.GetUser(username);
            if (user == null)
            {
                return false;
            }
            else
            {
                _userRepository.DeleteUser(user);
                _userRepository.SaveChanges();
                return true;
            }
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection MembershipUsers = new MembershipUserCollection();

            totalRecords = _userRepository.TotalUsersCount();
            IQueryable<WebUser> Users = _userRepository.GetUsersAsQueryable().Skip(pageIndex * pageSize).Take(pageSize);
            foreach (var user in Users)
            {
                MembershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.UserName, user.Id,
                                          user.Email, null, null, true, user.IsLockedOut, user.CreateDate.Value,
                                          user.LastLoginDate.Value, user.LastActivityDate.Value,
                                          user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
            }

            return MembershipUsers;
        }

        public override int GetNumberOfUsersOnline()
        {
            var DateActive = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(Convert.ToDouble(System.Web.Security.Membership.UserIsOnlineTimeWindow)));

            return _userRepository.GetNumberOfUsersActiveAfter(DateActive);
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection MembershipUsers = new MembershipUserCollection();

            totalRecords = _userRepository.CountUsersWithName(usernameToMatch);
            var Users = _userRepository.UsersWithNamePattern(usernameToMatch).Skip(pageIndex * pageSize).Take(pageSize);
            foreach (var user in Users)
            {
                MembershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.UserName, user.Id,
                                          user.Email, null, null, true, user.IsLockedOut, user.CreateDate.Value,
                                          user.LastLoginDate.Value, user.LastActivityDate.Value,
                                          user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
            }
            
            return MembershipUsers;
        }

        #region Properties

        public override string ApplicationName
        {
            get
            {
                return this.GetType().Assembly.GetName().Name.ToString();
            }
            set
            {
                this.ApplicationName = this.GetType().Assembly.GetName().Name.ToString();
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 0; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 5; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return String.Empty; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override bool EnablePasswordReset
        {
            get { return true; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        #endregion

        #region Not Supported

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new System.NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new System.NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}