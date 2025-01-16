using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LibraryApi.Models;

namespace LibraryApi.DTOs
{
    public class CreateBookDTO
    {
        public string? Title {get; set;}
        public string? Author {get; set;}
        public string? Description {get; set;}

        [JsonConverter(typeof(StringEnumConverter))] 
        public BookState State{get; set;}
        [JsonConverter(typeof(StringEnumConverter))] 
        public BookCategory Category {get; set;}
    }
}