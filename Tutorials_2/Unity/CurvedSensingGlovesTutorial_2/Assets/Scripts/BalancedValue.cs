using LinkedList;

public class BalancedValue{
    // 均值.
    public float Value = 0;

    // 串列容器.
    private SinglyLinkedList<float> singlyLinkedList = null;
    private int containerMax = 0;
    private int containerNow = 0;

    //------------------------------------------------------------------------
    // 建構式.
    //------------------------------------------------------------------------
    public BalancedValue(int value)
    {
        // 串列容器.
        singlyLinkedList = new SinglyLinkedList<float>();
        // 紀錄容器數量.
        containerMax = value;
    }

    //------------------------------------------------------------------------
    // 增加數值.
    //------------------------------------------------------------------------
    public void Add(float value)
    {
        float valueAverage = 0f;

        // 容器未滿前處理.
        if (containerNow < containerMax)
        {
            singlyLinkedList.AddLast(value);
            containerNow++;
        }
        // 容器已滿處理.
        else
        {
            singlyLinkedList.RemoveFirst();
            singlyLinkedList.AddLast(value);
        }

        // 算出均值.
        Node<float> n = singlyLinkedList.First;
        for (int i = 0; i < singlyLinkedList.Count; i++)
        {
            valueAverage += n.Value;
            n = n.Next;
        }
        Value = (valueAverage / singlyLinkedList.Count);
    }

}
