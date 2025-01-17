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

        [JsonConverter(typeof(StringEnumConverter))]
        [Required]
        public BookState State { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [Required]
        public BookCategory Category { get; set; }

        public Book(int id, string title, string author, string description, BookState state, BookCategory category)
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

    public enum BookCategory
    {
        Comedy,
        Fantasy,
        Horror,
        Thriller
    }
}
