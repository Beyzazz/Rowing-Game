using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public int VerticalKeyboard
    {
        get
        {
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W))
                return 0;
            return Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0;
        }
    }

    public int HorizontalKeyboard
    {
        get
        {
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
                return 0;
            return Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
        }
    }

    public int VerticalArrow
    {
        get
        {
            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.UpArrow))
                return 0;
            return Input.GetKey(KeyCode.DownArrow) ? -1 : Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
        }
    }

    public int HorizontalArrow
    {
        get
        {
            if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
                return 0;
            return Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
        }
    }

    public bool BoostKeyboard
    {
        get
        {
            if (Input.GetKeyDown(KeyCode.Space)) return true;
            return false;
        }
    }
    public bool BoostArrow
    {
        get
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter)) return true;
            return false;
        }
    }

    public bool canMove;

    public bool isKeyboard;

    public Transform player;
    public float power;
    public float maxSpeed;

    public Transform leftSpade, rightSpade;

    public Image boostBar;
    public Animation animation;

    private Vector3 _leftStartPos, _rightStartPos;

    private bool _isMoving;
    private bool _isSidePressed;
    private float _moveSpeed = 6f;
    private float _moveDistance = 3f;
    private float _moveTimer;

    private Rigidbody _rb;
    private bool _isBoostActive;
    public ParticleSystem explosionParticle;
    public AudioClip crashSound;
    private AudioSource playerAudio;

    private void Awake()
    {
        _leftStartPos = leftSpade.localPosition;
        _rightStartPos = rightSpade.localPosition;
        _rb = GetComponent<Rigidbody>();
        playerAudio =GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        HandleRotation();
        HandleMovement();
        HandleAnim();
    }
    
    
    private void Update()
    {
        HandleBoost();
    }

    private void HandleRotation()
    {
        float rotationChange = 30f;
        if (isKeyboard)
            rotationChange *= HorizontalKeyboard;
        else
            rotationChange *= HorizontalArrow;

        transform.rotation *= Quaternion.Euler(0.0f, rotationChange * Time.fixedDeltaTime, 0.0f);
    }

    private void HandleMovement()
    {
        var forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);

        if (isKeyboard)
        {
            if (VerticalKeyboard != 0)
                PhysicsHelper.ApplyForceToReachVelocity(_rb, forward * (VerticalKeyboard == 1 ? maxSpeed : -maxSpeed), power);
        }
        else
        {
            if (VerticalArrow != 0)
                PhysicsHelper.ApplyForceToReachVelocity(_rb, forward * (VerticalArrow == 1 ? maxSpeed : -maxSpeed), power);
        }
    }

    private void HandleAnim()
    {
        if (isKeyboard)
            SpadeAnim(VerticalKeyboard, HorizontalKeyboard);
        else
            SpadeAnim(VerticalArrow, HorizontalArrow);
    }

    private void HandleBoost()
    {
        if (_isBoostActive) return;

        if (isKeyboard)
        {
            if (BoostKeyboard)
                StartCoroutine(BoostDelay());
        }
        else
        {
            if (BoostArrow)
                StartCoroutine(BoostDelay());
        }
    }

    private IEnumerator BoostDelay()
    {
        var currentClip = animation.clip;
        var currentTime = animation[animation.clip.name].time;
        animation.Stop();

        _isBoostActive = true;
        var defMaxSpeed = maxSpeed;
        var boostValue = boostBar.fillAmount / 2 + 1.1f;
        maxSpeed *= boostValue;

        yield return new WaitForSeconds(1f);

        animation.clip = currentClip;
        animation[animation.clip.name].time = currentTime;
        animation.Play();

        _isBoostActive = false;
        maxSpeed = defMaxSpeed;
    }
    private void SpadeAnim(int vertical, int horizontal)
    {
        if (vertical != 0)
            _isMoving = true;
        else
            _isMoving = false;

        if (horizontal == 0)
            _isSidePressed = false;

        if (vertical == 0 && horizontal == 0)
            _moveTimer = 0f;

        if (horizontal == -1 && !_isMoving)
        {
            _isSidePressed = true;
            _moveTimer += Time.deltaTime * _moveSpeed;
            float offset = Mathf.Sin(_moveTimer) * _moveDistance;

            MoveSpade(leftSpade, _leftStartPos);
            MoveSpade(rightSpade, _rightStartPos - Vector3.forward * offset);
        }
        else if (horizontal == 1 && !_isMoving)
        {
            _isSidePressed = true;
            _moveTimer += Time.deltaTime * _moveSpeed;
            float offset = Mathf.Sin(_moveTimer) * _moveDistance;
            MoveSpade(rightSpade, _rightStartPos);
            MoveSpade(leftSpade, _leftStartPos - Vector3.forward * offset);
        }
        else if (_isMoving)
        {
            _isSidePressed = false;
            _moveTimer += Time.deltaTime * _moveSpeed;
            float offset = Mathf.Sin(_moveTimer) * _moveDistance;

            MoveSpade(leftSpade, _leftStartPos - Vector3.forward * offset);
            MoveSpade(rightSpade, _rightStartPos - Vector3.forward * offset);
        }
        else if (!_isSidePressed && !_isMoving)
        {
            MoveSpade(leftSpade, _leftStartPos);
            MoveSpade(rightSpade, _rightStartPos);
        }
    }

    private void MoveSpade(Transform obj, Vector3 targetPos)
    {
        obj.localPosition = Vector3.Lerp(obj.localPosition, targetPos, Time.fixedDeltaTime);
    }
    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Player")){
            Debug.Log("CRASH!");
            explosionParticle.Play();
            
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }
    
}
