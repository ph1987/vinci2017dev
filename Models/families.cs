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
    
    public partial class families
    {
        public int id { get; set; }
        public int perfil_id { get; set; }
        public string title { get; set; }
        public string regulamento { get; set; }
        public string factsheet { get; set; }
        public string lamina_info_essenciais { get; set; }
        public string prospecto { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<System.DateTime> updated { get; set; }
        public Nullable<int> status { get; set; }
    }
}
