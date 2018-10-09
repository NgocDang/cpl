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
	--[Status] int,
	NumberOfPrediction int,
	TotalPurchasePrice money,
	Title nvarchar(200),
	PurchaseDateTime date,
	--PurchaseDateTimeInString nvarchar(20),
	--NumberOfPredictionInString nvarchar(10),
	--StatusInString nvarchar(20),
	RowNum int
);

	WITH PricePredictionHistoryCTE AS
	(
		SELECT  SUM(ISNULL(pph.Amount,0)) as TotalPurchasePrice,
			    COUNT(ISNULL(pph.Prediction,0)) as NumberOfPrediction, 
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
			   ppdt.Title
		--ppt.status // todo
		FROM PricePrediction pp
		JOIN PricePredictionHistoryCTE cte	on pp.Id = cte.PricePredictionId
		JOIN PricePredictionDetail ppdt		on pp.Id = ppdt.PricePredictionId 
											--and ppdt.[LangId] = @LangId
											and ppdt.[LangId] = 1
		JOIN SysUser su						on cte.SysUserId = su.Id
		WHERE pp.PricePredictionCategoryId = 
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
				--CASE WHEN @OrderDirection = N'asc' THEN ''
				--	 WHEN @OrderColumn = N'Status' THEN [Status]
				--	 END DESC,
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
				--CASE WHEN @OrderDirection = N'asc' THEN cast (null as datetime)
				--	 WHEN @OrderColumn = N'PurchaseDateTimeInString' THEN PurchaseDateTime 
				--	 END DESC,
				--CASE WHEN @OrderDirection = N'asc' THEN ''
				--	 WHEN @OrderColumn = N'NumberOfPredictionInString' THEN NumberOfPrediction 
				--	 END DESC,
				--CASE WHEN @OrderDirection = N'asc' THEN ''
				--	 WHEN @OrderColumn = N'StatusInString' THEN [Status] 
				--	 END DESC,
	
				-- DESC
				CASE WHEN @OrderDirection = N'desc' THEN 0
					 WHEN @OrderColumn = N'SysUserId' THEN SysUserId
					 END ASC,
				CASE WHEN @OrderDirection = N'desc' THEN ''
					 WHEN @OrderColumn = N'Email' THEN Email
					 END DESC,
				--CASE WHEN @OrderDirection = N'desc' THEN ''
				--	 WHEN @OrderColumn = N'Status' THEN [Status]
				--	 END DESC,
				CASE WHEN @OrderDirection = N'desc' THEN 0
					 WHEN @OrderColumn = N'NumberOfPrediction' THEN NumberOfPrediction
					 END DESC,
				CASE WHEN @OrderDirection = N'desc' THEN cast(null as money)
					 WHEN @OrderColumn = N'TotalPurchasePrice' THEN TotalPurchasePrice
					 END DESC,
				CASE WHEN @OrderDirection = N'desc' THEN ''
					 WHEN @OrderColumn = N'Title ' THEN Title 
					 END DESC,
				CASE WHEN @OrderDirection = N'desc' THEN cast (null as date)
					 WHEN @OrderColumn = N'PurchaseDateTime ' THEN PurchaseDateTime 
					 END DESC--,
				--CASE WHEN @OrderDirection = N'desc' THEN cast (null as datetime)
				--	 WHEN @OrderColumn = N'PurchaseDateTimeInString' THEN PurchaseDateTime 
				--	 END DESC,
				--CASE WHEN @OrderDirection = N'desc' THEN ''
				--	 WHEN @OrderColumn = N'NumberOfPredictionInString' THEN NumberOfPrediction 
				--	 END DESC,
				--CASE WHEN @OrderDirection = N'desc' THEN ''
				--	 WHEN @OrderColumn = N'StatusInString' THEN [Status] 
				--	 END DESC
	)
		FROM PricePredictionHistoryResultCTE
		WHERE(Email like '%' + @SearchValue + '%'
			  OR 
			  CONVERT(nvarchar(23), PurchaseDateTime, 0) like ('%' + @SearchValue + '%')
			  OR 
			  Title like ('%' + @SearchValue + '%')
			  --OR
			  --[Status] = @SearchValue 
			  )
	)

INSERT INTO @TablePricePredictionHistory
SELECT SysUserId, Email, NumberOfPrediction, TotalPurchasePrice, Title, PurchaseDateTime, RowNum
FROM PricePredictionHistoryWithRowNumCTE;

SELECT *
FROM @TablePricePredictionHistory
WHERE RowNum  BETWEEN ((@PageIndex - 1) * @PageSize + 1) AND (@PageIndex * @PageSize);



--////////////////////////////// DATATABLE #2 - Total Count /////////////////////////////--
SELECT COUNT(ISNULL(pph.PricePredictionId,0)) as TotalCount
FROM PricePredictionHistory pph
GROUP BY CAST(pph.CreatedDate as date),
			 pph.PricePredictionId,
			 pph.SysUserId


--////////////////////////////// DATATABLE #3 - Filtered Count //////////////////////////--
SELECT COUNT(*) as FilteredCount
FROM @TablePricePredictionHistory
END

