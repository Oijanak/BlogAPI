CREATE OR ALTER PROCEDURE spCreateBlog
    @AuthorId UNIQUEIDENTIFIER,
    @BlogTitle NVARCHAR(255),
    @BlogContent NVARCHAR(MAX),
    @StartDate DATE,
    @EndDate DATE,
    @CreatedBy UNIQUEIDENTIFIER,
    @CategoryIds NVARCHAR(MAX),
    @DocumentIds NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @BlogId UNIQUEIDENTIFIER = NEWID();
    DECLARE @CurrentDate DATE = CAST(GETUTCDATE() AS DATE);
    DECLARE @ActiveStatus INT;

    IF (@StartDate <= @CurrentDate AND @EndDate >= @CurrentDate)
        SET @ActiveStatus = 0;
ELSE
        SET @ActiveStatus = 1;

INSERT INTO Blogs (
    BlogId,
    BlogTitle,
    BlogContent,
    AuthorId,
    StartDate,
    EndDate,
    ActiveStatus,
    ApproveStatus,
    CreatedBy,
    CreatedAt
)
VALUES (
           @BlogId,
           @BlogTitle,
           @BlogContent,
           @AuthorId,
           @StartDate,
           @EndDate,
           @ActiveStatus,
           0, 
           @CreatedBy,
           GETUTCDATE()
       );


INSERT INTO BlogCategories (BlogsBlogId, CategoriesCategoryId)
SELECT @BlogId, CAST(value AS UNIQUEIDENTIFIER)
FROM STRING_SPLIT(@CategoryIds, ',');



SELECT
    b.BlogId,
    b.BlogTitle,
    b.BlogContent,
    b.CreatedAt,
    b.UpdatedAt,
    b.StartDate,
    b.EndDate,
    b.ApproveStatus,
    b.ActiveStatus,
    -- User who created it
    cu.Id as Id,
    cu.Name as Name,
    cu.Email AS Email,
    -- Author info
    a.AuthorId,
    a.AuthorName,
    a.AuthorEmail,
    a.Age,
    a.CreatedBy
      
FROM Blogs b
         INNER JOIN Authors a ON b.AuthorId = a.AuthorId
         INNER JOIN AspNetUsers cu ON b.CreatedBy = cu.Id
WHERE b.BlogId = @BlogId;

SELECT
    c.CategoryId,
    c.CategoryName
FROM BlogCategories bc
         INNER JOIN Categories c ON bc.CategoriesCategoryId = c.CategoryId
WHERE bc.BlogsBlogId = @BlogId;
    

END
