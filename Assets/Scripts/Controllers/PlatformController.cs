using Managers;
using UnityEngine;

namespace Controllers
{
    public class PlatformController : MonoBehaviour
    {
        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private Camera mainCamera;
        private void OnEnable()
        {
            GamePlayEvents.instance.OnBuildPlatform += PlacePlatform;
        }

        private void OnDisable()
        {
           GamePlayEvents.instance.OnBuildPlatform -= PlacePlatform;
        }

        private void PlacePlatform()
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(platformPrefab, mousePosition, Quaternion.identity);
        }
    } 
}
