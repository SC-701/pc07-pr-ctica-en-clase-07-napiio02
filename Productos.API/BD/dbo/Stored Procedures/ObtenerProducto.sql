CREATE PROCEDURE ObtenerProducto
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Producto.Id, 
        Producto.IdSubCategoria, 
        Producto.Nombre, 
        Producto.Descripcion, 
        Producto.Precio, 
        Producto.Stock, 
        Producto.CodigoBarras, 
        SubCategorias.Nombre AS SubCategoria, 
        Categorias.Nombre AS Categoria
    FROM 
        dbo.Producto 
    INNER JOIN 
        dbo.SubCategorias ON Producto.IdSubCategoria = SubCategorias.Id 
    INNER JOIN 
        dbo.Categorias ON SubCategorias.IdCategoria = Categorias.Id
    WHERE 
        Producto.Id = @Id;
END;