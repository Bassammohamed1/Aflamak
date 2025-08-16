namespace DataAccessLayer.Models.ViewModels
{
    public class PersonDetailsVM<T>
    {
        public T Person { get; set; }
        public List<ItemViewModel> Works { get; set; }
    }
}
