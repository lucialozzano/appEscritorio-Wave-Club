using System.ComponentModel.DataAnnotations;


namespace WaveClubAppEscritorio2.Models
{
    public class Bond
    {
        [Key]
        public int Id { get; set; }

        public string NameActivity { get; set; } = string.Empty;

        public int Level { get; set; }

        public string Description { get; set; } = string.Empty;

        public double Price { get; set; }
    }
}
