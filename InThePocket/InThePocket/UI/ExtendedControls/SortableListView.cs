using System;
using System.Collections.Generic;
using Syncfusion.ListView.XForms;
using InThePocket.Data.Model;
using Syncfusion.DataSource.Extensions;
using Syncfusion.ListView.XForms.Helpers;
using System.Linq;

namespace InThePocket.UI.ExtendedControls
{
    public class SortableListView : SfListView
    {
        SortableListView() : base()
        {
            this.ItemDragging += SortableListView_ItemDragging;
        }

        /// <summary>
        /// Event thrown during events while the item is dragging.
        /// Note: This is virtual to support overriding by subclasses, if necessary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual async void SortableListView_ItemDragging(object sender, ItemDraggingEventArgs e)
        {
            if (e.Action == DragAction.Dragging)
            {
                var currentGroup = this.GetGroup(e.ItemData);
                VisualContainer container = this.GetVisualContainer() as VisualContainer;
                var groupIndex = DataSource.Groups.IndexOf(currentGroup);
                var nextGroup = (groupIndex + 1 < DataSource.Groups.Count) ? DataSource.Groups[groupIndex + 1] : null;
                ListViewItem groupItem = null;
                ListViewItem nextGroupItem = null;

                foreach (ListViewItem item in container.Children)
                {
                    if (item.BindingContext.Equals(currentGroup))
                        groupItem = item;

                    if (nextGroup != null && item.BindingContext.Equals(nextGroup))
                        nextGroupItem = item;
                }

                if (groupItem != null && e.Bounds.Y <= groupItem.Y + groupItem.Height || nextGroupItem != null && (e.Bounds.Y + e.Bounds.Height >= nextGroupItem.Y))
                    e.Handled = true;
            }
            else if (e.NewIndex != e.OldIndex & e.Action == Syncfusion.ListView.XForms.DragAction.Drop)
            {
                // Even though this is a SortableModelCollection, casting to SortableModelCollection is a bit tricky, so just use the static method from the ObservableCollection cast
                var itemSource = this.ItemsSource as IEnumerable<ISortableModel>;
                if (itemSource == null)
                {
                    throw new NotSupportedException("Attempt to sort a SortableListView with an ItemsSource that cannot be cast to an IEnumerable of ISortableModel.  ItemsSource must use ISortableModels to support reordering.");
                }

                // adjust indexes to remove group headers.
                List<int> groupIndexes = GetGroupIndexes();
                int oldIndexOffset = groupIndexes.Where(test => test < e.OldIndex).Count();
                int newIndexOffset = groupIndexes.Where(test => test < e.NewIndex).Count();

                await SortableModelBase.ReorderItem(itemSource, e.OldIndex - oldIndexOffset, e.NewIndex - newIndexOffset);
            }
        }

        private GroupResult GetGroup(object itemData)
        {
            GroupResult itemGroup = null;

            foreach (var item in DataSource.DisplayItems)
            {
                if (item is GroupResult)
                    itemGroup = item as GroupResult;

                if (item == itemData)
                    break;
            }
            return itemGroup;
        }


        private List<int> GetGroupIndexes()
        {
            GroupResult itemGroup = null;
            List<int> ret = new List<int>();

            int i = 0;
            foreach (var item in DataSource.DisplayItems)
            {
                if (item is GroupResult)
                    ret.Add(i);
                ++i;
            }
            return ret;
        }
    }
}
