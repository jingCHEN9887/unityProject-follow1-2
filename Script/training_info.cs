using System.IO;
using UnityEngine;

public class training_info : MonoBehaviour
{
    private string logFileName;

    private void Awake()
    {
        // 創建一個唯一的日誌文件名，包含日期時間信息
        logFileName = string.Format("log_{0:yyyyMMdd_HHmmss}.txt", System.DateTime.Now);
        //Debug.Log(logFileName);
        // 設定日誌文件的完整路徑
        string logFilePath = Path.Combine(Application.dataPath, logFileName);
        //Debug.Log(logFilePath);

        // 檢查文件是否存在，如果不存在，則創建新文件
        if (!File.Exists(logFilePath))
        {
            File.Create(logFilePath).Close();
        }
    }

    private void OnEnable()
    {
        // 訂閱 Unity 的 logMessageReceived 事件
        Application.logMessageReceived += LogMessageReceived;
    }

    private void OnDisable()
    {
        // 取消訂閱 logMessageReceived 事件
        Application.logMessageReceived -= LogMessageReceived;
    }

    private void LogMessageReceived(string logMessage, string stackTrace, LogType logType)
    {
        // 獲取日誌文件的完整路徑
        string logFilePath = Path.Combine(Application.dataPath, logFileName);

        // 將日誌訊息寫入到日誌文件
        string log = string.Format("[{0}] {1}: {2}\n", System.DateTime.Now, logType, logMessage);
        File.AppendAllText(logFilePath, log);
    }
}