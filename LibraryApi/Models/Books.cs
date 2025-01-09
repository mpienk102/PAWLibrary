using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LibraryApi.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string? Title { get; set; }
        
        [Required]
        public string? Author { get; set; }
        
        [Required]
        public string? Description { get; set; }

        // Remove the `true` flag from the JsonConverter here
        [JsonConverter(typeof(StringEnumConverter))]  // Enum will be serialized as its string value
        [Required]
        public BookState State { get; set; }

        [Required]
        public string? Category { get; set; }

        public Book(int id, string title, string author, string description, BookState state, string category)
        {
            Id = id;
            Title = title;
            Author = author;
            Description = description;
            State = state;
            Category = category;
        }

        public Book()
        {
        }
    }

    public enum BookState
    {
        Available,   
        Unavailable, 
        Reserved
    }
}
