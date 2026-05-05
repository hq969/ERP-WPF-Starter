using ERP.WPF.ViewModels;

namespace ERP.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public InventoryViewModel InventoryView { get; } = new();
    }
}
