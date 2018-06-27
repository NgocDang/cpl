use CPL;
alter table SysUser add Gender bit;
alter table SysUser add PostalCode nvarchar(10);
alter table SysUser add Country nvarchar(30);
alter table SysUser add City nvarchar(30);
alter table SysUser add StreetAddress nvarchar(100);
alter table SysUser drop column Address;

