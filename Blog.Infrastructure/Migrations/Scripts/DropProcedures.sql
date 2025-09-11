-- Drop all stored procedures created in this migration

IF OBJECT_ID('spCreateAuthor', 'P') IS NOT NULL
DROP PROCEDURE spCreateAuthor;
GO

IF OBJECT_ID('spCreateBlogWithAuthor', 'P') IS NOT NULL
DROP PROCEDURE spCreateBlogWithAuthor;
GO

IF OBJECT_ID('spCreateUser', 'P') IS NOT NULL
DROP PROCEDURE spCreateUser;
GO

IF OBJECT_ID('spDeleteAuthor', 'P') IS NOT NULL
DROP PROCEDURE spDeleteAuthor;
GO

IF OBJECT_ID('spDeleteBlog', 'P') IS NOT NULL
DROP PROCEDURE spDeleteBlog;
GO

IF OBJECT_ID('spDeleteUser', 'P') IS NOT NULL
DROP PROCEDURE spDeleteUser;
GO

IF OBJECT_ID('spUpdateAuthor', 'P') IS NOT NULL
DROP PROCEDURE spUpdateAuthor;
GO

IF OBJECT_ID('spUpdateBlog', 'P') IS NOT NULL
DROP PROCEDURE spUpdateBlog;
GO

IF OBJECT_ID('spUpdateUser', 'P') IS NOT NULL
DROP PROCEDURE spUpdateUser;
GO

IF OBJECT_ID('spGetAuthorsWithAgeBetween', 'P') IS NOT NULL
DROP PROCEDURE spGetAuthorsWithAgeBetween;
GO



