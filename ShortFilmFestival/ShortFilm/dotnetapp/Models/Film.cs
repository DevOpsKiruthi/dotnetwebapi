namespace dotnetapp.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }          // Title of the short film
        public string Director { get; set; }       // Name of the film's director
        public int Duration { get; set; }          // Duration in minutes
        public string Synopsis { get; set; }       // Brief summary of the film
        public string Genre { get; set; }          // Film genre (Drama, Mystery, etc.)
        public int VoteCount { get; set; }         // Total votes received
    }
}
