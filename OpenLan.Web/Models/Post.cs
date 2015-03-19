using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DatePublished { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }

        public virtual bool IsPublished
        {
            get { return this.DatePublished != null; }
        }

        public Post()
        {
            this.DateCreated = DateTime.UtcNow;
        }
    }
}