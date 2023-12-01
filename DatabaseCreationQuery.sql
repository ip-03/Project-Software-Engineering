CREATE TABLE gateway(
	gateway_id nvarchar(32) NOT NULL,
	latitude float NULL,
	longitude float NULL,
	altitude int NULL,
	rssi float NULL,
	snr float NULL,
	avg_airtime float NULL,
	PRIMARY KEY (gateway_id)
)
CREATE TABLE device(
	device_id int NOT NULL,
	gateway_id nvarchar(32) NULL,
	PRIMARY KEY (device_id),
	FOREIGN KEY (gateway_id) REFERENCES gateway(gateway_id)
)
CREATE TABLE battery_status(
	device_id int NOT NULL,
	battery_status int NULL,
	BatV float NULL,
	PRIMARY KEY (device_id),
	FOREIGN KEY (device_id) REFERENCES device(device_id)
)
CREATE TABLE sensor_data(
	data_id int IDENTITY(1, 1) NOT NULL,
	device_id int NULL,
	temperature_in float NULL,
	temperature_out float NULL,
	humidity float NULL,
	ambient_light float NULL,
	barometric_pressure float NULL,
	date_time datetime NULL,
	PRIMARY KEY (data_ID),
	FOREIGN KEY (device_id) REFERENCES device(device_id)
)