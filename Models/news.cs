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
    
    public partial class news
    {
        public int id { get; set; }
        public string category { get; set; }
        public string txt1 { get; set; }
        public string txt1_en { get; set; }
        public string txt2 { get; set; }
        public string txt2_en { get; set; }
        public string videoUrl { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<System.DateTime> updated { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> position { get; set; }
        public string highlight { get; set; }
    }
}