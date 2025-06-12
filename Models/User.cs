
using System.Text.Json.Serialization;

namespace WaveClubAppEscritorio2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        [JsonPropertyName("DNA")]
        public string Dna { get; set; }
        public string Rol { get; set; }
        public string EmailAdress { get; set; }
    }
}

