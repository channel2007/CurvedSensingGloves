
#define PIN_THUMB   A0    // 拇指.
#define PIN_POINTER A1    // 食指.
#define PIN_MIDDLE  A2    // 中指.
#define PIN_RING    A3    // 無名指.
#define PIN_LITTLE  A4    // 小指.

// 拇指數值.
int thumbValue = 0;
// 食指數值.
int pointerValue = 0;
// 中指數值.
int middleValue = 0;
// 無名指數值.
int ringValue = 0;
// 小指數值.
int littleValue = 0;

String str = "";

//-------------------------------------------------------
// 初始.
//-------------------------------------------------------
void setup() {  
  Serial.begin(9600);
}

//-------------------------------------------------------
// 主迴圈.
//-------------------------------------------------------
void loop() {  
  // 拇指數值.
  thumbValue = analogRead(PIN_THUMB);
  // 食指數值.
  pointerValue = analogRead(PIN_POINTER);
  // 中指數值.
  middleValue = analogRead(PIN_MIDDLE);
  // 無名指數值.
  ringValue = analogRead(PIN_RING);
  // 小指數值.
  littleValue = analogRead(PIN_LITTLE);

  // 顯示手指個部分訊息(拇指數值|食指數值|中指數值|無名指數值|小指數值|)
  str = (String)thumbValue + "|" + (String)pointerValue + "|" + (String)middleValue + "|" + (String)ringValue + "|" + (String)littleValue + "|";  
  Serial.println(str);
  
  delay(100);
}
