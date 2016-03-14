using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugSquish.Models
{
    public class Project
    {
        public Project()
        {
            this.Developers = new HashSet<ApplicationUser>();
            this.Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public string ManagerId { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }

        public virtual ApplicationUser Manager { get; set; }
        public virtual ICollection<ApplicationUser> Developers { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }       
    }

    public class Ticket
    {
        public Ticket()
        {
            this.Changes = new HashSet<TicketChange>();
            this.Comments = new HashSet<TicketComment>();
            this.Attachments = new HashSet<TicketAttachment>();

        }

        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string DeveloperId { get; set; }
        public string ProjectManagerId { get; set; }
        public int ProjectId { get; set; }

        public int TicketTypeId { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }

        public string Title { get; set; }

        [AllowHtml]
        public string Notes { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual ApplicationUser ProjectManager { get; set; }
        public virtual ApplicationUser Developer { get; set; }
        public virtual Project Project { get; set; }

        public virtual TicketType TicketType { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual Status Status { get; set; }

        public virtual ICollection<TicketChange> Changes { get; set; }
        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketAttachment> Attachments { get; set; }
    }

    public class TicketChange
    {

        public int Id { get; set; }
        public int TicketId { get; set; }
        public string EditorId { get; set; }
        public DateTimeOffset EditedTime { get; set; }

        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public virtual ApplicationUser Editor { get; set; }
        public virtual Ticket Ticket {get; set; }
    }

    public class TicketComment
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public int TicketId { get; set; }
        public DateTimeOffset Created { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual Ticket Ticket { get; set; }
        
    }

    public class TicketAttachment
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public int TicketId { get; set; }
        public string AttachmentUrl { get; set; }

        [AllowHtml]
        public string Notes { get; set; }
        public DateTimeOffset Created { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual Ticket Ticket { get; set; }
    }

    public class TicketType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Priority
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SendGridCredentials
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }


}