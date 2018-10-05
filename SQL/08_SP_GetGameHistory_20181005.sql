-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetGameHistory] 
	-- Add the parameters for the stored procedure here
	@SysUserId int,
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

    -- Insert statements for procedure here
------------------------------------------------------------------------------------
-------------------------- 1.1 CONSTRUCT SQL QUERY FIRST --------------------------- 
------------------------------------------------------------------------------------
DECLARE @TableGameHistory TABLE
(
	GameId int, 
	CreatedDate datetime,
	CreatedDateInString nvarchar(20),
	CreatedTimeInString nvarchar(20),
	Amount money,
	Result nvarchar(20),
	Balance money,
	Award money,
	GameType nvarchar(50),
	RowNum int,
	TotalCount int
);

WITH GameHistoryCTE AS
(
	------------------
	-- Lottery Game --
	------------------
	SELECT
		------------
		-- GameId --
		------------
		lh.LotteryId as GameId,

		-----------------
		-- CreatedDate --
		-----------------
		MAX(lh.CreatedDate) AS CreatedDate,

		-------------------------
		-- CreatedDateInString --
		-------------------------
		CONVERT(varchar, MAX(lh.CreatedDate), 111) AS CreatedDateInString,

		-------------------------
		-- CreatedTimeInString --
		-------------------------
		CONVERT(varchar, MAX(lh.CreatedDate), 8) AS CreatedTimeInString,

		------------
		-- Amount --
		------------
		SUM(lot.UnitPrice) AS Amount,

		------------
		-- Result --
		------------
		CASE WHEN SUM(CASE lh.Result WHEN 'WIN' THEN 1 ELSE 0 END) > 0
             THEN 'WIN'
			 WHEN SUM(CASE WHEN lh.Result is null THEN 1 ELSE 0 END) > 0
			 THEN ''
             ELSE 'LOSE'
		END AS Result,

		-------------
		-- Balance --
		-------------
		CASE WHEN SUM(CASE WHEN lh.Result is null THEN 1 ELSE 0 END) > 0
			 THEN 0
             ELSE SUM(lp.Value) - SUM(lot.UnitPrice)
		END AS Balance,

		-----------
		-- Award --
		-----------
		ISNULL(SUM(lp.Value), 0) AS Award,

		--------------
		-- GameType --
		--------------
		'LOTTERY' AS GameType

	FROM LotteryHistory lh
			join Lottery lot on lh.LotteryId = lot.Id
			join LotteryPrize lp on lh.LotteryPrizeId = lp.Id
	WHERE lh.SysUserId = @SysUserId
	GROUP BY lh.LotteryId

	UNION ALL

	---------------------------
	-- Price Prediction Game --
	---------------------------
	SELECT
		------------
		-- GameId --
		------------
		pph.PricePredictionId AS GameId,

		-----------------
		-- CreatedDate --
		-----------------
		MAX(pph.CreatedDate) AS CreatedDate,

		-------------------------
		-- CreatedDateInString --
		-------------------------
		CONVERT(varchar, MAX(pph.CreatedDate), 111) AS CreatedDateInString,

		-------------------------
		-- CreatedTimeInString --
		-------------------------
		CONVERT(varchar, MAX(pph.CreatedDate), 8) AS CreatedTimeInString,

		------------
		-- Amount --
		------------
		SUM(pph.Amount) AS Amount,

		------------
		-- Result --
		------------
		CASE WHEN SUM(CASE pph.Result WHEN 'WIN' THEN 1 ELSE 0 END) > 0
             THEN 'WIN'
			 WHEN SUM(CASE WHEN pph.Result is null THEN 1 ELSE 0 END) > 0
			 THEN ''
             ELSE 'LOSE'
		END AS Result,

		-------------
		-- Balance --
		-------------
		CASE WHEN SUM(CASE WHEN pph.Result is null THEN 1 ELSE 0 END) > 0
			 THEN 0
             ELSE ISNULL(SUM(pph.TotalAward), 0) - SUM(pph.Amount)
		END AS Balance,

		-----------
		-- Award --
		-----------
		ISNULL(SUM(pph.TotalAward), 0) AS Award,

		--------------
		-- GameType --
		--------------
		'PRICE_PREDICTION' AS GameType

	FROM PricePredictionHistory pph
	WHERE pph.SysUserId = @SysUserId
	GROUP BY pph.PricePredictionId
),
TotalCountCTE AS
(
SELECT COUNT(*) TotalCount FROM GameHistoryCTE
),
------------------------------------------------------------------------------------------
---------------------------- 1.2 APPLY SORT / SEARCH / PAGING  --------------------------- 
------------------------------------------------------------------------------------------
GameHistoryWithRowNumCTE AS
(	
	SELECT GameHistoryCTE.*,
		TotalCountCTE.TotalCount,
		RowNum = ROW_NUMBER() OVER (
			ORDER BY -- ASC
			CASE WHEN @OrderDirection = N'asc' THEN 0
				 WHEN @OrderColumn = N'GameId' THEN GameId
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN cast(null as datetime)
				 WHEN @OrderColumn = N'CreatedDate' THEN CreatedDate
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN ''
				 WHEN @OrderColumn = N'CreatedDateInString' THEN CreatedDate
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN ''
				 WHEN @OrderColumn = N'CreatedTimeInString' THEN CreatedDate
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN cast(null as money)
				 WHEN @OrderColumn = N'Amount' THEN Amount
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN ''
				 WHEN @OrderColumn = N'Result' THEN Result
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN cast(null as money)
				 WHEN @OrderColumn = N'Balance' THEN Balance
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN cast(null as money)
				 WHEN @OrderColumn = N'Award' THEN Award
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN ''
				 WHEN @OrderColumn = N'GameType' THEN GameType
				 END DESC,

				-- DESC
			CASE WHEN @OrderDirection = N'desc' THEN 0
				 WHEN @OrderColumn = N'GameId' THEN GameId
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN cast(null as datetime)
				 WHEN @OrderColumn = N'CreatedDate' THEN CreatedDate
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN cast(null as datetime)
				 WHEN @OrderColumn = N'CreatedDateInString' THEN CreatedDate
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN cast(null as datetime)
				 WHEN @OrderColumn = N'CreatedTimeInString' THEN CreatedDate
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN cast(null as money)
				 WHEN @OrderColumn = N'Amount' THEN Amount
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN ''
				 WHEN @OrderColumn = N'Result' THEN Result
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN cast(null as money)
				 WHEN @OrderColumn = N'Balance' THEN Balance
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN cast(null as money)
				 WHEN @OrderColumn = N'Award' THEN Award
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN ''
				 WHEN @OrderColumn = N'GameType' THEN GameType
				 END ASC)
	FROM GameHistoryCTE, TotalCountCTE

	WHERE(Result like '%' + @SearchValue + '%'
		  OR 
		  CONVERT(nvarchar(23), CreatedDate, 0) like ('%' + @SearchValue + '%')
		  OR
		  GameType like '%' + @SearchValue + '%')
)

INSERT INTO @TableGameHistory
SELECT GameId, CreatedDate, CreatedDateInString, CreatedTimeInString, Amount, Result, Balance, Award, GameType, RowNum, TotalCount
FROM GameHistoryWithRowNumCTE;

SELECT GameId, CreatedDate, CreatedDateInString, CreatedTimeInString, GameType, Amount, Result, Award, Balance
FROM @TableGameHistory
WHERE RowNum  BETWEEN ((@PageIndex - 1) * @PageSize + 1) AND (@PageIndex * @PageSize);

--///////////////////////////////////////////////////////////////////////////////////////--
--////////////////////////////// DATATABLE #2 - Total Count /////////////////////////////--
--///////////////////////////////////////////////////////////////////////////////////////--

 SELECT ISNULL((SELECT TOP 1 TotalCount
FROM @TableGameHistory), 0) AS TotalCount;


--///////////////////////////////////////////////////////////////////////////////////////--
--////////////////////////////// DATATABLE #3 - Filtered Count //////////////////////////--
--///////////////////////////////////////////////////////////////////////////////////////--

SELECT COUNT(*) as FilteredCount
FROM   @TableGameHistory
END

