using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
    public class Loading : MonoBehaviour
    {
        public GameObject Background, clock;
        public TMP_Text Txt;
        public float rotationSpeed, speed;

        private Coroutine rotateCoroutine, loadTxtCoroutine; // Lưu trữ tham chiếu tới Coroutine.

        List<string> list = new List<string> { ".", "..", "..." };

        public static Loading Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            rotationSpeed = -300f;
            speed = 0.5f;
        }

        public void Begin()
        {
            Debug.Log("Startttttttttttttttttttt");
            Debug.Log("11111");
            Background.gameObject.SetActive(true);

            if (rotateCoroutine == null)
            {
                rotateCoroutine = StartCoroutine(RotateCoroutine());
            }
            if (loadTxtCoroutine == null)
            {
                loadTxtCoroutine = StartCoroutine(LoadTxtCoroutine());
            }
            //rotateCoroutine = StartCoroutine(RotateCoroutine());
            //loadTxtCoroutine = StartCoroutine(LoadTxtCoroutine());
        }

        public void End()
        {
            Debug.Log("Endddddddd");

            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
            }
            if (loadTxtCoroutine != null)
            {
                StopCoroutine(loadTxtCoroutine);
            }
            rotateCoroutine = null;
            loadTxtCoroutine = null;
            Background.gameObject.SetActive(false);

        }

        IEnumerator RotateCoroutine()
        {
            while (true)
            {
                // Lấy giá trị hiện tại của rotation của đối tượng.
                Vector3 currentRotation = clock.transform.rotation.eulerAngles;

                // Thay đổi giá trị rotation.z bằng cách tăng nó theo tốc độ xoay.
                float newRotationZ = currentRotation.z + rotationSpeed * Time.deltaTime;

                // Tạo một Vector3 mới để cập nhật giá trị rotation của đối tượng.
                Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y, newRotationZ);

                // Cập nhật giá trị rotation của đối tượng.
                clock.transform.rotation = Quaternion.Euler(newRotation);
                yield return null;
            }
        }
        IEnumerator LoadTxtCoroutine()
        {
            var i = 0;
            while (true)
            {
                ++i;
                Txt.text = list[i % 3];
                yield return new WaitForSecondsRealtime(speed);
            }
        }

    }
}
