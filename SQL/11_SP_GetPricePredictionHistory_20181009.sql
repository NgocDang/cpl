SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		DamTran
-- Create date: 20181009
-- Description:	Get PricePrediction history to show in admin view
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetPricePredictionHistory] 
	-- Add the parameters for the stored procedure here
	@PricePredictionCategoryId int,
	@LangId int,
	@PageSize int,
	@PageIndex int,
	@OrderColumn nvarchar(30),
	@OrderDirection nvarchar(5),
	@SearchValue nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


--////////////////////////////// DATATABLE #1 - @TablePricePredictionHistory ////////////--
 
-- Insert statements for procedure here
-- CONSTRUCT SQL QUERY FIRST -- 
DECLARE @TablePricePredictionHistory TABLE
(
	SysUserId int, 
	Email nvarchar(50),
	[Status] int,
	StatusInString nvarchar(20),
	NumberOfPrediction int,
	TotalPurchasePrice money,
	Title nvarchar(200),
	PurchaseDateTime date,
	RowNum int
);

	WITH PricePredictionHistoryCTE AS
	(
		SELECT  SUM(ISNULL(pph.Amount,0)) as TotalPurchasePrice,
			    COUNT(pph.Prediction) as NumberOfPrediction, 
				CAST(pph.CreatedDate AS DATE) as PurchaseDateTime,
				PricePredictionId,
				SysUserId
		FROM PricePredictionHistory pph
		GROUP BY CAST(pph.CreatedDate as date),
				 pph.PricePredictionId,
				 pph.SysUserId
	
	),
	-- JOIN WITH OTHER TABLE -- 
	PricePredictionHistoryResultCTE AS
	(
		SELECT cte.*,
			   su.Email,
			   ppdt.Title,
			   pp.[Status],
			   CASE WHEN pp.[Status]= 1
					THEN 'ACTIVE'
					WHEN pp.[Status] = 2
					THEN 'COMPLETED'
					ELSE ''
				END AS StatusInString
		FROM PricePrediction pp
		JOIN PricePredictionHistoryCTE cte	on pp.Id = cte.PricePredictionId
		JOIN PricePredictionDetail ppdt		on pp.Id = ppdt.PricePredictionId 
											and ppdt.[LangId] = @LangId
		JOIN SysUser su						on cte.SysUserId = su.Id
		WHERE   pp.PricePredictionCategoryId = 
				CASE
				WHEN  @PricePredictionCategoryId > 0 THEN @PricePredictionCategoryId
				ELSE 
					pp.PricePredictionCategoryId
				END
	)
	--SELECT * FROM  PricePredictionHistoryResultCTE
	,
	-- APPLY SORT / SEARCH / PAGING  --
	PricePredictionHistoryWithRowNumCTE AS
	(	
		SELECT PricePredictionHistoryResultCTE.*,
			RowNum = ROW_NUMBER() OVER (
				ORDER BY -- ASC
				CASE WHEN @OrderDirection = N'asc' THEN 0
					 WHEN @OrderColumn = N'SysUserId' THEN SysUserId
					 END DESC,
				CASE WHEN @OrderDirection = N'asc' THEN ''
					 WHEN @OrderColumn = N'Email' THEN Email
					 END DESC,
				CASE WHEN @OrderDirection = N'asc' THEN ''
					 WHEN @OrderColumn = N'Status' THEN [Status]
					 END DESC,
				CASE WHEN @OrderDirection = N'asc' THEN 0
					 WHEN @OrderColumn = N'NumberOfPrediction' THEN NumberOfPrediction
					 END DESC,
				CASE WHEN @OrderDirection = N'asc' THEN cast(null as money)
					 WHEN @OrderColumn = N'TotalPurchasePrice' THEN TotalPurchasePrice
					 END DESC,
				CASE WHEN @OrderDirection = N'asc' THEN ''
					 WHEN @OrderColumn = N'Title ' THEN Title 
					 END DESC,
				CASE WHEN @OrderDirection = N'asc' THEN cast (null as date)
					 WHEN @OrderColumn = N'PurchaseDateTime ' THEN PurchaseDateTime 
					 END DESC,
				CASE WHEN @OrderDirection = N'asc' THEN ''
					 WHEN @OrderColumn = N'StatusInString' THEN [Status] 
					 END DESC,
	
				-- DESC
				CASE WHEN @OrderDirection = N'desc' THEN 0
					 WHEN @OrderColumn = N'SysUserId' THEN SysUserId
					 END ASC,
				CASE WHEN @OrderDirection = N'desc' THEN ''
					 WHEN @OrderColumn = N'Email' THEN Email
					 END ASC,
				CASE WHEN @OrderDirection = N'desc' THEN ''
					 WHEN @OrderColumn = N'Status' THEN [Status]
					 END ASC,
				CASE WHEN @OrderDirection = N'desc' THEN 0
					 WHEN @OrderColumn = N'NumberOfPrediction' THEN NumberOfPrediction
					 END ASC,
				CASE WHEN @OrderDirection = N'desc' THEN cast(null as money)
					 WHEN @OrderColumn = N'TotalPurchasePrice' THEN TotalPurchasePrice
					 END ASC,
				CASE WHEN @OrderDirection = N'desc' THEN ''
					 WHEN @OrderColumn = N'Title ' THEN Title 
					 END ASC,
				CASE WHEN @OrderDirection = N'desc' THEN cast (null as date)
					 WHEN @OrderColumn = N'PurchaseDateTime ' THEN PurchaseDateTime 
					 END ASC,
				CASE WHEN @OrderDirection = N'desc' THEN ''
					 WHEN @OrderColumn = N'StatusInString' THEN [Status] 
					 END ASC
	)
		FROM PricePredictionHistoryResultCTE
		WHERE(Email like '%' + @SearchValue + '%'
			  OR 
			  (CONVERT(varchar, PurchaseDateTime, 111) + ' ' + CONVERT(varchar, PurchaseDateTime, 8))  like ('%' + @SearchValue + '%')
			  OR 
			  Title like ('%' + @SearchValue + '%')
			  OR
			  StatusInString like ('%' + @SearchValue + '%'))
	)

INSERT INTO @TablePricePredictionHistory
SELECT SysUserId, Email, [Status], StatusInString, NumberOfPrediction, TotalPurchasePrice, Title, PurchaseDateTime, RowNum
FROM PricePredictionHistoryWithRowNumCTE;

SELECT *
FROM @TablePricePredictionHistory
WHERE RowNum  BETWEEN ((@PageIndex - 1) * @PageSize + 1) AND (@PageIndex * @PageSize);



--////////////////////////////// DATATABLE #2 - Total Count /////////////////////////////--
SELECT COUNT(*) as TotalCount
FROM (
		SELECT		COUNT(pph.PricePredictionId) as TotalCount
		FROM		PricePredictionHistory pph
		JOIN		PricePrediction pp
		on		pph.PricePredictionId = pp.Id
		WHERE		pp.PricePredictionCategoryId = 
						CASE
						WHEN  @PricePredictionCategoryId > 0 THEN @PricePredictionCategoryId
						ELSE 
							pp.PricePredictionCategoryId
						END
		GROUP BY	CAST(pph.CreatedDate as date),
					 pph.PricePredictionId,
					 pph.SysUserId) As Result



--////////////////////////////// DATATABLE #3 - Filtered Count //////////////////////////--
SELECT COUNT(*) as FilteredCount
FROM @TablePricePredictionHistory
END
