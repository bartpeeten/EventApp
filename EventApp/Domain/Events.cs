using System;
using System.Collections.Generic;
using System.Text;

namespace EventApp.Domain
{
    class Events
    {
        public int EventId { get; set; }
        public String Description { get; set; }
        public String Location { get; set; }
        public DateTime DateTime { get; set; }
    }
}
