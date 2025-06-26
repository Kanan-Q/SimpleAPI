using SimpleAPI.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.Core.Entities
{
    public sealed class Category:BaseEntity
    {
        public string CategoryName { get; set; }
        public ICollection<Information> Informations { get; set; }

    }
}
