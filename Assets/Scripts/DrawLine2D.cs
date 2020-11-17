using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityLibrary
{
    [RequireComponent(typeof(LineRenderer))]
    public class DrawLine2D : MonoBehaviour
    {
        #region Private Components

        private GameObject leftOverLine;
        private DotGameMechs gameMechs;
        private Color firstDotColor;
        private EdgeCollider2D lineEdgeCollider2D;
        private List<GameObject> dotsInCollision;
        private List<Vector2> mousePoints;

        #endregion
        #region Private Variables

        private int mouseStartingPos;
        private int mouseTrackingPos;
        private bool firstLinePoint;
        private bool firstDotCollision;
        private bool colliderNeeded;

        #endregion
        #region Serialized(Exposed to editor) Variables
        //Exposed to editor
        [SerializeField]
        private LineRenderer lineRenderer;
        [SerializeField]
        private GameObject leftoverLinePrefab;

        #endregion

        private void Awake()
        {
            InitialConfig();
        }
        private void Start()
        {
            gameMechs = GameObject.Find("DotMechanics").GetComponent<DotGameMechs>();
        }
        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                StartLineCreation();
            }
            if(Input.GetMouseButton(0))
            {
                LineTracking();
            }
            if(Input.GetMouseButtonUp(0))
            {
                if(dotsInCollision.Count == 1) //Preventing clicking/tapping dot from removing it
                {
                    ResetLineValues();
                    ClearVisualLines();
                    dotsInCollision.Clear();
                }
                else
                {
                    ResetLineValues();
                    ClearVisualLines();
                    gameMechs.RemoveDots(dotsInCollision);
                    dotsInCollision.Clear();
                }
            }
        }

        #region Line Manipulation

        private void StartLineCreation()//Starting Line on first dot only
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (firstLinePoint)
            {
                lineEdgeCollider2D.enabled = true;
                Reset();
                mousePoints.Add(mousePosition);
                lineRenderer.SetPosition(mouseStartingPos, mousePosition);
                lineRenderer.SetPosition(mouseTrackingPos, mousePosition);
                firstLinePoint = false;
            }
        }
        private void LineTracking()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(mouseTrackingPos, mousePosition);
            if (lineEdgeCollider2D != null && colliderNeeded)
            {
                mousePoints.Add(mousePosition);
                lineEdgeCollider2D.points = FirstAndLastPoint(mousePoints).ToArray();
            }
        }
        private void LeaveVisualConnectingLine(Collision2D pointToConnect)//Line left between last two connected dots
                {
                    lineRenderer.SetPosition(mouseTrackingPos, pointToConnect.transform.parent.transform.position);
                    leftOverLine = Instantiate(leftoverLinePrefab, transform);
                    leftOverLine.name = name + ' ' + mouseTrackingPos;
                    LineRenderer leftBehindLine = leftOverLine.GetComponent<LineRenderer>();
                    leftBehindLine.positionCount = 2;
                    leftBehindLine.SetPosition(0, lineRenderer.GetPosition(0));
                    leftBehindLine.SetPosition(1, lineRenderer.GetPosition(1));
                    leftBehindLine.startColor = pointToConnect.gameObject.GetComponent<SpriteRenderer>().color;
                    leftBehindLine.endColor = pointToConnect.gameObject.GetComponent<SpriteRenderer>().color;
                    Reset(leftBehindLine);
                    leftOverLine = null;
                    dotsInCollision.Add(pointToConnect.gameObject);
                }

        #endregion
        #region Collision Detection and Handling

        private List<Vector2> FirstAndLastPoint(List<Vector2>points) //Limiting the amount of points stored by the line and collider
        {
            int counter = points.Count;
            List<Vector2> newPoints = new List<Vector2>();
            for (int x = 0; x < counter; x++)
            {
                if(x == 0 || (x == counter - 1))
                {
                   newPoints.Add(points[x]);
                }
                else
                {
                    continue;
                }
            }
            return newPoints;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(firstDotCollision)
            {
                //First point in line
                CreateFirstDotCollision(collision);
            }
            else
            {
                //Checking that the color matches and it isn't going back into itself
                if(firstDotColor == collision.gameObject.GetComponent<SpriteRenderer>().color)
                {                
                    if(!dotsInCollision.Contains(collision.gameObject))//Do not allow the same lines to be drawn/lines to the same dot
                    {
                        LeaveVisualConnectingLine(collision);
                    }   
                }           
            }
        }
        private void CreateFirstDotCollision(Collision2D pointToConnect) //Changes color of line, to color of dot and begins the line
        {
            firstDotCollision = false;
            lineRenderer.SetPosition(mouseStartingPos, pointToConnect.transform.parent.transform.position);
            lineRenderer.startColor = pointToConnect.gameObject.GetComponent<SpriteRenderer>().color;
            lineRenderer.endColor = pointToConnect.gameObject.GetComponent<SpriteRenderer>().color;
            firstDotColor = pointToConnect.gameObject.GetComponent<SpriteRenderer>().color;
            dotsInCollision.Add(pointToConnect.gameObject);
        }

        #endregion
        #region Resetting
        
        private void Reset()
        { 
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 2;
            }
            if (mousePoints != null)
            {
                mousePoints.Clear();
            }
            if (lineEdgeCollider2D != null && colliderNeeded)
            {
                lineEdgeCollider2D.Reset();
            }
        }
        private void ResetLineValues()
        {
            firstLinePoint = true;
            firstDotCollision = true;
            lineEdgeCollider2D.enabled = false;
            lineRenderer.SetPosition(mouseStartingPos, new Vector2(0, 0));
            lineRenderer.SetPosition(mouseTrackingPos, new Vector2(0, 0));
            firstDotColor = Color.black;
            lineRenderer.startColor = Color.clear;
            lineRenderer.endColor = Color.clear; 
        }
        private void ClearVisualLines()
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        private void Reset(LineRenderer incRenderer)
        {
            mousePoints.Clear();
            mousePoints.Add(incRenderer.GetPosition(1));
            lineRenderer.SetPosition(0,incRenderer.GetPosition(1));
        }

        #endregion
        #region Defaults/Config

        private void CreateDefaultEdgeCollider2D()
        {
            lineEdgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
            lineEdgeCollider2D.enabled = false;
        }
        private void InitialConfig() //Should only be used with Awake()
        {
            firstLinePoint = true;
            firstDotCollision = true;
            colliderNeeded = true;
            //If Edge Collider is ever added/removed from object in the future via inspector/code
            if (lineEdgeCollider2D == null && colliderNeeded)
            {
                Debug.LogWarning("Edge Collider not added, creating default Edge Collider.");
                CreateDefaultEdgeCollider2D();
            }
            mousePoints = new List<Vector2>();
            dotsInCollision = new List<GameObject>();
            mouseStartingPos = 0;
            mouseTrackingPos = 1;
            firstDotColor = Color.black;
        }

        #endregion
    }
}