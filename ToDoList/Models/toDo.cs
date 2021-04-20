using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoList.Models
{
    public class toDo
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public bool Done { get; set; }
        public ApplicationUser User { get; set; }
    }
}