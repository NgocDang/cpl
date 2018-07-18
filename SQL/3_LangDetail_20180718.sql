Use CPL;

Delete from LangDetail where Name = 'FirstPrize';
Delete from LangDetail where Name = 'SecondPrize';
Delete from LangDetail where Name = 'ThirdPrize';
Delete from LangDetail where Name = 'FourthPrize';
Insert into LangDetail values (1, N'Prize', N'Prize');
