using System;

namespace Console.Models
{
    public class Job
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Salary { get; set; }

        public string Company { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }
    }
}