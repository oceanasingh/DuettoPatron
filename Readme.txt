OCEAN VIEW RESORT Player value service is used to push CSV file to the destinated 
location on network through SFTP. 

CSV File format- Player ID, ADT

Setting- 
	sqldb - Is used for SQL paramaters.
		Connection - SQL Connection string
		PullFrequency (IN MINUTES) - Frequency of pulling data from SQL server database.
			PullFrequency can't be less than 1 minute.
		DateFormat - Is Date format of SQL date field i.e.("yyyy-MM-dd HH:mm:ss.fff")
		LastUpdate - field is used to query data. If it is blank it rtetrives data for last 1 year.
			It keeps updated after every sent.

	sftp - Secure FTP parameters
		Host- Host IP address
		User - Username
		Password - Password
		Path - where you want to copy the csv files make sure "/" is at the end of path i.e. ("/home/sftpDuetto/")

	hotelName - Give hotel name.
