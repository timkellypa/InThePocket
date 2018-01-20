using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace InThePocket.Data.Model
{
    public interface ISortableModel : IModel
    {
        int OrderNdx { get; set; }
    }
}
