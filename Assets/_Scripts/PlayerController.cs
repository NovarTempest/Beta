using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    private bool isMoving;      //para que detecte si se esta moviendo o no el player, asi dejarlo en el ultimo frame.
    public float speed;         //para que aumentes o disminuyas la velocidad.
    private Vector2 input;      //para ingresar la entrada de la coordenada del player.

    private Animator _animator;     // variable para la animacion
    void Awake()
        {
            _animator = GetComponent<Animator>();               // en cargado de capturar la animacion
        }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!isMoving)       //solo da el siguiente paso si no se esta moviendo
        {
                    //usamos GetAxisRaw porque da valores exactos, osea te mueves 1 o 0 .
                    //En cambio el GetAxis te permite moverte hasta en decimales y como estos es 2D o te mueves o no te mueves.
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if(input.x != 0)                //mientras x se este moviendo Y valdra 0
            {
                input.y = 0;                //asi evitas que se mueva en diagional
            }

            if(input != Vector2.zero)   //solo se mueve si el vector es diferente a 0
            {
                    _animator.SetFloat("Move X", input.x);
                    _animator.SetFloat("Move Y", input.y);

                var targetPosition = transform.position;    //cambia la posicion en el eje seleccionado, como escogimos un vector2 solo se movera en 2 direcciones
                targetPosition.x += input.x;
                targetPosition.y += input.y;

                StartCoroutine(MoveTowards(targetPosition));
            }
        }
    }

    private void LateUpdate()
    {
        _animator.SetBool("Is Moving", isMoving);
    }
    IEnumerator MoveTowards (Vector3 destination)
    {
        //mientras la distancia de donde estoy y mi destino sea superior a la presicion de la maquina, se desplazara un poco 
        isMoving = true;
        while (Vector3.Distance(transform.position, destination) > Mathf.Epsilon)  
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            yield return null;      // no devolver nada y esperar al siguiente frame
        }
        transform.position = destination;
        isMoving = false;
    }
}
