using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }

    public SpawnableObject[] easyObjects;   // ���� 1
    public SpawnableObject[] normalObjects; // ���� 2
    public SpawnableObject[] hardObjects;   // ���� 3
    public float easySpawnRate = 5f;   // ��������ʻ͹���� 1
    public float normalSpawnRate = 3f; // ��������ʻ͹���� 2
    public float hardSpawnRate = 1f;   // ��������ʻ͹���� 3
    public float modeDuration = 30f;   // ����������������

    private int[] modeSequence = new int[] { 1, 2, 1, 2, 3 }; // ez normal ez normal hard
    private int sequenceIndex = 0;
    private int currentMode = 1;
    private float timer = 0f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnObject();

            float currentSpawnRate = currentMode == 1 ? easySpawnRate :
                                   currentMode == 2 ? normalSpawnRate :
                                   hardSpawnRate;

            yield return new WaitForSeconds(currentSpawnRate);
            timer += currentSpawnRate;

            if (timer >= modeDuration)
            {
                timer = 0f;
                sequenceIndex = (sequenceIndex + 1) % modeSequence.Length;
                currentMode = modeSequence[sequenceIndex];
                Debug.Log($"Mode: {currentMode} ({(currentMode == 1 ? "Easy" : currentMode == 2 ? "Normal" : "Hard")})");
            }
        }
    }

void SpawnObject()
{
    SpawnableObject[] currentObjects = currentMode == 1 ? easyObjects :
                                     currentMode == 2 ? normalObjects :
                                     hardObjects;

    float spawnChance = Random.value;
    foreach (SpawnableObject obj in currentObjects)
    {
        if (spawnChance < obj.spawnChance)
        {
            GameObject spawned = Instantiate(obj.prefab);

            // ตรวจสอบว่าเป็นวัตถุ Bird 2 หรือไม่
            if (obj.prefab.name == "Bird 2")
            {
                // ถ้าเป็น Bird 2 ให้ยกตำแหน่งแกน y สูงขึ้น 2 หน่วย
                spawned.transform.position = transform.position + new Vector3(0, 1.3f, 0);
            }
            else
            {
                // ถ้าเป็นวัตถุอื่น ให้ spawn ตามปกติ
                spawned.transform.position = transform.position;
            }
            break;
        }
        spawnChance -= obj.spawnChance;
    }
}

}