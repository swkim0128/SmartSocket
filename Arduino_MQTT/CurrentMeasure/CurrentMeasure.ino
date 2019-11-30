#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <ArduinoJson.h>
#include <String>

WiFiClient client;

/*
 * Wifi setting
 */
const char* ssid = "suntech";
const char* password = "suntech3567";
const char* host = "192.168.0.158";
const int port = 2020;
char packetBuffer[255];
char replayBuffer[255];

StaticJsonBuffer<200> jsonBuffer;
JsonObject& root = jsonBuffer.createObject();

/*
 * 전력 측정 세팅
 */
const int analogIn = 0;
int mVerAmp = 66;
int ACSoffset = 2500;

double rawValue = 0;
double voltage = 0;
double current = 0;

// the setup function runs once when you press reset or power the board
void setup() {
  // initialize digital pin LED_BUILTIN as an output.
  pinMode(A0, INPUT);
  Serial.begin(115200);
  
  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  
  WiFi.begin(ssid, password);
  Serial.println("Wait for WiFi... ");

  while(WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  delay(1000);
}

// the loop function runs over and over again forever
void loop() {
  /*
  rawValue = analogRead(A0);
  voltage = ((rawValue - 236) / 1024.0) * 5000;
  current = ((voltage - ACSoffset) / mVerAmp);

  Serial.print("RawValue = ");
  Serial.print(rawValue);
  Serial.print("\t mV = ");
  Serial.print((voltage - ACSoffset) / 1000, 3);
  Serial.print("\t Amp = ");
  Serial.print(current, 3);
  Serial.println();
*/
  delay(5000);

  Serial.print("connecting to ");
  Serial.print(host);
  Serial.print(":");
  Serial.println(port);
  
  if(!client.connect(host, port)) {
    Serial.println("Connection failed");
  }
  else {
    Serial.println("Connected");

    Serial.println("sending data to server");

    if(client.connected()){
      String data = "";
      root["measureProduct_id"] = "01";
      root["power"] = "1.212";
      root.printTo(data);

      String send_data = "0001:" + data;
      Serial.println(send_data);
      client.println(send_data);
    }
  }
}
