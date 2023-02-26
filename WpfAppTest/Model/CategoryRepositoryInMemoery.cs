using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace WpfAppTest.Model
{
    internal class CategoryRepositoryInMemoery : ICategoryRepository
    {
        private static List<Category> _categories = new List<Category>();

        public CategoryRepositoryInMemoery()
        {
            _categories = new List<Category>()
            {
                new Category() { CategoryId = 1, CategoryName = "책"},
                new Category() { CategoryId = 2, CategoryName = "강의"},
                new Category() { CategoryId = 3, CategoryName = "컴퓨터"},
            };

        }

        public Category Add(Category model)
        {
            _categories.Add(model);
            return model;
        }

        public Category Browse(int id)
        {
            return _categories.Where(c => c.CategoryId == id).SingleOrDefault(); 
        }

        public void Delete(int id)
        {
            //var category = _categories.FirstOrDefault(c => c.CategoryId == id);

            //if(category != null)
            //    _categories.Remove(category);


            _categories.RemoveAll(p => p.CategoryId == id);
        }

        public Category Edit(Category model)
        {
            var category = _categories.Where(p => p.CategoryId == model.CategoryId)
                                      .FirstOrDefault();

            if (category != null)
            {
                category.CategoryName= model.CategoryName;
            }

            return category;
        }

        public int Has()
        {
            return _categories.Count();
        }


        /// <summary>
        /// 정렬
        /// </summary>
        /// <param name="order">SortOrder 열거형</param>
        /// <returns>읽기전용(IEnumerable)으로 정렬된 레코드셋</returns>
        public IEnumerable<Category> Ordering(SortOrder order)
        {
            IEnumerable<Category> categories;

            switch (order)
            {
                case SortOrder.Ascending:
                    //확장 메서드
                    categories = _categories.OrderBy(p => p.CategoryId);
                    break;
                case SortOrder.Descending:
                    categories = (from category in _categories
                                  orderby category.CategoryId descending
                                  select category);

                    break;
                case SortOrder.None:
                    categories = _categories;
                    break;
                default:
                    categories = null;
                    break;
            }



            return categories;
        }


        /// <summary>
        /// 페이징
        /// </summary>
        /// <param name="pageNumber">페이지 번호 : 1,2,3,4</param>
        /// <param name="pageSize">페이지 크기 : 한 페이지 당 10개 씩 표시</param>
        /// <returns>페이징 처리된 레코드 셋</returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<Category> Paging(int pageNumber = 1, int pageSize = 10)
        {
            return _categories.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
        }

        public List<Category> Read()
        {
            return _categories;
        }

        public List<Category> Search(string query)
        {
            return _categories.Where(category => category.CategoryName.Contains(query)).ToList();
        }

    }

    public class MainClass
    {
        static CategoryRepositoryInMemoery _category;

        public static void MainTest()
        {
            _category = new CategoryRepositoryInMemoery();

            Console.WriteLine($"[1]기본값이 있는지 확인: {_category.Has() > 0}");
            HasCategory();

            Console.WriteLine($"[2]기본 데이터 출력");
            ReadCategories();

            Console.WriteLine($"[3]데이터 입력");
            AddCategory();

            Console.WriteLine($"[4]상세 보기");
            AddCategory();

            Console.WriteLine($"[5]데이터 수정");
            EditCategory();


            //Console.WriteLine($"[6]데이터 삭제");
            //DeleteCategory();

            Console.WriteLine($"[7]데이터 검색");
            SearchCategories();

            Console.WriteLine($"[8]페이징");
            PagingCategories();

            Console.WriteLine($"[9]정렬");
            OrderingCategories();
        }

        /// <summary>
        /// [1] 여부
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void HasCategory()
        {
            if(_category.Has() > 0)
            {
                Console.WriteLine("기본데이터가 있습니다.");
            }
            else
            {
                Console.WriteLine("기본데이터가 없습니다.");

            }
        }

        /// <summary>
        /// [2] 출력
        /// </summary>
        public static void ReadCategories()
        {
            var categories = _category.Read();

            PrintCategories(categories);
        }

        public static void AddCategory()
        {
            var category = new Category()
            {
                CategoryId = 4,
                CategoryName = "생활용품"
            };

            _category.Add(category);    

            ReadCategories();
        }

        /// <summary>
        /// Browse
        /// </summary>
        private static void BrowseCategory()
        {
            var cateogry = _category.Browse(4);

            if(cateogry != null ) 
            { 
                Console.WriteLine($"{cateogry.CategoryId} - {cateogry.CategoryName}");

            }
            else
            {
                Console.WriteLine("4번 카테고리가 없습니다.");

            }
        }

        private static void EditCategory()
        {
            _category.Edit(new Category { CategoryId = 4, CategoryName = "가전용품" });

            ReadCategories();
        }

        private static void DeleteCategory()
        {
            _category.Delete(1);
            ReadCategories();
        }

        private static void SearchCategories()
        {
            var categories = _category.Search("좋은");

            PrintCategories(categories);
        }

        private static void PrintCategories(List<Category> categories)
        {
            foreach (var category in categories)
            {
                Console.WriteLine($"{category} - {category.CategoryName}");

            }
        }

        private static void PagingCategories()
        {
            var categories = _category.Paging(2, 2);
            if(categories.Count > 1)
            {
                categories.RemoveAt(0);
            }

            PrintCategories(categories);
        }

        private static void OrderingCategories()
        {
            var categories = _category.Ordering(SortOrder.Descending);

            PrintCategories(categories.ToList());
        }
    }

}
