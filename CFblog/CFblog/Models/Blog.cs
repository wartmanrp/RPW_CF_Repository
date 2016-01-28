using System;
using System.Collections.Generic;
using System.Web.Mvc;



namespace CFblog.Models
{
    public class Post
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.Categories = new HashSet<Category>();
        }

        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string MediaUrl { get; set; }
        public bool Published { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class Category
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }

    public class Comment
    {
        public Comment()
        {
            this.Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorId { get; set; }
        public string EditorId { get; set; }
        public int ParentCommentId { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public bool MarkForDeletion { get; set; }


        public Post Post { get; set; }
        public ApplicationUser Author { get; set; }
        public ApplicationUser Editor { get; set; }
        public Comment ParentComment { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}