namespace Tekon
{
    public class Item
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public List<int> ChildrenId { get; set; } = new();
    }
}
