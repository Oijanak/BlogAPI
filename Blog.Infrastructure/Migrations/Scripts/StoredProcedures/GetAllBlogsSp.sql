CREATE OR ALTER PROCEDURE spGetBlogList
(
    @StartDate DATE = NULL,
    @EndDate DATE = NULL,
    @CreatedBy NVARCHAR(100) = NULL,
    @ApprovedBy NVARCHAR(100) = NULL,
    @ApproveStatus INT = NULL,
    @ActiveStatus BIT = NULL,
    @SortBy NVARCHAR(50) = NULL,
    @SortOrder NVARCHAR(10) = 'asc',
    @Page INT = 1,
    @Limit INT = 10
)
    AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@Page - 1) * @Limit;

    WITH FilteredBlogs AS
                                  (
                                      SELECT
                                          b.BlogId,
                                          b.BlogTitle,
                                          b.BlogContent,
                                          b.CreatedAt,
                                          b.UpdatedAt,
                                          b.StartDate,
                                          b.EndDate,
                                          b.ActiveStatus,
                                          b.ApproveStatus,
                                          a.AuthorId,
                                          a.AuthorName,
                                          a.AuthorEmail,
                                          a.Age,
                                          a.CreatedBy,
                                          cu.Id as CreatedById,
                                          cu.Name as CreatedByName,
                                          cu.Email as CreatedByEmail,
                                          uu.Id as UpdatedById,
                                          uu.Name as UpdatedByName,
                                          uu.Email as UpdatedBYEmail,
                                          auu.Id as ApprovedById,
                                          auu.Name as ApprovedByName,
                                          auu.Email as ApprovedByEmail
                                      FROM Blogs b
                                               LEFT JOIN Authors a ON b.AuthorId = a.AuthorId
                                               LEFT JOIN AspNetUsers cu ON b.CreatedBy = cu.Id
                                               LEFT JOIN AspNetUsers uu ON b.UpdatedBy = uu.Id
                                               LEFT JOIN AspNetUsers auu ON b.ApprovedBy = auu.Id
                                      WHERE
                                          (@StartDate IS NULL OR CAST(b.StartDate AS DATE) = @StartDate) AND
                                          (@EndDate IS NULL OR CAST(b.EndDate AS DATE) = @EndDate) AND
                                          (@CreatedBy IS NULL OR b.CreatedBy = @CreatedBy) AND
                                          (@ApprovedBy IS NULL OR b.ApprovedBy = @ApprovedBy) AND
                                          (@ApproveStatus IS NULL OR b.ApproveStatus = @ApproveStatus) AND
                                          (@ActiveStatus IS NULL OR b.ActiveStatus = @ActiveStatus)
                                  )
    SELECT *
    INTO #TempBlogs
    FROM FilteredBlogs;

     SELECT
         fb.BlogId,
         fb.BlogTitle,
         fb.BlogContent,
         fb.CreatedAt,
         fb.UpdatedAt,
         fb.StartDate,
         fb.EndDate,
         fb.ActiveStatus,
         fb.ApproveStatus,
         fb.AuthorId,
         fb.AuthorName,
         fb.AuthorEmail,
         fb.Age,
         fb.CreatedBy,
         fb.CreatedById as Id,
         fb.CreatedByName as Name,
         fb.CreatedByEmail as Email,
         fb.UpdatedById as Id,
         fb.UpdatedByName as Name,
         fb.UpdatedByEmail as Email,
         fb.ApprovedById as Id,
         fb.ApprovedByName as Name,
         fb.ApprovedByEmail as Email
         
     FROM #TempBlogs fb
     ORDER BY
         CASE WHEN LOWER(@SortBy) = 'author' AND LOWER(@SortOrder) = 'asc' THEN fb.AuthorName END ASC,
         CASE WHEN LOWER(@SortBy) = 'author' AND LOWER(@SortOrder) = 'desc' THEN fb.AuthorName END DESC,
         CASE WHEN LOWER(@SortBy) = 'blogtitle' AND LOWER(@SortOrder) = 'asc' THEN fb.BlogTitle END ASC,
         CASE WHEN LOWER(@SortBy) = 'blogtitle' AND LOWER(@SortOrder) = 'desc' THEN fb.BlogTitle END DESC,
         CASE WHEN LOWER(@SortBy) = 'approvestatus' AND LOWER(@SortOrder) = 'asc' THEN fb.ApproveStatus END ASC,
         CASE WHEN LOWER(@SortBy) = 'approvestatus' AND LOWER(@SortOrder) = 'desc' THEN fb.ApproveStatus END DESC,
         CASE WHEN LOWER(@SortBy) = 'startdate' AND LOWER(@SortOrder) = 'asc' THEN fb.StartDate END ASC,
         CASE WHEN LOWER(@SortBy) = 'startdate' AND LOWER(@SortOrder) = 'desc' THEN fb.StartDate END DESC,
         CASE WHEN LOWER(@SortBy) = 'enddate' AND LOWER(@SortOrder) = 'asc' THEN fb.EndDate END ASC,
         CASE WHEN LOWER(@SortBy) = 'enddate' AND LOWER(@SortOrder) = 'desc' THEN fb.EndDate END DESC,
         CASE WHEN @SortBy IS NULL OR @SortBy = '' THEN fb.CreatedAt END DESC
     OFFSET @Offset ROWS
     FETCH NEXT @Limit ROWS ONLY;

            

    SELECT COUNT(*) AS TotalCount FROM #TempBlogs;
END;

