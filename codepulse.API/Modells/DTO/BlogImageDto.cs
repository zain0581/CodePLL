namespace codepulse.API.Modells.DTO
{
    public class BlogImageDto
    {
        public Guid Id { get; set; }
        public string FileNmae { get; set; }
        public string FileExtension { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
