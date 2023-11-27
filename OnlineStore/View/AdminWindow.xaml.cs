using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OnlineStore.View
{
    public partial class AdminWindow : Window
    {
        private readonly DatabaseContext context;

        public AdminWindow()
        {
            InitializeComponent();
            context = new DatabaseContext();
            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = context.Products.Include("Category").ToList();
            dgProducts.ItemsSource = products;

            cbCategories.ItemsSource = context.ProductCategories.ToList();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную категорию из ComboBox
            ProductCategory selectedCategory = (ProductCategory)cbCategories.SelectedItem;

            if (selectedCategory != null)
            {
                // Получаем значения из TextBox
                string productName = txtProductName.Text;
                string productDescription = txtProductDescription.Text;

                // Проверяем, что оба значения из TextBox не пустые
                if (!string.IsNullOrEmpty(productName) && !string.IsNullOrEmpty(productDescription))
                {
                    // Создаем новый продукт с указанием категории
                    Product newProduct = new Product
                    {
                        ProductName = productName,
                        Description = productDescription,
                        CategoryID = selectedCategory.CategoryID
                    };

                    // Добавляем новый продукт в контекст данных
                    context.Products.Add(newProduct);

                    // Сохраняем изменения в базе данных
                    context.SaveChanges();

                    // Перезагружаем список продуктов
                    LoadProducts();
                }
                else
                {
                    MessageBox.Show("Please fill in both product name and description.");
                }
            }
            else
            {
                MessageBox.Show("Please select a category for the new product.");
            }
        }



        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product selectedProduct)
            {
                context.Products.Remove(selectedProduct);
                context.SaveChanges();

                LoadProducts();
            }
        }

        private void btnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product selectedProduct)
            {
                selectedProduct.ProductName = txtProductName.Text;
                selectedProduct.Description = txtProductDescription.Text;

                if (cbCategories.SelectedItem is ProductCategory selectedCategory)
                {
                    selectedProduct.CategoryID = selectedCategory.CategoryID;
                }

                context.SaveChanges();
                LoadProducts();
            }
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product selectedProduct)
            {
                txtProductName.Text = selectedProduct.ProductName;
                txtProductDescription.Text = selectedProduct.Description;
                cbCategories.SelectedItem = selectedProduct.Category;
            }
        }
    }
}
