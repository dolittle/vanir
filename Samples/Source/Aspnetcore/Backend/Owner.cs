using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dolittle.SDK.Concepts;

namespace Backend
{
    public class OwnerId : ConceptAs<Guid>
    {

        public static implicit operator OwnerId(Guid id) => new() { Value = id };
    }

    public class Owner
    {
        [Key]
        public OwnerId Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public CommonObject CommonObject { get; set; }

        public int ReadOnlyProperty => 42;
    }
}
