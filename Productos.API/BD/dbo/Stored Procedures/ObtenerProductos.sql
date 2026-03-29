CREATE PROCEDURE ObtenerProductos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id,
        p.IdSubCategoria,
        sc.Nombre AS SubCategoria,
        c.Id      AS IdCategoria,
        c.Nombre  AS Categoria,
        p.Nombre,
        p.Descripcion,
        p.Precio,
        p.Stock,
        p.CodigoBarras
    FROM dbo.Producto p
    INNER JOIN dbo.SubCategorias sc ON sc.Id = p.IdSubCategoria
    INNER JOIN dbo.Categorias c ON c.Id = sc.IdCategoria
    ORDER BY c.Nombre, sc.Nombre, p.Nombre;
END;