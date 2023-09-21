using System.IO;
using UnityEngine;

public class training_info : MonoBehaviour
{
    private string logFileName;

    private void Awake()
    {
        // �Ыؤ@�Ӱߤ@����x���W�A�]�t����ɶ��H��
        logFileName = string.Format("log_{0:yyyyMMdd_HHmmss}.txt", System.DateTime.Now);
        //Debug.Log(logFileName);
        // �]�w��x��󪺧�����|
        string logFilePath = Path.Combine(Application.dataPath, logFileName);
        //Debug.Log(logFilePath);

        // �ˬd���O�_�s�b�A�p�G���s�b�A�h�Ыطs���
        if (!File.Exists(logFilePath))
        {
            File.Create(logFilePath).Close();
        }
    }

    private void OnEnable()
    {
        // �q�\ Unity �� logMessageReceived �ƥ�
        Application.logMessageReceived += LogMessageReceived;
    }

    private void OnDisable()
    {
        // �����q�\ logMessageReceived �ƥ�
        Application.logMessageReceived -= LogMessageReceived;
    }

    private void LogMessageReceived(string logMessage, string stackTrace, LogType logType)
    {
        // �����x��󪺧�����|
        string logFilePath = Path.Combine(Application.dataPath, logFileName);

        // �N��x�T���g�J���x���
        string log = string.Format("[{0}] {1}: {2}\n", System.DateTime.Now, logType, logMessage);
        File.AppendAllText(logFilePath, log);
    }
}