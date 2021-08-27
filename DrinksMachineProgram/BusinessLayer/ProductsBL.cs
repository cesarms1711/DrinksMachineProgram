using DrinksMachineProgram.Entities;
using DrinksMachineProgram.Resources;

using System;
using System.Collections.Generic;
using System.Linq;

namespace DrinksMachineProgram.BusinessLayer
{

    public class ProductsBL : IEntityBL<Product, short>
    {

        #region Private Attributes

        private short MaxId = 0;

        private static List<Product> Products = new();

        #endregion Private Attributes

        #region CTOR

        private ProductsBL() { }

        #endregion CTOR

        #region Singleton Instance

        private static ProductsBL _instance;

        public static ProductsBL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProductsBL();
                }

                return _instance;
            }
        }

        #endregion Singleton Instance

        #region Public Methods

        public List<Product> List()
        {
            return Products;
        }

        public Product Detail(short id)
        {
            Product product = Products.First(p => p.Id == id);

            if (product == null) throw new Exception(TextResources.MessageErrorRecordDoesNotExist);

            return product;
        }

        public void Create(Product product)
        {
            MaxId++;

            product.Id = MaxId;

            Products.Add(product);
        }

        public void Edit(Product product)
        {
            Product modifiedProduct = Products.First(p => p.Id == product.Id);

            modifiedProduct.Name = product.Name;
            modifiedProduct.Cost = product.Cost;
            modifiedProduct.QuantityAvailable = product.QuantityAvailable;
        }

        public void Delete(short id)
        {
            Products = Products
                .Where(p => p.Id != id)
                .ToList();
        }

        #endregion Public Methods

    }

}