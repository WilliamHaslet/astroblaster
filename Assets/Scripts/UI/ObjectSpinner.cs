using UnityEngine;

public class ObjectSpinner : MonoBehaviour {

    [SerializeField] private GameObject[] objectsToRotate;
    [SerializeField] private float spinSpeed;
    [SerializeField] private float pressedSpinMultiplier;
    [SerializeField] private float pressedSpinDecreaseRate;

    private GameObject spinObject;
    private float spinTime;

    private void Update()
    {

        foreach (GameObject obj in objectsToRotate)
        {

            if (obj == spinObject)
            {

                spinTime += Time.deltaTime;

                float exponentialDecay = spinSpeed * Time.deltaTime * pressedSpinMultiplier * Mathf.Pow(1 - pressedSpinDecreaseRate, spinTime);

                obj.transform.Rotate(new Vector3(0, 0, exponentialDecay));

                if (exponentialDecay <= spinSpeed * Time.deltaTime)
                {

                    spinTime = 0;

                    spinObject = null;

                }

            }
            else
            {

                obj.transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));

            }

        }

    }

    public void Spin(GameObject obj)
    {

        spinObject = obj;

    }

}
