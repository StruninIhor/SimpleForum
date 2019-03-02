using System.ComponentModel.DataAnnotations;

namespace Web.Models.Topic
{
    public class CommentViewModel
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public int TopicId { get; set; }
    }
}