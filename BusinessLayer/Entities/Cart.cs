using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;

namespace BusinessLayer.Entities
{
    public class Cart
    {
        private List<CartLine> lines = new List<CartLine>();
    

        public void AddItem(ProductBLL product, int quantity)
        {
            CartLine line = lines.Where(p => p.Product.Id == product.Id).FirstOrDefault();

            if(line == null)
            {
                lines.Add(new CartLine()
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(ProductBLL product)
        {
            lines.RemoveAll(l => l.Product.Id == product.Id);
        }

        public int ComputeTotalQuantity()
        {
            return lines.Sum(q => q.Quantity);
        }
        public int ComputeTotalValue()
        {
            return lines.Sum(e => e.Product.Price * e.Quantity);

        }
        public void Clear()
        {
            lines.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lines; }
        }
    }

    public class CartLine
    {
        public ProductBLL Product;
        public int Quantity;
    }
}
