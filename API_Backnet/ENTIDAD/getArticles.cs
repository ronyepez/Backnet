using System.ComponentModel;

namespace ENTIDAD
{
    public class getArticles
    {
        [DisplayName("Cód.")]
        public int id { get; set; }

        [DisplayName("Name")]
        public string name { get; set; }

        [DisplayName("Description")]
        public string description { get; set; }

        [DisplayName("Cód.")]
        public double price { get; set; }

        [DisplayName("Total in Shelf")]
        public int total_in_shelf { get; set; }

        [DisplayName("Total in Vault")]
        public int total_in_vault { get; set; }

        [DisplayName("Store")]
        public int store_id { get; set; }

        //public ICollection<getStores> Stories { get; set; }
    }
}
