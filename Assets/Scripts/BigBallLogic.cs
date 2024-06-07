using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BigBallLogic : MonoBehaviour
{
    public TextMeshProUGUI number;
    public Image ball;
    private float speed;

    public Vector3 desSize;
    // Start is called before the first frame update
    void Start()
    {
        SetDesSize(new Vector3 (0,0,0), 5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, desSize, Time.deltaTime * speed);
    }

    public void SetDesSize(Vector3 size, float s)
    {
        desSize = size;
        speed = s;
    }

    public void Spawn(int n, Color c)
    {
        number.text = n.ToString();
        ball.color = c;
    }
}
