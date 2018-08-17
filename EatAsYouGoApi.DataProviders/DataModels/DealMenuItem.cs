using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    public class DealMenuItem
    {
        [Key, Column(Order = 1)]
        public long DealId { get; set; }

        [Key, Column(Order = 2)]
        public long MenuItemId { get; set; }

        public virtual Deal Deal { get; set; }

        public virtual MenuItem MenuItem { get; set; }
    }
}