using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication7.ViewModels
{
    public class AdminEditUserViewModel
    {
        public string Id { get; set; }

        [MaxLength(256)]
        public string UserName { get; set; }

        [MaxLength(256), EmailAddress]
        public string Email { get; set; }

        public string IsInRole { get; set; }
        public List<SelectListItem> AllRoles { get; set; }
    }
}