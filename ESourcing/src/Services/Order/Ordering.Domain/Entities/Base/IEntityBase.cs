using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities.Base
{
    public abstract class IEntityBase
    {
        public int Id { get; set; }
        //public IEntityBase Clone()
        //{
        //    return (IEntityBase)this.MemberwiseClone();
        //}
         
    }
}
