using Moq;

namespace TestingBasic.Test;

public class ShoppingCartMoqTest
{
    public readonly Mock<IDbService> _dbServiceMock = new();

    [Fact]
    public void AddProduct_Success()
    {
        // Arrange
        var product = new Product(0, "Shoes", 200);
        _dbServiceMock.Setup(x => x.SaveShoppingCartItem(product)).Returns(true);
        ShoppingCart shoppingCart = new(_dbServiceMock.Object);

        // Act
        var result = shoppingCart.AddProduct(product);

        // Assert
        Assert.True(result);
        _dbServiceMock.Verify(x => x.SaveShoppingCartItem(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public void AddProduct_Failure_InvalidPayload()
    {
        // Arrange
        ShoppingCart shoppingCart = new(_dbServiceMock.Object);

        // Act
        var result = shoppingCart.AddProduct(null);

        // Assert
        Assert.False(result);
        _dbServiceMock.Verify(x => x.SaveShoppingCartItem(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public void RemoveProduct_Success()
    {
        var product = new Product(1, "Shoes", 200);
        _dbServiceMock.Setup(x => x.RemoveShoppingCartItem(product.Id)).Returns(true);

        // Arrange
        ShoppingCart shoppingCart = new(_dbServiceMock.Object);

        // Act
        var result = shoppingCart.DeleteProduct(product.Id);

        // Assert
        Assert.True(result);
        _dbServiceMock.Verify(x => x.RemoveShoppingCartItem(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void RemoveProduct_Failed()
    {
        _dbServiceMock.Setup(x => x.RemoveShoppingCartItem(null)).Returns(false);

        // Arrange
        ShoppingCart shoppingCart = new(_dbServiceMock.Object);

        // Act
        var result = shoppingCart.DeleteProduct(null);

        // Assert
        Assert.False(result);
        _dbServiceMock.Verify(x => x.RemoveShoppingCartItem(null), Times.Never);
    }

    [Fact]
    public void RemoveProduct_Failed_InvalidId()
    {
        _dbServiceMock.Setup(x => x.RemoveShoppingCartItem(null)).Returns(false);

        // Arrange
        ShoppingCart shoppingCart = new(_dbServiceMock.Object);

        // Act
        var result = shoppingCart.DeleteProduct(0);

        // Assert
        Assert.False(result);
        _dbServiceMock.Verify(x => x.RemoveShoppingCartItem(null), Times.Never);
    }
}