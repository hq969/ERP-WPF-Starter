using ERP.WPF.Commands;
using ERP.WPF.Data;
using ERP.WPF.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace ERP.WPF.ViewModels
{
    public class InventoryViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly AppDbContext _context;
        private string _newProductName = "";
        private string _newProductQuantity = "";
        private string _newProductPrice = "";

        public InventoryViewModel()
        {
            _context = new AppDbContext();
            LoadProducts();
            AddProductCommand = new RelayCommand(AddProduct, CanAddProduct);
        }

        // Collections
        public ObservableCollection<Product> Products { get; } = new();

        // Commands
        public ICommand AddProductCommand { get; }

        // Properties
        public int ProductCount => Products.Count;
        public int TotalQuantity => Products.Sum(p => p.Quantity);
        public decimal TotalInventoryValue => Products.Sum(p => p.TotalValue);

        public string NewProductName
        {
            get => _newProductName;
            set => SetProperty(ref _newProductName, value);
        }

        public string NewProductQuantity
        {
            get => _newProductQuantity;
            set => SetProperty(ref _newProductQuantity, value);
        }

        public string NewProductPrice
        {
            get => _newProductPrice;
            set => SetProperty(ref _newProductPrice, value);
        }

        private bool CanAddProduct()
        {
            return !string.IsNullOrWhiteSpace(NewProductName) &&
                   int.TryParse(NewProductQuantity, out _) &&
                   decimal.TryParse(NewProductPrice?.Replace("$", "").Replace(",", ""), out _);
        }

        private void AddProduct()
        {
            if (int.TryParse(NewProductQuantity, out int quantity) &&
                decimal.TryParse(NewProductPrice.Replace("$", "").Replace(",", ""), out decimal price))
            {
                var product = new Product
                {
                    Name = NewProductName.Trim(),
                    Quantity = quantity,
                    Price = price
                };

                _context.Products.Add(product);
                _context.SaveChanges();

                Products.Add(product);
                ClearForm();
            }
        }

        private void ClearForm()
        {
            NewProductName = "";
            NewProductQuantity = "";
            NewProductPrice = "";
        }

        private void LoadProducts()
        {
            foreach (var product in _context.Products)
            {
                Products.Add(product);
            }
        }

        public new event PropertyChangedEventHandler? PropertyChanged;
    }
}
