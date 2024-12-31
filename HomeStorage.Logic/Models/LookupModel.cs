using System.Diagnostics.CodeAnalysis;

namespace HomeStorage.Logic.Models
{
    public class LookupModel<TKey>()
    {
        [SetsRequiredMembers]
        public LookupModel(string name, TKey id) : this()
        {
            Name = name;
            Id = id;
        }

        public required TKey Id { get; set; }
        public required string Name { get; set; }
    }
}
