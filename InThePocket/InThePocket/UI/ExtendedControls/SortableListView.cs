using System;
using System.Collections.Generic;
using System.Text;

using Syncfusion.ListView.XForms;
using InThePocket.Data.Model;

using System.Collections.ObjectModel;
using System.Windows.Markup;

using System.Threading.Tasks;

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
            if (e.NewIndex != e.OldIndex & e.Action == Syncfusion.ListView.XForms.DragAction.Drop)
            {
                // Even though this is a SortableModelCollection, casting to SortableModelCollection is a bit tricky, so just use the static method from the ObservableCollection cast
                var itemSource = this.ItemsSource as IEnumerable<ISortableModel>;
                if (itemSource == null)
                {
                    throw new NotSupportedException("Attempt to sort a SortableListView with an ItemsSource that cannot be cast to an IEnumerable of ISortableModel.  ItemsSource must use ISortableModels to support reordering.");
                }
                await SortableModelBase.ReorderItem(itemSource, e.OldIndex, e.NewIndex);
            }
        }
    }
}
