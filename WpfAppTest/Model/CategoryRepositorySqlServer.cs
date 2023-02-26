using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfAppTest.Model
{
    public class CategoryRepositorySqlServer : ICategoryRepository
    {
        public Category Add(Category model)
        {
            throw new NotImplementedException();
        }

        public Category Browse(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Category Edit(Category model)
        {
            throw new NotImplementedException();
        }

        public int Has()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> Ordering(SortOrder order)
        {
            throw new NotImplementedException();
        }

        public List<Category> Paging(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public List<Category> Read()
        {
            throw new NotImplementedException();
        }

        public List<Category> Search(string query)
        {
            throw new NotImplementedException();
        }
    }
}
