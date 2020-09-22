using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebAPI.Entities
{
    public class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        /// <summary>
        /// 学院
        /// </summary>
        public string Dept { get; set; }
    }
}
