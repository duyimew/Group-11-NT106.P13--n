CREATE DATABASE mydatabase;
GO

USE mydatabase;
GO

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(256) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Birthday DATE NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Groups (
    GroupId INT PRIMARY KEY IDENTITY(1,1),
    GroupName NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE GroupMembers (
    GroupId INT,
    UserId INT,
    PRIMARY KEY (GroupId, UserId),
    FOREIGN KEY (GroupId) REFERENCES Groups(GroupId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE Channels (
    ChannelId INT PRIMARY KEY IDENTITY(1,1),
    ChannelName NVARCHAR(100) NOT NULL,
    GroupId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (GroupId) REFERENCES Groups(GroupId)
);

CREATE TABLE Messages (
    MessageId INT PRIMARY KEY IDENTITY(1,1),
    ChannelId INT NOT NULL,
    UserId INT,
    MessageText NVARCHAR(MAX),
	IsAttachment INT NOT NULL,
    SentTime DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ChannelId) REFERENCES Channels(ChannelId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE Attachments (
    AttachmentId INT PRIMARY KEY IDENTITY(1,1),
    MessageId INT,
	Filename NVARCHAR(255) NOT NULL,
    UploadedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MessageId) REFERENCES Messages(MessageId)
);

CREATE TABLE UserRoles (
    UserRoleId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    GroupId INT,
    Role NVARCHAR(50) NOT NULL, -- 'Admin', 'Member', 'Guest', etc.
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (GroupId) REFERENCES Groups(GroupId),
    UNIQUE (UserId, GroupId)
);
SELECT   ms.MessageId,    us.Username,   ms.MessageText,    STRING_AGG(am.Filename, '; ') AS Filename FROM    Messages ms JOIN     Users us ON us.UserId = ms.UserId JOIN     Channels ch ON ch.ChannelId = ms.ChannelId JOIN     Groups gr ON gr.GroupId = ch.GroupId LEFT JOIN     Attachments am ON ms.MessageId = am.MessageId WHERE    ch.ChannelName = @channelname    AND gr.GroupName = groupname GROUP BY    ms.MessageId, us.Username, ms.MessageText,ms.SentTime ORDER BY   ms.SentTime DESC