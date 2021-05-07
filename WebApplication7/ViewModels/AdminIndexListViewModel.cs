using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApplication7.ViewModels
{
    public class AdminIndexListViewModel
    {
        public List<IndexViewModel> ListIndex { get; set; }
        public class IndexViewModel
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public IList<string> Role { get; set; }
        }
    }
}