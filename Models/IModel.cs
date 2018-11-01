namespace Libro.Models
{
     public interface IModel
    {
        bool IsEmpty { get; }
         bool IsValid { get; }
         void Save();
         void Remove();
    }
}
