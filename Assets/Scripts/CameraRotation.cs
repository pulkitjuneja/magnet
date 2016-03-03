using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

    public bool isRotating = false ;
    public float TargetRotation = 0.0f;
    float RotationFactor = 10.0f;
    public float ElapsedTime = 0;
	
    void Start ()
    {  

	}
	
	void Update () 
    {
        ElapsedTime += Time.unscaledDeltaTime;
        if(ElapsedTime>=5.0f && !isRotating)
        {
            TargetRotation += (int)Random.Range(-1, 2) * RotationFactor;
            if (TargetRotation >= 50)
                TargetRotation += -1 * RotationFactor * 2;
            else if (TargetRotation <= -50)
                TargetRotation += 1 * RotationFactor * 2;
            Debug.Log(TargetRotation);
            StartCoroutine(Rotate());
            ElapsedTime = 0;
        }
	}

    IEnumerator Rotate()
    {
        isRotating = true;
        float currentRot = transform.rotation.z ;
        TargetRotation = Mathf.Deg2Rad * TargetRotation;
        float rot = (TargetRotation - currentRot);
        Debug.Log(rot);
        if (currentRot < TargetRotation)
        {
            while (transform.rotation.z <= TargetRotation)
            {
                transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + rot));
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if(currentRot>TargetRotation)
        {
            while (transform.rotation.z >= TargetRotation)
            {
                transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + rot));
                yield return new WaitForSeconds(0.05f);
                Debug.Log(transform.rotation.z);
            }
        }
        isRotating = false;
        Debug.Log(false);
    }
}
