namespace Libro.Models
{
    class Category : ModelBase<Category>
    {
        public string Number { get; set; }
        public string Name { get; set; }

        protected override bool GetIsEmpty()
        {
            return false;
        }
    }
}
