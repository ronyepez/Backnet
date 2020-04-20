using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace ENTIDAD
{
    public class getStores
    {
        [DisplayName("Cód.")]
        public int id { get; set; }

        [DisplayName("Name")]
        public string name { get; set; }

        [DisplayName("Address")]
        public string address { get; set; }

        //public getArticles Article { get; set; }
    }
}
