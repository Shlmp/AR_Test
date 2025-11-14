using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [Header("Prefabs linked to reference image names")]
    public List<GameObject> Prefabs;

    ARTrackedImageManager imageManager;
    Dictionary<string, GameObject> arObjects;

    void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>();
        arObjects = new Dictionary<string, GameObject>();
    }

    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void Start()
    {
        foreach (var prefab in Prefabs)
        {
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.name = prefab.name;
            obj.SetActive(false);
            arObjects.Add(obj.name, obj);
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
            UpdateImage(trackedImage);

        foreach (var trackedImage in args.updated)
            UpdateImage(trackedImage);

        foreach (var trackedImage in args.removed)
        {
            if (arObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
                obj.SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        if (!arObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
            return;

        if (trackedImage.trackingState == TrackingState.None ||
            trackedImage.trackingState == TrackingState.Limited)
        {
            obj.SetActive(false);
            return;
        }

        obj.transform.SetPositionAndRotation(
            trackedImage.transform.position,
            trackedImage.transform.rotation
        );

        obj.SetActive(true);
        Debug.Log("Found image name: " + trackedImage.referenceImage.name);
    }
}
