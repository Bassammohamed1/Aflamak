using BusinessLogicLayer.Helpers;

namespace PresentationLayer.ViewModels
{
    public class PersonDetailsVM<T>
    {
        public T Person { get; set; }
        public List<ItemDTO> Works { get; set; }
    }
}
