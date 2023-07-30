using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameManager
{
    public class Loading : MonoBehaviour
    {
        public GameObject Background, clock;
        public TMP_Text Txt;
        public float rotationSpeed, speed;
        public Image LoadingImage;

        private Coroutine rotateCoroutine, loadTxtCoroutine, DelayCoroutine; // Lưu trữ tham chiếu tới Coroutine.

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

        public void SetUpImage(Sprite sprite)
        {
            LoadingImage.sprite = sprite;
        }

        public void Begin()
        {
            Background.gameObject.SetActive(true);
            Game_Manager.Instance.IsBusy = true;
            if (rotateCoroutine == null)
            {
                rotateCoroutine = StartCoroutine(RotateCoroutine());
            }
            if (loadTxtCoroutine == null)
            {
                loadTxtCoroutine = StartCoroutine(LoadTxtCoroutine());
            }
            if (DelayCoroutine != null)
            {
                StopCoroutine(DelayCoroutine);
                DelayCoroutine = null;
            }
        }

        public void End()
        {
            if (DelayCoroutine == null)
            {
                DelayCoroutine = StartCoroutine(DelayBackground());
            }
        }

        IEnumerator DelayBackground()
        {
            yield return new WaitForSeconds(3f);
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
            Game_Manager.Instance.IsBusy = false;
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
