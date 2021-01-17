using UnityEngine;
using UnityEngine.UI;
using System.Collections;       // 引用 系統.集合 API (包含協同程序)

public class NPC : MonoBehaviour
{
    [Header("NPC 資料")]
    public NPCData data;
    [Header("對話框")]
    public GameObject dialog;
    [Header("對話內容")]
    public Text textContent;
    [Header("對話者名稱")]
    public Text textName;
    [Header("對話間隔")]
    public float interval = 0.2f;

    public Animator Npc;

    // 定義列舉 eunm (下拉式選單 - 只能選一個)
    public enum NPCState
    {
        FirstDialog, Missioning, Finish
    }

    // 列舉欄位
    // 修飾詞 列舉名稱 自訂欄位名稱 指定 預設值；
    [Header("NPC 狀態")]
    public NPCState state = NPCState.FirstDialog;

    /// <summary>
    /// 玩家是否進入感應區
    /// </summary>
    private bool playerInArea;

    /// <summary>
    /// 玩家是否曾進入過感應區
    /// </summary>
    int inarea;

    


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "小明")
        {
            playerInArea = true;
            StartCoroutine(Dialog());
            Npc.SetBool("玩家靠近", true);
            inarea++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "小明")
        {
            playerInArea = false;
            StopDialog();
            Npc.SetBool("玩家靠近", false);
        }
    }

    /// <summary>
    /// 停止對話
    /// </summary>
    private void StopDialog()
    {
        dialog.SetActive(false);    // 關閉對話框
        StopAllCoroutines();        // 停止所有協程
    }

    /// <summary>
    /// 開始對話
    /// </summary>
    private IEnumerator Dialog()
    {
        
        // 顯示對話框
        dialog.SetActive(true);
        // 清空文字
        textContent.text = "";
        // 對話者名稱 指定為 此物件的名稱
        textName.text = name;

        // 要說的對話
        string dialogString = data.dialogB;

        if (inarea >= 1)
        {
            state = NPCState.Missioning;
        }
        if(inarea >= 1&&data.countCurrent==data.count)
        {
            state = NPCState.Finish;
        }

        // 判斷 NPC 狀態 來顯示對應的 對話內容
        switch (state)
        {
            case NPCState.FirstDialog:
                dialogString = data.dialogA;
                break;
            case NPCState.Missioning:
                dialogString = data.dialogB;
                break;
            case NPCState.Finish:
                dialogString = data.dialogC;
                break;
        }

        // 字串的長度 dialogA.Length
        for (int i = 0; i < dialogString.Length; i++)
        {
            // print(data.dialogA[i]);
            // 文字 串聯 
            textContent.text += dialogString[i] + "";
            yield return new WaitForSeconds(interval);
        }
        
    }
}
