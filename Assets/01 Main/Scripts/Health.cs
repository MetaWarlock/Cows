using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private int _currentHealth;

    //Canvas
    private Slider _healthSlider;
    private GameObject _canvas;

    private Character _character;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _character = GetComponent<Character>();
        _healthSlider = GetComponentInChildren<Slider>();
        _canvas = GetComponentInChildren<Canvas>().gameObject;
    }

    private void Update()
    {
        if(_canvas.transform.rotation != Camera.main.transform.rotation)
        {
            _canvas.transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        _healthSlider.value = _currentHealth;
        if (_currentHealth < 0)
        {
            GameObject.Destroy(gameObject);
        }
        Debug.Log(gameObject.name + " took damage: " + damage);
        Debug.Log(gameObject.name + " current health: " + _currentHealth);
    }

}
