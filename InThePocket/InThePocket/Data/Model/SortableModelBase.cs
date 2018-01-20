using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SQLite;

using System.Threading.Tasks;

namespace InThePocket.Data.Model
{
    public class SortableModelBase : ModelBase, ISortableModel
    {
        [Column("OrderNdx")]
        public int OrderNdx { get; set; }

        public override async Task InitTable()
        {
            await base.InitTable();
            string tableName = this.GetType().Name,
                triggerName = $"{tableName}OrderNdx",
                queryStmt = $"CREATE TRIGGER {triggerName} AFTER INSERT ON {tableName} "
                                + "BEGIN "
                                + $"UPDATE {tableName} "
                                + "SET OrderNdx = NEW.ROWID "
                                + "WHERE ROWID = NEW.ROWID AND OrderNdx = 0; "
                                + "END",
                dropStmt = $"DROP TRIGGER IF EXISTS {triggerName}";

            await DataAccess.Database.ExecuteAsync(dropStmt);
            await DataAccess.Database.ExecuteAsync(queryStmt);
        }

        public static async Task ReorderItem(IEnumerable<ISortableModel> model, int fromIndex, int toIndex)
        {
            List<ISortableModel> orderedItems = (from item in model
                                                 orderby item.OrderNdx
                                                 select item as ISortableModel).ToList();

            // cache of new sort index for mobile item (original item's index).  Shift all other items first
            int mobileOrderIndex = orderedItems[toIndex].OrderNdx;

            int i = toIndex,
                iterator = toIndex > fromIndex ? -1 : 1;

            while (i != fromIndex)
            {
                // set each item's order index to the next one in the direction we are going.
                orderedItems[i].OrderNdx = orderedItems[i + iterator].OrderNdx;
                await orderedItems[i].Save();
                i += iterator;
            }

            orderedItems[fromIndex].OrderNdx = mobileOrderIndex;
            await orderedItems[fromIndex].Save();
        }
    }
}
