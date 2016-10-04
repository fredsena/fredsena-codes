
 DECLARE    @string varchar(1000),   @ShowReferences char(1)
 
 SET @string = 'DB_OBJECT_NAME' -- searchstring
 SET @ShowReferences = 'S'
 /*******************/
 -- localiza objeto  
 -- Descrição: Search in syscomments for the @string, returning the object name
 /********/ 
 set nocount on
 
 declare @errnum         int         ,
         @errors         char(1)     ,
         @rowcnt         int         ,
         @output         varchar(255)
 
 select  @errnum         = 0         ,
         @errors         = 'N'       ,
         @rowcnt         = 0         ,
         @output         = ''        
 
 /******/
 
 -- Create temporary table to get the results 
 
 DECLARE @Results table 
 (Name        varchar(55),   
 Type        varchar(12),   
 DateCreated datetime, 
 ProcLine    varchar(4000) )
 
  IF (@ShowReferences = 'N') 
 BEGIN   
 
	 insert into @Results   
	 select distinct
		  'Name' = convert(varchar(55),SO.name),
          'Type' = SO.type,
          crdate,
          ''
     from sysobjects  SO
     join syscomments SC on SC.id = SO.id    
	 where SC.text like '%' + @string + '%'   
	 union   
	 select distinct
          'Name' = convert(varchar(55),SO.name),
          'Type' = SO.type,
          crdate,
          ''
     from sysobjects  SO    
	 where SO.name like '%' + @string + '%'   
	 union   
	 select distinct
          'Name' = convert(varchar(55),SO.name),
          'Type' = SO.type,
          crdate,
          ''
     from sysobjects  SO
     join syscolumns SC on SC.id = SO.ID    
	 where SC.name like '%' + @string + '%'    
	 order by 2,1 
END 
ELSE 
BEGIN   
	 insert into @Results  
		select 
          'Name'      = convert(varchar(55),SO.name),
          'Type'      = SO.type,
          crdate,
          'Proc Line' = text
     from sysobjects  SO
     join syscomments SC on SC.id = SO.id    
	 where SC.text like '%' + @string + '%'   
	 union   
	 select 
          'Name'      = convert(varchar(55),SO.name),
          'Type'      = SO.type,
          crdate,
          'Proc Line' = ''
     from sysobjects  SO    
	 where SO.name like '%' + @string + '%'   
	 union   
	 select 
          'Name' = convert(varchar(55),SO.name),
          'Type' = SO.type,
          crdate,
          'Proc Line' = ''
     from sysobjects  SO
     join syscolumns SC on SC.id = SO.ID    
	 where SC.name like '%' + @string + '%'    
	 order by 2,1 
END
 
 IF (@ShowReferences = 'N') 
 BEGIN   
	select Name,
          'Type' = Case (Type)
                     when 'P'  then 'Procedure'
                     when 'TR' then 'Trigger'
                     when 'X'  then 'Xtended Proc'
                     when 'U'  then 'Table'
                     when 'C'  then 'Check Constraint'
                     when 'D'  then 'Default'
                     when 'F'  then 'Foreign Key'
                     when 'K'  then 'Primary Key'
                     when 'V'  then 'View'
                     else Type
                   end,
          DateCreated
     from @Results
     order by 2,1 
END 
ELSE 
BEGIN   
	select Name,
          'Type' = Case (Type)
                     when 'P'  then 'Procedure'
                     when 'TR' then 'Trigger'
                     when 'X'  then 'Xtended Proc'
                     when 'U'  then 'Table'
                     when 'C'  then 'Check Constraint'
                     when 'D'  then 'Default'
                     when 'F'  then 'Foreign Key'
                     when 'K'  then 'Primary Key'
                     when 'V'  then 'View'
                     else Type
                   end,
          DateCreated,
          ProcLine
     from @Results
     order by 2,1 
END
