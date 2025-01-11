using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string TitleRus { get; set; }
        public string TitleEng { get; set; }
        public string Type { get; set; }
        public string Sound { get; set; }
        public string ImagePath { get; set; }
        public string Link { get; set; }
    }
}
