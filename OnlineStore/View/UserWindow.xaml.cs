using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OnlineStore.View
{
    public partial class UserWindow : Window
    {
        private readonly DatabaseContext context;
        private List<ProductCategory> Categories; // Добавленное свойство

        public UserWindow()
        {
            InitializeComponent();
            context = new DatabaseContext();
            LoadProducts();
            LoadCategories(); // Вызываем метод для заполнения списка категорий
        }

        private void LoadCategories()
        {
            // Получаем все категории из базы данных
            Categories = context.ProductCategories.ToList();

            // Добавляем "All" в начало списка
            Categories.Insert(0, new ProductCategory { CategoryName = "All" });

            // Проверяем, есть ли уже элементы в ComboBox
            if (cbCategories.Items.Count == 0)
            {
                // Выводим в консоль для отладки
                foreach (var category in Categories)
                {
                    Console.WriteLine(category.CategoryName);
                }

                // Очищаем ComboBox от всех элементов
                cbCategories.Items.Clear();

                // Добавляем элементы в ComboBox
                foreach (var category in Categories)
                {
                    cbCategories.Items.Add(category);
                }

                cbCategories.SelectedIndex = 0; // Выбираем "All" по умолчанию
            }
        }



        private void LoadProducts()
        {
            // Получаем все продукты из базы данных
            var products = context.Products.Include(p => p.Category).ToList();

            // Устанавливаем источник данных для DataGrid
            dgProducts.ItemsSource = products;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обработчик изменения выбранной категории для сортировки продуктов
            ComboBox comboBox = (ComboBox)sender;
            ProductCategory selectedCategory = (ProductCategory)comboBox.SelectedItem;

            if (selectedCategory != null)
            {
                // Фильтруем продукты по выбранной категории
                var filteredProducts = (selectedCategory.CategoryName == "All")
                    ? context.Products.ToList() // Отображаем все продукты при выборе "All"
                    : context.Products.Where(p => p.Category.CategoryName == selectedCategory.CategoryName).ToList();

                dgProducts.ItemsSource = filteredProducts;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик кнопки поиска
            string searchTerm = txtSearch.Text.ToLower();

            // Фильтруем продукты по введенному запросу
            var filteredProducts = context.Products
                .Where(p => p.ProductName.ToLower().Contains(searchTerm) ||
                            p.Description.ToLower().Contains(searchTerm) ||
                            p.Category.CategoryName.ToLower().Contains(searchTerm))
                .ToList();

            dgProducts.ItemsSource = filteredProducts;
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик кнопки очистки поиска
            txtSearch.Clear();
            LoadProducts();
        }
    }
}
