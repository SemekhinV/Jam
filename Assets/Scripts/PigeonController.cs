using UnityEngine;

public class PigeonController : MonoBehaviour
{
    enum State { Idle, Alert, Flee }
    State _state = State.Idle;
    Animator _anim;
    Vector3 _homePosition;
    float _alertTimer = 0f;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _homePosition = transform.position;
    }

    void OnEnable()
    {
        GameEvents.OnPlayerInteract += OnPlayerInteract;
        GameEvents.OnDoorToggled += OnDoorToggled;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerInteract -= OnPlayerInteract;
        GameEvents.OnDoorToggled -= OnDoorToggled;
    }

    void Update()
    {
        switch (_state)
        {
            case State.Alert:
                _alertTimer -= Time.deltaTime;
                if (_alertTimer <= 0) _state = State.Idle;
                break;
            case State.Flee:
                transform.position = Vector3.MoveTowards(transform.position, _homePosition + Vector3.up * 3f, 2f * Time.deltaTime);
                if (Vector3.Distance(transform.position, _homePosition + Vector3.up * 3f) < 0.1f) _state = State.Idle;
                break;
        }
        _anim.SetInteger("state", (int)_state);
    }

    void OnPlayerInteract(Vector2 pos)
    {
        float dist = Vector2.Distance(transform.position, pos);
        if (dist < 3f)
        {
            _state = State.Alert;
            _alertTimer = 2f;
        }
    }

    void OnDoorToggled(int doorId)
    {
        // проста€ реакци€: если дверь р€дом Ч испугатьс€ => flee
        float dist = Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2f, Screen.height/2f, 0f)));
        if (dist < 5f)
        {
            _state = State.Flee;
        }
    }
}