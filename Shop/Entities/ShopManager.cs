using System.Collections.Generic;
using System.Linq;
using Shop.Entities;
using Shop.Exceptions;

namespace Shop.Entities;

public class ShopManager
{
    private int _nextShopId = 1;
    private int _nextProductId = 1;
    private Dictionary<int, Shop> _shopsByShopId = new Dictionary<int, Shop>();
    public Shop CreateShop(string name, string street, int buildingNumber)
    {
        if (string.IsNullOrEmpty(name))
            ShopManagerException.InvalidShopName();
        if (string.IsNullOrEmpty(street))
            ShopManagerException.InvalidStreet();
        if (buildingNumber <= 0)
            ShopManagerException.InvalidBuildingNumber();
        var shop = new Shop(name, _nextShopId, street, buildingNumber);
        _shopsByShopId.Add(_nextShopId++, shop);
        return shop;
    }

    public Product RegisterProduct(string productName, int price)
    {
        if (string.IsNullOrEmpty(productName))
            ShopManagerException.InvalidProductName();
        if (price <= 0)
            ShopManagerException.InvalidPrice();
        var product = new Product(productName, _nextProductId++, price);
        foreach (var shop in _shopsByShopId.Values)
        {
            shop.RegisterProductInThisShop(productName, product.Id, price);
        }

        return product;
    }

    public Shop GetShopWithCheapestSetOfProducts(params BatchOfGoods[] batchOfGoodsArray)
    {
        int? minPrice = null;
        Shop result = null;
        foreach (var shop in _shopsByShopId.Values)
        {
            bool doesShopSuit = true;
            for (int i = 0; i < batchOfGoodsArray.Length; i++)
            {
                if (shop.FindProduct(batchOfGoodsArray[i].ProductId).ProductNumber < batchOfGoodsArray[i].ProductNumber)
                    doesShopSuit = false;
            }

            if (!doesShopSuit)
            {
                continue;
            }

            int shopPrice = batchOfGoodsArray.Sum(batch => shop.FindProduct(batch.ProductId).Price * batch.ProductNumber);
            if (!minPrice.HasValue || minPrice > shopPrice)
            {
                minPrice = shopPrice;
                result = shop;
            }
        }

        return result;
    }

    public Shop GetShopWithCheapestProduct(int productId)
    {
        if (_shopsByShopId.Count == 0)
            ShopManagerException.NoShopsYet();
        int min = _shopsByShopId.Values.Min(x => x.FindProduct(productId).Price);
        return _shopsByShopId.Values.FirstOrDefault(x => x.FindProduct(productId).Price == min)
              ?? throw new ShopManagerException($"product with {productId} id not found");
    }
}