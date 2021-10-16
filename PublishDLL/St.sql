--
-- File generated with SQLiteStudio v3.2.1 on Sun Jul 26 22:08:30 2020
--
-- Text encoding used: UTF-8
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: db_settings
CREATE TABLE "db_settings" (
	`setting_id`	INTEGER,
	`aws_sellerid`	TEXT,
	`access_key`	TEXT,
	`secret_key`	TEXT,
	`refresh_minute`	INTEGER,
	`is_alert_mail`	INTEGER,
	`email_ids`	TEXT,
	`sender_emailid`	TEXT,
	`sender_emailpwd`	TEXT,
	`sender_smtp`	TEXT,
	`sender_port`	INTEGER,
	`sender_chkssl`	INTEGER,
	`send_mail_on_eachscan`	INTEGER,
	`Eb_App_ID`	TEXT,
	`Eb_Dev_ID`	TEXT,
	`Eb_Cer_ID`	TEXT,
	`is_show_notification`	INTEGER,
	`is_show_notification_type`	INTEGER,
	`api_call_interval`	INTEGER,
	`keepa_access_key`	TEXT,
	`mws_secretkey`	TEXT,
	`mws_accesskey`	TEXT,
	`mws_sellerid`	TEXT,
	`mws_marketplaceid`	TEXT,
	`api_call_intervalMWS`	INTEGER,
	`enable_schedule`	INTEGER,
	`schedule_time`	TEXT,
	`enable_ebay`	INTEGER,
	`enable_ebay_act_browser`	INTEGER,
	`enable_ebay_comp_browser`	INTEGER
);
INSERT INTO db_settings (setting_id, aws_sellerid, access_key, secret_key, refresh_minute, is_alert_mail, email_ids, sender_emailid, sender_emailpwd, sender_smtp, sender_port, sender_chkssl, send_mail_on_eachscan, Eb_App_ID, Eb_Dev_ID, Eb_Cer_ID, is_show_notification, is_show_notification_type, api_call_interval, keepa_access_key, mws_secretkey, mws_accesskey, mws_sellerid, mws_marketplaceid, api_call_intervalMWS, enable_schedule, schedule_time, enable_ebay, enable_ebay_act_browser, enable_ebay_comp_browser) VALUES (1, 'desaihardikj-21', 'AKIAIKO4O4JRUG5SOWBA', '3XKyJkqb0ZHJiRe4ZFSQnA+uM2hCcXJQB7GRc6H6', 2, 0, 'info@oozeetech.com', 'tdemo34@gmail.com', 'oozeetech@1234', 'smtp.gmail.com', 587, 1, 1, 'JustinWi-ATool-PRD-f5d8651d7-56f78bf3', '9cb9dbdb-2758-4488-9f71-b101b2598fc0', 'PRD-5d8651d7c9f0-5227-4db0-bd8f-749b', 1, 0, 200, '7v18rhcv59mhhlju9j0o39uojjhojo85ll07cptqk1h6h9qlnt7n4vv9un3fnvfd', 'xuamMLBo0zc1A+24qSIt+C4JSsW9qRqMCPNwhVWT', 'AKIAJUZLOKGNMX5QM7PQ', 'ABF6QR8S79RK1', 'ATVPDKIKX0DER', 0, 0, '09.30,10.30,18.00', 1, 0, 0);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
