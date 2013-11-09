using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
    }

    public class Attribute
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Image { get; set; }
    }

    public class Place
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Address { get; set; }
        
    }
}
