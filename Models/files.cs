//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vinci_novo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class files
    {
        public int id { get; set; }
        public string filename { get; set; }
        public int content_id { get; set; }
        public string reference { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<System.DateTime> updated { get; set; }
        public int status { get; set; }
        public Nullable<int> position { get; set; }
    }
}
