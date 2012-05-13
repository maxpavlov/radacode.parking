using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RadaCode.Web.Data.Entities
{
    public class WebUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public virtual string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }

        public virtual int PasswordFailuresSinceLastSuccess { get; set; }

        public virtual DateTime? LastPasswordFailureDate { get; set; }
        public virtual DateTime? LastActivityDate { get; set; }
        public virtual DateTime? LastLockoutDate { get; set; }
        public virtual DateTime? LastLoginDate { get; set; }
        public virtual String ConfirmationToken { get; set; }
        public virtual DateTime? CreateDate { get; set; }
        public virtual Boolean IsLockedOut { get; set; }
        public virtual DateTime? LastPasswordChangedDate { get; set; }
        public virtual String PasswordVerificationToken { get; set; }
        public virtual DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        public virtual IList<WebUserRole> Roles { get; set; }
    }
}
