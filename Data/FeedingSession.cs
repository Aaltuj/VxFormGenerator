using System;
namespace FormGeneratorDemo.Data
{
    public class FeedingSession
    {
        public FoodKind KindOfFood { get; set; }
        public string Note { get; set; }
        public decimal Amount { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public enum FoodKind
    {
        SOLID,
        LIQUID
    }
}
