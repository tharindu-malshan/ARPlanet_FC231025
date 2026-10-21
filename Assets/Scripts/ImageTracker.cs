
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    [System.Serializable]
    public class ImagePrefab
    {
        public string imageName;
        public GameObject prefab;
    }

    [Header("AR Image Tracking")]
    public ARTrackedImageManager trackedImageManager;

    [Header("Image Prefabs")]
    public List<ImagePrefab> imagePrefabs = new List<ImagePrefab>();

    [Header("Object Position")]
    public float heightAboveImage = 0.05f;

    [Header("Sound (Optional)")]
    public AudioSource audioSource;
    public AudioClip foundSound;

    private Dictionary<TrackableId, GameObject> spawned =
        new Dictionary<TrackableId, GameObject>();

    private void OnEnable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackablesChanged.AddListener(
                OnImagesChanged
            );
        }
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackablesChanged.RemoveListener(
                OnImagesChanged
            );
        }
    }

    private void OnImagesChanged(
        ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        // Images detected for the first time
        foreach (ARTrackedImage image in args.added)
        {
            SpawnFor(image);
        }

        // Images that are being tracked or updated
        foreach (ARTrackedImage image in args.updated)
        {
            if (spawned.TryGetValue(
                image.trackableId,
                out GameObject obj))
            {
                bool isTracking =
                    image.trackingState == TrackingState.Tracking;

                obj.SetActive(isTracking);
            }
            else if (image.trackingState == TrackingState.Tracking)
            {
                SpawnFor(image);
            }
        }

        // Images removed from tracking
        foreach (var removed in args.removed)
        {
            TrackableId id = removed.Key;

            if (spawned.TryGetValue(id, out GameObject obj))
            {
                Destroy(obj);
                spawned.Remove(id);
            }
        }
    }

    private void SpawnFor(ARTrackedImage image)
    {
        string detectedImageName = image.referenceImage.name;

        foreach (ImagePrefab item in imagePrefabs)
        {
            if (item.imageName == detectedImageName &&
                item.prefab != null)
            {
                // Create the prefab as a child of the tracked image
                GameObject obj = Instantiate(
                    item.prefab,
                    image.transform
                );

                // Position relative to the tracked image
                obj.transform.localPosition = new Vector3(
                    0f,
                    heightAboveImage,
                    0f
                );

                obj.transform.localRotation = Quaternion.identity;

                spawned[image.trackableId] = obj;

                // Play sound when the image is detected
                if (audioSource != null && foundSound != null)
                {
                    audioSource.PlayOneShot(foundSound);
                }

                Debug.Log(
                    "Image detected: " + detectedImageName
                );

                return;
            }
        }

        Debug.LogWarning(
            "No prefab assigned for image: " + detectedImageName
        );
    }
}
