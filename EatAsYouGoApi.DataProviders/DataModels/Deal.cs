using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    
    public class Deal
    {
        [Key]
        public long DealId { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        [Required]
        public string DealName { get; set; }

        [Required]
        public DateTime DealDate { get; set; }

        [Required]
        public TimeSpan TimeFrom { get; set; }

        [Required]
        public TimeSpan TimeTo { get; set; }
        
        public TimeSpan? OrderByTime { get; set; }

        public int? MaxOrder { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public virtual ICollection<DealMenuItem> DealMenuItems { get; set; }
    }
}
