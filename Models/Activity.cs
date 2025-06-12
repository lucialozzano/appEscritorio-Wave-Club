

namespace WaveClubAppEscritorio2.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RemainingClasses { get; set; }
        public int Capacity { get; set; }
        public int Level { get; set; }
        public DateTime Date { get; set; }
        public string DayWeek { get; set; } = string.Empty;
        public string Start { get; set; } = string.Empty;
        public string End { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
    }
}
