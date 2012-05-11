using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RadaCode.Web.Data.Entities
{
    public class WebUserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string RoleName { get; set; }

        public virtual string Description { get; set; }

        public virtual IList<WebUser> Users { get; set; } 
    }
}
