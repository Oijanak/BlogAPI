CREATE OR ALTER PROCEDURE spCreateBlog
    @AuthorId UNIQUEIDENTIFIER,
    @BlogTitle NVARCHAR(255),
    @BlogContent NVARCHAR(MAX),
    @StartDate DATE,
    @EndDate DATE,
    @CreatedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @BlogId UNIQUEIDENTIFIER = NEWID();
    DECLARE @CurrentDate DATE = CAST(GETUTCDATE() AS DATE);
    DECLARE @ActiveStatus INT;

    IF (@StartDate <= @CurrentDate AND @EndDate >= @CurrentDate)
        SET @ActiveStatus = 1;
ELSE
        SET @ActiveStatus = 0;

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
END
