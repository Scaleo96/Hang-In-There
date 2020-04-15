using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Helper : MonoBehaviour
    {
        [Range(-1, 3)]
        [SerializeField]
        private float vertical;

        public string animName;
        public bool playAnim;

        Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playAnim)
            {
                //vertical = 0;
                anim.CrossFade(animName, 0.2f);
                playAnim = false;
                    
            }

            anim.SetFloat("Vertical", vertical);

        }
    }
}
