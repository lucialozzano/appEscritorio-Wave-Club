
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WaveClubAppEscritorio2.Models
{
    public class UserBond
    {
        [Key]
        public int Id { get; set; }
        public int RemainingClasses { get; set; } = 10;

        public int IdUser { get; set; }
        [ForeignKey("IdUser")]
        public User? User { get; set; }

        public int IdBond { get; set; }
        [ForeignKey("IdBond")]
        public Bond? Bond { get; set; }
    }

    public class AcquireBondRequest
    {
        public int UserId { get; set; }
        public int BondId { get; set; }
        public int RemainingClasses { get; set; }
    }
}
