using System;
using UnityEngine;

public class SelectLogic : MonoBehaviour
{
    [SerializeField] private LayerMask _raycastMask;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _fallingObjectPrefab;
    private Camera _camera;
    private Color _originalColor;

    public MeshRenderer currentHover;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        GetHoveredObject();
        SelectIngredient();
    }

    private void GetHoveredObject()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _raycastMask)) {
            if (!currentHover)
            {
                currentHover = hit.transform.GetComponent<MeshRenderer>();
                _originalColor = currentHover.material.color;
                currentHover.material.color *= 0.7f;   
            }
        }
        else if (currentHover)
        {
            currentHover.material.color = _originalColor;
            currentHover = null;
            _originalColor = Color.clear;
        }
    }

    private void SelectIngredient()
    {
        if (!currentHover) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject spawnedIngredient = Instantiate(_fallingObjectPrefab, _spawnPoint);
            spawnedIngredient.GetComponent<MeshRenderer>().material.color = _originalColor;    
        }
    }
}
