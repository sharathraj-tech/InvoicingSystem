using InvoicingSystem.Models;

namespace InvoicingSystem.Services
{
    public class CategoryService
    {
        private List<Category> _categories = new List<Category>();

        public CategoryService()
        {
            _categories = new List<Category>()
             {
                 new Category{Id=1,Name="Electronics",Description="Best Electronics"},
                 new Category{Id=2,Name="Kitchen",Description="Best Kitchen products"},
                 new Category{Id=3,Name="Books",Description="Best Books"},
                 new Category{Id=4,Name="Health",Description="Best Fitness products"},
                 new Category{Id=5,Name="Sports",Description="Best Sports"},
             };
        }
        public List<Category> GetAllCategories()
        {
            return _categories;
        }

        public Category GetCategoryById(int id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        public void AddCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            category.Id = GenerateCategoryId();

            _categories.Add(category);
        }

        public void UpdateCategory(int id, Category updatedCategory)
        {
            if (updatedCategory == null)
            {
                throw new ArgumentNullException(nameof(updatedCategory));
            }

            var existingCategory = _categories.FirstOrDefault(c => c.Id == id);
            if (existingCategory == null)
            {
                throw new ArgumentException($"Category with ID {id} not found.");
            }

            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;
        }

        public void DeleteCategory(int id)
        {
            var existingCategory = _categories.FirstOrDefault(c => c.Id == id);
            if (existingCategory == null)
            {
                throw new ArgumentException($"Category with ID {id} not found.");
            }

            _categories.Remove(existingCategory);
        }

        private int GenerateCategoryId()
        {
            return _categories.Count > 0 ? _categories.Max(c => c.Id) + 1 : 1;
        }

        public bool CategoryExists(string categoryName)
        {
            return _categories.Any(c => c.Name == categoryName);
        }
    }
}
