use CPL;
alter table SysUser add Gender bit;
alter table SysUser add PostalCode nvarchar(10);
alter table SysUser add Country nvarchar(30);
alter table SysUser add City nvarchar(30);
alter table SysUser add StreetAddress nvarchar(100);
alter table SysUser drop column Address;

insert into LangDetail values (1, 'Name', 'Name');
insert into LangDetail values (1, 'Gender', 'Gender');
insert into LangDetail values (1, 'Male', 'Male');
insert into LangDetail values (1, 'Female', 'Female');
insert into LangDetail values (1, 'DOB', 'Date of birth');