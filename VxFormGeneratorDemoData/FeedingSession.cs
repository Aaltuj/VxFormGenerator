
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VxFormGenerator.Core;
using VxFormGenerator.Models;

namespace VxFormGeneratorDemoData
{
    public class FeedingSession
    {
        [Display(Name = "Kind of food")]
        public FoodKind KindOfFood { get; set; }

        [Display(Name = "Note")]
        [MinLength(5)]
        public string Note { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
        [Display(Name = "Start")]
        public DateTime Start { get; set; }
        [Display(Name = "End")]
        public DateTime End { get; set; }
        [Display(Name = "Throwing up")]
        public bool ThrowingUp { get; set; }

        [Display(Name = "Throwing up dict")]
        public IDictionary<bool, string> ThrowingUpDict { get; set; } = new Dictionary<bool, string>();

        [Display(Name = "Color")]
        public VxColor Color { get; set; }
    }

    [Flags]
    public enum FoodKind
    {
        SOLID,
        LIQUID
    }
}
