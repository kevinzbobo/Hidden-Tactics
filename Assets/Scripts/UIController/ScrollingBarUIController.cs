using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBarUIController : MonoBehaviour
{

    public GameObject FirstPiece;
    public GameObject SecondPiece;
    public float ScrollingSpeed = 0;

    private Rigidbody2D _firstRidigBody;
    private Rigidbody2D _secondRidigBody;
    private BoxCollider2D _firstCollider;
    private BoxCollider2D _secondCollider;
    private float _pieceWidth = 0;

    // Start is called before the first frame update
    void Start()
    {
        _firstCollider = FirstPiece.GetComponent<BoxCollider2D>();
        _secondCollider = SecondPiece.GetComponent<BoxCollider2D>();

        _firstRidigBody = FirstPiece.GetComponent<Rigidbody2D>();
        _secondRidigBody = SecondPiece.GetComponent<Rigidbody2D>();

        _pieceWidth = _firstCollider.bounds.size.x;
        SecondPiece.transform.localPosition = new Vector3(FirstPiece.transform.localPosition.x + _firstCollider.bounds.size.x, FirstPiece.transform.localPosition.y, FirstPiece.transform.localPosition.z);

        _secondRidigBody.velocity = new Vector2(ScrollingSpeed, 0);
        _firstRidigBody.velocity = new Vector2(ScrollingSpeed, 0);
    }

    private GameObject GetLeftPiece()
    {
        if (FirstPiece.transform.position.x < SecondPiece.transform.position.x)
        {
            return FirstPiece;
        }
        else
        {
            return SecondPiece;
        }
    }

    private GameObject GetRightPiece()
    {
        if (FirstPiece.transform.position.x >= SecondPiece.transform.position.x)
        {
            return FirstPiece;
        }
        else
        {
            return SecondPiece;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject leftPiece = GetLeftPiece();
        if (leftPiece.transform.localPosition.x < -_pieceWidth * 0.5)
        {
            GameObject rightPiece = GetRightPiece();

            leftPiece.transform.localPosition = new Vector3(rightPiece.transform.localPosition.x + _pieceWidth , FirstPiece.transform.localPosition.y, FirstPiece.transform.localPosition.z);
        }
    }
}
