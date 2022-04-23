/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
	  [rime].[weighting] as 'Rarity Score'
	  ,[bf].[id]
      ,[bf].[Back]
	  ,[rime].[Back] as 'Back Score'
      ,[bf].[Face]
	  ,[rime].[Face] as 'Face Score'
      ,[bf].[Head]
	  ,[rime].[Head] as 'Head Score'
      ,[bf].[Outfit]
	  ,[rime].[Outfit] as 'Outfit Score'
      ,[bf].[Character]
	  ,[rime].[Character] as 'Character Score'
      ,[bf].[Background]
	  ,[rime].[Background] as 'Background Score'
      ,[bf].[name]
      --,[bf].[asset]
      --,[bf].[fingerprint]
      ,[bf].[traitCount] as 'Trait Count'
	  ,[rime].[traitCount]  as 'Trait Count Score'
  FROM [Blockfrost].[dbo].[FalseIdols] bf
  INNER JOIN [Rime].[dbo].[FalseIdolsRarity] rime
  ON bf.asset = rime.asset
  order by [rime].[weighting] desc