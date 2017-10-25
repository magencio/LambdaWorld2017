namespace SWAPISDK.Entities
{
    public class PagedResults<T>
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public T[] Results { get; set; }
    }
}
