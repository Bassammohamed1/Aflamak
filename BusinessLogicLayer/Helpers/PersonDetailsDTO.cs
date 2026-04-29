
namespace BusinessLogicLayer.Helpers
{
    public class PersonDetailsDTO<T>
    {
        public T Person { get; set; }
        public List<ItemDTO> Works { get; set; }
    }
}
