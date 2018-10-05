#include <SoftwareSerial.h>

#define PIN_THUMB   A0    // 拇指.
#define PIN_POINTER A1    // 食指.
#define PIN_MIDDLE  A2    // 中指.
#define PIN_RING    A3    // 無名指.
#define PIN_LITTLE  A6    // 小指.

// HC-05.
SoftwareSerial BT(10, 11);

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

// 手套數值.
String InfinityStr = "";

// 時脈相關變數.
unsigned long timer = 0;
float timeTick = 0;

//-------------------------------------------------------
// 初始.
//-------------------------------------------------------
void setup() {    
  BT.begin(9600);  
}

//-------------------------------------------------------
// 主迴圈.
//-------------------------------------------------------
void loop() {  
  // 紀錄時脈.      
  timer = millis();
  
  //-------------------------------------------------------
  // 手指彎曲數值.  
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

  // 判斷時脈傳送.
  if(timeTick > 25){
    // 組合手指數值指令.
    InfinityStr = "inf|" + (String)thumbValue + "," + (String)pointerValue + "," + (String)middleValue + "," + (String)ringValue + "," + (String)littleValue + "|~";    
    // 將組合字串透過HC-05傳送給PC.
    BT.println(InfinityStr);
    // 初始時脈.
    timeTick = 0;  
  }
  timeTick += millis()-timer;  
}
