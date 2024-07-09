using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class PointCloudCapture : MonoBehaviour
{
    [SerializeField]
    ARPointCloudManager arPointCloudManager;

    private List<Vector3> _pointCloudData;

    void Start()
    {
        /*arPointCloudManager = GetComponent<ARPointCloudManager>();*/
        _pointCloudData = new List<Vector3>();
    }

    void OnEnable()
    {
        if (arPointCloudManager != null)
        {
            arPointCloudManager.pointCloudsChanged += OnPointCloudsChanged;
        }
        
    }

    void OnDisable()
    {
        if (arPointCloudManager != null)
        {
            arPointCloudManager.pointCloudsChanged -= OnPointCloudsChanged;

        }
    }

    void OnPointCloudsChanged(ARPointCloudChangedEventArgs eventArgs)
    {
        foreach (ARPointCloud pointCloud in eventArgs.updated)
        {
            _pointCloudData.Clear();
            if (pointCloud.positions != null)
                foreach (var point in pointCloud.positions)
                {
                    _pointCloudData.Add(point);
                }
        }
    }

    public List<Vector3> GetPointCloudData()
    {
        return new List<Vector3>(_pointCloudData);
    }
}
