using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugSquish.Models
{
    public class Project
    {
        public Project()
        {
            this.Developers = new HashSet<ApplicationUser>();
            this.Tickets = new HashSet<Ticket>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ApplicationUser Manager { get; set; }

        public virtual ICollection<ApplicationUser> Developers { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        


    }

    public class Ticket
    {
        public Ticket()
        {
            this.Changes = new HashSet<TicketChange>();
            this.Comments = new HashSet<Comment>();
            this.Attachments = new HashSet<Attachment>();

        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int DevId { get; set; }
        public int ProjectId { get; set; }

        public int TypeId { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }

        public string Title { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<TicketChange> Changes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }

    public class 
}