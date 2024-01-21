/**
 * @file LoRaEnvSensor.ino
 *
 * @brief Arduino sketch for reading sensor data from the MKR WAN and transmitting it using LoRa communication.
 *
 * This sketch utilizes the Arduino MKR WAN 1300 board along with the MKR ENV shield to read temperature, humidity,
 * and pressure sensor data. The data is then encoded in Base64 format and transmitted over a LoRa network using
 * (OTAA).
 *
 * @author Project Software Engineering Group 5
 */

#include <Wire.h>
#include <Arduino_MKRENV.h>
#include <MKRWAN.h>
#include "base64.hpp"

LoRaModem modem;

/**
 * @brief Arduino setup function.
 *
 * This function initializes the serial communication, LoRa modem, and the environmental sensor.
 * It also connects to the LoRa network using OTAA.
 */
void setup() {
  Serial.begin(9600);
  while (!Serial);

  // Initialize the LoRa modem with the proper band, baud rate, and configuration
  if (!modem.begin(EU868)) {
    Serial.println("Failed to start module");
    while (1) {}
  }

  Serial.print("Your module version is: ");
  Serial.println(modem.version());

  // Connect to the LoRa network using OTAA (Over-The-Air Activation)
  int connected = modem.joinOTAA("0000000000000000", "B96BBC8CA38427DA8FBBD3F5A7994894", "A8610A34321C8711", 60000); // Adjust timeout as needed

  if (!connected) {
    Serial.println("Failed to join the network");
    while (1) {}
  }

  // Initialize the MKR ENV shield
  if (!ENV.begin()) {
    Serial.println("Failed to initialize MKR ENV shield!");
    while (1) {}
  }
}

/**
 * @brief Arduino loop function.
 *
 * This function reads sensor data, encodes it in Base64, and transmits it over the LoRa network.
 * It then waits for a specified duration before sending the next packet.
 */
void loop() {
  // Read sensor data
  float temperature = ENV.readTemperature();
  float humidity = ENV.readHumidity();
  float pressure = ENV.readPressure();

  // Convert sensor data to byte array
  byte data[sizeof(float) * 3];
  memcpy(data, &temperature, sizeof(float));
  memcpy(data + sizeof(float), &humidity, sizeof(float));
  memcpy(data + 2 * sizeof(float), &pressure, sizeof(float));

  // Display the raw byte data on the serial monitor
  for (size_t i = 0; i < sizeof(data); i++) {
    Serial.print(data[i], HEX);
    Serial.print(" ");
  }
  Serial.println();

  // Encode data to Base64
  unsigned char base64[9]; // 8 bytes for output + 1 for null terminator
  unsigned int base64_length = encode_base64(data, 3, base64);

  // Start the LoRa packet
  modem.beginPacket();

  // Write the Base64 encoded data to the LoRa packet
  modem.write(base64);

  // End the LoRa packet and send it
  int err = modem.endPacket(true);

  // Check if the packet was sent successfully
  if (err > 0) {
    Serial.println("Packet sent successfully!");
  } else {
    Serial.println("Error sending packet");
  }

  // Wait for some time before sending the next packet
  delay(90000);
}