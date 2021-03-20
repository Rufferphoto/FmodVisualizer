using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangentCircles : CircleTangent {

    public GameObject _circlePrefab;
    private GameObject _innerCircleGO, _outerCircleGO, _tangentCircleGO;
    public Vector4 _innerCircle, _outerCircle;
    public float _tangentCircleRadius;
    public float _degree;


	// Use this for initialization
	void Start () {
        _innerCircleGO = (GameObject)Instantiate(_circlePrefab);
        _outerCircleGO = (GameObject)Instantiate(_circlePrefab);
        _tangentCircleGO = (GameObject)Instantiate(_circlePrefab);
	}
	
	// Update is called once per frame
	void Update () {
        _innerCircleGO.transform.position = new Vector3(_innerCircle.x, _innerCircle.y, _innerCircle.z);
        _innerCircleGO.transform.localScale = new Vector3(_innerCircle.w, _innerCircle.w, _innerCircle.w) * 2;
        _outerCircleGO.transform.position = new Vector3(_outerCircle.x, _outerCircle.y, _outerCircle.z);
        _outerCircleGO.transform.localScale = new Vector3(_outerCircle.w, _outerCircle.w, _outerCircle.w) * 2;
        _tangentCircleGO.transform.position = GetRotatedTangent(_degree, _outerCircle.w) + _outerCircleGO.transform.position;
        _tangentCircleGO.transform.localScale = new Vector3(_tangentCircleRadius, _tangentCircleRadius, _tangentCircleRadius) * 2;
    }
}
