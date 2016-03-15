using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugSquish.Models
{
    public class ProjectUsersViewModel
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ApplicationUser> ProjectDevelopers { get; set;}
        public ApplicationUser ProjectManager { get; set; }

        public string[] CurrentDevelopers { get; set; }
        public string CurrentManager { get; set; }

        public MultiSelectList AvailableDevelopers { get; set; }
        public SelectList AvailableManagers { get; set; }
    }

    //public class ManageTicketViewModel
    //{
    //    public int TicketId { get; set; }
    //    public string Name { get; set; }
    //    public string Notes { get; set; }

    //    public string CurrentDeveloper { get; set; }

    //    public SelectList AvailableDevelopers { get; set; }
    //}
}