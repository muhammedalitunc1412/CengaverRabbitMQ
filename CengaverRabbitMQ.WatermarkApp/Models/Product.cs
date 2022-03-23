﻿using System.ComponentModel.DataAnnotations;

namespace CengaverRabbitMQ.WatermarkApp.Models
{
    public class Product
    {
        [Key]
        public int Id{ get; set; }
        public string Name{ get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string PictureUrl { get; set; }
    }
}
