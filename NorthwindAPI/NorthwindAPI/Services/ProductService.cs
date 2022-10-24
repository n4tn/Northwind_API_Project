﻿using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;

namespace NorthwindAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _context;

        public ProductService()
        {
            _context = new NorthwindContext();
        }
        public ProductService(NorthwindContext context)
        {
            _context = context;
        }
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryIdAsync(int id)
        {
            return await _context.Products.Include(x=> x.Category).Where(x => x.CategoryId == id).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
        }

        public async Task<Product?> GetProductByNameAsync(string name)
        {
            return await _context.Products
                .Include(p => p.OrderDetails)
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .Where(p => p.ProductName == name)
                .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Product>> GetProductBySupplierIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.OrderDetails)
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .Where(p => p.SupplierId == id)
                .ToListAsync();
        }

        public bool ProductsExsits(int id)
        {
            return _context.Products.Any(p => p.ProductId == id);
        }

        public async Task RemoveProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void ModifyState(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}