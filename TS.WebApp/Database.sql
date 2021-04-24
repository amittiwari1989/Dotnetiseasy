USE TS
GO
SELECT * FROM UserMaster
SELECT * FROM Client
SELECT * FROM Ticket
GO
CREATE TABLE dbo.Client(
	cid INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	name NVARCHAR(256) UNIQUE NOT NULL,
	street_address NVARCHAR(512) NULL,
	city NVARCHAR(256) NULL,
	country NVARCHAR(256) NULL,
	email VARCHAR(256) UNIQUE NOT NULL,
	phone VARCHAR(15) NULL,
	fax VARCHAR(15) NULL,
	website VARCHAR(512) NULL,
	CustomerType INT NULL,
	ProductCategory INT NULL,
	allow_signup TINYINT NOT NULL,
	date_created DATETIME NOT NULL,
	date_modified DATETIME NOT NULL
)
GO

CREATE TABLE dbo.UserMaster(
	uid INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	cid INT REFERENCES Client(cid) NOT NULL,
	User_type TINYINT NOT NULL,
	display_name NVARCHAR(256) NOT NULL,
	email VARCHAR(256) NOT NULL,
	domain VARCHAR(256) NULL,
	domain_username VARCHAR(256) NULL,
	hashed_password VARCHAR(40) NULL,
	salt VARCHAR(40) NULL,
	mobile_number VARCHAR(15) NULL,
	mobile_OTP VARCHAR(10) NULL,
	password_reset TINYINT NOT NULL,
	LastLogin DATETIME NULL,
	LastPasswordChanged DATETIME NULL,
	LastLogout DATETIME NULL,
	LastLoginFail DATETIME NULL,
	LoginFailCount INT NOT NULL,
	user_status TINYINT NOT NULL,
	session_token VARCHAR(40) NULL,
	session_token_expiry DATETIME NULL,
	reset_token VARCHAR(40) NULL,
	reset_token_expiry DATETIME NULL,
	created_on DATETIME NOT NULL,
	created_by_uid INT NULL,
	date_created DATETIME NOT NULL,
	date_modified DATETIME NOT NULL
)
GO
CREATE TABLE TicketStatus
(
	TSid BIGINT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	cid INT REFERENCES Client(cid) NOT NULL,
	StatusName VARCHAR(50) NOT NULL,
	FinalStatus INT NOT NULL,
	uid INT REFERENCES UserMaster(uid) NOT NULL,
	Active INT NOT NULL DEFAULT(1),
	created_date DATETIME	
)
GO
INSERT INTO TicketStatus(cid,StatusName,FinalStatus,uid,Active,created_date) 
VALUES(1,'Open',1,1,1,GETDATE()),(1,'Pending',1,1,1,GETDATE()),(1,'Work-In-Progress',1,1,1,GETDATE()),
(1,'Send-For-Evaluation',1,1,1,GETDATE()),(1,'Closed',2,1,1,GETDATE()),(1,'Invalid',2,1,1,GETDATE());
GO
CREATE TABLE TicketType
(
	TTid BIGINT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	cid INT REFERENCES Client(cid) NOT NULL,
	TypeName VARCHAR(50) NOT NULL,
	uid INT REFERENCES UserMaster(uid) NOT NULL,
	Active INT NOT NULL DEFAULT(1),
	created_date DATETIME 
)
GO
INSERT INTO TicketType(cid,TypeName,uid,Active,created_date) 
VALUES(1,'Issue',1,1,GETDATE()),(1,'Requirement',1,1,GETDATE()),(1,'Question',1,1,GETDATE()),
(1,'Other',1,1,GETDATE());

GO
--DROP TABLE dbo.TicketAttachement
--DROP TABLE dbo.TicketMessage
--DROP TABLE TicketLog
--DROP TABLE dbo.Ticket
GO
CREATE TABLE dbo.Ticket
(
	Tid INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	cid INT REFERENCES Client(cid) NOT NULL,
	uid INT REFERENCES UserMaster(uid) NOT NULL,
	ticket_subject VARCHAR(512) NOT NULL,
	ticket_type BIGINT NOT NULL,
	ticket_priority TINYINT NOT NULL,
	ticket_status VARCHAR(50) NOT NULL,
	created_via INT NULL,
	ticket_date DATETIME NOT NULL,
	date_created DATETIME NOT NULL,
	date_modified DATETIME NOT NULL,
	custom_field_1 VARCHAR(500) NULL,
	custom_field_2 VARCHAR(500) NULL,
	custom_field_3 VARCHAR(500) NULL,
	custom_field_4 VARCHAR(500) NULL,
	custom_field_5 VARCHAR(500) NULL,
	custom_field_6 VARCHAR(500) NULL,
	custom_field_7 VARCHAR(500) NULL,
	custom_field_8 VARCHAR(500) NULL,
	custom_field_9 VARCHAR(500) NULL,
	custom_field_10 VARCHAR(500) NULL,
	custom_field_11 VARCHAR(500) NULL,
	custom_field_12 VARCHAR(500) NULL,
	custom_field_13 VARCHAR(500) NULL,
	custom_field_14 VARCHAR(500) NULL,
	custom_field_15 VARCHAR(500) NULL,
	custom_field_16 VARCHAR(500) NULL,
	custom_field_17 VARCHAR(500) NULL,
	custom_field_18 VARCHAR(500) NULL,
	custom_field_19 VARCHAR(500) NULL,
	custom_field_20 VARCHAR(500) NULL
) 
GO
CREATE TABLE dbo.TicketLog(
	Lid INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Tid INT REFERENCES Ticket(Tid) NOT NULL,
	log_details VARCHAR(max) NOT NULL,
	date_created DATETIME NOT NULL,
	date_modified DATETIME NOT NULL
) 
GO
CREATE TABLE dbo.TicketMessage(
	Mid INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Tid INT REFERENCES Ticket(Tid) NOT NULL,
	uid INT NOT NULL,
	Message VARCHAR(MAX) NOT NULL,
	date_created DATETIME NOT NULL,
	date_modified DATETIME NOT NULL
)
GO
CREATE TABLE dbo.TicketAttachement(
	Aid INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Mid INT REFERENCES TicketMessage(Mid) NULL,
	Data VARBINARY(MAX) NOT NULL,
	date_created DATETIME NOT NULL,
	date_modified DATETIME NOT NULL
)
GO

CREATE TABLE CustomFieldMaster
(
	CFMid BIGINT IDENTITY(1,1) PRIMARY KEY,
	cid INT REFERENCES Client(cid) NOT NULL,
	Label varchar(30) NOT NULL,
	FieldName varchar(30) NOT NULL,
	DataType INT NOT NULL,
	compulsory int DEFAULT(0) NOT NULL,
	active INT,
	created_by INT REFERENCES UserMaster(uid) NOT NULL,
	date_created DATETIME NOT NULL,
	date_modified DATETIME NOT NULL
)
GO
INSERT INTO CustomFieldMaster SELECT 1,'Bank Name','custom_field_1',2,1,1,1,GETUTCDATE(),GETUTCDATE()
go
CREATE TABLE CustomFieldList
(
	CFLid BIGINT IDENTITY(1,1) PRIMARY KEY,
	cid INT REFERENCES Client(cid) NOT NULL,
	CFMid BIGINT REFERENCES CustomFieldMaster(CFMid) NOT NULL,
	DataField VARCHAR(500) NOT NULL,
	TextField VARCHAR(500) NOT NULL,
	active INT,
	created_by INT REFERENCES UserMaster(uid) NOT NULL,
	date_created DATETIME NOT NULL,
	date_modified DATETIME NOT NULL
)
GO
INSERT INTO CustomFieldList SELECT 1,1,'Sutex','Sutex',1,1,GETUTCDATE(),GETUTCDATE()
INSERT INTO CustomFieldList SELECT 1,1,'SUDICO','SUDICO',1,1,GETUTCDATE(),GETUTCDATE()
GO
CREATE TABLE FieldOrder
(
	FOid INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	cid INT REFERENCES Client(cid) NOT NULL,
	uid INT REFERENCES UserMaster(uid) NOT NULL,
	FieldName VARCHAR(20) NOT NULL,
	visible INT NOT NULL DEFAULT(1),
	SeqNo INT NOT NULL,
	Active INT  NOT NULL DEFAULT(1),
	date_created DATETIME NOT NULL,
	date_modified DATETIME NOT NULL
)
GO
INSERT INTO FieldOrder
SELECT  
	1,1,col.name,1,col.column_id,1,GETDATE(),GETDATE()
FROM 
	sys.columns col join sys.types typ on col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id  
WHERE 
	object_id = object_id('Ticket')  

UPdATE FieldOrder SET visible=0  

UPdATE FieldOrder SET visible=1  WHERE FOid BETWEEN 4 and 7

