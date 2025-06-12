

namespace WaveClubAppEscritorio2.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int IdActivity { get; set; }
        public Activity? Activity { get; set; }
        public int IdUser { get; set; }
        public User? User { get; set; }
    }
}
