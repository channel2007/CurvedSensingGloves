using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO.Ports;
using LinkedList;
using System.Threading;

public class MainCamera : MonoBehaviour
{
    // 連線COM(請查看您電腦藍牙接收器使用的COM編號作修改).
    const string PORT_NAME = "COM7";

    public Image ImageHandR = null;
    public Sprite ImageHandOpenR = null;
    public Sprite ImageHandCloseR = null;

    public Text ThumbText = null;                   // 拇指數值.
    public Text PointerText = null;                 // 食指數值.
    public Text MiddleText = null;                  // 中指數值
    public Text RingText = null;                    // 無名指數值.
    public Text LittleText = null;                  // 小指數值.

    public Slider SliderThumb = null;               // 拇指拉霸.
    public Slider SliderPointer = null;             // 食指拉霸.
    public Slider SliderMiddle = null;              // 中指拉霸.
    public Slider SliderRing = null;                // 無名指拉霸.
    public Slider SliderLittle = null;              // 小指拉霸.

    public InputField ThumbInputMax = null;         // 拇指張手最大值.
    public InputField PointerInputMax = null;       // 食指張手最大值.
    public InputField MiddleInputMax = null;        // 中指張手最大值.
    public InputField RingInputMax = null;          // 無名指張手最大值.
    public InputField LittleInputMax = null;        // 小指張手最大值.

    public InputField ThumbInputNow = null;         // 拇指數值.
    public InputField PointerInputNow = null;       // 食指數值.
    public InputField MiddleInputNow = null;        // 中指數值.
    public InputField RingInputNow = null;          // 無名指數值.
    public InputField LittleInputNow = null;        // 小指張數值.

    public InputField ThumbInputMin = null;         // 拇指張手最小值.
    public InputField PointerInputMin = null;       // 食指張手最小值.
    public InputField MiddleInputMin = null;        // 中指張手最小值.
    public InputField RingInputMin = null;          // 無名指張手最小值.
    public InputField LittleInputMin = null;        // 小指張手最小值.

    //------------------------------------------------------------------------
    // 手套相關,
    //------------------------------------------------------------------------
    private float thumbMax = 0;                     // 拇指最大數值.    
    private float pointerMax = 0;                   // 食指最大數值.    
    private float middleMax = 0;                    // 中指最大數值    
    private float ringMax = 0;                      // 無名指最大數值.    
    private float littleMax = 0;                    // 小指最大數值.

    private float thumbMin = 0;                     // 拇指最小數值.    
    private float pointerMin = 0;                   // 食指最小數值.    
    private float middleMin = 0;                    // 中指最小數值    
    private float ringMin = 0;                      // 無名指最小數值.    
    private float littleMin = 0;                    // 小指最小數值.

    private float thumbTransform = 0;               // 拇指轉換數值.    
    private float pointerTransform = 0;             // 食指轉換數值.    
    private float middleTransform = 0;              // 中指轉換數值    
    private float ringTransform = 0;                // 無名指轉換數值.    
    private float littleTransform = 0;              // 小指轉換數值.

    private float thumbShock = 0;                   // 拇指防震動.
    private float pointerShock = 0;                 // 食指防震動.
    private float middleShock = 0;                  // 中指防震動.
    private float ringShock = 0;                    // 無名指防震動.
    private float littleShock = 0;                  // 小指防震動.

    // 手指數據平均值物件.
    private BalancedValue thumbBalancedValue;
    private BalancedValue pointerBalancedValue;
    private BalancedValue middleBalancedValue;
    private BalancedValue ringBalancedValue;
    private BalancedValue littleBalancedValue;

    // 不全指令備份.
    private string receivedDataTemp = "";
    // 指令串列.
    private SinglyLinkedList<string> commandLinkedList = null;

    // 多執行緒.
    private Thread thread;
    private bool isRun = true;

    // 藍牙.
    private SerialPort sp;

    //------------------------------------------------------------------------
    // 初始.
    //------------------------------------------------------------------------
    void Start()
    {
        // 建立指令串列.
        commandLinkedList = new SinglyLinkedList<string>();

        // 藍牙.
        try
        {
            sp = new SerialPort("\\\\.\\" + PORT_NAME, 9600);
            sp.Open();
            sp.ReadTimeout = 50;
        }
        catch (System.IO.IOException e) { }
        catch (System.InvalidOperationException e) { }

        // 建立多執行緒接收指令.
        thread = new Thread(getReceivedData);
        thread.IsBackground = true;
        thread.Start();

        // 建立手指數據平均值物件.
        thumbBalancedValue = new BalancedValue(8);
        pointerBalancedValue = new BalancedValue(8);
        middleBalancedValue = new BalancedValue(8);
        ringBalancedValue = new BalancedValue(8);
        littleBalancedValue = new BalancedValue(8);
    }

    //------------------------------------------------------------------------
    // 更新.
    //------------------------------------------------------------------------
    void Update()
    {
        // 未與藍芽連線離開.
        if (!sp.IsOpen) { return; }

        try
        {
            // 最大值.
            thumbMax = float.Parse(ThumbInputMax.text);            // 拇指最大數值.
            pointerMax = float.Parse(PointerInputMax.text);        // 食指最大數值.
            middleMax = float.Parse(MiddleInputMax.text);          // 中指最大數值.
            ringMax = float.Parse(RingInputMax.text);              // 無名指最大數值.
            littleMax = float.Parse(LittleInputMax.text);          // 小指最大數值.

            // 現值.
            ThumbInputNow.text = thumbBalancedValue.Value.ToString();
            PointerInputNow.text = pointerBalancedValue.Value.ToString();
            MiddleInputNow.text = middleBalancedValue.Value.ToString();
            RingInputNow.text = ringBalancedValue.Value.ToString();
            LittleInputNow.text = littleBalancedValue.Value.ToString();

            // 最小值.
            thumbMin = float.Parse(ThumbInputMin.text);             // 拇指最小數值.
            pointerMin = float.Parse(PointerInputMin.text);         // 食指最小數值.    
            middleMin = float.Parse(MiddleInputMin.text);           // 中指最小.    
            ringMin = float.Parse(RingInputMin.text);               // 無名指最小.    
            littleMin = float.Parse(LittleInputMin.text);           // 小指最小.

            // 接收數值.            
            string value = getCommand();
            if (value != "")
            {
                // 解指令.
                string[] decoding = null;
                decoding = value.Split('|');

                // 判斷指令.
                switch (decoding[0])
                {
                    case "inf":      // 判斷手套指令.
                        // 解字串.
                        string[] infDecoding = null;
                        infDecoding = decoding[1].Split(',');

                        //----------------------------------------
                        // 拇指數.
                        //----------------------------------------
                        // 存入手套取的數值以計算平均值.
                        thumbBalancedValue.Add(float.Parse(infDecoding[0]));
                        // 避免手指抖動(將上一次與最新的平均值相減後取絕對值).
                        if (Math.Abs(thumbBalancedValue.Value - thumbShock) > 4)
                        {
                            // 取得平均值.
                            thumbShock = thumbBalancedValue.Value;
                            // 最大.
                            if (thumbShock > thumbMax)
                            {
                                thumbTransform = 100;
                            }
                            // 最小.
                            else if (thumbShock < thumbMin)
                            {
                                thumbTransform = 0;
                            }
                            // 中間.
                            else
                            {
                                // 換算顯示數值(0~100).
                                thumbTransform = Mathf.Round((100 / (thumbMax - thumbMin)) * (thumbShock - thumbMin));
                            }
                            // 顯示拉條位置.
                            SliderThumb.value = thumbTransform * 0.01f;
                            // 顯示數值.
                            ThumbText.text = thumbTransform.ToString();
                        }

                        //----------------------------------------
                        // 食指數值.
                        //----------------------------------------
                        // 存入手套取的數值以計算平均值.
                        pointerBalancedValue.Add(float.Parse(infDecoding[1]));
                        // 避免手指抖動(將上一次與最新的平均值相減後取絕對值).
                        if (Math.Abs(pointerBalancedValue.Value - pointerShock) > 4)
                        {
                            // 取得平均值.
                            pointerShock = pointerBalancedValue.Value;
                            // 最大.
                            if (pointerShock > pointerMax)
                            {
                                pointerTransform = 100;
                            }
                            // 最小.
                            else if (pointerShock < pointerMin)

                            {
                                pointerTransform = 0;
                            }
                            // 中間.
                            else
                            {
                                // 換算顯示數值(0~100).
                                pointerTransform = Mathf.Round((100 / (pointerMax - pointerMin)) * (pointerShock - pointerMin));
                            }
                            // 顯示拉條位置.
                            SliderPointer.value = pointerTransform * 0.01f;
                            // 顯示數值.
                            PointerText.text = pointerTransform.ToString();
                        }

                        //----------------------------------------
                        // 中指數值.
                        //----------------------------------------
                        // 存入手套取的數值以計算平均值.
                        middleBalancedValue.Add(float.Parse(infDecoding[2]));
                        // 避免手指抖動(將上一次與最新的平均值相減後取絕對值).
                        if (Math.Abs(middleBalancedValue.Value - middleShock) > 4)
                        {
                            // 取得平均值.
                            middleShock = middleBalancedValue.Value;
                            // 最大.
                            if (middleShock > middleMax)
                            {
                                middleTransform = 100;
                            }
                            // 最小.
                            else if (middleShock < middleMin)

                            {
                                middleTransform = 0;
                            }
                            // 中間.
                            else
                            {
                                // 換算顯示數值(0~100).
                                middleTransform = Mathf.Round((100 / (middleMax - middleMin)) * (middleShock - middleMin));
                            }
                            // 顯示拉條位置.
                            SliderMiddle.value = middleTransform * 0.01f;
                            // 顯示數值.
                            MiddleText.text = middleTransform.ToString();
                        }

                        //----------------------------------------
                        // 無名指數值.
                        //----------------------------------------
                        // 存入手套取的數值以計算平均值.
                        ringBalancedValue.Add(float.Parse(infDecoding[3]));
                        // 避免手指抖動(將上一次與最新的平均值相減後取絕對值).
                        if (Math.Abs(ringBalancedValue.Value - ringShock) > 4)
                        {
                            // 取得平均值.
                            ringShock = ringBalancedValue.Value;
                            // 最大.
                            if (ringShock > ringMax)
                            {
                                ringTransform = 100;
                            }
                            // 最小.
                            else if (ringShock < ringMin)

                            {
                                ringTransform = 0;
                            }
                            // 中間.
                            else
                            {
                                // 換算顯示數值(0~100).
                                ringTransform = Mathf.Round((100 / (ringMax - ringMin)) * (ringShock - ringMin));
                            }
                            // 顯示拉條位置.
                            SliderRing.value = ringTransform * 0.01f;
                            // 顯示數值.
                            RingText.text = ringTransform.ToString();
                        }

                        //----------------------------------------
                        // 小指數值.
                        //----------------------------------------
                        // 存入手套取的數值以計算平均值.
                        littleBalancedValue.Add(float.Parse(infDecoding[4]));
                        // 避免手指抖動(將上一次與最新的平均值相減後取絕對值).
                        if (Math.Abs(littleBalancedValue.Value - littleShock) > 4)
                        {
                            // 取得平均值.
                            littleShock = littleBalancedValue.Value;
                            // 最大.
                            if (littleShock > littleMax)
                            {
                                littleTransform = 100;
                            }
                            // 最小.
                            else if (littleShock < littleMin)

                            {
                                littleTransform = 0;
                            }
                            // 中間.
                            else
                            {
                                // 換算顯示數值(0~100).
                                littleTransform = Mathf.Round((100 / (littleMax - littleMin)) * (littleShock - littleMin));
                            }
                            // 顯示拉條位置.
                            SliderLittle.value = littleTransform * 0.01f;
                            LittleText.text = littleTransform.ToString();
                        }
                        break;
                }
            }
        }
        catch (TimeoutException)
        {}
    }

    //------------------------------------------------------------------------
    // 離開程式.
    //------------------------------------------------------------------------
    void OnApplicationQuit()
    {
        isRun = false;
        thread = null;
        if (sp != null && sp.IsOpen)
            sp.Close();
    }

    //------------------------------------------------------------------------
    // 取得HC-05傳送的指令.
    //------------------------------------------------------------------------
    public String getCommand()
    {
        string s = "";

        s = "";
        if (commandLinkedList.Count > 0)
        {
            s = commandLinkedList.First.Value;
            commandLinkedList.RemoveFirst();
            return s;
        }
        return "";
    }

    //------------------------------------------------------------------------
    // 接收指令.
    //------------------------------------------------------------------------
    private string receivedData = "";
    public void getReceivedData()
    {
        while (isRun)
        {
            try
            {
                string s = "";
                int len = 0;

                // 組合字串.
                receivedData += sp.ReadLine();
                // 分解字串.
                len = receivedData.Length;
                for (int i = 0; i < len; i++)
                {
                    // 組合指令.
                    if (receivedData[i] != '~')
                    {
                        s += receivedData[i];
                    }
                    else
                    {
                        // 加入指令.
                        commandLinkedList.AddLast(s);
                        s = "";
                    }
                }
                receivedData = s;
            }
            catch (System.InvalidOperationException e) { }
            catch (System.TimeoutException e) { }
        }
    }

}
