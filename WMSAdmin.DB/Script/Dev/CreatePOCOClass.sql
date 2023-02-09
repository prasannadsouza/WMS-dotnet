use WMSAdmin
declare @tableName varchar(200) = 'AppUserRefreshToken'
declare @columnName varchar(200)
declare @nullable varchar(50)
declare @datatype varchar(50)
declare @maxlen int
declare @addAnnotations bit = 0
declare @sType varchar(50)
declare @sProperty varchar(200)
declare @assignmentText varchar(max)
declare @assignmentText1 varchar(max)

DECLARE table_cursor CURSOR FOR 
SELECT TABLE_NAME
FROM [INFORMATION_SCHEMA].[TABLES] where (@tableName is null OR len(@tableName) = 0 OR [TABLE_NAME] = @tableName)

OPEN table_cursor

FETCH NEXT FROM table_cursor 
INTO @tableName

WHILE @@FETCH_STATUS = 0
BEGIN
select @assignmentText = + @tableName + CHAR(13)+CHAR(10) + '{ ' + CHAR(13)+CHAR(10)
select @assignmentText1 = ''
PRINT 'public class ' + @tableName + ' {'

print ''
    DECLARE column_cursor CURSOR FOR 
    SELECT COLUMN_NAME, IS_NULLABLE, DATA_TYPE, isnull(CHARACTER_MAXIMUM_LENGTH,'-1') 
  from [INFORMATION_SCHEMA].[COLUMNS] 
	WHERE [TABLE_NAME] = @tableName
	order by [ORDINAL_POSITION]

    OPEN column_cursor
    FETCH NEXT FROM column_cursor INTO @columnName, @nullable, @datatype, @maxlen

    WHILE @@FETCH_STATUS = 0
    BEGIN

	select @assignmentText = @assignmentText + @columnName + ' = from.' +  @columnName + ',' + CHAR(13)+CHAR(10)

	if (@columnName ! = 'Id')
	select @assignmentText1 = @assignmentText1 + + 'to.' + @columnName + ' = from.' +  @columnName + ';' + CHAR(13)+CHAR(10)
	
	-- datatype
	select @sType = case @datatype
	when 'int' then 'int?'
	when 'bigint' then 'long?'
	when 'decimal' then 'decimal?'
	when 'money' then 'decimal?'
	when 'char' then 'string'
	when 'nchar' then 'string'
	when 'varchar' then 'string'
	when 'nvarchar' then 'string'
	when 'uniqueidentifier' then 'Guid?'
	when 'datetime' then 'DateTime?'
	when 'datetime2' then 'DateTime?'
	when 'bit' then 'bool?'
	else 'string'
	END
		if (@addAnnotations = 1)
		BEGIN
			If (@nullable = 'NO')
				PRINT '[Required]'
			if (@sType = 'string' and @maxLen <> '-1')
				Print '[MaxLength(' +  convert(varchar(4),@maxLen) + ')]'
		END
		
		SELECT @sProperty = 'public ' + @sType + ' ' + @columnName + ' { get; set;}'
		PRINT @sProperty

		--print ''
		FETCH NEXT FROM column_cursor INTO @columnName, @nullable, @datatype, @maxlen
	END
    CLOSE column_cursor
    DEALLOCATE column_cursor

	print '}'
	print ''
	select @assignmentText = @assignmentText + '};'
	print ''
	print @assignmentText 
	
	print ''
	print @assignmentText1

    FETCH NEXT FROM table_cursor 
    INTO @tableName
END
CLOSE table_cursor
DEALLOCATE table_cursor