using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Entites
{
    public class NewsFeedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsId { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [MinLength(10)]
        public string Title { get; set; }
        [Required]
        [MinLength(50)]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [Required]
        public string HashTags { get; set; }
    }
}
