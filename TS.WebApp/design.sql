DROP TABLE dbo.TicketAttachement
DROP TABLE dbo.TicketMessage
DROP TABLE TicketLog
DROP TABLE dbo.Ticket
GO
CREATE TABLE dbo.Ticket
(
	Tid INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	cid INT REFERENCES Client(cid) NOT NULL,
	uid INT REFERENCES UserMaster(uid) NOT NULL,
	ticket_type BIGINT NOT NULL,
	ticket_priority TINYINT NOT NULL,
	ticket_status VARCHAR(50) NOT NULL,
	ticket_subject VARCHAR(512) NOT NULL,
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