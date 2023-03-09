﻿namespace CIWeb.Models
{
    public class PaginationList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public string searchTerm { get; set; }

        public PaginationList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginationList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count;
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginationList<T>(items, count, pageIndex, pageSize);

        }
    }
}
