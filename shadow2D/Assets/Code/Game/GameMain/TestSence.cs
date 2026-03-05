using System.Collections;
using UnityEngine;

namespace Assets.Code.GameMain
{
    public class TestSence : MonoBehaviour
    {

        public Transform camera;
        public Transform player;
        public float speed;

        public Transform arrow;
        public Transform Target;


        // Use this for initialization
        void Start()
        {

        }

        
        // Update is called once per frame
        void Update()
        {
            Vector3 dire = Target.position - arrow.position;
            float zAngle = Mathf.Atan2(dire.y, dire.x) * (180 / Mathf.PI);

            Debug.Log(zAngle);

            arrow.eulerAngles = new Vector3(0, 0, zAngle);
            //TryMove();
        }

        private bool TryMove()
        {
            float hInput = Input.GetAxisRaw("Horizontal");
            float vInput = Input.GetAxisRaw("Vertical");
            var dir = new Vector3(hInput, vInput, 0);

            if (!checkHaveInput())
                dir = Vector3.zero;

            if (dir == Vector3.zero)
                return false;

            //avComp.Forward = dir.normalized;
            player.position = player.position + dir.normalized * speed * Time.deltaTime;
            camera.position = player.position;
            return true;
        }

        private bool checkHaveInput()
        {
            if (Input.GetKey(KeyCode.W)) return true;
            if (Input.GetKey(KeyCode.A)) return true;
            if (Input.GetKey(KeyCode.S)) return true;
            if (Input.GetKey(KeyCode.D)) return true;
            if (Input.GetKey(KeyCode.UpArrow)) return true;
            if (Input.GetKey(KeyCode.LeftArrow)) return true;
            if (Input.GetKey(KeyCode.DownArrow)) return true;
            if (Input.GetKey(KeyCode.RightArrow)) return true;
            return false;
        }
    }
}