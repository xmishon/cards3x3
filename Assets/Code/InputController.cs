using System;
using UnityEngine;

namespace cards
{
    public class InputController
    {
        private Camera _camera;

        public InputController(Camera camera)
        {
            _camera = camera;
        }

        public void Execute()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit;
                Vector2 worldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if (hit.collider != null)
                {
                    hit.transform.gameObject.GetComponent<Cell>().MouseClick();
                }
            }
        }
    }
}
