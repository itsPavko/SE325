using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LogicLuckySix : MonoBehaviour
{
    [SerializeField] private List<int> balls;
    private List<int> balls2;
    private int spawnedBall;
    public GameObject[] holders;
    public GameObject ballPrefab;
    public List <GameObject> spawnedBalls;
    public int ballsDrawn;
    public BigBallLogic bigBall;

    public Color[] colors;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        ResetBoard();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            ResetBoard();
            StartCoroutine(DrawPhase());
        }
    }

    public IEnumerator DrawPhase()
    {
        yield return new WaitForSeconds(1);
        DrawBall();
    }

    public void ResetBoard()
    {
        balls.Clear();

        for (int i = 1; i <= 48; i++)
        {
            balls.Add(i);
        }

        if(ballsDrawn > 0)
        {
			for (int i = 0; i < 35; i++)
			{
                Destroy(spawnedBalls[i]);
			}
			spawnedBalls.Clear();
		}


        ballsDrawn = 0;
	}

    public void DrawBall()
    {
		int randomIndex = Random.Range(0, balls.Count);
		spawnedBall = balls[randomIndex];
		balls.RemoveAt(randomIndex);

        /*
		if (ballsDrawn != 0)
        {
			while (balls2.Contains(spawnedBall))
			{
				spawnedBall = Random.Range(0, balls.Count) + 1;
                balls2.Add(spawnedBall);
			}
		}
		*/

        ballsDrawn++;

        int c = (spawnedBall - 1) % 8;

		Debug.Log("Picking color index " + c + ". for ball number " + spawnedBall);
        color = colors[c];

        StartCoroutine(ShowBallInDrum());
    }

    public IEnumerator ShowBallInDrum()
    {
        yield return new WaitForSeconds(1.5f);

        bigBall.Spawn(spawnedBall, color);
        bigBall.SetDesSize(new Vector3(1, 1, 1), 5);

        yield return new WaitForSeconds(2.5f);

		bigBall.SetDesSize(new Vector3(0,0,0), 15);

		yield return new WaitForSeconds(1f);
		SpawnBall();
    }

    public void SpawnBall()
    {
		GameObject go = Instantiate(ballPrefab, transform.position, transform.rotation);
        go.transform.SetParent(holders[ballsDrawn - 1].transform.GetChild(0).transform);
        go.GetComponent<RectTransform>().position = holders[ballsDrawn - 1].transform.GetChild(0).GetComponent<RectTransform>().position;


		go.transform.GetComponent<Image>().color = color;
        go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = spawnedBall.ToString();
        spawnedBalls.Add(go);

        if(ballsDrawn < 35)
        {
            DrawBall();
        }
        else
        {
            Debug.Log("Game ended");
        }
	}
}
