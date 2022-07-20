using Microsoft.EntityFrameworkCore;

namespace Tekon
{
    public class Controller : DbContext
    {
        public Controller(DbContextOptions<Controller> options) : base(options) { }

        public bool AddItem(Item item, Dictionary<int, Item> Dict)
        {
            if (!Dict.ContainsKey(item.Id))
            {
                if (item.ParentId != null && Dict.ContainsKey((int)item.ParentId)) Dict[(int)item.ParentId].ChildrenId.Add(item.Id);
                else if (item.ParentId != null && !Dict.ContainsKey((int)item.ParentId)) return false;

                Dict.Add(item.Id, item);
                return true;
            }
            return false;
        }
        public bool RemoveItem(int Id, Dictionary<int, Item> Dict)
        {
            if (Dict.ContainsKey(Id))
            {
                if (Dict[Id].ParentId != null)
                {
                    Dict[(int)Dict[Id].ParentId].ChildrenId.Remove(Id);
                    foreach (var child in Dict[Id].ChildrenId)
                    {
                        Dict[child].ParentId = Dict[Id].ParentId;
                        Dict[(int)Dict[Id].ParentId].ChildrenId.Add(child);
                    }
                }
                else
                {
                    foreach (var child in Dict[Id].ChildrenId)
                    {
                        Dict[child].ParentId = null;
                    }
                }

                Dict.Remove(Id);
                return true;
            }
            return false;
        }
        public bool UpdateItem(Item item, Dictionary<int, Item> Dict)
        {
            if (Dict.ContainsKey(item.Id))
            {
                if (Dict[item.Id].ParentId != null)
                {
                    Dict[(int)Dict[item.Id].ParentId].ChildrenId.Remove(item.Id);

                    if (item.ParentId != null)
                    {
                        Dict[(int)item.ParentId].ChildrenId.Add(item.Id);
                    }
                }
                Dict[item.Id].Name = item.Name;
                Dict[item.Id].Description = item.Description;
                Dict[item.Id].ParentId = item.ParentId;

                return true;
            }
            return false;
        }
        public List<Item> ToList(Dictionary<int, Item> Dict)
        {
            List<Item> list = new List<Item>();

            foreach(var item in Dict)
            {
                list.Add(item.Value);
            }

            return list;
        }
    }
}
