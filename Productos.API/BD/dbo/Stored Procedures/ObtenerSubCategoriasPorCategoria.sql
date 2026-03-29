CREATE PROCEDURE ObtenerSubCategoriasPorCategoria
    @IdCategoria UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        IdCategoria,
        Nombre
    FROM dbo.SubCategorias
    WHERE IdCategoria = @IdCategoria
    ORDER BY Nombre;
END