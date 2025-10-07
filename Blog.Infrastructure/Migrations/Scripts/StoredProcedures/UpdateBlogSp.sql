CREATE OR ALTER PROCEDURE spUpdateBlog
    @BlogId UNIQUEIDENTIFIER,
    @BlogTitle NVARCHAR(255),
    @BlogContent NVARCHAR(MAX),
    @StartDate DATE,
    @EndDate DATE,
    @AuthorId UNIQUEIDENTIFIER,
    @UpdatedBy UNIQUEIDENTIFIER
    AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CurrentDate DATE = CAST(GETUTCDATE() AS DATE);
    DECLARE @ActiveStatus INT;
     IF (@StartDate <= @CurrentDate AND @EndDate >= @CurrentDate)
        SET @ActiveStatus = 1;
     ELSE
        SET @ActiveStatus = 0;

UPDATE Blogs
SET
    BlogTitle = @BlogTitle,
    BlogContent = @BlogContent,
    AuthorId = @AuthorId,
    StartDate=@StartDate,
    EndDate=@EndDate,
    ActiveStatus=@ActiveStatus,
    UpdatedAt = GETUTCDATE(),
    UpdatedBy = @UpdatedBy
WHERE BlogId = @BlogId;

-- Return updated blog details with Author and UpdatedBy info
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

    -- CreatedBy Info
    cu.Id AS Id,
    cu.Name AS Name,
    cu.Email AS Email,

    -- UpdatedBy Info
    uu.Id AS Id,
    uu.Name AS Name,
    uu.Email AS Email,

    -- Author Info
    a.AuthorId,
    a.AuthorName,
    a.AuthorEmail,
    a.Age,
    a.CreatedBy

FROM Blogs b
         INNER JOIN Authors a ON b.AuthorId = a.AuthorId
         INNER JOIN AspNetUsers cu ON b.CreatedBy = cu.Id
         LEFT JOIN AspNetUsers uu ON b.UpdatedBy = uu.Id
WHERE b.BlogId = @BlogId;
END
