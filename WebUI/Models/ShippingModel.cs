using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace WebUI.Models
{
    public class ShippingModel
    {
        [Required(ErrorMessage = "Укажите как вас зовут")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите место доставки")]
        public string Place { get; set; }
        

        public string Note { get; set; }

        public bool GiftWrap { get; set; }
    }
}