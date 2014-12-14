namespace FootwearShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Maker
    {
        public Maker()
        {
            this.Footwears = new HashSet<Footwear>();
        }
    
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    
        public virtual ICollection<Footwear> Footwears { get; set; }
    }
}
