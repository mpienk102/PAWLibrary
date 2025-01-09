using LibraryApi.Models;

namespace LibraryApi.DTOs
{
    public class CreateBookDTO
    {
        public string? Title {get; set;}
        public string? Author {get; set;}
        public string? Description {get; set;}
        public BookState State{get; set;}
        public string? Category {get; set;}
    }
}