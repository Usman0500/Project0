CREATE DATABASE P0_usman_bankingDB

USE P0_usman_bankingDB

CREATE TABLE users
(
username VARCHAR(20) PRIMARY KEY,
password VARCHAR(20),
user_role VARCHAR(10) CHECK (user_role IN('Admin','Customer'))
)

INSERT INTO users VALUES
('Ben','b_pass_5050','Admin'),
('Damian','d_pass_2050','Customer');


CREATE TABLE account_info
(
acc_no INT,
acc_name VARCHAR(20),
acc_type VARCHAR(20),
acc_balance FLOAT,
acc_is_active BIT,
acc_username VARCHAR(20)

CONSTRAINT PK_acc_no PRIMARY KEY(acc_no)
CONSTRAINT FK_acc_username FOREIGN KEY(acc_username) REFERENCES users
)


INSERT INTO account_info VALUES
(201,'Damian Bello','Savings',9000,1,'Damian')

CREATE TABLE new_service_request
(
request_id INT IDENTITY,
acc_no INT,
request_description VARCHAR(200),

CONSTRAINT PK_request_id PRIMARY KEY(request_id),
CONSTRAINT FK_acc_no FOREIGN KEY(acc_no) REFERENCES account_info
)


CREATE TABLE transactions
(
transaction_id INT IDENTITY,
acc_no INT,
transaction_date DATETIME DEFAULT GETDATE(),
transaction_type VARCHAR(20), 
transaction_amount FLOAT,

CONSTRAINT PK_transaction_id PRIMARY KEY (transaction_id),
CONSTRAINT FK_t_acc_no FOREIGN KEY(acc_no) REFERENCES account_info(acc_no)
)

INSERT INTO transactions(acc_no, transaction_type, transaction_amount)
VALUES(201,'Deposit',2000);


SELECT TOP 5 transaction_date,  transaction_type, transaction_amount
FROM transactions
WHERE acc_no = 201
ORDER BY transaction_date DESC;

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'transactions' AND TABLE_CATALOG = 'P0_usman_bankingDB';

INSERT INTO transactions (acc_no, transaction_type, transaction_amount)
VALUES (@acc_no, @transaction_type, @transaction_amount);

ALTER TABLE new_service_request
ADD request_date DATETIME DEFAULT GETDATE();

CREATE TABLE new_service_request
(
	request_id INT IDENTITY,
	acc_no INT,
	service_type VARCHAR(50) NOT NULL,
	request_status VARCHAR(30) NOT NULL,
	request_date DATETIME DEFAULT GETDATE(),

	CONSTRAINT PK_request_id PRIMARY KEY(request_id),
	CONSTRAINT FK_acc_no FOREIGN KEY(acc_no) REFERENCES account_info(acc_no),
	CONSTRAINT CHK_request_status CHECK (request_status IN ('Pending', 'Approved', 'Rejected'))
);

ALTER TABLE account_info 
ADD CONSTRAINT FK_acc_username FOREIGN KEY (acc_username) REFERENCES users;

SELECT 
name, definition
FROM
sys.check_constraints
WHERE
name = 'CHK_request_status'

([request_status]='Rejected' OR [request_status]='Approved' OR [request_status]='Pending')

ALTER TABLE new_service_request
DROP COLUMN request_description

ALTER TABLE transactions
DROP CONSTRAINT FK_t_acc_no

ALTER TABLE transactions
ADD CONSTRAINT FK_t_acc_no FOREIGN KEY(acc_no) REFERENCES account_info(acc_no) 
ON DELETE CASCADE;

ALTER TABLE users
ALTER COLUMN password VARCHAR(60);