using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Models
{
    public class TemplateFilm
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Date { get; set; }
        public string Country { get; set; }
        public string Genre { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public string LinkToPlayer { get; set; }
    }
}
