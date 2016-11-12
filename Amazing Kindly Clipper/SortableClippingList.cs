using System.Collections.ObjectModel;

namespace ClippingManager {

    public class SortableClippingList : ObservableCollection<Clipping> {
        // Creating the Tasks collection in this fashion enables data binding from XAML.
    }
}