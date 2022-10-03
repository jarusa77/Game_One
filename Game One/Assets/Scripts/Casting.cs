using UnityEngine;
using UnityEngine.InputSystem;

public class Casting : MonoBehaviour
{

    public GameObject spellItem;
    public Transform castPoint;
    public bool canCast = true;

    private float coolDownTimer = Mathf.Infinity;
    public float coolDown = 2f;

    private Animator Anim;


    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer += Time.deltaTime;
        

        if (coolDownTimer > coolDown && !canCast)
        {
            UnityEngine.Debug.Log("coolDownTimer: " + string.Format("{0:N2}", coolDownTimer));
            canCast = true;
            coolDownTimer = 0;
            
        }

    }


    public void Cast(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            

            if (!canCast)
                return;

            UnityEngine.Debug.Log("Cast");

            canCast = false;


            GameObject si = Instantiate(spellItem, castPoint);
            si.transform.parent = null;

            Anim.SetTrigger("Cast");
        }


            

    }
}
