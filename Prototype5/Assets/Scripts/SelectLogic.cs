using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SelectLogic : MonoBehaviour
{
    [SerializeField] private LayerMask _raycastMask;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _fallingObjectPrefab;
    [SerializeField] private GameObject _potionResult;
    private Camera _camera;
    private Color _originalColor;
    private PotionStats _potionStats;
    private MeshRenderer _potionLid, _potionBottom, _potionSubstance1, _potionSubstance2;

    public MeshRenderer currentHover;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        _potionStats = _potionResult.gameObject.GetComponent<PotionStats>();
        _potionSubstance1 = _potionResult.transform.Find("Substance1").gameObject.GetComponent<MeshRenderer>();
        _potionSubstance2 = _potionResult.transform.Find("Substance2").gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetHoveredObject();
        SelectObject();
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

    private void SelectObject()
    {
        if (!currentHover) return;
        if (_potionResult.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentHover.gameObject.CompareTag($"Ingredient"))
            {
                GameObject spawnedIngredient = Instantiate(_fallingObjectPrefab, _spawnPoint);
                spawnedIngredient.GetComponent<MeshRenderer>().material.color = _originalColor;

                PotionStats ingredientStats = currentHover.gameObject.GetComponent<PotionStats>();
                
                // Hardcoded
                _potionStats.strength += ingredientStats.strength;
                _potionStats.luck += ingredientStats.luck;
                _potionStats.amnesia += ingredientStats.amnesia;

            }
            else if (currentHover.gameObject.CompareTag($"Cauldron"))
            {
                _potionResult.gameObject.GetComponent<PotionStats>().ResetAllToDefault();
            }
            else if (currentHover.gameObject.CompareTag($"Ladle"))
            {
                Color randomColor = Random.ColorHSV();;
                _potionSubstance1.material.color = randomColor;
                _potionSubstance2.material.color = randomColor;
                
                _potionResult.SetActive(true);
                StartCoroutine(DeactivatePotion());
            }
        }
        
    }

    IEnumerator DeactivatePotion()
    {
        yield return new WaitForSeconds(3f);
        _potionResult.SetActive(false);
        _potionResult.gameObject.GetComponent<PotionStats>().ResetAllToDefault();
    }
}
